using System.ComponentModel.DataAnnotations.Schema;
using Techa.DocumentGenerator.Domain.Entities.AAA;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;

namespace Techa.DocumentGenerator.Domain.Entities.ExecutionArea
{
    public class ScriptDebugger : BaseEntity
    {
        public string Script { get; set; }

        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public int? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }

        public ICollection<ScriptDebuggerDetail> ScriptDebuggerDetails { get; set; }
    }
}
