using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureParameterFiles.Handlers
{
    public class GetAllStoredProcedureParametersQueryHandler : IRequestHandler<GetAllStoredProcedureParametersQuery, HandlerResponse<List<StoredProcedureParameterDisplayDto>>>
    {
        private readonly IBaseService<StoredProcedureParameter> _service;

        public GetAllStoredProcedureParametersQueryHandler(IBaseService<StoredProcedureParameter> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<List<StoredProcedureParameterDisplayDto>>> Handle(GetAllStoredProcedureParametersQuery request, CancellationToken cancellationToken)
        {
            var items = _service.GetAll();

            if(request.SearchDto != null)
            {
                if (!request.SearchDto.GetAllItems)
                {
                    if (request.SearchDto.StoredProcedureId.HasValue)
                        items = items.Where(x => x.StoredProcedureId == request.SearchDto.StoredProcedureId);

                    if (!string.IsNullOrEmpty(request.SearchDto.ParameterName))
                        items = items.Where(x => x.ParameterName.Contains(request.SearchDto.ParameterName));

                    if (!string.IsNullOrEmpty(request.SearchDto.ParameterType))
                        items = items.Where(x => x.ParameterType.Contains(request.SearchDto.ParameterType));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.Title))
                        items = items.Where(x => x.Title.Contains(request.SearchDto.Title));

                    if (!string.IsNullOrEmpty(request.SearchDto.Description))
                        items = items.Where(x => x.Description.Contains(request.SearchDto.Description));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.Alias))
                        items = items.Where(x => x.Alias.Contains(request.SearchDto.Alias));

                    if (request.SearchDto.NullableOption.HasValue)
                        items = items.Where(x => x.NullableOption == request.SearchDto.NullableOption.Value);

                    if (!string.IsNullOrEmpty(request.SearchDto.DefaultValue))
                        items = items.Where(x => x.DefaultValue.Contains(request.SearchDto.DefaultValue));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.MinValue))
                        items = items.Where(x => x.MinValue.Contains(request.SearchDto.MinValue));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.MaxValue))
                        items = items.Where(x => x.MaxValue.Contains(request.SearchDto.MaxValue));

                    if (request.SearchDto.MinLength.HasValue)
                        items = items.Where(x => x.MinLength == request.SearchDto.MinLength.Value);

                    if (request.SearchDto.MaxLength.HasValue)
                        items = items.Where(x => x.MaxLength == request.SearchDto.MaxLength.Value);

                    if (!request.SearchDto.Take.HasValue || request.SearchDto.Take <= 0)
                        request.SearchDto.Take = 10;

                    if (!request.SearchDto.Skip.HasValue || request.SearchDto.Skip < 0)
                        request.SearchDto.Skip = 0;

                    items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);
                }
            }

            return items.Adapt<List<StoredProcedureParameterDisplayDto>>();
        }
    }
}
