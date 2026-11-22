// ==============================================================================
// 11_AutoMapper: Program.cs
// Bu projede Nesne-Nesne Eşleme (Object-to-Object Mapping), DTO deseni ve
// AutoMapper kütüphanesi entegrasyonu incelenmektedir.
// ==============================================================================

using _10_AutoMapper.MappingProfile;
var builder = WebApplication.CreateBuilder(args);

// MVC Servisleri
builder.Services.AddControllersWithViews();

#region AUTOMAPPER SERVIS KAYDI
// Modern AutoMapper (v13+) Kurallarına Göre Servis Kaydı:
// İlk parametre boş bir konfigürasyon aksiyonu (cfg => { }) alır.
// İkinci parametre ise taranacak profil tiplerini (Marker Type) kabul eder.
builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile));

//builder.Services.AddAutoMapper(typeof(UserProfile), typeof(ExternalIntegrationProfile)); // v13 öncesi bu şekilde eklenmesi gerekiyor.
#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Başlangıç rotası olarak UserController / Index ayarlandı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();
