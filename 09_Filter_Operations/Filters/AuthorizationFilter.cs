using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _09_Filter_Operations.Filters
{
    /// <summary>
    /// Authorization Filter(Yetkilendirme Filtesi):
    /// ============================================================
    /// Filter Pipeline'da EN İLK çalışan filtredir.
    /// Kullanıcının bu Action'a erişim yetkisi olup olmadığını kontrol eder
    /// Yetkisiz ise isteği sonlandırır(short-circuit) ve Login sayfasına veya 403 Forbidden'a yönlendirir.
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // İstek URL'inde ?role=admin parametresi veya kimlik doğrulaması kontrol edilir.
            bool hasAdminParam = context.HttpContext.Request.Query["role"] == "admin";
            bool isAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;

            if(!hasAdminParam && !isAuthenticated)
            {
                // Yetkisiz Erişim: İstek Action metoduna HİÇ ULAŞMADAN kesilir. 
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    ContentType = "text/html; charset=utf-8",
                    Content = @"<div style='background: #0f172a;color:#f87171;padding:40px;font-family:sans-serif;text-align:center;'>
                    <h1 style='font-size:24px;'>403 - Yetkisiz Erişim(AuthorizationFilter Engelledi) </h1>
                    <p style='color:#94a3b8;'>Bu sayfaya sadece 'admin' rolüne sahip kullanıcılar erişebilir </p>
                    <p style='color:#e2e8f0;'>Test için URL sonuna <code>?role=admin</code> ekleyiniz</p>
                    <a href='/' style='color:#818cf8; display:inline-block;margin-top:20px'> ⬅️ Ana Sayfaya Dön</a>
                    </div>"
                };
            }

        }
    }
}
