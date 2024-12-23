using Techa.DocumentGenerator.Domain.Entities.DbInfo;

namespace Techa.DocumentGenerator.Domain.Entities.BaseInfo
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string DbName { get; set; }
        public string DbServerName { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string? ConnectionString { get; set; }

        public ICollection<StoredProcedure> StoredProcedures { get; set; }
    }
}
