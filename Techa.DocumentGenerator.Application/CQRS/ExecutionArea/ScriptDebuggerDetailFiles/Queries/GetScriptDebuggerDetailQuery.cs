using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Queries;

public class GetScriptDebuggerDetailQuery(int id) : IRequest<HandlerResponse<ScriptDebuggerDetailDisplayDto>>
{
    public int Id { get; } = id;
}