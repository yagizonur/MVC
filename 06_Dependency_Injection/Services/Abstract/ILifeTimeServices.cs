namespace _06_Dependency_Injection.Services.Abstract
{
    /// <summary>
    /// Servis Yaşam Döngüsü(Service LifeTime) Arayüzleri;
    /// Her bir arayüz, nesne üretildiğinde oluşturulan benzersiz bir kimlik (GUID) döndüren bir metot içerir.
    /// Bu sayede, servislerin yaşam döngüsü boyunca aynı nesne mi yoksa farklı nesneler mi kullanıldığını anlamak için kullanılabilir.
    /// </summary>
    public interface ITransientService
    {
        Guid GetOperationId();
    }

    public interface IScopedService
    {
        Guid GetOperationId();
    }

    public interface ISingletonService
    {
        Guid GetOperationId();
    }
}
