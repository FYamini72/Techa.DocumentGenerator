using Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Queries;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using MediatR;
using Newtonsoft.Json;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Handlers
{
    public class GenerateStoredProceduresQueryHandler : IRequestHandler<GenerateStoredProceduresQuery, HandlerResponse>
    {
        private readonly IBaseService<Project> _service;
        private readonly IAdoService _adoService;
        private readonly IBaseService<StoredProcedure> _storedProcedureService;

        public GenerateStoredProceduresQueryHandler(IBaseService<Project> service, 
            IAdoService adoService, 
            IBaseService<StoredProcedure> storedProcedureService)
        {
            _service = service;
            _adoService = adoService;
            _storedProcedureService = storedProcedureService;
        }

        public async Task<HandlerResponse> Handle(GenerateStoredProceduresQuery request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);
            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد");

            var queryResult = await _adoService.GetAllProceduresInfoAsync(request.Id, cancellationToken);
            if (queryResult.HasError)
                return new(false, queryResult.Messages);

            var procedures = JsonConvert.DeserializeObject<List<StoredProcedureInfoDto>>(queryResult.Dataset);
            if(procedures != null && procedures.Any())
            {
                var storedProcedures = new List<StoredProcedure>();
                foreach (var procedure in procedures)
                {
                    StoredProcedureType type = StoredProcedureType.NotSet;
                    if (procedure.ProcedureName.ToLower().Contains("_delete"))
                        type = StoredProcedureType.Delete;
                    else if (procedure.ProcedureName.ToLower().Contains("_insert") || procedure.ProcedureName.ToLower().Contains("_update"))
                        type = StoredProcedureType.CreateOrUpdate;
                    else if (procedure.ProcedureName.ToLower().Contains("_get_"))
                        type = StoredProcedureType.Read;

                    storedProcedures.Add(new StoredProcedure()
                    {
                        ProjectId = request.Id,
                        ProcedureName = procedure.ProcedureName,
                        StoredProcedureType = type,
                        StoredProcedureParameters = new List<StoredProcedureParameter>()
                    });
                }
                    
                await _storedProcedureService.AddRangeAsync(storedProcedures, cancellationToken);
            }

            return new(true, "عملیات با موفقیت انجام شد");
        }
    }
}