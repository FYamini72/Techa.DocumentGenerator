using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Commands;

public class DeleteScriptDebuggerDetailCommand(int id) : IRequest<HandlerResponse<bool>>
{
    public int Id { get; } = id;
}