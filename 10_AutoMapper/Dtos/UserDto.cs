namespace _10_AutoMapper.Dtos
{
    /// <summary>
    /// User DTO(Data Transfer Object)
    /// ==================================================
    /// DTO Nedir ve Neden Kullanılır?
    /// 1. Veri Güvenliği:
    ///     Entity'deki hassas alanlar(PasswordHash, Salt vb) bu sınıfa eklenmez;
    ///     böylece istemciye yanlışıkla sızması engellenir.
    /// 2. Ağ performansı:
    ///     Yalnızca View veya API istemcinin ihtiyaç duyduğu alanlar taşınır.
    /// 3. Özel formatlama:
    ///     FirstName ve LastName yerine tek bir FullName özelliği sunulur
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }   
    }
}
