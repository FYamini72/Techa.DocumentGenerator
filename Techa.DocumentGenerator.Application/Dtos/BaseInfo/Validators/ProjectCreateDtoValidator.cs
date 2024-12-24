using FluentValidation;
using FluentValidation.Results;

namespace Techa.DocumentGenerator.Application.Dtos.BaseInfo.Validators
{
    public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
    {
        public ProjectCreateDtoValidator()
        {
            
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<ProjectCreateDto> context, CancellationToken cancellation = default)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("وارد کردن نام پروژه الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام پروژه الزامی است");

            RuleFor(x => x.DbName)
                .NotEmpty()
                .WithMessage("وارد کردن نام دیتابیس الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام دیتابیس الزامی است");

            RuleFor(x => x.DbServerName)
                .NotEmpty()
                .WithMessage("وارد کردن آدرس سرور الزامی است")
                .NotNull()
                .WithMessage("وارد کردن آدرس سرور الزامی است");

            RuleFor(x => x.DbUserName)
                .NotEmpty()
                .WithMessage("وارد کردن نام کاربری الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام کاربری الزامی است");

            RuleFor(x => x.DbPassword)
                .NotEmpty()
                .WithMessage("وارد کردن کلمه عبور الزامی است")
                .NotNull()
                .WithMessage("وارد کردن کلمه عبور الزامی است");

            return base.ValidateAsync(context, cancellation);
        }
    }
}