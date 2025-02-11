using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class GetAllStoredProceduresQueryHandler : IRequestHandler<GetAllStoredProceduresQuery, HandlerResponse<BaseGridDto<StoredProcedureDisplayDto>>>
    {
        private readonly IBaseService<StoredProcedure> _service;

        public GetAllStoredProceduresQueryHandler(IBaseService<StoredProcedure> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<BaseGridDto<StoredProcedureDisplayDto>>> Handle(GetAllStoredProceduresQuery request, CancellationToken cancellationToken)
        {
            var items = _service.GetAll();
            var totalCount = await items.CountAsync();

            if (request.SearchDto != null)
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

                    totalCount = await items.CountAsync();
                    items = items.Skip(request.SearchDto.Skip.Value).Take(request.SearchDto.Take.Value);
                }
            }

            var response = new BaseGridDto<StoredProcedureDisplayDto>()
            {
                Data = items.Adapt<List<StoredProcedureDisplayDto>>(),
                TotalCount = totalCount
            };
            return response;
        }
    }
}
