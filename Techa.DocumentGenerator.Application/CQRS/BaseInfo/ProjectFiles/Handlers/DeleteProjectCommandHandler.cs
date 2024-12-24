using Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Commands;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Application.Services.Interfaces;
using MediatR;

namespace Techa.DocumentGenerator.Application.CQRS.BaseInfo.ProjectFiles.Handlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, HandlerResponse<bool>>
    {
        private readonly IBaseService<Project> _service;

        public DeleteProjectCommandHandler(IBaseService<Project> service)
        {
            _service = service;
        }

        public async Task<HandlerResponse<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var obj = await _service.GetByIdAsync(cancellationToken, request.Id);

            if (obj == null)
                return new(false, "رکورد موردنظر یافت نشد", false);

            await _service.DeleteAsync(obj, cancellationToken);
            return true;
        }
    }
}
