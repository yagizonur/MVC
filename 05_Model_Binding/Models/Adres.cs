namespace _05_Model_Binding.Models
{
    public class Adres
    {
        public string? AdresTanim { get; set; } // ? = nullable tipler için kullanılır. ?? işareti ise GetValueOrDefault() metodunun operatörüdür.

        public string? Sehir { get; set; }

    }
}
