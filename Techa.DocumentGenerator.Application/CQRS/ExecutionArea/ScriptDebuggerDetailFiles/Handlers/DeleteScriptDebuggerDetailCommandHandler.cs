using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Commands;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Handlers;

public class DeleteScriptDebuggerDetailCommandHandler(IBaseService<ScriptDebuggerDetail> service)
    : IRequestHandler<DeleteScriptDebuggerDetailCommand, HandlerResponse<bool>>
{
    public async Task<HandlerResponse<bool>> Handle(DeleteScriptDebuggerDetailCommand request, CancellationToken cancellationToken)
    {
        var obj = await service.GetByIdAsync(cancellationToken, request.Id);

        if (obj == null)
            return new HandlerResponse<bool>(false, "رکورد موردنظر یافت نشد", false);

        await service.DeleteAsync(obj, cancellationToken);
        return true;
    }
}