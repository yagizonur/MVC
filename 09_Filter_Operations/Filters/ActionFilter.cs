using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace _09_Filter_Operations.Filters
{
    /// <summary>
    /// Action Filter(Eylem Filtresi):
    /// ================================================
    /// AOP(Aspect Oriented Programming) prensibine göre Action metotların çalışmasından 
    /// HEMEN ÖNCE ve HEMEN SONRA araya girerek mantık işletir
    /// 1. OnActionExecuting: Action metodu çağrılmadan hemen önce tetiklenir. 
    ///    - Gelen parametreleri denetleme(Model State validasyon kontrolü)
    ///    - Süre ölçümü başlatma (stopwatch)
    ///    - Loglama yapma
    /// 2. OnActionExecuted: Action metodu çalışıp döndükten hemen sonra tetiklenir. 
    ///     - Süreyi durdurma
    ///     - Üretilen ActionResult'ı (View/Json) manipüle etme veya loglama
    /// </summary>
    public class ActionFilter : IActionFilter
    {
        private Stopwatch _stopwatch;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();

            context.HttpContext.Items["ActionExecutingTime"] = DateTime.Now.ToString("HH:mm:ss.fff");
            Debug.WriteLine($"[ActionFilter] OnActionExecuting: {context.ActionDescriptor.DisplayName} başladı");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch?.Stop();

            var elapsedMs = _stopwatch?.ElapsedMilliseconds ?? 0;

            // Süre bilgisini Controller veya View'in okuyabilmesi için Items içine yazıyoruz.
            context.HttpContext.Items["ActionDurationMs"] = elapsedMs;
            Debug.WriteLine($"[ActionFilter] OnActionExecuted: {context.ActionDescriptor.DisplayName} tamamlandı. ({elapsedMs} ms)");

        }

       
    }
}
