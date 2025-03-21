using Techa.DocumentGenerator.Application.Dtos.AAA;
using Mapster;
using MediatR;
using Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries;
using Techa.DocumentGenerator.Application.Services.Interfaces.AAA;
using Microsoft.EntityFrameworkCore;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, HandlerResponse<List<UserDisplayDto>>>
    {
        private readonly IUserService _userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<HandlerResponse<List<UserDisplayDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var items = _userService.GetAll()
                .Include(x => x.Profile)
                .Include(x=>x.UserRoles)
                    .ThenInclude(x=>x.Role)
                .AsQueryable();

            if (request.SearchDto != null)
            {
                if (!request.SearchDto.GetAllItems)
                {
                    if (!string.IsNullOrEmpty(request.SearchDto.UserName))
                        items = items.Where(x => x.UserName.Contains(request.SearchDto.UserName));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.FirstName))
                        items = items.Where(x => x.FirstName.Contains(request.SearchDto.FirstName));
                    
                    if (!string.IsNullOrEmpty(request.SearchDto.LastName))
                        items = items.Where(x => x.LastName.Contains(request.SearchDto.LastName));

                    if (request.SearchDto.ProjectId.HasValue)
                        items = items.Where(x => x.ProjectId == request.SearchDto.ProjectId.Value);
                    
                    if (request.SearchDto.RoleId.HasValue)
                        items = items.Where(x => x.UserRoles.Any(x => x.RoleId == request.SearchDto.RoleId));

                    if (!request.SearchDto.Take.HasValue || request.SearchDto.Take <= 0)
                        request.SearchDto.Take = 10;

                    if (!request.SearchDto.Skip.HasValue || request.SearchDto.Skip < 0)
                        request.SearchDto.Skip = 0;

                    items.Take(request.SearchDto.Take.Value).Skip(request.SearchDto.Skip.Value);
                }
            }

            return items.Adapt<List<UserDisplayDto>>();
        }
    }
}
