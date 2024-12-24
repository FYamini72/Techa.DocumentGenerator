using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
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
                .WithMessage("وارد کردن پروژه الزامی است")
                .NotNull()
                .WithMessage("وارد کردن پروژه الزامی است")
                .MustAsync(async (projectId, cancellationToken) =>
                {
                    return await _projectService.GetAll(x=>x.Id == projectId).AnyAsync();
                })
                .WithMessage("پروژه انتخاب شده معتبر نیست");

            return base.ValidateAsync(context, cancellation);
        }
    }
}