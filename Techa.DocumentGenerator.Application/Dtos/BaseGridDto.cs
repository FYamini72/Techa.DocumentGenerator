namespace Techa.DocumentGenerator.Application.Dtos
{
    public class BaseGridDto<TDisplayDto>
    {
        public BaseGridDto()
        {
            this.Data = new();
        }
        public List<TDisplayDto> Data { get; set; }
        public int TotalCount { get; set; }
    }
}