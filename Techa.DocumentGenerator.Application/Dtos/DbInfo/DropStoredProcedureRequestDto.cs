namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class DropStoredProcedureRequestDto
    {
        public int? ProjectId { get; set; }
        public string ProcedureName { get; set; }
    }
}