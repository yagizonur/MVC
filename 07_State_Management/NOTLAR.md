# 08 — Durum Yönetimi: Session, Cookie, Memory Cache (ve TempData)

## Bu derste ne öğreniyoruz?
HTTP **durumsuzdur** (stateless): sunucu her isteği ilk kez görüyormuş gibi karşılar ve "bu kullanıcı az önce ne yaptı" bilgisini hatırlamaz. Bu bilgiyi istekler arasında saklamanın yolları.

## Karşılaştırma
| | Nerede saklanır? | Kimin için? | Ömrü | Bu projede |
|---|---|---|---|---|
| **Session** | Sunucu belleği (tarayıcıda sadece oturum kimliği çerezi) | Tek kullanıcı | Hareketsizlikte 30 dk | `UserName` |
| **Cookie** | Kullanıcının tarayıcısı | Tek kullanıcı | `Expires` ile (7 gün) | `AppTheme` |
| **Memory Cache** | Sunucu belleği | **Tüm kullanıcılar** | Absolute 60 sn / Sliding 20 sn | `CachedData` |
| **TempData** | Session/Cookie üzerinde | Tek kullanıcı | **Sadece bir sonraki istek** | İşlem mesajları |

## Projede neler var?
- `Program.cs` → `AddMemoryCache()`, `AddDistributedMemoryCache()` + `AddSession(...)` ve pipeline'da `app.UseSession()`
- `HomeController` → her biri için Set/Clear action'ları; `Index` üçünü birden okuyup gösterir
- Her POST'tan sonra `RedirectToAction("Index")` → **PRG deseni** (F5 ile form tekrar gönderilmez); sonuç mesajı `TempData` ile taşınır

## Cookie güvenlik ayarları
| Ayar | Anlamı |
|---|---|
| `HttpOnly = true` | JavaScript (`document.cookie`) okuyamaz → XSS ile çalınamaz |
| `SameSite = Lax/Strict` | Başka sitelerden gelen isteklerde gönderilmez → CSRF koruması |
| `Secure = true` | Sadece HTTPS'te gönderilir |
| `IsEssential = true` | Çerez onayı reddedilse bile yazılır (zorunlu çerez) |

## Cache süreleri
- **Absolute (60 sn):** Ne olursa olsun 60 saniye sonra silinir.
- **Sliding (20 sn):** 20 saniye kimse okumazsa silinir; her okuma süreyi uzatır.
- İkisi birlikte: Sürekli okunsa bile en geç 60 saniyede silinir.

## Nasıl denenir?
1. Session'a isim yazın → F12 → Application → Cookies → `.ModernMvc.Session` çerezi oluşur (değer şifreli bir kimliktir, ismin kendisi değil!).
2. Tema cookie'si yazın → çerezlerde `AppTheme` düz değerle görünür. Tarayıcıyı kapatıp açın → hâlâ orada.
3. Cache'e veri yazın, **başka bir tarayıcıdan** (veya gizli pencereden) açın → cache verisi orada da görünür, session görünmez. Fark budur.
4. Uygulamayı yeniden başlatın → session ve cache silinir, cookie kalır.

## Sık yapılan hatalar
- `app.UseSession()`'ı eklemeyi unutmak veya `MapControllerRoute`'tan sonra yazmak → "Session has not been configured" hatası.
- Session'a büyük nesneler koymak → sunucu belleği şişer. Birden fazla sunucu varsa (load balancer) session paylaşılmaz → Redis gibi dağıtık cache gerekir.
- Cookie'ye hassas bilgi (şifre, kart no) yazmak → kullanıcının bilgisayarında açık durur.

## Web API ve React'te karşılığı
Bu ders, API dünyasına geçerken en çok değişen konudur:
- Web API'ler genellikle **durumsuz** tasarlanır; Session kullanılmaz.
- "Giriş yapmış kullanıcı" bilgisi çerez/session yerine **JWT token** ile taşınır; React her istekte `Authorization: Bearer ...` başlığı gönderir (08_React/13_axios_ve_jwt).
- Tarayıcı tarafında saklama: React'te `localStorage` (favoriler, sepet taslağı — 08_React/09 `useLocalStorage`, E-Ticaret `FavoritesContext`).
- `IMemoryCache` API'lerde de aynen kullanılır (ör. kategori listesini 5 dakika önbellekte tutmak).
- E-Ticaret'in eski MVC sürümü çerez, yeni API sürümü JWT kullanıyor; iki projeyi karşılaştırın.

## Mülakat soruları
1. HTTP neden durumsuzdur, durum nasıl korunur?
2. Session ile Cookie farkı nedir? Session da aslında cookie kullanır mı?
3. Absolute ve Sliding expiration farkı nedir?
4. Cookie tabanlı oturum ile JWT arasındaki farklar nelerdir?

## Alıştırma
Bir "Son görüntülenen ürünler" özelliği yazın: Her ürün sayfası açıldığında ürün Id'sini Session'daki listeye ekleyin (en fazla 5). `HttpContext.Session.SetString` + `JsonSerializer` ile listeyi saklayın.
