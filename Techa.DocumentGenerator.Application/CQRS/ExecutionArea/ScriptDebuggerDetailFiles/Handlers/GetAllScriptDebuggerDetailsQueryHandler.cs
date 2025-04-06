using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Queries;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Handlers;

public class GetAllScriptDebuggerDetailsQueryHandler(IBaseService<ScriptDebuggerDetail> service)
    : IRequestHandler<GetAllScriptDebuggerDetailsQuery, HandlerResponse<List<ScriptDebuggerDetailDisplayDto>>>
{
    public async Task<HandlerResponse<List<ScriptDebuggerDetailDisplayDto>>> Handle(GetAllScriptDebuggerDetailsQuery request, CancellationToken cancellationToken)
    {
        var items = service.GetAll();

        if (request.SearchDto is not { GetAllItems: false })
            return items.Adapt<List<ScriptDebuggerDetailDisplayDto>>();
        if (request.SearchDto.ScriptDebuggerId.HasValue)
            items = items.Where(x => x.ScriptDebuggerId == request.SearchDto.ScriptDebuggerId.Value);

        if (request.SearchDto.Take is null or <= 0)
            request.SearchDto.Take = 10;

        if (request.SearchDto.Skip is null or < 0)
            request.SearchDto.Skip = 0;

        items = items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);

        return items.Adapt<List<ScriptDebuggerDetailDisplayDto>>();
    }
}