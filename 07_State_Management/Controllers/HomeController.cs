using _07_State_Management.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace _07_State_Management.Controllers
{
    /// <summary>
    /// Durum Yönetimi (State Management) Controller'ı;
    /// =====================================================================
    /// HTTP PROTOKOLÜ VE DURUMSUZLUK(STATELESS):
    /// HTTP protokolü durumsuzdur; yani istemciden gelen her istek sunucu için yepyeni bir istektir.
    /// İstemcinin önceki isteklerde ne yaptığını hatırlamak için 3 temel mekanizma kullanılırÇ
    /// 1: SESSION: Sunucu RAM'inde saklanır. Kullanıcı tarayıcıyı kapatınca veya süre dolunca silinir. Sunucu RAM'inde saklandığı için sunucu yeniden başlatılırsa kaybolur.
    /// 2. COOKIE: Kullanıcının tarayıcısında saklanır. Süresi bitene kadar(günler/aylar) kalıcı olabilir.
    /// 3. MEMORY CACHE: Sunucu RAM'inde tüm uygulama ve tüm kullanıcılar için saklanan yüksek hızlı önbellektir.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache; // Program.cs tarafında Services kısmına eklenmesi gerekir. Yoksa uygulama çalışınca hata verir.

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            // 1. Session verisini oku
            string? sessionUser = HttpContext.Session.GetString("UserName");

            // 2. Cookie verisini oku
            string? cookieTheme = Request.Cookies["AppTheme"];

            // 3. Memory cache verisini oku(TryGetValue ile güvenli kontrol)
            bool hasCache = _memoryCache.TryGetValue("CachedData", out string? cachedData);

            ViewBag.SessionUser = sessionUser;
            ViewBag.CookieTheme = cookieTheme;
            ViewBag.CachedData = hasCache ? cachedData : null;
            ViewBag.HasCache = hasCache;

            return View();
        }


        [HttpPost]
        public IActionResult SetSession(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                HttpContext.Session.SetString("UserName", userName);
                TempData["Message"] = $"Session verisi '{userName}' olarak ayarlandı. (Sunucu önbelleğinde saklanır)";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Remove("UserName");
            TempData["Message"] = "Session verisi temizlendi";
            return RedirectToAction("Index");
        }

       

        [HttpPost]
        public IActionResult SetCookie(string theme)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7), // 7 gün boyunca tarayıcıda sakla.
                HttpOnly = true, // Sadece HTTP üzerinden erişilebilir, JavaScript ile erişilemez(XSS koruması)
                IsEssential = true, // Çerez onaylaırndan bağımsız zorunlu çerez olarak işaretle.
                SameSite = SameSiteMode.Lax // CSRF saldırılarına karşı koruma sağlar.(Lax, Strict, None)
            };

            Response.Cookies.Append("AppTheme", theme, cookieOptions);
            TempData["Message"] = $"Cookie '{theme} değeriyle istemci tarayıcısına kaydedildi(7 Gün Geçerli)'";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCookie()
        {
            Response.Cookies.Delete("AppTheme");
            TempData["Message"] = "Cookie temizlendi";
            return RedirectToAction("Index");
        }

        // MEMORY CACHE METOTLARI(Bellek içi önbellek)
        public IActionResult SetCache(string cacheData)
        {
            if (!string.IsNullOrWhiteSpace(cacheData))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // 1. Absolute Expiration: Belirli bir süre sonra önbellekten otomatik olarak silinir.
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60))
                    // 2. Sliding Expiration: Belirli bir süre boyunca erişilmezse önbellekten otomatik olarak silinir.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));

                _memoryCache.Set("CachedData", cacheData, cacheEntryOptions);
                TempData["Message"] = $"Memory Cache verisi '{cacheData}' olarak ayarlandı. (60 sn Absolute, 20 sn Sliding)";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ClearCache()
        {
            _memoryCache.Remove("CachedData");
            TempData["Message"] = "Memory Cache verisi temizlendi.";
            return RedirectToAction("Index");
        }
    }
}
