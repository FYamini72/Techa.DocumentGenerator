using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Commands;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Handlers;

public class DeleteScriptDebuggerCommandHandler(IBaseService<ScriptDebugger> service)
    : IRequestHandler<DeleteScriptDebuggerCommand, HandlerResponse<bool>>
{
    public async Task<HandlerResponse<bool>> Handle(DeleteScriptDebuggerCommand request, CancellationToken cancellationToken)
    {
        var obj = await service.GetByIdAsync(cancellationToken, request.Id);

        if (obj == null)
            return new HandlerResponse<bool>(false, "رکورد موردنظر یافت نشد", false);

        await service.DeleteAsync(obj, cancellationToken);
        return true;
    }
}