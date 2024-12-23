using FluentValidation;
using System.Text.RegularExpressions;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.AAA;
using Techa.DocumentGenerator.Domain.Utilities;

namespace Techa.DocumentGenerator.Application.Dtos.AAA.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IBaseService<User> _userService;

        public UserCreateDtoValidator(IBaseService<User> userService)
        {
            _userService = userService;

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("وارد کردن نام کاربری الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام کاربری الزامی است")
                .Must((obj, username) =>
                {
                    return !_userService.GetAll(x => x.Id != obj.Id && x.UserName == username).Any();
                })
                .WithMessage("نام کاربری وارد شده تکراری می باشد")
                ;

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("وارد کردن کلمه عبور الزامی است")
                .NotNull()
                .WithMessage("وارد کردن کلمه عبور الزامی است");

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
                .Matches(new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
                .When(x => x.Email != null && x.Email.Trim() != "")
                .WithMessage("ایمیل وارد شده معتبر نمی باشد");

        }
    }
}