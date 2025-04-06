using Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Queries;
using Techa.DocumentGenerator.Domain.Entities.ExecutionArea;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.Dtos.ExecutionArea.ScriptDebuggerDetail;

namespace Techa.DocumentGenerator.Application.CQRS.ExecutionArea.ScriptDebuggerDetailFiles.Handlers;

public class GetScriptDebuggerDetailQueryHandler(IBaseService<ScriptDebuggerDetail> service)
    : IRequestHandler<GetScriptDebuggerDetailQuery, HandlerResponse<ScriptDebuggerDetailDisplayDto>>
{
    public async Task<HandlerResponse<ScriptDebuggerDetailDisplayDto>> Handle(GetScriptDebuggerDetailQuery request,
        CancellationToken cancellationToken)
    {
        var obj = await service.GetByIdAsync(cancellationToken, request.Id);

        return obj == null
            ? new HandlerResponse<ScriptDebuggerDetailDisplayDto>(false, "رکورد موردنظر یافت نشد", null)
            : obj.Adapt<ScriptDebuggerDetailDisplayDto>();
    }
}