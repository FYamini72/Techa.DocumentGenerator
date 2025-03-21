using FluentValidation;
using System.Text.RegularExpressions;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Domain.Utilities;

namespace Techa.DocumentGenerator.Application.Dtos.AAA.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IBaseService<Project> _projectService;
        public UserUpdateDtoValidator(IBaseService<Project> projectService)
        {
            _projectService = projectService;

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