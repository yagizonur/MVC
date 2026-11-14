# 04 — Model ve Güçlü Tipli (Strongly-Typed) View

## Bu derste ne öğreniyoruz?
Verileri `ViewData`/`ViewBag` yerine bir **sınıf** (model) ile taşımak, doğrulama kurallarını modelin üzerine yazmak ve hesaplamayı modelin kendisine yaptırmak. Örnek: kredi hesaplama.

## Temel kavramlar
| Kavram | Açıklama |
|---|---|
| `@model LoanCalculationModel` | View'in hangi tipte veri beklediğini söyler. `Model.Amount` diye IntelliSense ile erişilir. |
| `asp-for="Amount"` | Tag Helper: input'un `name`, `id`, `value` ve doğrulama özniteliklerini modelden üretir |
| `[Required]`, `[Range]` | Data Annotations: doğrulama kuralları (`System.ComponentModel.DataAnnotations`) |
| `ModelState.IsValid` | Gelen veri kurallara uyuyor mu? |
| `asp-validation-for` | Alanın hata mesajını gösterir |
| Hesaplanan özellik | `public double MontlyPayment => ...` — veritabanında tutulmaz, her okunuşta hesaplanır |

## Projede neler var?
- `Models/LoanCalculationModel.cs` → tutar + vade, kurallar ve hesaplanan alanlar (`MontlyPayment`, `TotalRepayable`)
- `LoanController.Index` (GET) → boş model ile formu açar
- `LoanController.Calculate` (POST) → `ModelState.IsValid` ise `IsCalculated = true` yapıp aynı View'i sonuçla gösterir

## Hesaplama (basit faiz)
```
Toplam faiz   = Tutar × Aylık faiz × Vade
Aylık taksit  = (Tutar + Toplam faiz) / Vade
Toplam ödeme  = Aylık taksit × Vade
Örnek: 12.000 TL, 12 ay, %3,5 → faiz 5.040 TL, taksit 1.420 TL, toplam 17.040 TL
```
> **[DÜZELTME]** Kodda daha önce `Amount + totalInterest / MonthTerm` yazıyordu. İşlem önceliği (önce bölme, sonra toplama) yüzünden taksit neredeyse kredinin tamamı çıkıyordu. Parantez eklendi. Ders için güzel bir örnek: **operatör önceliğini parantezle açıkça belirtin.**

## Nasıl denenir?
1. 50.000 TL, 12 ay → sonuç kutusu dolar.
2. 500 TL girin → "1.000 TL ile 100.000 TL arasında olmalıdır" hatası.
3. Vade seçmeden gönderin → "Lütfen vade süresini seçiniz".

## Neden Model?
| ViewData / ViewBag | Model |
|---|---|
| Tip dönüşümü (cast) gerekir | Gerekmez |
| Yazım hatası çalışırken anlaşılır | Derleme anında anlaşılır |
| Doğrulama elle yazılır | Attribute'larla bildirimsel (declarative) |

## Sık yapılan hatalar
- GET action'ında `return View()` diye **modelsiz** dönmek → View'de `Model.IsCalculated` okununca `NullReferenceException`. (Bu yüzden `return View(new LoanCalculationModel())` yazıldı.)
- Doğrulama başarısızken başka sayfaya yönlendirmek → kullanıcının yazdıkları ve hata mesajları kaybolur. Aynı View'e modelle dönün.
- Bir View'de birden fazla `@model` yazmaya çalışmak → yapılamaz; ViewModel kullanın (05).

## Web API ve React'te karşılığı
- Aynı `[Required]` / `[Range]` kuralları API'deki DTO'larda da kullanılır. `[ApiController]` varsa `ModelState.IsValid` kontrolünü kendisi yapar ve 400 döner (13_Web_API_Giris, E-Ticaret `Dtos/` klasörü).
- React'te hata mesajları API'nin `errors` nesnesinden okunup alanların altında gösterilir (E-Ticaret `Checkout.jsx`).

## Mülakat soruları
1. Strongly-typed View nedir, avantajları nelerdir?
2. Client-side ve server-side validation farkı nedir? Hangisi zorunludur?
3. `ModelState` nedir, içinde neler bulunur?

## Alıştırma
Modele `[Range(0.01, 0.10)] double InterestRate` ekleyip faizi kullanıcının girmesini sağlayın. Sonuçta aylık ödeme tablosunu (1. ay, 2. ay, ...) `@for` döngüsüyle listeleyin.
