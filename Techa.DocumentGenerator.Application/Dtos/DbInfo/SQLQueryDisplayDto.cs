namespace Techa.DocumentGenerator.Application.Dtos.DbInfo
{
    public class SQLQueryDisplayDto
    {
        public SQLQueryDisplayDto()
        {
            this.HasError = false;
        }

        public bool HasError { get; set; }
        public string Messages { get; set; }
        public string Script { get; set; }

        public string Dataset { get; set; }

        public bool HasDataTable
        {
            get
            {
                if (Dataset == null)
                    return false;
                else
                    return true;

            }
        }
    }
}