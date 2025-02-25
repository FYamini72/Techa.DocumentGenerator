using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;

namespace Techa.DocumentGenerator.Application.Dtos.DbInfo.Validators
{
    public class StoredProcedureCreateDtoValidator : AbstractValidator<StoredProcedureCreateDto>
    {
        private readonly IBaseService<Project> _projectService;
        public StoredProcedureCreateDtoValidator(IBaseService<Project> projectService)
        {
            _projectService = projectService;
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<StoredProcedureCreateDto> context, CancellationToken cancellation = default)
        {
            RuleFor(x => x.ProcedureName)
                .NotEmpty()
                .WithMessage("وارد کردن نام پروسیجر الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام پروسیجر الزامی است");
            
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("انتخاب کردن پروژه الزامی است")
                .NotNull()
                .WithMessage("انتخاب کردن پروژه الزامی است")
                .MustAsync(async (projectId, cancellationToken) =>
                {
                    return await _projectService.GetAll(x=>x.Id == projectId).AnyAsync();
                })
                .WithMessage("پروژه انتخاب شده معتبر نیست");

            return base.ValidateAsync(context, cancellation);
        }
    }
    public class StoredProcedureScriptDtoValidator : AbstractValidator<StoredProcedureScriptDto>
    {
        private readonly IBaseService<Project> _projectService;
        public StoredProcedureScriptDtoValidator(IBaseService<Project> projectService)
        {
            _projectService = projectService;
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<StoredProcedureScriptDto> context, CancellationToken cancellation = default)
        {
            RuleFor(x => x.ProcedureCode)
                .NotEmpty()
                .WithMessage("وارد کردن اسکریپت الزامی است")
                .NotNull()
                .WithMessage("وارد کردن اسکریپت الزامی است")
                .Must((obj, procedureCode) =>
                {
                    var sp_name = obj.ProcedureName.Contains(".") 
                        ? obj.ProcedureName.Split(".")[1].Replace("[", "").Replace("]", "") 
                        : obj.ProcedureName.Replace("[", "").Replace("]", "");
                    var script_sp_name = ExtractStoredProcedureName(procedureCode);

                    return (procedureCode.ToLower().StartsWith($"alter") && sp_name.ToLower() == script_sp_name.ToLower());
                })
                .WithMessage("امکان تغییر نام و یا ایجاد پروسیجر جدید وجود ندارد.");

            RuleFor(x => x.ProcedureName)
                .NotEmpty()
                .WithMessage("وارد کردن نام پروسیجر الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام پروسیجر الزامی است");
            
            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("انتخاب کردن پروژه الزامی است")
                .NotNull()
                .WithMessage("انتخاب کردن پروژه الزامی است")
                .MustAsync(async (projectId, cancellationToken) =>
                {
                    return await _projectService.GetAll(x=>x.Id == projectId).AnyAsync();
                })
                .WithMessage("پروژه انتخاب شده معتبر نیست");

            return base.ValidateAsync(context, cancellation);
        }

        static string ExtractStoredProcedureName(string sqlQuery)
        {
            // الگوی regex برای یافتن نام پروسیجر  
            var regex = new Regex(@"ALTER\s+PROC\s+(\[?[a-zA-Z0-9_]+\]?\.\[?[a-zA-Z0-9_]+\]?)|\s*(\.[a-zA-Z0-9_]+)|(\[?[a-zA-Z0-9_]+\]?)",
                                    RegexOptions.IgnoreCase);

            var match = regex.Match(sqlQuery);

            if (match.Success)
            {
                // نام پروسیجر را از اولین گروه استخراج کنید و"  
                string procedureName = match.Groups[1].Value;

                // حذف علامت‌های اضافی  
                procedureName = procedureName.Replace("[", "").Replace("]", "").Trim();

                // جدا کردن `schema` در صورت وجود  
                var parts = procedureName.Split('.');
                return parts.Length > 1 ? parts[1] : parts[0]; // اگر schema وجود دارد، نام آن را برگردانید  
            }

            return null; // اگر چیزی پیدا نشد  
        }
    }
}