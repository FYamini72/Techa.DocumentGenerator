using Newtonsoft.Json;

namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class ExecuteStoredProcedureRequestDto
    {
        public ExecuteStoredProcedureRequestDto()
        {
            this.ProcedureName = "";
            this.Parameters = new();
        }

        public int ProjectId { get; set; }

        public bool? HasDataTable { get; set; }
        //[JsonIgnore]
        public string ProcedureName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }

    public class StoredProcedureSummeryResponseDto
    {
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public ExecuteStoredProcedureRequestDto InputData { get; set; }
    }
}