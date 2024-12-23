namespace Techa.DocumentGenerator.Application.Dtos
{
    public class BaseSearchDto
    {
        public BaseSearchDto()
        {
            this.GetAllItems = false;
        }

        public int? Id { get; set; }

        public int? Take { get; set; }
        public int? Skip { get; set; }

        public bool GetAllItems { get; set; }
    }
}