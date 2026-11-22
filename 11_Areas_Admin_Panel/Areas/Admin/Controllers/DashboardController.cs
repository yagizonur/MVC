using Microsoft.AspNetCore.Mvc;

namespace _11_Areas_Admin_Panel.Areas.Admin.Controllers
{

    /// <summary>
    /// Admin Dashboard Controller'ı: 
    /// ====================================================
    /// [Area("Admin)] Attribute
    /// Bu öznitelik zorunludur! Controller'ın "Admin" alanına ait olduğunu MVC'ye bildirir.
    /// URL Deseni: /Admin/Dashboard/Index
    /// </summary>
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TotalSales = "367.944₺";
            ViewBag.ActiveUsers = "1.364";
            ViewBag.TotalOrders = "8656";
            ViewBag.PendingTickets = "154";

            return View();
        }
    }
}
