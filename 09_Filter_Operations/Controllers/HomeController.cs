using _09_Filter_Operations.Filters;
using _09_Filter_Operations.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _09_Filter_Operations.Controllers
{  /// <summary>
   /// Filtre Operasyonları Controller'ı:
   /// ============================================================
   /// FİLTRE TÜRLERİ VE ÇALIŞMA SIRASI(FILTER PIPELINE):
   /// 1. Authorization Filter(Yetkilendirme Kontrolü)
   /// 2. Resource Filter(Model Binding Öncesi/Sonrası Önbellek)
   /// 3. Action Filter(action öncesi ve sonrası)
   /// 4. Exception Filter(Hata Yakalama)
   /// 5. Result Filter (View/Json Öncesi ve Sonrası)
   /// 
   /// NEDEN SERVICEFILTER?
   ///  [ServiceFilter(typeof(MyFilter))]: Filtrenin Constructor'ında servis enjeksiyonu(DI) 
   ///  alabilmesi için IoC Container'dan çözülmesini sağlar.
   /// </summary>
    public class HomeController : Controller
    {
        // ActionFilter ile eylemin başlangıç ve bitiş zamanı yakalanır
        [ServiceFilter(typeof(ActionFilter))]
        public IActionResult Index()
        {
            ViewBag.ExecutingTime = HttpContext.Items["ActionExecutingTime"] ?? "Belirlenmedi";
            return View();
        }

        // ExceptionFilter ile kasıtlı olarak fırlatılan hata yakalanır ve Error View'ine aktarılır.
        [ServiceFilter(typeof(ExceptionFilter))]
       public IActionResult SpecialAction()
        {
            throw new InvalidOperationException("Bu hata ExceptionFilter tarafından başarıyla yakalandı ve sistem çökmeden Error sayfasına yönlendirildi.");
        }

        // AuthorizationFilter ile yetkilendirme kontrolü yapılır(?role?admin gerek)
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult AdminOnly()
        {
            return Content("Tebrikler! Yetkilendirme başarılı: Admin paneline erişim sağlandı");
        }

        // Standart sayfa hatası 
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = HttpContext.TraceIdentifier,
                ErrorMessage = "Bilinmeyen bir hata oluştu"
            });
        }
    }
}
