using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Queries;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebugger;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerFiles.Handlers;

public class GetAllScriptDebuggersQueryHandler(IBaseService<ScriptDebugger> service)
    : IRequestHandler<GetAllScriptDebuggersQuery, HandlerResponse<List<ScriptDebuggerDisplayDto>>>
{
    public async Task<HandlerResponse<List<ScriptDebuggerDisplayDto>>> Handle(GetAllScriptDebuggersQuery request, CancellationToken cancellationToken)
    {
        var items = service.GetAll();

        if (request.SearchDto is not { GetAllItems: false }) return items.Adapt<List<ScriptDebuggerDisplayDto>>();
        if (request.SearchDto.UserId.HasValue)
            items = items.Where(x => x.UserId == request.SearchDto.UserId.Value);

        if (request.SearchDto.Take is null or <= 0)
            request.SearchDto.Take = 10;

        if (request.SearchDto.Skip is null or < 0)
            request.SearchDto.Skip = 0;

        items = items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);

        return items.Adapt<List<ScriptDebuggerDisplayDto>>();
    }
}