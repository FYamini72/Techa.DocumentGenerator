using MediatR;
using Techa.DocumentGenerator.Application.Dtos.AAA;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries
{
    public class GetAllUsersQuery : IRequest<HandlerResponse<List<UserDisplayDto>>>
    {
        public UserSearchDto? SearchDto { get; }

        public GetAllUsersQuery(UserSearchDto? searchDto)
        {
            this.SearchDto = searchDto;
        }
    }
}
