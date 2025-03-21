using Microsoft.AspNetCore.Http;

namespace Techa.DocumentGenerator.Application.Dtos.AAA
{
    public class UserUpdateDto : BaseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public int? ProjectId { get; set; }

        public IFormFile? SelectedFile { get; set; }
    }
}