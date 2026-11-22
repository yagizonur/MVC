using _08_Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _08_Middleware.Controllers
{
    public class HomeController : Controller
    {
       public IActionResult Index()
        {
            ViewBag.RequestPath = HttpContext.Request.Path; // İdtek yolu
            ViewBag.RequestMethod = HttpContext.Request.Method; // İstek metodu(GET, POST vb.)
            ViewBag.UserAgent = Request.Headers.ToString(); // Kullanıcı ajanı bilgisi
            ViewBag.IsDelay = false; // Varsayılan olarak gecikme yok
            return View();
        }

        // 250ms kasıtlı gecikme oluşturarak RequestTimingMiddleware'in çalışmasını test etmek için bir action metodu yazdık.
        public async Task<IActionResult> SimulateDelay()
        {
            await Task.Delay(250); // 250ms gecikme


            ViewBag.RequestPath = HttpContext.Request.Path; // İstek yolu
            ViewBag.RequestMethod = HttpContext.Request.Method; // İstek metodu (GET, POST, vb.)
            ViewBag.UserAgent = Request.Headers.UserAgent.ToString(); // Kullanıcı ajanı bilgisi
            ViewBag.IsDelay = true; // Gecikme olduğunu belirtmek için true

            return View("Index");
        }
    }
}
