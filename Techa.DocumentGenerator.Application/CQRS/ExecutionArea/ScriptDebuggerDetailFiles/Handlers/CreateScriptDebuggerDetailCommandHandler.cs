using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Commands;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Handlers;

public class CreateScriptDebuggerDetailCommandHandler(IBaseService<ScriptDebuggerDetail> service)
    : IRequestHandler<CreateScriptDebuggerDetailCommand, HandlerResponse<ScriptDebuggerDetailDisplayDto>>
{
    public async Task<HandlerResponse<ScriptDebuggerDetailDisplayDto>> Handle(CreateScriptDebuggerDetailCommand request, CancellationToken cancellationToken)
    {
        var scriptDebuggerDetail = request.ScriptDebuggerDetail.Adapt<ScriptDebuggerDetail>();

        var result = await service.AddAsync(scriptDebuggerDetail, cancellationToken);
        return result.Adapt<ScriptDebuggerDetailDisplayDto>();
    }
}