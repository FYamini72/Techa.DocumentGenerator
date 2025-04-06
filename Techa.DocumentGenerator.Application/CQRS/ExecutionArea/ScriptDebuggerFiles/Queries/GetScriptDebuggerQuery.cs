using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Queries;

public class GetScriptDebuggerQuery(int id) : IRequest<HandlerResponse<ScriptDebuggerDisplayDto>>
{
    public int Id { get; } = id;
}