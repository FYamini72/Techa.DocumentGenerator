using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Commands;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Handlers;

public class CreateScriptDebuggerCommandHandler(IBaseService<ScriptDebugger> service)
    : IRequestHandler<CreateScriptDebuggerCommand, HandlerResponse<ScriptDebuggerDisplayDto>>
{
    public async Task<HandlerResponse<ScriptDebuggerDisplayDto>> Handle(CreateScriptDebuggerCommand request, CancellationToken cancellationToken)
    {
        var scriptDebugger = request.ScriptDebugger.Adapt<ScriptDebugger>();

        var result = await service.AddAsync(scriptDebugger, cancellationToken);
        return result.Adapt<ScriptDebuggerDisplayDto>();
    }
}