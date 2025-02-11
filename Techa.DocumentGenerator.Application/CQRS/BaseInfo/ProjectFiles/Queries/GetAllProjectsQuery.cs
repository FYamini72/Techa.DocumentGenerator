using MediatR;
using Techa.DocumentGenerator.Application.Dtos;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Queries
{
    public class GetAllProjectsQuery : IRequest<HandlerResponse<BaseGridDto<ProjectDisplayDto>>>
    {
        public ProjectSearchDto? SearchDto { get; }

        public GetAllProjectsQuery(ProjectSearchDto? searchDto)
        {
            this.SearchDto = searchDto;
        }
    }
}