using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Commands;

public class CreateScriptDebuggerCommand(ScriptDebuggerCreateDto scriptDebugger)
    : IRequest<HandlerResponse<ScriptDebuggerDisplayDto>>
{
    public ScriptDebuggerCreateDto ScriptDebugger { get; } = scriptDebugger;
}