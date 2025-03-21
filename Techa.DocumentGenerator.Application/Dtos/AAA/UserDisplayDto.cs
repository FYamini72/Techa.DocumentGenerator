namespace Techa.DocumentGenerator.Application.Dtos.AAA
{
    public class UserDisplayDto : BaseDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? ProjectId { get; set; }

        public List<UserRoleDisplayDto> UserRoles { get; set; }


        public int? ProfileId { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
