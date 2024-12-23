using MediatR;
using Techa.DocumentGenerator.Application.Dtos.AAA;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries
{
    public class GetUserByUsernameQuery : IRequest<HandlerResponse<UserDisplayDto>>
    {
        public string Username { get; }

        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
