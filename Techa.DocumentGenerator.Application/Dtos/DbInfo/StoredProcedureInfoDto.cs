namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class StoredProcedureInfoDto
    {
        public string? ProcedureName { get; set; }
        public string? ProcedureCode { get; set; }
    }

    public class StoredProcedureParameterInfoDto
    {
        public string? ParameterName { get; set; }
        public string? ParameterDataType { get; set; }
        public bool IsRequired { get; set; }
        public string? DefaultValue { get; set; }
        public bool IsOutParameter { get; set; }
    }
}