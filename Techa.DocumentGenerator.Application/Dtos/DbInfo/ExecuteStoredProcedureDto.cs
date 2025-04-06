using Newtonsoft.Json;
using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class ExecuteStoredProcedureRequestDto
    {
        public ExecuteStoredProcedureRequestDto()
        {
            this.ProcedureName = "";
            this.Parameters = new();
            this.LinesToDebug = new();
        }

        public int? ProjectId { get; set; }

        public bool? HasDataTable { get; set; }
        //[JsonIgnore]
        public string ProcedureName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public List<LineToDebugDto> LinesToDebug { get; set; }
    }

    public class StoredProcedureSummeryResponseDto
    {
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public ExecuteStoredProcedureRequestDto InputData { get; set; }
    }

    public class LineToDebugDto
    {
        public string Script { get; set; } = string.Empty;
        public ScriptType ScriptType { get; set; }
    }
}