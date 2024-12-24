using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.Application.Dtos.BaseInfo
{
    public class ProjectDisplayDto : BaseDto
    {
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? DbName { get; set; }
		public string? DbServerName { get; set; }
		public string? DbUserName { get; set; }
		public string? DbPassword { get; set; }
		public string? ConnectionString { get; set; }

    }
}