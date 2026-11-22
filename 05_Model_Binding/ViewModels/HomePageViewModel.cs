using _05_Model_Binding.Models;

namespace _05_Model_Binding.ViewModels
{
    /// <summary>
    /// ViewModel(Görünüm Modeli) Deseni:
    /// Razor View'ler sadece TEK bir model tipi kabul eder(@model MyModel)
    /// Bir sayfada birden çok modele(örnek: hem Kisi hem de Adres) ihtiyaç duyulduğunda
    /// bu modelleri tek bir çatı altında birleştiren ViewModel sınıfları oluşturulur
    /// Bu yaklaşım:
    /// 1. Güçlü tiplilik(Strongly-Typed) avantajını korur
    /// 2. Sayfanın gereksinimlerine tam uyan özel bir veri yapısını sunar
    /// 3. View ile Domain modelleri arasındaki bağımlılığı gevşetir.
    /// </summary>
    public class HomePageViewModel
    {
        /// <summary>
        /// Sayfada görüntülenecek veya formdan alınacak Kisi nesnesi
        /// </summary>
        public Kisi KisiNesnesi { get; set; } = new Kisi();

        /// <summary>
        /// Sayfada görüntülenecek veya formdan alınacak Adres nesnesi
        /// </summary>
        public Adres AdresNesnesi { get; set; } = new Adres();

        /// <summary>
        /// Sayfa başlığı veya ek mesaj gibi Ek UI bilgileri taşınabilir.
        /// </summary>
        public string? BilgiMesaji { get; set; }
    }
}
