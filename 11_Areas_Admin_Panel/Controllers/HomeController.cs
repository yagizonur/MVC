using _11_Areas_Admin_Panel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _11_Areas_Admin_Panel.Controllers
{
    /// <summary>
    /// Genel Kullanıcı Arayüzü(GUI) 
    /// ============================================
    /// Bu Controller herhangi bir Area içinde yer almaz.
    /// Standart web ziyaretçilerinin ana sayfasıdır
    /// Buradan asp-area="Admin" asp-controller="Dashboard" asp-action="Index"
    /// bağlantısı verilerek Admin Area'sına geçiş yapılır
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    
    }
}
