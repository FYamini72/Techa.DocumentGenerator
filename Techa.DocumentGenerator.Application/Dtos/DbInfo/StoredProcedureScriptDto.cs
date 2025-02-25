namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class StoredProcedureScriptDto : BaseDto
    {
        public int ProjectId { get; set; }
        public string? ProcedureName { get; set; }
        public string? ProcedureCode { get; set; }
    }
}