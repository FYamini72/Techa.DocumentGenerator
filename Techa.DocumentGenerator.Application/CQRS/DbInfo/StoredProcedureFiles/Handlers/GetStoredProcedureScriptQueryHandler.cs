using MediatR;
using Newtonsoft.Json;
using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class GetStoredProcedureScriptQueryHandler : IRequestHandler<GetStoredProcedureScriptQuery, HandlerResponse<StoredProcedureScriptDto>>
    {
        private readonly IBaseService<StoredProcedure> _service;
        private readonly IAdoService _adoService;

        public GetStoredProcedureScriptQueryHandler(IBaseService<StoredProcedure> service, IAdoService adoService)
        {
            _service = service;
            _adoService = adoService;
        }

        public async Task<HandlerResponse<StoredProcedureScriptDto>> Handle(GetStoredProcedureScriptQuery request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);
            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", null);
            var queryResult = await _adoService.GetProcedureInfoAsync(obj.ProjectId, obj.ProcedureName, cancellationToken);
            if (queryResult.HasError)
                return new(false, queryResult.Messages, null);
            var procedures = JsonConvert.DeserializeObject<List<StoredProcedureScriptDto>>(queryResult.Dataset);
            if (procedures == null || !procedures.Any())
                return new(false, "پروسیجر مربوطه یافت نشد", null);
            var procedure = procedures.First();
            procedure.Id = obj.Id;
            procedure.ProjectId = obj.ProjectId;
            if(procedure.ProcedureCode.ToLower().StartsWith("CREATE PROC".ToLower()))
            {
                procedure.ProcedureCode = procedure.ProcedureCode.Replace("CREATE PROC", "ALTER PROC");
            }
            return procedure;
        }
    }
}