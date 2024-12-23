using Techa.DocumentGenerator.Domain.Enums;

namespace Techa.DocumentGenerator.Domain.Entities
{
    public class EventLog : BaseEntity
    {
        public string? EntityName { get; set; }
        /// <summary>
        /// چون ممکنه دربرگیرنده شناسه چندین رکورد باشد
        /// 1, 2, 3, 4
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// این رکورد در کوئری های ثبت، ویرایش و حذف با مقدار نتیجه پر خواهد شد
        /// </summary>
        public string? DataJson { get; set; }

        public EventType EventType { get; set; }

        /// <summary>
        /// این پراپرتی برای کوئری های اجرا شده در محیط های برخط می باشد و شامل دستورات اجرا شده کاربر است
        /// </summary>
        public string? ExecutedScript { get; set; }

        /// <summary>
        /// اگر خطایی در زمان اجرای درخواست رخ دهد در این فیلد ذخیره می شود
        /// این فیلد همچنین میتواند شامل خطا های سیستم هم باشد
        /// </summary>
        public string? Message { get; set; }

        public bool HasError { get; set; }

        public string? IPAddress { get; set; }
        public string? Url { get; set; }
        public string? Method { get; set; }
    }
}
