using _06_Dependency_Injection.Services.Abstract;
using _06_Dependency_Injection.Services.Concrete;

namespace _06_Dependency_Injection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            #region DEPENDENCY INJECTION & SERVIS YASAM DONGULERI (SERVICE LIFETIMES)
            // ==============================================================================
            // 1. AddTransient (Geçici):
            //    - Ne zaman: Her talep edildiğinde (Constructor enjeksiyonunda) YEPYENİ bir örnek oluşturulur.
            //    - Kullanım: Hafif, durum bilgisi tutmayan (stateless) yardımcı servisler için idealdir.
            //
            // 2. AddScoped (Kapsamlı):
            //    - Ne zaman: Her HTTP İsteği (Request) başına BİR KEZ üretilir.
            //    - Aynı HTTP isteği içinde 10 farklı yerde çağrılsa bile AYNI örnek kullanılır.
            //    - Farklı bir HTTP isteği geldiğinde yeni bir nesne oluşturulur.
            //    - Kullanım: Entity Framework Core DbContext, Unit of Work, kullanıcıya özel servisler.
            //
            // 3. AddSingleton (Tekil):
            //    - Ne zaman: Uygulama ayağa kalktığında İLK çağrıda BİR KEZ üretilir.
            //    - Uygulama kapanana kadar tüm kullanıcılar ve tüm HTTP istekleri boyunca TEK nesne paylaşılır.
            //    - Kullanım: Bellek içi önbellek (Memory Cache), loglayıcılar, sayaçlar, konfigürasyon yöneticileri.
            // ==============================================================================

            // Yaşam Döngüsü Demo Servisleri
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

            // Öğrenci İş Mantığı Servisi
            builder.Services.AddScoped<IMyService, MyService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
