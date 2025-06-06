﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Betalgo.Ranul.OpenAI.Extensions;
using Techa.DocumentGenerator.Infrastructure.Data;
using Techa.DocumentGenerator.Domain.Settings;
using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Application.Services.Implementations;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Infrastructure.IdentityServices;
using Techa.DocumentGenerator.API.Utilities.Exceptions;
using Techa.DocumentGenerator.API.Utilities.Api;
using Techa.DocumentGenerator.API.Mapping;
using Techa.DocumentGenerator.Infrastructure.Repositories;
using Techa.DocumentGenerator.Domain.Entities.AAA;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Commands;
using Techa.DocumentGenerator.Application.Dtos.AAA.Validators;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;
using Techa.DocumentGenerator.Application.Services.Implementations.AAA;
using Techa.DocumentGenerator.Application.Services.Implementations.Ai;
using Techa.DocumentGenerator.Application.Services.Interfaces.Ai;
using Newtonsoft.Json;
using Techa.DocumentGenerator.Application.Dtos.AAA;

namespace Techa.DocumentGenerator.API.Utilities
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder =>
                builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
            });

            services.AddEndpointsApiExplorer();

            var _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            services.AddSingleton(_jwtSettings);

            var _avalAiSettings = configuration.GetSection("AvalAiSetting").Get<AvalAiSetting>();
            services.AddSingleton(_avalAiSettings);

            services.AddOpenAIService(settings =>
            {
                settings.ApiKey = _avalAiSettings?.ApiKey ?? "";
                settings.BaseDomain = "https://api.avalapis.ir/v1";
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                var encryptkey = Encoding.UTF8.GetBytes(_jwtSettings.Encryptkey);
                var validationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,

                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var strProjectId = claimsIdentity.FindFirstValue("ProjectId");
                        if (!string.IsNullOrEmpty(strProjectId))
                        {
                            if(int.TryParse(strProjectId, out int projectId))
                            {
                                var adoService = context.HttpContext.RequestServices.GetRequiredService<IAdoService>();

                                var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                                if (!securityStamp.HasValue())
                                    context.Fail("This token has no secuirty stamp");

                                var userId = claimsIdentity.GetUserId<int>();
                                var userAdoResponse = await adoService.GetDataAsync(projectId, $"SELECT [SecurityStamp] FROM USERS WHERE Id = {userId}", true, true, true, context.HttpContext.RequestAborted);

                                var data = JsonConvert.DeserializeObject<List<UserSecurityStamp>>(userAdoResponse.Dataset);
                                var dbSecurityStamp = data?.FirstOrDefault()?.SecurityStamp ?? "";

                                if (string.IsNullOrEmpty(dbSecurityStamp))
                                    context.Fail("Token is not valid.");

                                if (Guid.Parse(dbSecurityStamp) != Guid.Parse(securityStamp))
                                    context.Fail("Token secuirty stamp is not valid.");
                            }
                            else
                            {
                                context.Fail("ProjectId not found.");
                            }
                        }
                        else
                        {
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IBaseRepository<User>>();
                            var userRoleRepository = context.HttpContext.RequestServices.GetRequiredService<IBaseRepository<UserRole>>();

                            var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                            if (!securityStamp.HasValue())
                                context.Fail("This token has no secuirty stamp");

                            var userId = claimsIdentity.GetUserId<int>();
                            var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                            if (user.SecurityStamp != Guid.Parse(securityStamp))
                                context.Fail("Token secuirty stamp is not valid.");

                            var endPoint = context.HttpContext.GetEndpoint();
                            if (endPoint != null)
                            {
                                var authAttr = endPoint.Metadata.OfType<AuthorizeAttribute>();
                                if (authAttr != null)
                                {
                                    var authorizedRoles = authAttr.Select(x => x.Roles);
                                    if (authorizedRoles != null && authorizedRoles.Where(x => !string.IsNullOrEmpty(x)).Any())
                                    {
                                        var roleClaims = claimsIdentity.GetUserRoles();
                                        if (roleClaims != null)
                                        {
                                            if (!authorizedRoles.Any(x => roleClaims.Contains(x)))
                                                context.Fail("You are unauthorized to access this resource.");
                                        }
                                    }
                                }
                            }
                        }
                    },
                    OnChallenge = context =>
                    {
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.Message != "You are unauthorized to access this resource.")
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
                        throw new AppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);
                    }
                };
            });

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserCommand)));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAttachmentFileService, AttachmentFileService>();
            services.AddScoped<IHttpContextHelper, HttpContextHelper>();
            services.AddScoped<IAdoRepository, AdoRepository>();
            services.AddScoped<IAdoService, AdoService>();
            services.AddScoped<IAvalAiService, AvalAiService>();

            services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>();

            services.RegisterMapsterConfiguration();

            services.AddHttpContextAccessor();
            services.AddCors();

            services.AddControllersWithViews();

            services.AddSwaggerGen();
        }
    }

    public class UserSecurityStamp
    {
        public string SecurityStamp { get; set; }
    }
}
