# 11 — AutoMapper ve DTO Deseni

## Bu derste ne öğreniyoruz?
Bir nesneyi başka bir nesneye dönüştürmeyi (Entity → DTO, dış API cevabı → iç model) elle satır satır yazmak yerine **kurallarla** yapmayı.

## Problem
```csharp
var dto = new UserDto
{
    Id = user.Id,
    FullName = user.FirstName + " " + user.LastName,
    Email = user.Email
};   // 20 alanlı bir nesnede, 30 farklı yerde... Bir alan eklenince hepsini bulup güncellemek gerekir.
```

## Çözüm: Profile + IMapper
```csharp
// 1) Kurallar tek yerde (MappingProfile/UserProfile.cs)
CreateMap<User, UserDto>()
    .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
    .ReverseMap();                       // UserDto → User yönü de

// 2) Kullanım (Controller)
var dto = _mapper.Map<UserDto>(user);    // tek satır
```
Aynı isimli alanlar (`Id`, `Email`) **kendiliğinden** eşlenir; sadece farklı olanlar için kural yazılır.

## DTO neden var?
1. **Güvenlik:** `User.PasswordHash` asla istemciye gitmemeli → `UserDto`'da bu alan yok.
2. **Performans:** Sadece gereken alanlar taşınır.
3. **Biçim:** `FirstName` + `LastName` → tek `FullName`.

## Projede neler var?
- `UserController.Index` → `User` (Entity) → `UserDto`; ekranda iki nesne yan yana, PasswordHash'in DTO'da olmadığı görülür.
- `UserController.MappingTest` → `ReverseMap` ile DTO → Entity.
- `IntegrationController` + `ExternalIntegrationProfile` → **gerçek hayat senaryosu**: dış bir CRM API'sinden gelen snake_case, kuruş cinsinden tutar ve iç içe adres içeren "kirli" JSON'u temiz `Customer` modeline çevirir:
  - `external_id` → `CustomerCode` (isim farkı)
  - `first_name` + `last_name` → `FullName` (birleştirme)
  - `address.city` → `City` (düzleştirme / flattening)
  - `"ACTIVE"` → `IsActive = true` (tip dönüşümü)
  - `856543` kuruş → `8565.43m` TL
- `Program.cs` → `AddAutoMapper(cfg => { }, typeof(UserProfile))`: `UserProfile`'ın bulunduğu projedeki **tüm** Profile sınıflarını tarar (ExternalIntegrationProfile da bulunur).

## Nasıl denenir?
- `/` → User → UserDto karşılaştırması
- `/User/MappingTest` → ters yönde eşleme; `PasswordHash` boş kalır
- `/Integration` → ham JSON, dış DTO ve iç model üç sütunda

## Anti-Corruption Layer (Bozulma Önleyici Katman)
Dış sistem alan adını değiştirirse (`first_name` → `firstName`) sadece **DTO ve Profile** güncellenir; Controller, servis ve veritabanı kodu etkilenmez. Dış dünyanın "dağınıklığı" uygulamanın içine sızmaz.

## Sık yapılan hatalar
- Profile yazıp `AddAutoMapper` ile kaydetmemek → `AutoMapperMappingException: Missing type map configuration`.
- Hedefte kaynağa karşılık gelmeyen alanı unutmak → sessizce varsayılan değer kalır. Geliştirme sırasında `configuration.AssertConfigurationIsValid()` ile tüm eşlemeleri doğrulayın.
- Karmaşık iş mantığını Profile'a gömmek → Profile sadece **dönüşüm** içindir; iş kuralları serviste kalmalı.
- **Lisans notu:** AutoMapper'ın yeni sürümleri ticari kullanımda ücretli lisans istiyor. Ücretsiz alternatifler: Mapster, Mapperly veya elle yazılan extension metotları.

## Web API ve React'te karşılığı
- E-Ticaret API, AutoMapper yerine elle yazılmış dönüşüm metotları kullanıyor: `Extensions/DtoMappingExtensions.cs` → `product.ToDto()`, `order.ToDto()`. İkisini karşılaştırın: hangisi daha okunur, hangisi daha az kod?
- API'nin döndüğü JSON'un şeklini DTO belirler; React tarafı bu şekle göre yazılır (`product.imageUrl`, `cart.totalPrice`).

## Mülakat soruları
1. DTO nedir, neden Entity doğrudan dönülmez?
2. `ForMember`, `MapFrom`, `Ignore` ve `ReverseMap` ne işe yarar?
3. Flattening nedir?
4. AutoMapper'ın dezavantajları nelerdir?

## Alıştırma
`ProductDto { Id, Name, PriceText, CategoryName }` oluşturun. `Product { Id, Name, Price, Category { Name } }` → `ProductDto` eşlemesinde `PriceText = "1.250,00 ₺"` (`ToString("N2", new CultureInfo("tr-TR"))`) ve `CategoryName` düzleştirmesini yapın.
