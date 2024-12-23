namespace Techa.DocumentGenerator.Application.Dtos.AAA
{
    public class UserSearchDto : BaseSearchDto
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }
        public int? RoleId { get; set; }
    }
}
