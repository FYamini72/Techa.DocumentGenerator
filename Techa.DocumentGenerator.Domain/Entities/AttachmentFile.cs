namespace Techa.DocumentGenerator.Domain.Entities
{
    public class AttachmentFile : BaseEntity
    {
        public string FileName { get; set; }
        public long Size { get; set; }
    }
}
