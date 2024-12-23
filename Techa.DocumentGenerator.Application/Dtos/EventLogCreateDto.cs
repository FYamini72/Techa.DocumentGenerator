using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.Application.Dtos
{
    public class EventLogCreateDto : BaseDto
    {
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? DataJson { get; set; }
        public EventType EventType { get; set; }
        public string? ExecutedScript { get; set; }
        public string? Message { get; set; }
        public bool HasError { get; set; }
        public string? IPAddress { get; set; }
        public string? Url { get; set; }
        public string? Method { get; set; }
    }
}