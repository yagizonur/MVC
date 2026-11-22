namespace _07_State_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
           
            #region State Management Servis Yapılandırması
            // 1. IMemoryCache Servisi: Sunucu RAM'inde hızlı veri önbellekleme sağlar. Genellikle sık erişilen veriler için kullanılır.
            builder.Services.AddMemoryCache();

            // 2. Dağıtılmış Bellek Ön Belleği: Session verilerinin sunucu belleğinde tutulması için gereklidir. Bu, özellikle birden fazla sunucu arasında oturum paylaşımı yaparken önemlidir.
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                // Oturumun hareketsiz kalma süresi (Idle Timeout) ayarlanır. Bu süre boyunca kullanıcı herhangi bir işlem yapmazsa oturum sona erer.
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi 30 dakika olarak ayarlanır.

                // HttpOnly: Cookie'nin JavaScript(document.cookie) ile okunmasını engelleyerek XSS (Cross-Site Scripting) saldırılarına karşı güvenliği artırır.
                options.Cookie.HttpOnly = true;

                options.Cookie.IsEssential = true;// Cookie'nin kullanıcı tarafından reddedilse bile gerekli olduğunu belirtir. Bu, GDPR / KVVK gibi veri koruma yasalarına uyum için önemlidir.

                options.Cookie.Name = ".ModernMvc.Session";
            });
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
            // DİKKAT: Session Middleware'i, UseRouting ve UseEndpoints arasında çağırmak önemlidir. Bu, oturum verilerinin doğru şekilde yönetilmesini sağlar.
            app.UseSession(); // Session Middleware'i eklenir. Bu, oturum verilerinin yönetilmesini sağlar.
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
