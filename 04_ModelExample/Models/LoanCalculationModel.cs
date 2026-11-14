using System.ComponentModel.DataAnnotations;

namespace _04_ModelExample.Models
{
    public class LoanCalculationModel
    {
        [Required(ErrorMessage = "Lütfen kredi tutarını giriniz")]
        [Range(100,100000, ErrorMessage = "Kredi tutarı 1.000 TL ile 100.000 TL arasında olmalıdır")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Lütfen vade süresini seçiniz")]
        [Range(3,36, ErrorMessage = "Vade 3 ile 36 ay arasında olmalıdır.")]
        public int MonthTerm { get; set; }

        // Salt Okunur Hesaplama Özellikleri(Modelin kendi yeteneği)
        public double InterestRate => 0.035; // Sabit Faiz Oranı: %3.5
    
        public double MonthlyPayment
        {
            get
            {
                if (Amount <= 0 || MonthTerm <= 0) return 0;

                double totalInterest = Amount * InterestRate * MonthTerm;

                // (Anapara + toplam faiz) / vade
                return Math.Round((Amount + totalInterest) / MonthTerm, 2);
            }
        }

        public double TotalRepayable => Math.Round(MonthlyPayment * MonthTerm, 2);

        public bool IsCalculated { get; set; } = false; // Sonucun ekranda görünüp görünmeyeceğini belirler.
    }
}
