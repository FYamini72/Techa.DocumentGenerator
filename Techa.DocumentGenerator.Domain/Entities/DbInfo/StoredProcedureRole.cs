using System.ComponentModel.DataAnnotations.Schema;
using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.Domain.Entities.DbInfo
{
    public class StoredProcedureRole : BaseEntity
    {
        public int StoredProcedureId { get; set; }
        [ForeignKey(nameof(StoredProcedureId))]
        public StoredProcedure StoredProcedure { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}
