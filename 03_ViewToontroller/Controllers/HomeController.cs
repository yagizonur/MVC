using _03_ViewToontroller.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _03_ViewToontroller.Controllers
{
    public class HomeController : Controller
    {
        // Boş formu ekrana getiren GET metodu
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        // Model kullanmadan veriyi parametre olarak yakalayan POST metodu
        [HttpPost]
        public IActionResult SubmitForm(string userName, string userEmail)
        {
           if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userEmail))
            {
                ViewBag.Message = $"Model kullanmadan alındı! Hoş geldin, {userName}";
            }

            else
            {
                ViewBag.Error = "Lütfen tüm alanları doldurun";
            }

            return View("Index");
        }
    }
}
