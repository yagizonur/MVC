using Microsoft.AspNetCore.Mvc;

namespace _11_Areas_Admin_Panel.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin ürün yönetimi controller'ı:
    /// URL: /Admin/Product/Index
    /// </summary>
    [Area("Admin")]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
