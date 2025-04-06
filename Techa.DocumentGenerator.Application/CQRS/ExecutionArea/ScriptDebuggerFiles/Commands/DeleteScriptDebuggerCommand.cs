using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Commands;

public class DeleteScriptDebuggerCommand(int id) : IRequest<HandlerResponse<bool>>
{
    public int Id { get; } = id;
}