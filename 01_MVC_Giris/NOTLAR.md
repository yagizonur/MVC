# 01 - MVC'ye Giriş 

## Bu derste ne öğrendik ?
Bir ASP.NET Core MVC uygulamasının nasıl ayağa kalktığını, ,
bir adres yazıldığında hangi kodun çalıştığını ve ekrana gelen HTML'in 
nereden üretildiğini.

## MVC Nedir? 
Uygulamayı üç sorumluluğa ayıran bir tasarım desenidir:

|     Parça      |                    Görevi                       |                   Bu projede                             |
|----------------|-------------------------------------------------|----------------------------------------------------------|
| **M**odel      | Veri ve iş kuralları                            | `Models/ErrorViewModel.cs`                               |
| **V**iew       | Kullanıcının gördüğü HTML                       | `Views/Home/Index.cshtml`, `Views/Category/Index.cshtml` |
| **C**ontroller | İsteği karşılar, Model'i hazırlar, View'i seçer | `Controllers/HomeController.cs`, `CategoryController.cs` |

Neden ayırıyoruz? Tasarımcı View'e, backend geliştirici Controller/Model'e dokunur; birinin değişikliği diğerini bozmaz. Test etmek de kolaylaşır.

## Bir isteğin yolculuğu 
```
Tarayıcı: GET /Category/Index
   │
   ▼  Program.cs → MapControllerRoute("{controller=Home}/{action=Index}/{id?}")
   │     controller = Category, action = Index
   ▼
CategoryController.Index()  →  return View();
   │     View() parametresizse: Views/{Controller adı}/{Action adı}.cshtml aranır
   ▼
Views/Category/Index.cshtml  +  Views/Shared/_Layout.cshtml (@RenderBody)
   │
   ▼
HTML tarayıcıya döner

## Projede neler var?
- **Program.cs** — Uygulamanın başlangıç noktası. İki bölümü var:
  1. `builder.Services...` → servis kayıtları (`AddControllersWithViews`: MVC + Razor View motoru)
  2. `app.Use...` → her isteğin geçtiği ara katmanlar (middleware) ve rota tanımı
- **Rota şablonu** `{controller=Home}/{action=Index}/{id?}`
  - `=Home` → adres boşsa varsayılan değer
  - `id?` → isteğe bağlı parametre
- **_ViewStart.cshtml** — Tüm View'lerin `_Layout.cshtml`'i kullanacağını söyler.
- **_ViewImports.cshtml** — Tüm View'lerde geçerli `@using` ve Tag Helper tanımları.
- **_Layout.cshtml** — Ortak sayfa iskeleti (header, footer). `@RenderBody()` sayfanın değişen kısmıdır.

## Nasıl denenir?
1. Uygulamayı çalıştırın.
2. `/` → HomeController.Index
3. `/Category` veya `/Category/Index` → CategoryController.Index
4. `/Category/Olmayan` → 404 (böyle bir action yok)

## Sık yapılan hatalar
- **"The view 'Index' was not found"**: View dosyası yanlış klasörde. `Views/Category/Index.cshtml` olmalı; klasör adı Controller adıyla ("Controller" eki olmadan) aynı olmalı.
- Controller sınıf adının sonuna **Controller** yazmamak → ASP.NET Core onu controller olarak tanımaz.
- Action metodunu `private` yapmak → dışarıdan çağrılamaz.

## Web API ve React'te karşılığı
- MVC'de Controller **View** döndürür → HTML. Web API'de Controller **veri** döndürür → JSON (13_Web_API_Giris).
- `_Layout.cshtml` + `@RenderBody()` ↔ React'te `Layout.jsx` + `<Outlet />` (08_React/10_react_router).
- Rota şablonu ↔ React'te `<Route path="...">`.

## Mülakat soruları
1. MVC'nin her harfi neyi temsil eder, neden ayrılır?
2. `AddControllersWithViews()` ile `AddControllers()` farkı nedir?
3. `return View()` hangi dosyayı arar?
4. Conventional routing ile attribute routing farkı nedir?

## Alıştırma
`ProductController` ekleyin, `Index` ve `About` adlı iki action yazın ve ikisine View oluşturun. Menüye `asp-controller="Product" asp-action="About"` ile bağlantı koyun.

```
