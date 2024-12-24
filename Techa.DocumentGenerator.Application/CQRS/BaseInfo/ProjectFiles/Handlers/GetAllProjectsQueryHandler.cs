using Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.BaseInfo;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Handlers
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, HandlerResponse<List<ProjectDisplayDto>>>
    {
        private readonly IBaseService<Project> _service;

        public GetAllProjectsQueryHandler(IBaseService<Project> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<List<ProjectDisplayDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var items = _service.GetAll();

            if(request.SearchDto != null)
            {
                if (!request.SearchDto.GetAllItems)
                {
                    if (!string.IsNullOrEmpty(request.SearchDto.Title))
                        items = items.Where(x => x.Title.Contains(request.SearchDto.Title));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.Description))
                        items = items.Where(x => x.Description.Contains(request.SearchDto.Description));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.DbServerName))
                        items = items.Where(x => x.DbServerName.Contains(request.SearchDto.DbServerName));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.DbName))
                        items = items.Where(x => x.DbName.Contains(request.SearchDto.DbName));

                    if (!string.IsNullOrEmpty(request.SearchDto.DbUserName))
                        items = items.Where(x => x.DbUserName.Contains(request.SearchDto.DbUserName));

                    if (!string.IsNullOrEmpty(request.SearchDto.DbPassword))
                        items = items.Where(x => x.DbPassword.Contains(request.SearchDto.DbPassword));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.ConnectionString))
                        items = items.Where(x => x.ConnectionString.Contains(request.SearchDto.ConnectionString));
                    
                    if (!request.SearchDto.Take.HasValue || request.SearchDto.Take <= 0)
                        request.SearchDto.Take = 10;

                    if (!request.SearchDto.Skip.HasValue || request.SearchDto.Skip < 0)
                        request.SearchDto.Skip = 0;

                    items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);
                }
            }

            return items.Adapt<List<ProjectDisplayDto>>();
        }
    }
}
