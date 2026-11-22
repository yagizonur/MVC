namespace _10_AutoMapper.Models
{
    /// <summary>
    /// User Entity(Veritabanı Varlık Modeli):
    /// Veritabanı tablosundaki tüm sütunları birebir temsil eder.
    /// Hassas veriler(PasswordHash gibi) içeriğinden doğrudan istemciye gönderilmemelidir.
    /// </summary>
    public class User
    {
        public int Id { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }    

        public string Email { get; set; }   

        // Hassas Bilgi: DTO'da ASLA yer almamalıdır!
        public string PasswordHash { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
