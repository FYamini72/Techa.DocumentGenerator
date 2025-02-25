using MediatR;
using Newtonsoft.Json;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Techa.DocumentGenerator.Application.Services.Interfaces.Ai;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Queries;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Domain.Entities.DbInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.Application.CQRS.DbInfo.StoredProcedureFiles.Handlers
{
    public class GenerateStoredProcedureParametersUsingAiQueryHandler : IRequestHandler<GenerateStoredProcedureParametersUsingAiQuery, HandlerResponse>
    {
        private readonly IBaseService<StoredProcedure> _service;
        private readonly IBaseService<StoredProcedureParameter> _storedProcedureParameterService;
        private readonly IAdoService _adoService;
        private readonly IAvalAiService _avalAiService;

        public GenerateStoredProcedureParametersUsingAiQueryHandler(IBaseService<StoredProcedure> service,
            IBaseService<StoredProcedureParameter> storedProcedureParameterService,
            IAdoService adoService,
            IAvalAiService avalAiService)
        {
            _service = service;
            _storedProcedureParameterService = storedProcedureParameterService;
            _adoService = adoService;
            _avalAiService = avalAiService;
        }

        public async Task<HandlerResponse> Handle(GenerateStoredProcedureParametersUsingAiQuery request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);
            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد");

            var queryResult = await _adoService.GetProcedureInfoAsync(obj.ProjectId, obj.ProcedureName, cancellationToken);
            if (queryResult.HasError)
                return new(false, queryResult.Messages);
            var procedures = JsonConvert.DeserializeObject<List<StoredProcedureInfoDto>>(queryResult.Dataset);
            if (procedures == null || !procedures.Any())
                return new(false, "پروسیجر مربوطه یافت نشد");
            var procedure = procedures.First();
            var messages = new List<ChatMessage>()
            {
                ChatMessage.FromUser(procedure.ProcedureCode),
                ChatMessage.FromUser("Analyze the stored procedure and generate a list of its parameters in the following JSON format. Only provide the JSON output without any additional text.\r\n{\r\n\"ParameterName\": \"\",\r\n\"ParameterDataType\": \"\",\r\n\"IsRequired\": true,\r\n\"DefaultValue\": \"\",\r\n\"IsOutParameter\": false\r\n}")
            };
            var strParameters = await _avalAiService.CreateCompletionAsync(messages, Models.Gpt_4o_mini);
            var parameters = JsonConvert.DeserializeObject<List<StoredProcedureParameterInfoDto>>(strParameters.RemoveSpecialCharacters());
            if (parameters != null && parameters.Any())
            {
                var oldParameters = await _storedProcedureParameterService
                    .GetAll(x => x.StoredProcedureId == request.Id)
                    .ToListAsync();
                var newStoredProcedureParameters = new List<StoredProcedureParameter>();
                var oldStoredProcedureParameters = new List<StoredProcedureParameter>();
                foreach (var parameter in parameters)
                {
                    if (parameter == null || string.IsNullOrEmpty(parameter.ParameterName))
                        continue;
                    //if (parameter.ParameterName.ToLower() == "res" || parameter.ParameterName.ToLower() == "@res" )
                    //    continue;

                    parameter.ParameterName = parameter.ParameterName.StartsWith("@") ? parameter.ParameterName : $"@{parameter.ParameterName}";

                    var oldParameter = oldParameters.FirstOrDefault(x => x.ParameterName.ToLower() == parameter.ParameterName.ToLower());
                    if (oldParameter == null)
                    {
                        newStoredProcedureParameters.Add(new StoredProcedureParameter()
                        {
                            StoredProcedureId = request.Id,
                            DefaultValue = parameter.DefaultValue,
                            NullableOption = parameter.IsRequired ? Domain.Enums.NullableOption.Required : Domain.Enums.NullableOption.Nullable,
                            ParameterName = parameter.ParameterName,
                            ParameterType = parameter.ParameterDataType,
                            IsOutParameter = parameter.IsOutParameter
                        });
                    }
                    else
                    {
                        oldParameter.DefaultValue = parameter.DefaultValue;
                        oldParameter.ParameterType = parameter.ParameterDataType;
                        oldParameter.NullableOption = parameter.IsRequired
                            ? Domain.Enums.NullableOption.Required
                            : Domain.Enums.NullableOption.Nullable;
                        oldParameter.IsOutParameter = parameter.IsOutParameter;
                        oldStoredProcedureParameters.Add(oldParameter);
                    }
                }

                var removedStoredProcedureParameters = oldParameters.Where(x => /*x.ParameterName.ToLower() != "@res" && */!parameters.Any(z => z.ParameterName == x.ParameterName)).ToList();

                await _storedProcedureParameterService.DeleteRangeAsync(removedStoredProcedureParameters, cancellationToken);
                await _storedProcedureParameterService.AddRangeAsync(newStoredProcedureParameters, cancellationToken);
                await _storedProcedureParameterService.UpdateRangeAsync(oldStoredProcedureParameters, cancellationToken);

            }
            return new(true, "عملیات با موفقیت انجام شد");
        }
    }
}