# 10 — Filtreler (Filters)

## Bu derste ne öğreniyoruz?
Controller action'ları çalışmadan **önce** ve **sonra** araya girip ortak işleri (yetki kontrolü, süre ölçümü, hata yakalama) her action'a kopyalamadan yapmayı. Bu yaklaşımın adı AOP'dir (Aspect Oriented Programming).

## Filtre türleri ve çalışma sırası
```
İstek ─▶ 1. Authorization  ─▶ 2. Resource ─▶ [Model Binding] ─▶ 3. Action (önce) ─▶ ACTION ─▶ 3. Action (sonra) ─▶ 5. Result ─▶ Cevap
                                                                          │
                                                               hata olursa ▼
                                                                  4. Exception
```
| Filtre | Arayüz | Ne zaman? | Örnek kullanım |
|---|---|---|---|
| Authorization | `IAuthorizationFilter` | **İlk** | Giriş/rol kontrolü; yetkisizse kısa devre |
| Resource | `IResourceFilter` | Model binding'den önce/sonra | Önbellekten cevap dönmek |
| Action | `IActionFilter` | Action'dan hemen önce/sonra | Loglama, süre, parametre kontrolü |
| Exception | `IExceptionFilter` | Yakalanmamış hata olunca | Kullanıcı dostu hata sayfası |
| Result | `IResultFilter` | View/JSON üretilmeden önce/sonra | Cevaba başlık ekleme |

## Projede neler var?
- `Filters/AuthorizationFilter.cs` → `?role=admin` yoksa `context.Result` atayarak isteği **action'a hiç ulaşmadan** keser (403).
- `Filters/ActionFilter.cs` → `OnActionExecuting` / `OnActionExecuted` ile süreyi ölçer, `HttpContext.Items`'a yazar.
- `Filters/ExceptionFilter.cs` → hatayı yakalar, `Error` View'ini gösterir, `context.ExceptionHandled = true` ile hatanın işlendiğini bildirir.
- `HomeController` → her action'a `[ServiceFilter(typeof(...))]` ile farklı filtre uygulanmış.
- `Program.cs` → `AddScoped<ActionFilter>()` vb. ServiceFilter için DI kaydı **zorunlu**.

## Nasıl denenir?
- `/` → ActionFilter'ın yazdığı başlangıç zamanı ekranda
- `/Home/AdminOnly` → 403 sayfası; `/Home/AdminOnly?role=admin` → erişim başarılı
- `/Home/SpecialAction` → bilerek fırlatılan hata, sarı ekran yerine özel Error sayfasında
- Visual Studio Output → `[ActionFilter] OnActionExecuting...` satırları

## Filtre uygulama yolları
| Yol | Kapsam | Not |
|---|---|---|
| `[MyFilter]` (Attribute'tan türeyen) | Action / Controller | Constructor'a servis alamaz |
| `[ServiceFilter(typeof(MyFilter))]` | Action / Controller | DI'dan çözülür → servis alabilir; DI kaydı gerekir |
| `[TypeFilter(typeof(MyFilter))]` | Action / Controller | DI kaydı gerekmez, yine servis alabilir |
| `options.Filters.Add<MyFilter>()` | **Global** (tüm action'lar) | `AddControllersWithViews(o => ...)` içinde |

## Sık yapılan hatalar
- ServiceFilter kullanıp filtreyi DI'a kaydetmemek → "No service for type 'ActionFilter' has been registered".
- ExceptionFilter'da `ExceptionHandled = true` yapmamak → hata yukarı fırlamaya devam eder.
- Exception filtresinin her hatayı yakaladığını sanmak → sadece MVC pipeline'ı içindeki (action, filtreler) hataları yakalar. Middleware'deki hatalar için global exception middleware gerekir (09).
- Yetkiyi query string ile kontrol etmek (bu projedeki `?role=admin`) **sadece eğitim içindir**; gerçekte rol bilgisi giriş yapan kullanıcının kimliğinden (cookie/JWT) okunur.

## Web API ve React'te karşılığı
- `[Authorize]` ve `[Authorize(Roles = "admin")]` → ASP.NET Core'un hazır **authorization filtresi**. E-Ticaret API'de `AdminProductsController` bununla korunuyor.
- `[ApiController]`'ın "geçersiz modelde otomatik 400" davranışı aslında bir action filtresidir (`ModelStateInvalidFilter`).
- React'teki `ProtectedRoute` aynı işi arayüzde yapar ama **asıl koruma her zaman sunucudaki filtredir**.

## Mülakat soruları
1. Filtre türlerini çalışma sırasıyla sayın.
2. `ServiceFilter` ile `TypeFilter` farkı nedir?
3. Exception Filter ile Exception Middleware farkı nedir?
4. Bir filtrede isteği nasıl kısa devre yaparsınız?

## Alıştırma
Bir `ResultFilter` yazın (`IResultFilter`): Tüm cevaplara `X-Action-Duration-Ms` başlığını eklesin (süreyi ActionFilter'ın `HttpContext.Items["ActionDurationMs"]` değerinden okuyun). Global olarak kaydedin.
