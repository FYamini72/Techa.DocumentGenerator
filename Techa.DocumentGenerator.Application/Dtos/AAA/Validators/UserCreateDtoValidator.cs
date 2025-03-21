using FluentValidation;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.AAA;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;

namespace Techa.DocumentGenerator.Application.Dtos.AAA.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IBaseService<User> _userService;
        private readonly IBaseService<Project> _projectService;

        public UserCreateDtoValidator(IBaseService<User> userService, IBaseService<Project> projectService)
        {
            _userService = userService;
            _projectService = projectService;

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("وارد کردن نام کاربری الزامی است")
                .NotNull()
                .WithMessage("وارد کردن نام کاربری الزامی است")
                .Must((obj, username) =>
                {
                    return !_userService.GetAll(x => x.Id != obj.Id && x.UserName == username && x.ProjectId == obj.ProjectId).Any();
                })
                .WithMessage("نام کاربری وارد شده تکراری می باشد")
                ;

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("وارد کردن کلمه عبور الزامی است")
                .NotNull()
                .WithMessage("وارد کردن کلمه عبور الزامی است");

            RuleFor(x => x.ProjectId)
                .Must((obj, projectId) =>
                {
                    return _projectService.GetAll(x => x.Id == projectId).Any();
                })
                .When(x => x.ProjectId.HasValue)
                .WithMessage("پروژه انتخاب شده معتبر نمی باشد")
                ;
        }
    }
}