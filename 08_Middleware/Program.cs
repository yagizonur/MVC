using _08_Middleware.Middlewares;

namespace _08_Middleware
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            #region Middleware Pipeline(İstek İşleme Boru Hattı) Sıralaması
            // Middlweare Sıralaması Çok Önemlidir!! 
            // Her istek bu sırayla boru hattından geçer ve yanıt bu sırayla geri döner.
            // [İstek] ---> 1. ExceptionHandler
            //         ---> 2. Hsts(HTTP Strict Transport Security) bir web sitesi için güvenlik önlemi olarak tarayıcılara, yalnızca HTTPS üzerinden iletişim kurmalarını zorunlu kılar.
            //         ---> 3. HttpsRedirection
            //         ---> 4. Custom Timing & Header Middleware'lerimiz
            //         ---> 5. StaticFiles(Css, js, resimler)
            //         ---> 6. Routing(Hangi Controller/ Action?)
            //         ---> 7. Authentication & Authorization(Yetki kontrolü)
            //         ---> 8. Endpoints(Controller Action / Razor View) --> [Yanıt]
            #endregion
            // 1. Hata Yakalama Middlware'i: Hata oluştuğunda kullanıcıya özel bir hata sayfası gösterir.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // 2. HTTPS yönlendirme middleware'i: HTTP isteklerini HTTPS'ye yönlendirir.
            app.UseHttpsRedirection();

            // 3. Özel İstek Süresi Ölçüm ve Başlık Middleware'leri: Her isteğin süresini ölçer ve yanıt başlıklarına ekler.
            app.UseMiddleware<RequestTimingMiddleware>();

            // 4. Özel HTTP Başlık Middleware'i: Her yanıtın başlıklarına kurumsal güvenlik ve ortam bilgisi ekler.
            app.UseMiddleware<CustomHeaderMiddleware>();

            // 5. Statik Dosya Middleware'i: Css, js, resimler gibi statik dosyaları sunar. 
            app.UseStaticFiles();

            // 6. Rota Eşleme Middleware'i: Hangi controller ve action'ın çalışacağını belirler.
            app.UseRouting();

            // 7. Yetkilendirme Middleware'i: Kullanıcının yetkilerini kontrol eder.
            app.UseAuthorization();

            // 8. Endpoint Middleware'i: Controller action'larını çalıştırır ve yanıtı döner.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            // 9. Uygulamayı çalıştırır.
            app.Run();
        }
    }
}
