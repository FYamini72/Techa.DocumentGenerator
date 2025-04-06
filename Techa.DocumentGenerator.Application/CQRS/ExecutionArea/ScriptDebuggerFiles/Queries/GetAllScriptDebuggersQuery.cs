using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Queries;

public class GetAllScriptDebuggersQuery(ScriptDebuggerSearchDto? searchDto)
    : IRequest<HandlerResponse<List<ScriptDebuggerDisplayDto>>>
{
    public ScriptDebuggerSearchDto? SearchDto { get; } = searchDto;
}