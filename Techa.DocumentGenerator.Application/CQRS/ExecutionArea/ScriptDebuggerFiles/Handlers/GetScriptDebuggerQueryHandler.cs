using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Queries;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Handlers;

public class GetScriptDebuggerQueryHandler(IBaseService<ScriptDebugger> service)
    : IRequestHandler<GetScriptDebuggerQuery, HandlerResponse<ScriptDebuggerDisplayDto>>
{
    public async Task<HandlerResponse<ScriptDebuggerDisplayDto>> Handle(GetScriptDebuggerQuery request,
        CancellationToken cancellationToken)
    {
        var obj = await service.GetByIdAsync(cancellationToken, request.Id);

        return obj == null
            ? new HandlerResponse<ScriptDebuggerDisplayDto>(false, "رکورد موردنظر یافت نشد", null)
            : obj.Adapt<ScriptDebuggerDisplayDto>();
    }
}