using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class GetAllStoredProceduresQueryHandler : IRequestHandler<GetAllStoredProceduresQuery, HandlerResponse<List<StoredProcedureDisplayDto>>>
    {
        private readonly IBaseService<StoredProcedure> _service;

        public GetAllStoredProceduresQueryHandler(IBaseService<StoredProcedure> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<List<StoredProcedureDisplayDto>>> Handle(GetAllStoredProceduresQuery request, CancellationToken cancellationToken)
        {
            var items = _service.GetAll();

            if(request.SearchDto != null)
            {
                if (!request.SearchDto.GetAllItems)
                {
                    if (request.SearchDto.ProjectId.HasValue)
                        items = items.Where(x => x.ProjectId == request.SearchDto.ProjectId);

                    if (!string.IsNullOrEmpty(request.SearchDto.Title))
                        items = items.Where(x => x.Title.Contains(request.SearchDto.Title));

                    if (!string.IsNullOrEmpty(request.SearchDto.Description))
                        items = items.Where(x => x.Description.Contains(request.SearchDto.Description));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.ProcedureName))
                        items = items.Where(x => x.ProcedureName.Contains(request.SearchDto.ProcedureName));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.Alias))
                        items = items.Where(x => x.Alias.Contains(request.SearchDto.Alias));
                    
                    if (request.SearchDto.StoredProcedureType.HasValue)
                        items = items.Where(x => x.StoredProcedureType == request.SearchDto.StoredProcedureType);

                    if (!request.SearchDto.Take.HasValue || request.SearchDto.Take <= 0)
                        request.SearchDto.Take = 10;

                    if (!request.SearchDto.Skip.HasValue || request.SearchDto.Skip < 0)
                        request.SearchDto.Skip = 0;

                    items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);
                }
            }

            return items.Adapt<List<StoredProcedureDisplayDto>>();
        }
    }
}
