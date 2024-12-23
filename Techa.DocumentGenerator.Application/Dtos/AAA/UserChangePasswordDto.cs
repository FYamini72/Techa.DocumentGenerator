namespace Techa.DocumentGenerator.Application.Dtos.AAA
{
    public class UserChangePasswordDto : BaseDto
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}