using _09_Filter_Operations.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace _09_Filter_Operations.Filters
{
    /// <summary>
    /// Exception Filter (Özel Durum / Hata Filtresi).
    /// ==============================================================
    /// Action metodu veya Action Filter çalışırken yakalanmamış bir Exception(Hata)
    /// fırlatıldığında bu filtre otomatik devreye girer.
    /// 
    /// 1. context.ExceptionHandled = true:
    ///     Bu özellik true yapılmazsa hata pipeline boyunca yukarı fırlamaya devam eder 
    ///     ve tarayıcıya çirkin bir 500 hata sayfası veya sarı ekran düşer.
    ///     true yapıldığında ASP.NET Core hatanın ele alındığını anlar
    /// 2. context.Result:
    ///     Kullanıcıya gösterilecek özel hata View'i veya JSON yanıtı burada belirlenir.
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Kullanıcı dostu hata modelimizi hazırladık.
            var errorModel = new ErrorViewModel
            {
                RequestId = context.HttpContext.TraceIdentifier,
                ErrorMessage = context.Exception.Message,
                Timestamp = DateTime.Now
            };

            // Hata sayfasını doğrudan döndürdük
            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                {
                    Model = errorModel
                }
            };

            // Hatanın başarıyla yakalandığını ve işlendiğini belirtiyoruz.
            context.ExceptionHandled = true;
        }
    }
}
