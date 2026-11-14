using Microsoft.AspNetCore.Mvc;

namespace _02_ControllerToView.Controllers
{
    /// <summary>
    /// IActionResult => Geriye View döndüreceğini söyler.
    /// </summary>
    /// <returns>View döner</returns>
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<string> { "Ürün 1", "Ürün 2", "Ürün 3" };

            // Veriyi data ile yollama
            ViewData["products"] = products;
            return View();
        }

        /// <summary>
        /// Belirli bir ürünün detaylarını göster. IActionResult
        /// </summary>
        /// <param name="id">Verilen ID'ye göre, o Id ilişkili ürünü gösterir.</param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            var product = $"Ürün {id} Detayları";
            ViewData["productDetail"] = product;
            return View();
        }
    }
}
