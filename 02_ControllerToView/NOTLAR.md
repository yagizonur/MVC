# 02 — Controller'dan View'e Veri Göndermek

## Bu derste ne öğreniyoruz?
Controller'da hazırlanan bir veriyi (ürün listesi) View'e taşıyıp ekranda listelemek ve adresten parametre (`id`) almak.

## Temel kavramlar
| Kavram | Açıklama |
|---|---|
| `IActionResult` | Action'ın "bir sonuç" döndüreceğini söyler: View, Redirect, NotFound, Json... |
| `ViewData["anahtar"]` | Controller → View veri taşıyan sözlük (`Dictionary<string, object>`) |
| `@foreach` | Razor içinde C# döngüsü; her eleman için HTML üretir |
| Rota parametresi | `/Product/Details/5` → `Details(int id)` metodundaki `id = 5` |

## Projede neler var?
- `ProductController.Index()` → 3 ürünlük listeyi `ViewData["products"]` ile gönderir.
- `ProductController.Details(int id)` → adresteki id'yi alıp `ViewData["productDetail"]`e yazar.
- `Views/Product/Index.cshtml` → `(List<string>)ViewData["products"]` ile listeyi kartlar halinde basar.

## Nasıl denenir?
- `/` veya `/Product` → ürün listesi
- `/Product/Details/7` → "Ürün 7 Detayları"
- `/Product/Details` → id gönderilmezse `int`'in varsayılanı **0** gelir

## ViewData'nın zayıf yanı
ViewData her şeyi `object` olarak saklar:
```csharp
@foreach (var item in (List<string>)ViewData["products"])   // tip dönüşümü (cast) zorunlu
```
- Anahtarı yanlış yazarsanız (`"product"`) derleyici uyarmaz, çalışırken `null` gelir.
- Tipi yanlış cast ederseniz çalışırken hata alırsınız.

**Çözüm:** Güçlü tipli model → `return View(products);` ve View'de `@model List<string>` (04_ModelExample).

## Sık yapılan hatalar
- ViewData'yı null kontrolü yapmadan döngüye sokmak → `NullReferenceException`. (Bu projedeki `@if (ViewData["products"] != null)` kontrolü bu yüzden var.)
- Action'ı `Details(int productId)` diye yazıp adreste `id` kullanmak → değer bağlanmaz, 0 gelir. Parametre adı rota şablonundaki adla aynı olmalı.

## Web API ve React'te karşılığı
- API'de ViewData yok: `return Ok(products);` → liste JSON olarak gider.
- React tarafında bu JSON `useState` + `useEffect` ile alınır ve `.map()` ile listelenir (08_React/08). Razor'daki `@foreach` ↔ JSX'teki `{liste.map(...)}`.

## Mülakat soruları
1. ViewData, ViewBag ve güçlü tipli model arasındaki fark nedir?
2. `IActionResult` yerine `ViewResult` döndürmek ne zaman mantıklı?
3. Adresteki `id` action parametresine nasıl bağlanır?

## Alıştırma
Ürünleri `string` yerine `Product { Id, Name, Price }` sınıfıyla tutun. `return View(products)` ile gönderip `@model List<Product>` ile listeleyin; her ürüne `Details/{Id}` bağlantısı verin.
