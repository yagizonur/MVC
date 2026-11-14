# 03 — View'den Controller'a Veri Göndermek (Form)

## Bu derste ne öğreniyoruz?
Kullanıcının bir formda yazdıklarını sunucuya göndermek ve Controller'da yakalamak. Bu derste model kullanılmıyor; değerler doğrudan metot parametresi olarak alınıyor.

## Temel kavramlar
| Kavram | Açıklama |
|---|---|
| `[HttpGet]` | Sayfayı **gösteren** action (formu boş açar) |
| `[HttpPost]` | Formdan **gelen** veriyi işleyen action |
| `name` özniteliği | `<input name="userName">` → `SubmitForm(string userName)`. **Eşleşme `name` ile yapılır, `id` ile değil.** |
| `asp-controller` / `asp-action` | Tag Helper: formun `action="/Home/SubmitForm"` adresini üretir |
| `ViewBag` | ViewData'nın `dynamic` hali: `ViewBag.Message = "..."` |
| `return View("Index")` | Başka bir View'i göstermek (action adı farklı olsa da) |

## Akış
```
GET  /            → Index()            → boş form
POST /Home/SubmitForm (userName=Ali&userEmail=ali@x.com)
                  → SubmitForm(string userName, string userEmail)
                  → ViewBag.Message ayarlanır → View("Index") → aynı sayfa, mesajla
```

## Projede neler var?
- `HomeController.Index` (GET) ve `SubmitForm` (POST)
- `Views/Home/Index.cshtml` → form + `ViewBag.Message` / `ViewBag.Error` kutuları

## Nasıl denenir?
1. Formu doldurup gönderin → yeşil mesaj.
2. F12 → Network → isteğe tıklayın → **Payload** sekmesinde `userName=...&userEmail=...` (form-urlencoded) görünür.
3. Tarayıcının `required` kontrolünü F12 ile kaldırıp boş gönderin → sunucu tarafı kontrol çalışır, kırmızı mesaj gelir. **Ders:** Tarayıcı doğrulamasına asla güvenmeyin; asıl kontrol sunucuda yapılır.

## Sık yapılan hatalar
- Input'a `name` vermeyi unutmak → parametre `null` gelir.
- POST'tan sonra sayfayı yenileyince (F5) formun **tekrar gönderilmesi**. Çözüm: **PRG deseni** (Post-Redirect-Get): POST işlendikten sonra `RedirectToAction("Index")` dönülür (08_State_Management'ta TempData ile kullanılıyor).
- Parametre sayısı arttıkça (10 alan) metot imzası okunmaz hale gelir → model kullanın (04, 05).

## Web API ve React'te karşılığı
- React'te form gönderilince sayfa yenilenmez; veri **JSON** olarak gönderilir: `axios.post("/api/...", { userName, userEmail })`.
- API'de karşılayan: `[HttpPost] public IActionResult Kaydet(KullaniciDto model)` — `[ApiController]` gövdeyi otomatik okur (13_Web_API_Giris).
- `e.preventDefault()` (08_React/07_formlar) → tarayıcının bu dersteki "sayfayı yenileyerek gönderme" davranışını durdurur.

## Mülakat soruları
1. GET ve POST arasındaki farklar nelerdir? Hangisi idempotent'tir?
2. Form verisi action parametresine nasıl bağlanır?
3. PRG (Post-Redirect-Get) deseni nedir, hangi sorunu çözer?

## Alıştırma
Forma "Yaş" alanı ekleyin (`int yas`). 18'den küçükse hata mesajı gösterin. Ardından PRG desenini uygulayın: POST'ta `TempData["Message"]` ayarlayıp `RedirectToAction("Index")` dönün.
