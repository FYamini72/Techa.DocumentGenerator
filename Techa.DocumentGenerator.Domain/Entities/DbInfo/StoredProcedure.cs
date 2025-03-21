using System.ComponentModel.DataAnnotations.Schema;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.Domain.Entities.DbInfo
{
    public class StoredProcedure : BaseEntity
    {
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public string ProcedureName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public StoredProcedureType StoredProcedureType { get; set; }

        public ICollection<StoredProcedureParameter> StoredProcedureParameters { get; set; }
    }
}
