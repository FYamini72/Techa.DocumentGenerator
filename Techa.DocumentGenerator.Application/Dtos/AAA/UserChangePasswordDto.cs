namespace Techa.DocumentGenerator.Application.Dtos.AAA
{
    public class UserChangePasswordDto : BaseDto
    {
        public int? ProjectId { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}