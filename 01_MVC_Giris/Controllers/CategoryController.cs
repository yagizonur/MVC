using Microsoft.AspNetCore.Mvc;

namespace _01_MVC_Giris.Controllers
{
    // CategoryController'ı oluşturduktan sonra uygulamayı çalıştırırsak. Ve Index'e 
    // debug koyduğumuzda, Browser(tarayıcı)  URL'de https:// localhost:7000/category/index yazarsak hata alırız.
    public class CategoryController : Controller
    {
        // Add View dedikten sonrra Views altında Category klasörü altında Index eklenmiş olacak. Bu yapıldıktan sonra erişim sağlanabilir.
        public IActionResult Index()
        {
            return View();
        }
    }
}
