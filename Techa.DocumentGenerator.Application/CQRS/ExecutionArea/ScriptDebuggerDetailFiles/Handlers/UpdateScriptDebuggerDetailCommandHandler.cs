using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Commands;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Handlers;

public class UpdateScriptDebuggerDetailCommandHandler(IBaseService<ScriptDebuggerDetail> service)
    : IRequestHandler<UpdateScriptDebuggerDetailCommand,
        HandlerResponse<ScriptDebuggerDetailDisplayDto>>
{
    public async Task<HandlerResponse<ScriptDebuggerDetailDisplayDto>> Handle(
        UpdateScriptDebuggerDetailCommand request, CancellationToken cancellationToken)
    {
        var obj = await service.GetByIdAsync(cancellationToken, request.ScriptDebuggerDetail.Id);

        if (obj == null)
            return new HandlerResponse<ScriptDebuggerDetailDisplayDto>(false, "رکورد مورد نظر یافت نشد", null);

        request.ScriptDebuggerDetail.Adapt(obj);
        var result = await service.UpdateAsync(obj, cancellationToken);
        return result.Adapt<ScriptDebuggerDetailDisplayDto>();
    }
}