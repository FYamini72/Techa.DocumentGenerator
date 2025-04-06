using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Commands;

public class UpdateScriptDebuggerDetailCommand(ScriptDebuggerDetailCreateDto scriptDebuggerDetail)
    : IRequest<HandlerResponse<ScriptDebuggerDetailDisplayDto>>
{
    public ScriptDebuggerDetailCreateDto ScriptDebuggerDetail { get; } = scriptDebuggerDetail;
}