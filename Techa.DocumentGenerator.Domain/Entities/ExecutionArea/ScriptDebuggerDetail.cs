using System.ComponentModel.DataAnnotations.Schema;

namespace Techa.DocumentGenerator.Domain.Entities.ExecutionArea
{
    public class ScriptDebuggerDetail : BaseEntity
    {
        public int ScriptDebuggerId { get; set; }
        [ForeignKey(nameof(ScriptDebuggerId))]
        public ScriptDebugger ScriptDebugger { get; set; }

        public string ResultValue { get; set; }
    }
}
