using FluentValidation;

namespace Techa.DocumentGenerator.Application.Dtos.AAA.Validators
{
    public class UserChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto>
    {
        public UserChangePasswordDtoValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("وارد کردن کلمه عبور قبلی الزامی است")
                .NotNull()
                .WithMessage("وارد کردن کلمه عبور قبلی الزامی است");
            
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("وارد کردن کلمه عبور جدید الزامی است")
                .NotNull()
                .WithMessage("وارد کردن کلمه عبور جدید الزامی است")
                .Must((obj, newPassword) =>
                {
                    return newPassword != obj.OldPassword;
                })
                .WithMessage("کلمه عبور وارد شده با مقدار قبلی آن یکسان هستند");

        }
    }
}