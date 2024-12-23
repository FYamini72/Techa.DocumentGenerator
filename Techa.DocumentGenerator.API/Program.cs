using Techa.DocumentGenerator.API.Utilities.Middelwares;
using Techa.DocumentGenerator.API.Utilities;
using Techa.DocumentGenerator.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.API
{
    public class Program
    {
        private static bool hasRun = false;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;

            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            builder.Services.AddServices(builder.Configuration);


            var app = builder.Build();

            var dbContext = builder.Services.BuildServiceProvider().GetRequiredService<ApplicationDbContext>();
            DataInitializer.Initialize(dbContext);

            var dbName = (builder.Configuration.GetSection("DatabaseSetting:DatabaseName").Value ?? "").ToString();
            var eventLogFilePath = (builder.Configuration.GetSection("DatabaseSetting:EventLogFilePath").Value ?? "").ToString();

            if (!string.IsNullOrEmpty(dbName) && !hasRun)
            {
                dbContext.Database.ExecuteSqlRaw(@$"USE [{dbName}]
                                                IF NOT EXISTS (SELECT * FROM sys. filegroups WHERE type = 'FG' AND name = 'EventLogFileGroup')
                                                BEGIN
	                                                USE [master]
	                                                ALTER DATABASE [{dbName}] ADD FILEGROUP [EventLogFileGroup]
	                                                ALTER DATABASE [{dbName}] ADD FILE ( NAME = N'{dbName}_EventLogFile_01', FILENAME = N'{eventLogFilePath}\{dbName}_EventLogFile_01.ndf' , SIZE = 8192KB , FILEGROWTH = 65536KB ) TO FILEGROUP [EventLogFileGroup]

	                                                USE [{dbName}]
	                                                ALTER TABLE EventLogs DROP CONSTRAINT PK_EventLogs WITH (MOVE TO [EventLogFileGroup])
	                                                ALTER TABLE EventLogs ADD CONSTRAINT PK_EventLogs PRIMARY KEY(ID)
                                                END");

                hasRun = true;
            }

            // Configure the HTTP request pipeline.

            app.UseCustomExceptionHandler();

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            //WWWROOT
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}