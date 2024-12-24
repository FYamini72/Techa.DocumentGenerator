using Techa.DocumentGenerator.Domain.Enums;
using Techa.DocumentGenerator.Application.Dtos;

namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class StoredProcedureParameterDisplayDto : BaseDto
    {
		public int StoredProcedureId { get; set; }
		public string? ParameterName { get; set; }
		public string? ParameterType { get; set; }
		public string? Title { get; set; }
		public string? Description { get; set; }
		public string? Alias { get; set; }
		public NullableOption NullableOption { get; set; }
		public string? DefaultValue { get; set; }
		public string? MinValue { get; set; }
		public string? MaxValue { get; set; }
		public int? MinLength { get; set; }
		public int? MaxLength { get; set; }

    }
}