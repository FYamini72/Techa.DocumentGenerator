using System.ComponentModel.DataAnnotations.Schema;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;

namespace Techa.DocumentGenerator.Domain.Entities.AAA
{
    //[Table(nameof(Role))]
    public class Role : BaseEntity
    {
        public string Title { get; set; }

        public int? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
    }
}
