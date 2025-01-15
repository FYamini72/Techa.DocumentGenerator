using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace Techa.DocumentGenerator.Application.Services.Interfaces.Ai
{
    public interface IAvalAiService
    {
        /// <summary>
        /// به روش آسنکرون، یک درخواست به مدل هوش مصنوعی مشخص شده ارسال می‌کند
        /// </summary>
        /// <param name="messages">لیست پیام های ارسالی</param>
        /// <param name="model">نام مدل هوش مصنوعی</param>
        /// <returns>درصورت اجرای درخواست بصورت موفق پاسخ مربوطه و یا متن خطا برگردانده می‌شود</returns>
        Task<string> CreateCompletionAsync(List<ChatMessage> messages, string? model = "gpt-4o-mini");
    }
}
