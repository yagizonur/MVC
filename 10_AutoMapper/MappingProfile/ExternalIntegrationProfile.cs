using _10_AutoMapper.Dtos;
using _10_AutoMapper.Models;
using AutoMapper;

namespace _10_AutoMapper.MappingProfile
{
    /// <summary>
    /// Harici Entegrasyon AutoMapper Profili:
    /// ==============================================================================
    /// GERÇEK HAYAT ENTEGRASYONUNDA AUTOMAPPER'IN GÜCÜ:
    /// Dış servisten (API/Webhook) gelen kirli ve farklı formatlardaki veriyi,
    /// kendi iç sistemimizin tertemiz Domain Entity modeline çevirirken:
    /// 
    /// 1. İsim Farklılıkları (Property Name Mapping):
    ///    src.ExternalId ➔ dest.CustomerCode
    ///    src.ContactEmail ➔ dest.Email
    /// 
    /// 2. Alan Birleştirme (String Concatenation):
    ///    src.FirstName + " " + src.LastName ➔ dest.FullName
    /// 
    /// 3. İç İçe Nesneleri Düzleştirme (Flattening & Formatting):
    ///    src.Address.City ➔ dest.City
    ///    src.Address (Street + District + City + ZipCode) ➔ dest.FullAddress
    /// 
    /// 4. Veri Tipi ve Format Dönüşümleri (Type & Value Transformation):
    ///    src.StatusCode ("ACTIVE") ➔ dest.IsActive (true / false)
    ///    src.BalanceCents (254990) ➔ dest.TotalBalance (2549.90m)
    ///    src.CreatedTimestamp ("2026-03-15T09:30:00Z") ➔ dest.RegisteredDate (DateTime)
    /// 
    /// 5. Güvenlik ve İzolasyon (Anti-Corruption Layer):
    ///    Dış servis ne kadar değişirse değişsin, sadece bu profildeki kuralları
    ///    güncelleriz; Controller, Servis ve Veritabanı kodlarımız ASLA etkilenmez!
    /// ==============================================================================
    /// </summary>
    public class ExternalIntegrationProfile : Profile
    {
        public ExternalIntegrationProfile()
        {
            // Veritabanı Id'si dış kaynakta olmadığı için eşleşmeyi yok sayıyoruz.
            CreateMap<ExternalCustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())

            .ForMember(dest => dest.CustomerCode, opt => opt.MapFrom(src => src.ExternalId))

            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))

            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactEmail))

            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))

            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : string.Empty))

            .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src =>
            src.Address != null
            ? $"{src.Address.Street}, {src.Address.District} / {src.Address.City} ({src.Address.ZipCode}) - {src.Address.CountryCode}"
            : "Adres bilinmiyor"
                ))

            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src =>
            string.Equals(src.StatusCode, "ACTIVE", StringComparison.OrdinalIgnoreCase)))

            .ForMember(dest => dest.TotalBalance, opt => opt.MapFrom(src => src.BalanceCents / 100m))

            .ForMember(dest => dest.RegisteredDate, opt => opt.MapFrom(src => ParseDate(src.CreatedTimestamp)));

        }

        private static DateTime ParseDate(string? timestamp)
        {
            return DateTime.TryParse(timestamp, out var parseDate) ? parseDate : DateTime.Now;
        }
    }
}
