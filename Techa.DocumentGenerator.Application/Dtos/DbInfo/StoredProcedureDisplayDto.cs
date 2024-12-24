using Techa.DocumentGenerator.Domain.Enums;
using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class StoredProcedureDisplayDto : BaseDto
    {
		public int ProjectId { get; set; }
		public string? ProcedureName { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Alias { get; set; }
		public StoredProcedureType StoredProcedureType { get; set; }

    }
}