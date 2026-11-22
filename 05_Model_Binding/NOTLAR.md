# 05 — Model Binding ve ViewModel

## Bu derste ne öğreniyoruz?
İstekteki ham metinlerin (form alanları, adres, sorgu dizesi, başlıklar) ASP.NET Core tarafından **otomatik olarak C# nesnelerine** nasıl dönüştürüldüğünü; birden fazla modeli tek View'de kullanmak için ViewModel'i ve güvenlik için `[Bind]`'ı.

## Model Binding nedir?
```
POST /Home/Index   Ad=Onur&Soyad=Yağız&Yas=32      ← tarayıcının gönderdiği ham METİN
          │
          ▼  Model Binder: isimleri eşleştirir, "32" → int'e çevirir
          ▼
Index(Kisi kisi)   kisi.Ad = "Onur", kisi.Yas = 32  ← hazır C# nesnesi
```

## Veri kaynakları
| Attribute | Nereden okur? | Örnek |
|---|---|---|
| `[FromForm]` | Form gövdesi | `<input name="Ad">` |
| `[FromRoute]` | Adres parçası | `/Home/BindingSources/5` → `id = 5` |
| `[FromQuery]` | Sorgu dizesi | `?q=mvc` → `q = "mvc"` |
| `[FromHeader]` | HTTP başlığı | `User-Agent` |
| `[FromBody]` | JSON gövdesi | API'lerde (`[ApiController]` ile otomatik) |
| `[FromServices]` | DI container | 07. ders |

Attribute yazılmazsa MVC sırayla form → rota → sorgu dizesine bakar.

## Projede neler var?
- `Index` (GET/POST) → `Kisi` nesnesinin formdan bağlanması, `[Bind("Ad","Soyad","Yas")]` ile beyaz liste
- `HomePage2` → `HomePageViewModel` içinde **hem** `Kisi` **hem** `Adres`; iç içe alanlar `KisiNesnesi.Ad`, `AdresNesnesi.Sehir` adlarıyla bağlanır
- `BindingSources` → aynı action'da `[FromRoute]`, `[FromQuery]`, `[FromHeader]` yan yana

## Nasıl denenir?
- `/Home/BindingSources/42?q=react` → ekranda id = 42, q = react ve tarayıcınızın User-Agent bilgisi
- `/Home/HomePage2` → formu gönderin, F12 → Network → Payload'da `KisiNesnesi.Ad=...` adlarını görün

## Overposting (Mass Assignment) saldırısı
Modelinizde `IsAdmin` diye bir alan olduğunu düşünün. Formda yoksa bile kötü niyetli biri isteğe `IsAdmin=true` ekleyebilir; Model Binder bunu da bağlar!
Korunma yolları:
1. `[Bind("Ad","Soyad","Yas")]` → sadece listedekiler bağlanır (bu projede).
2. **Daha iyisi:** Sadece gerekli alanları içeren ayrı bir ViewModel/DTO kullanmak (E-Ticaret API'deki `RegisterDto`, `CheckoutDto`).

## ViewModel nedir?
Bir View **tek** `@model` alır. Sayfada birden fazla nesne gerekiyorsa onları bir "taşıyıcı" sınıfta toplarız: `HomePageViewModel { Kisi, Adres, BilgiMesaji }`. ViewModel o sayfaya özeldir; veritabanı Entity'si değildir.

## Sık yapılan hatalar
- Input `name`'inin property adıyla eşleşmemesi (`name="isim"` ama property `Ad`) → değer gelmez.
- `int Yas` alanına "abc" gönderilmesi → hata fırlamaz, `ModelState`'e hata eklenir ve değer 0 kalır. `ModelState.IsValid` kontrol edilmeli.
- İç içe nesnede ViewModel'in alt nesnesini `new` ile başlatmamak → View'de `Model.KisiNesnesi.Ad` null hatası (bu projede `= new Kisi()` ile önlenmiş).

## Web API ve React'te karşılığı
- E-Ticaret API'de: `List([FromQuery] ProductQuery query)` → `?category=telefon&minPrice=1000&sort=price-asc` tek nesneye bağlanıyor.
- `[FromForm] ProductFormDto` → admin panelinden gelen resim dosyaları (`IFormFile`) ve alanlar.
- React'te `FormData` ile gönderilen alan adları DTO'nun property adlarıyla eşleşir (`services.js → toProductFormData`).

## Mülakat soruları
1. Model Binding hangi kaynaklardan veri okur, öncelik sırası nedir?
2. Overposting nedir, nasıl önlenir?
3. ViewModel ile Entity ve DTO arasındaki fark nedir?

## Alıştırma
`/Home/Ara?kelime=...&sayfa=2` adresini karşılayan bir action yazın. Parametreleri `AramaFiltresi { Kelime, Sayfa, SayfaBoyutu = 10 }` sınıfına `[FromQuery]` ile bağlayıp ekranda gösterin.
