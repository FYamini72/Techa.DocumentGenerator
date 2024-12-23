using FluentValidation;
using System.Text.RegularExpressions;
using Techa.DocumentGenerator.Domain.Utilities;

namespace Techa.DocumentGenerator.Application.Dtos.AAA.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("وارد کردن نام الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام الزامی است");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("وارد کردن نام خانوادگی الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام خانوادگی الزامی است");

            RuleFor(x => x.NationalCode)
                .Must((obj, nationalCode) =>
                {
                    return nationalCode.NationalCodeValidator();
                })
                .When(x => x.NationalCode != null && x.NationalCode.Trim() != "")
                .WithMessage("کدملی وارد شده صحیح نمی باشد")
                ;

            RuleFor(x => x.Mobile)
                .Matches(new Regex("^(\\+98|0)?9\\d{9}$"))
                .When(x => x.Mobile != null && x.Mobile.Trim() != "")
                .WithMessage("شماره تماس وارد شده معتبر نمی باشد");

            RuleFor(x => x.Email)
                .Matches(new Regex("/^((?!\\.)[\\w\\-_.]*[^.])(@\\w+)(\\.\\w+(\\.\\w+)?[^.\\W])$/gm"))
                .When(x => x.Email != null && x.Email.Trim() != "")
                .WithMessage("ایمیل وارد شده معتبر نمی باشد");
        }
    }
}