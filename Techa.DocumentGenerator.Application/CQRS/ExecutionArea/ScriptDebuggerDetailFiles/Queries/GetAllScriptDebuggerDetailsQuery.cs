using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Queries;

public class GetAllScriptDebuggerDetailsQuery(ScriptDebuggerDetailSearchDto? searchDto)
    : IRequest<HandlerResponse<List<ScriptDebuggerDetailDisplayDto>>>
{
    public ScriptDebuggerDetailSearchDto? SearchDto { get; } = searchDto;
}