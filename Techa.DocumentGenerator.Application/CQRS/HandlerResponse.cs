namespace Techa.DocumentGenerator.Application.CQRS
{
    public class HandlerResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public HandlerResponse(bool status, string message)
        {
            Status = status;
            Message = message;
        }
    }

    public class HandlerResponse<TData> : HandlerResponse
    {
        public TData Data { get; set; }

        public HandlerResponse(bool status, string message, TData data)
            : base(status, message)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public static implicit operator HandlerResponse<TData>(TData data)
        {
            return new HandlerResponse<TData>(true, "عملیات با موفقیت انجام شد", data);
        }
    }
}
