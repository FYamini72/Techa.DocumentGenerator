using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Commands;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Handlers;

public class UpdateScriptDebuggerCommandHandler(IBaseService<ScriptDebugger> service)
    : IRequestHandler<UpdateScriptDebuggerCommand, HandlerResponse<ScriptDebuggerDisplayDto>>
{
    public async Task<HandlerResponse<ScriptDebuggerDisplayDto>> Handle(UpdateScriptDebuggerCommand request, CancellationToken cancellationToken)
    {
        var obj = await service.GetByIdAsync(cancellationToken, request.ScriptDebugger.Id);

        if (obj == null)
            return new HandlerResponse<ScriptDebuggerDisplayDto>(false, "رکورد مورد نظر یافت نشد", null);

        request.ScriptDebugger.Adapt(obj);
        var result = await service.UpdateAsync(obj, cancellationToken);
        return result.Adapt<ScriptDebuggerDisplayDto>();
    }
}