using _05_Model_Binding.Models;
using _05_Model_Binding.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _05_Model_Binding.Controllers
{
    // Model Binding ve ViewModel Mimarisi
    // Model Binding Nedir? 
    // İstemciden(tarayıcı/mobil/API) gelen HTTP isteğindeki ham metin verilerini(Form girdileri, URL sorgu parametreleri, Rota değişkenleri, HTTP Başlıkları)
    // otomatik olarak C# nesnelerine ve metot parametrelerine eşleyen ve tür dönüşümünü (string -> int, DateTime vb.) gerçekleştiren ASP.NET Core alt sistemidir.

    // VERİ KAYNAKLARI(Varsayılan):
    // 1. Form Değerleri (Form Values - POST)
    // 2. Rota Değerleri (Route Values - Home/Index/5)
    // 3. Sorgu Dizesi(Query String - ?ad=Onur&yas=32)
    // 4. Yüklenen Dosyalar (Uploaded Files - IFormFile)
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Sayfa ilk açıldığında gösterilmek üzere örnek bir nesne oluşturup view'e gönderiyoruz
            Kisi defaultKisi = new Kisi()
            {
                Ad = "Onur",
                Soyad = "Yağız",
                Yas = 32
            };

            return View(defaultKisi);
        }

        // POST: /Home/Index
        // HttpPost: Yalnızca POST isteklerini kabul eder.
        // [Bind("Ad","Soyad","Yas")]: Overposting(Mass Assigment) saldırılarına karşı 
        // sadece izin verilen property'lerin bind edilmesini sağlar(Whitelist Güvenliği)
        [HttpPost]
        public IActionResult Index([Bind("Ad","Soyad","Yas")] Kisi kisi)
        {
            ViewBag.IsPostBack = true;
            ViewBag.Message = "Form verileri Model Binding ile C# nesnesine başarıyla dönüştürüldü";
            return View(kisi);
        }

        [HttpGet]
        public IActionResult HomePage2()
        {

            
            var viewModel = new HomePageViewModel
            {
                KisiNesnesi = new Kisi
                {
                    Ad = "Buse",
                    Soyad = "Bayrak",
                    Yas = 20
                },
               
                
                AdresNesnesi = new Adres
                {
                    AdresTanim = "Atatürk Cd. No: 23",
                    Sehir = "Ankara"
                },
                BilgiMesaji = "Bu veriler Controller tarafında HomePageViewModel içinde hazırlandı."
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult HomePage2(HomePageViewModel model)
        {
            // Karmaşık nesnelerde form alanları "KisiNesnesi.Ad" ve "AdresNesnesi.Sehir" şeklinde
            // gönderildiğinde Model Binding iç içe geçmiş(nested) nesneleri de kusursuz bağlar.
            ViewBag.IsPostBack = true;
            ViewBag.Message = "ViewModel içindeki hem Kisi hem Adres nesnesi başarıyla bind edildi.";
            return View(model);
        }

        public IActionResult BindingSources(
            [FromRoute] int? id,
            [FromQuery] string? q,
            [FromHeader(Name = "User-Agent")] string? userAgent)
        {
            ViewBag.RouteID = id ?? 0;
            ViewBag.SearchQuery = string.IsNullOrWhiteSpace(q) ? "Parametre Gönderilmedi(Örn: ?q=mvc)" : q;
            ViewBag.UserAgent = userAgent ?? "Bilinmiyor";
            return View();
              
        }
    }
}
