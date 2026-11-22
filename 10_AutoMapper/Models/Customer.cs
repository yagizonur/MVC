namespace _10_AutoMapper.Models
{
    /// <summary>
    /// Kendi iç sistemimizdeki Müşteri Varlık Modeli(Domain Entity / Internal Model):
    /// =================================================================================
    /// NEDEN AYRI İÇ MODEL KULLANILIR? (Anti-Corruption Layer - ACL)
    /// 1. Temiz Mimari(Clean Architecture):
    ///     Dış sistemlerin(3. parti CRM, kargo, ödeme veya tedarikçi API'lerinin)
    ///     şemaları, değişken isimleri veya alan formatları değiştiğinde kendi 
    ///     veritabanımızı ve iş mantığımızı korumak isteriz.
    /// 2. Standart C# Kuralları:
    ///     Dış API'den snake_case (first_name, contact_email vb) gelse bile bizim
    ///     iç modelimiz C# PascalCase ve güçlü tipli(Strongly-Typed) standartları korur
    /// 3. Düzleştirilmiş ve İşlenmiş Veri:
    ///     İç içe nesneler(nested address vb) bizim sistemimizde gereksimimize göre 
    ///     doğrudan tek bir alana (FullAddress) veya uygun dönüştürülür.
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        /// <summary>
        /// Dış sistemdeki ID / referans numarası(Örn: EXT-91391)
        /// </summary>
        public string CustomerCode { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string FullAddress { get; set; }

        public bool IsActive { get; set; }


        public decimal TotalBalance { get; set; }


        public DateTime RegisteredDate { get; set; }
    }
}
