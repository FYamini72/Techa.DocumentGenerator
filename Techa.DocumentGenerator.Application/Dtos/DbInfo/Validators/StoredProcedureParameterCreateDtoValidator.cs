using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;

namespace Techa.DocumentGenerator.Application.Dtos.DbInfo.Validators
{
    public class StoredProcedureParameterCreateDtoValidator : AbstractValidator<StoredProcedureParameterCreateDto>
    {
        private readonly IBaseService<StoredProcedure> _storedProcedureService;
        public StoredProcedureParameterCreateDtoValidator(IBaseService<StoredProcedure> storedProcedureService)
        {
            _storedProcedureService = storedProcedureService;
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<StoredProcedureParameterCreateDto> context, CancellationToken cancellation = default)
        {
            RuleFor(x => x.ParameterName)
                .NotEmpty()
                .WithMessage("وارد کردن نام پارامتر الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام پارامتر الزامی است");

            RuleFor(x => x.StoredProcedureId)
                .NotEmpty()
                .WithMessage("انتخاب کردن پروسیجر الزامی است")
                .NotNull()
                .WithMessage("انتخاب کردن پروسیجر الزامی است")
                .MustAsync(async (storedProcedureId, cancellationToken) =>
                {
                    return await _storedProcedureService.GetAll(x => x.Id == storedProcedureId).AnyAsync();
                })
                .WithMessage("پروسیجر انتخاب شده معتبر نیست");

            return base.ValidateAsync(context, cancellation);
        }
    }
}