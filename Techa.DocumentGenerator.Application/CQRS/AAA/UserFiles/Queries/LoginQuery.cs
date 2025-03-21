using MediatR;
using Techa.DocumentGenerator.Application.Dtos.AAA;

namespace Techa.DocumentGenerator.Application.CQRS.AAA.UserFiles.Queries
{
    public class LoginQuery : IRequest<HandlerResponse<UserAndTokenDisplayDto>>
    {
        public string UserName { get; }
        public string Password { get; }
        public int? ProjectId { get; }

        public LoginQuery(string userName, string password, int? projectId)
        {
            UserName = userName;
            Password = password;
            ProjectId = projectId;
        }
    }
}
