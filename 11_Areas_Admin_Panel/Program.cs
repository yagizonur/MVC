namespace _11_Areas_Admin_Panel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
            #region AREA & DEFAULT ROUTING YAPILANDIRMASI
            // ==============================================================================
            // 1. AREA ROTASI (ÖNCELİKLİDİR):
            // - {area:exists} kısıtlaması (constraint), yalnızca üzerinde [Area("...")] özniteliği
            //   bulunan Controller'ların bu rota ile eşleşmesini garanti eder.
            // - Örnek URL: /Admin/Dashboard/Index veya /Admin/Product/Index
            // ==============================================================================
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            // 2. VARSAYILAN KULLANICI ARAYÜZÜ ROTASI:
            // - Area belirtilmeyen standart controller'lar için çalışır.
            // - Örnek URL: / veya /Home/Index
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            #endregion

            app.Run();
        }
    }
}
