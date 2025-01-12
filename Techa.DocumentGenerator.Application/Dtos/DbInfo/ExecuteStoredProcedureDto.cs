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
        public string ProcedureName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}