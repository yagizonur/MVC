# 12 — Areas (Alanlar) ile Admin Paneli

## Bu derste ne öğreniyoruz?
Büyüyen bir projede birbirinden bağımsız bölümleri (ziyaretçi arayüzü, yönetim paneli) **ayrı klasörlerde**, kendi Controller ve View'leriyle düzenlemeyi.

## Problem
Tüm Controller'lar tek `Controllers` klasöründe: `HomeController`, `ProductController`, `AdminProductController`, `AdminOrderController`... İsimler karışır; aynı adda iki controller (ziyaretçi `ProductController` ve admin `ProductController`) olamaz.

## Çözüm: Area
```
Areas/
  Admin/
    Controllers/  DashboardController.cs, ProductController.cs   ← [Area("Admin")]
    Views/        Dashboard/Index.cshtml, Product/Index.cshtml,
                  _ViewImports, _ViewStart, Shared/_AdminLayout.cshtml   ← panelin kendi layout'u
Controllers/      HomeController.cs                               ← ziyaretçi tarafı
Views/            Home/Index.cshtml
```
Aynı isimli `ProductController` hem kök dizinde hem Admin alanında olabilir; namespace ve Area farklıdır.

## Projede neler var?
- `Areas/Admin/Controllers/*` → `[Area("Admin")]` attribute'u **zorunlu**; yoksa MVC controller'ı Area'ya bağlamaz.
- `Program.cs` → iki rota:
  1. `{area:exists}/{controller=Dashboard}/{action=Index}/{id?}` → **önce** yazılır. `area:exists` kısıtı, sadece gerçekten var olan bir Area adıyla eşleşmesini sağlar.
  2. `{controller=Home}/{action=Index}/{id?}` → Area'sız normal rota.
- Bağlantı üretme: `<a asp-area="Admin" asp-controller="Dashboard" asp-action="Index">`; Area dışına dönerken `asp-area=""`.

## Nasıl denenir?
- `/` → ziyaretçi anasayfası
- `/Admin` veya `/Admin/Dashboard` → yönetim paneli (varsayılan controller Dashboard)
- `/Admin/Product` → admin ürün sayfası

## Sık yapılan hatalar
- `[Area("Admin")]`'i unutmak → `/Admin/Dashboard` 404 verir.
- Area rotasını varsayılan rotadan **sonra** yazmak → `/Admin/Dashboard` adresi "AdminController'ın Dashboard action'ı" diye yorumlanır ve 404 olur.
- Area içindeki View'lerin `_ViewImports` / `_ViewStart` dosyalarını unutmak → Tag Helper'lar çalışmaz, layout gelmez.
- **Güvenlik:** Area sadece bir **düzen** aracıdır, koruma sağlamaz! Bu projede `/Admin` herkese açık. Gerçek projede Admin controller'larına `[Authorize(Roles = "admin")]` eklenmelidir.

## Web API ve React'te karşılığı
- React + API mimarisinde admin paneli **React tarafında** bir bölüm olur: E-Ticaret `frontend/src/pages/admin/` klasörü, `App.jsx`'te `/admin/...` rotaları ve `<ProtectedRoute role="admin">`.
- API tarafında Area yerine basitçe klasör + rota öneki yeterli: `Controllers/Admin/AdminProductsController.cs` → `[Route("api/admin/products")]` ve `[Authorize(Roles = "admin")]`.

## Mülakat soruları
1. Area ne zaman kullanılır?
2. `{area:exists}` kısıtı ne işe yarar?
3. Area rotası neden varsayılan rotadan önce tanımlanmalı?
4. Area bir güvenlik mekanizması mıdır?

## Alıştırma
`Areas/Admin/Controllers` altındaki iki controller'ı ortak bir `AdminBaseController : Controller` sınıfından türetin; `[Area("Admin")]`'ı sadece base class'a yazın. Ardından `[Authorize]` ekleyip giriş yapılmadan erişilemediğini görün.
