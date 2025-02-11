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

    public class StoredProcedureRole : BaseEntity
    {
        public int StoredProcedureId { get; set; }
        [ForeignKey(nameof(StoredProcedureId))]
        public StoredProcedure StoredProcedure { get; set; }

        public int ProjectRoleId { get; set; }
        [ForeignKey(nameof(ProjectRoleId))]
        public ProjectRole ProjectRole { get; set; }
    }

    public class ProjectRole : BaseEntity
    {
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public int Title { get; set; }
    }
}
