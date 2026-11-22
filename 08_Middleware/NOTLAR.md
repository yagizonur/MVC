# 09 — Middleware (Ara Katman) ve İstek Boru Hattı

## Bu derste ne öğreniyoruz?
Her HTTP isteğinin Controller'a ulaşmadan önce ve cevap dönerken içinden geçtiği **boru hattını** (pipeline), sıralamanın neden önemli olduğunu ve kendi middleware'imizi yazmayı.

## Boru hattı
```
İstek  ─▶ [ExceptionHandler] ─▶ [HttpsRedirection] ─▶ [RequestTiming] ─▶ [CustomHeader] ─▶ [StaticFiles] ─▶ [Routing] ─▶ [Authorization] ─▶ Controller
Cevap ◀─        ◀─                    ◀─                  ◀─                 ◀─                ◀─              ◀─              ◀─
```
Her middleware üç şeyden birini yapar:
1. İsteği inceler/değiştirir ve `await _next(context)` ile **bir sonrakine** iletir.
2. Cevap dönerken (`_next`'ten sonra) iş yapar (süre ölçmek, log yazmak).
3. İsteği **kısa devre** yapıp (short-circuit) hiç ilerletmeden kendisi cevap verir (ör. StaticFiles bir CSS dosyası bulursa Controller'a gitmez).

## Projede neler var?
- `Middlewares/RequestTimingMiddleware.cs` → `Stopwatch` başlatır, `Response.OnStarting` ile cevap gönderilmeden hemen önce `X-Response-Time-Ms` başlığını ekler, sonra süreyi loglar.
- `Middlewares/CustomHeaderMiddleware.cs` → her cevaba `X-Powered-By-Course` ve `X-Server-Time` başlıkları ekler.
- `Program.cs` → `app.UseMiddleware<...>()` ile sıraya ekleme; sıralama yorumlarla açıklanmış.
- `HomeController.SimulateDelay` → bilerek 250 ms bekler; süre başlığında farkı görmek için.

## Neden `Response.OnStarting`?
Cevabın gövdesi yazılmaya başladıktan sonra **başlık eklenemez** ("Headers are read-only, response has already started" hatası). `OnStarting`, başlıklar gönderilmeden hemen önce çalışan bir geri çağırmadır (callback); başlık eklemek için güvenli yer burasıdır.

## Nasıl denenir?
1. Sayfayı açın → F12 → Network → ilk isteğe (document) tıklayın → **Response Headers**: `X-Response-Time-Ms`, `X-Powered-By-Course`, `X-Server-Time`.
2. `/Home/SimulateDelay` → süre 250 ms'nin üzerine çıkar.
3. Visual Studio'da Output penceresinde (veya terminalde) "HTTP GET / tamamlandı. Süre: ... ms" loglarını görün.

## Sıralama neden önemli?
| Yanlış sıra | Sonuç |
|---|---|
| `UseAuthorization` → `UseAuthentication` | Yetki, kullanıcı tanınmadan kontrol edilir; herkes yetkisiz sayılır |
| `UseExceptionHandler` en sonda | Önceki middleware'lerdeki hatalar yakalanmaz |
| `UseSession` route'tan sonra | Controller'da session kullanılamaz |
| Timing middleware `UseStaticFiles`'tan sonra | CSS/JS isteklerinin süresi ölçülmez (kısa devre) |

## Middleware mi Filter mı?
| Middleware (bu ders) | Filter (10. ders) |
|---|---|
| **Her** isteğe uygulanır (CSS, resim dahil) | Sadece Controller action'larına |
| MVC'den habersiz (`HttpContext` görür) | Action, model, ModelState gibi MVC bilgilerini görür |
| Global: loglama, hata yakalama, CORS, kimlik | Action'a özel: yetki, doğrulama, süre |

## Sık yapılan hatalar
- `await _next(context)`'i çağırmayı unutmak → istek orada kalır, sayfa boş döner.
- `_next`'ten sonra `Response.Headers.Append` yapmak → "response has already started" hatası.
- Middleware'e Scoped servis enjekte etmeye çalışmak (constructor'a) → middleware tek bir kez oluşturulur. Scoped servisi `InvokeAsync(HttpContext context, IMyScopedService s)` parametresiyle alın.

## Web API ve React'te karşılığı
- E-Ticaret API'deki `Middlewares/ExceptionMiddleware.cs` → beklenmeyen tüm hataları yakalayıp `{ message }` JSON'u döner. React'teki axios interceptor bu mesajı okur.
- `UseCors`, `UseAuthentication` (JWT doğrulama) birer middleware'dir.
- React tarafındaki benzeri: **axios interceptor** — her istek gitmeden ve her cevap component'e ulaşmadan önce araya giren kod (08_React/13).

## Mülakat soruları
1. Middleware nedir, `RequestDelegate _next` ne işe yarar?
2. `app.Use`, `app.Run` ve `app.Map` farkı nedir?
3. Middleware ile Filter arasındaki fark nedir?
4. `UseAuthentication` neden `UseAuthorization`'dan önce gelir?

## Alıştırma
Bir `MaintenanceMiddleware` yazın: `appsettings.json`'da `"BakimModu": true` ise tüm isteklere 503 ve "Sitemiz bakımda" mesajı dönsün (kısa devre). `/admin` ile başlayan adresler hariç tutulsun.
