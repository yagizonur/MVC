using _06_Dependency_Injection.Models;
using _06_Dependency_Injection.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _06_Dependency_Injection.Controllers
{
    /// <summary>
    ///  Dependency Injection ve Servis Yaşam Döngüsü Controller'ı;
    ///  Constructor Injection ve Action Injection([FromServices]) yöntemlerini birlikte kullanarak,
    ///  Transient, Scoped ve Singleton farkını tek HTTP isteği içerisinde görsel olarak tanımlar.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IMyService _myService;

        private readonly ITransientService _transientService1;

        private readonly IScopedService _scopedService1;

        private readonly ISingletonService _singletonService1;

        // Constructor Injection(Yapıcı Metot ile Servis Alma - En yaygın ve tavsiye edilen yöntem)
        public HomeController(IMyService myService, ITransientService transientService1, IScopedService scopedService1, ISingletonService singletonService1)
        {
            _myService = myService;
            _transientService1 = transientService1;
            _scopedService1 = scopedService1;
            _singletonService1 = singletonService1;
        }

        
        public IActionResult Index(
            [FromServices] ITransientService transientService2,
            [FromServices] IScopedService scopedService2,
            [FromServices] ISingletonService singletonService2)
        {
            // 1.Transient Karşılaştırması(Aynı istekte 2 kez istendi)
            ViewBag.Transient1 = _transientService1.GetOperationId();
            ViewBag.Transient2 = transientService2.GetOperationId();
            ViewBag.TransientSame = _transientService1.GetOperationId() == transientService2.GetOperationId(); // False

            // 2. Scoped Karşılaştırması(Aynı istekte 2 kez istendi)
            ViewBag.Scoped1 = _scopedService1.GetOperationId();
            ViewBag.Scoped2 = scopedService2.GetOperationId();
            ViewBag.ScopedName = _scopedService1.GetOperationId() == scopedService2.GetOperationId(); // True(İstek yenilenirse değişir)

            ViewBag.Singleton1 = _singletonService1.GetOperationId();
            ViewBag.Singleton2 = singletonService2.GetOperationId();
            ViewBag.SingletonSame = _singletonService1.GetOperationId() == singletonService2.GetOperationId(); // True(Sayfa yenilense de ASLA değişmez.)

            // Öğrenci listesini de model olarak View'e aktaralım.
            List<Student> students = _myService.GetStudents();
            return View(students);


        }
    }
}
