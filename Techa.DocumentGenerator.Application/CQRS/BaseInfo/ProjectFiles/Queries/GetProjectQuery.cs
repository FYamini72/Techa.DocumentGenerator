using MediatR;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Queries
{
    public class GetProjectQuery : IRequest<HandlerResponse<ProjectDisplayDto>>
    {
        public int Id { get; }

        public GetProjectQuery(int id)
        {
            Id = id;
        }
    }
}