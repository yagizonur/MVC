using System.Text.Json.Serialization;

namespace _10_AutoMapper.Dtos
{
    /// <summary>
    /// Harici(3.parti) API'den Dönen Ham Yanıt Modeli(External API DTO):
    /// =====================================================================
    /// GERÇEK HAYAT ENTEGRASYON SENARTYOSU:
    /// Bir dış servis(Örn: Global CRM, B2B Tedarikçi, Kargo veya Ödeme API'si)
    /// REST/JSON üzerinden veri döner
    /// 
    /// Dış API'lerde sıkça karşılaşılan durumlar:
    /// 1. Farklı Naming Convention: JSON'da snake_case kullanılır("external_id", "first_name")
    /// 2. İç içe nesneler(Nested Objects): Adres bilgileri ayrı bir nesne("address") içindedir.
    /// 3. Farklı Veri Tipleri: 
    ///     - Durum bilgisi string gelir "ACTIVE" veya "PASSIVE"
    ///     - Para tutarı kuruş(cents) cinsinden long/int gelir: 22424(yani 2.242,40 TL)
    ///     - Tarih ISO-8434 string formatında gelir: "2026-09-21T09:30:00Z"
    /// </summary>
    public class ExternalCustomerDto  
    {

        [JsonPropertyName("external_id")]
        public string ExternalId { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("contact_email")]
        public string ContactEmail { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("status_code")]
        public string StatusCode { get; set; }

        [JsonPropertyName("balance_cents")]
        public long BalanceCents { get; set; }

        [JsonPropertyName("created_timestamp")]
        public string CreatedTimestamp { get; set; }

        [JsonPropertyName("address")]
        public ExternalAddressDto Address { get; set; }
    }

    public class ExternalAddressDto
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("zip_code")]
        public string ZipCode { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }
    }
}
