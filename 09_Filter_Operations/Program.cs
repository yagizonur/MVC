using _09_Filter_Operations.Filters;

namespace _09_Filter_Operations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // [ServiceFilter(typeof(MyFilter))] özniteliğini(attributes) kullanabilmek için,
            // filtre sınıflarının IoC Container'a kaydedilmesi ZORUNLUDUR!
            builder.Services.AddScoped<ActionFilter>();
            builder.Services.AddScoped<ExceptionFilter>();
            builder.Services.AddScoped<AuthorizationFilter>();

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
