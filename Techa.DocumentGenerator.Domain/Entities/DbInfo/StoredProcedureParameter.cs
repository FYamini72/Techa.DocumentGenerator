using System.ComponentModel.DataAnnotations.Schema;
using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.Domain.Entities.DbInfo
{
    public class StoredProcedureParameter : BaseEntity
    {
        public int StoredProcedureId { get; set; }
        [ForeignKey(nameof(StoredProcedureId))]
        public StoredProcedure StoredProcedure { get; set; }

        public string ParameterName { get; set; }
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