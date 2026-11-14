using _04_ModelExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _04_ModelExample.Controllers
{
    public class LoanController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // View ilk açıldığında boş bir model gönderiyoruz(Hata almamak için)
            return View(new LoanCalculationModel());
        }

        [HttpPost]
        public IActionResult Calculate(LoanCalculationModel model)
        {
            // Model kurallara uyuyor mu? (Amount(tutar) ve MonthTerm(Vade) doğru aralıkta mı?)
            if (ModelState.IsValid)
            {
                model.IsCalculated = true;
                return View("Index", model); // Hesaplanmış modeli View'e geri dön.
            }

            // Eğer validasyon başarısızca, hatalarla birlikte modeli geri gönder.
            return View("Index", model);
        }
    }
}
