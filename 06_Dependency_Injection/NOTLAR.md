# 07 — Dependency Injection (Bağımlılık Enjeksiyonu)

## Bu derste ne öğreniyoruz?
Bir sınıfın ihtiyaç duyduğu nesneleri kendisi `new` ile üretmek yerine **dışarıdan hazır almasını** (DI), servisleri `Program.cs`'te kaydetmeyi ve üç yaşam döngüsünün (Transient, Scoped, Singleton) farkını.

## Problem: Sıkı bağlılık
```csharp
public class HomeController : Controller
{
    private readonly MyService _service = new MyService();   // ✗ Somut sınıfa yapıştık
}
```
- Yarın `MyService` yerine veritabanından okuyan `EfStudentService` yazarsak Controller'ı değiştirmemiz gerekir.
- Test ederken sahte (mock) servis veremeyiz.

## Çözüm: Arayüz + DI
```csharp
// Program.cs → "IMyService istenirse MyService ver"
builder.Services.AddScoped<IMyService, MyService>();

// Controller → sadece arayüzü bilir
public HomeController(IMyService myService) { _myService = myService; }
```
Servisi değiştirmek için sadece `Program.cs`'teki tek satır değişir (Loose Coupling, SOLID'in **D**'si: Dependency Inversion).

## Yaşam döngüleri
| | Ne zaman yeni nesne? | Tipik kullanım |
|---|---|---|
| `AddTransient` | **Her istendiğinde** | Hafif, durumsuz yardımcılar |
| `AddScoped` | **Her HTTP isteğinde bir kez** | EF Core `DbContext`, iş servisleri |
| `AddSingleton` | **Uygulama boyunca bir kez** | Önbellek, yapılandırma, sayaçlar |

## Projede neler var?
- `Services/Abstract` → `IMyService`, `ITransientService`, `IScopedService`, `ISingletonService`
- `Services/Concrete/LifeTimeServices.cs` → her servis oluşturulurken bir `Guid` üretir; Guid'e bakarak "aynı nesne mi?" anlaşılır
- `HomeController` → her servisi **iki kez** alır: constructor'dan ve action parametresinden (`[FromServices]`)

## Nasıl denenir? (Dersin kalbi)
Sayfayı açın ve Guid'leri karşılaştırın, sonra **F5** ile yenileyin:
| | Aynı istekte 1. ve 2. | Sayfa yenilenince |
|---|---|---|
| Transient | **Farklı** | Farklı |
| Scoped | **Aynı** | **Değişir** |
| Singleton | Aynı | **Değişmez** (uygulama kapanana kadar) |

## Sık yapılan hatalar
- Servisi kaydetmeyi unutmak → `Unable to resolve service for type 'IMyService'`.
- **Captive dependency:** Singleton bir servisin içine Scoped servis (ör. DbContext) enjekte etmek → DbContext uygulama boyunca yaşar; veri karışır, bellek şişer. ASP.NET Core geliştirme ortamında bunu hata olarak gösterir. Kural: **Uzun ömürlü, kısa ömürlüyü tutamaz.**
- `[FromServices]`'i her yerde kullanmak → bağımlılıklar gizlenir. Varsayılan tercih constructor injection olmalı.

## Web API ve React'te karşılığı
- E-Ticaret API'nin `Program.cs`'i baştan sona DI kayıtlarıdır: `AddScoped<IProductService, ProductService>()`, `AddScoped<IProductDal, ProductDal>()`...
- 13_Web_API_Giris'te kitap deposu `AddSingleton` → bellek içi liste istekler arasında korunsun diye.
- React'te bunun benzeri **Context**'tir: `AuthProvider` en dışta "kaydedilir", `useAuth()` ile isteyen component'e "enjekte" edilir (08_React/11).

## Mülakat soruları
1. Transient, Scoped ve Singleton farkını örnekle açıklayın.
2. DbContext neden Scoped kaydedilir?
3. Captive dependency nedir?
4. DI'ın test edilebilirliğe katkısı nedir?

## Alıştırma
`IMyService`'in ikinci bir uygulamasını yazın: `ReverseStudentService` öğrencileri ters sırada döndürsün. Controller'a dokunmadan sadece `Program.cs`'i değiştirerek ekranın değiştiğini görün.
