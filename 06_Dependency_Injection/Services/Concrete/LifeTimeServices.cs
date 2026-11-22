using _06_Dependency_Injection.Services.Abstract;

namespace _06_Dependency_Injection.Services.Concrete
{
    /// <summary>
    /// TransientService: Her talep edildiğinde(Constructor enjeksiyonu veya GetService çağrısı) yeni bir örnek(new instance) üretilir.
    /// </summary>
    public class TransientService : ITransientService
    {
        private readonly Guid _operationId;
        public TransientService()
        {
            _operationId = Guid.NewGuid();
        }

        public Guid GetOperationId() => _operationId;
    }

    /// <summary>
    /// ScopedService: Her istek için yeni bir örnek oluşturulur ve istek sonunda yok edilir.Aynı HTTP Request içinde aynı örnek kullanılır. (Scoped Service, genellikle web uygulamalarında kullanılır ve her HTTP isteği için bir örnek oluşturulur.)
    /// </summary>
    public class ScopedService : IScopedService
    {
        private readonly Guid _operationId;

        public ScopedService()
        {
            _operationId = Guid.NewGuid();
        }


        public Guid GetOperationId() => _operationId;
    }
}

/// <summary>
/// SingletonService: Uygulama başlatıldığında bir kez oluşturulur ve uygulama kapanana kadar aynı örnek kullanılır. (Singleton Service, genellikle uygulama genelinde paylaşılan veriler veya durumlar için kullanılır.)
/// </summary>
public class SingletonService : ISingletonService
{
    private readonly Guid _operationId;

    public SingletonService()
    {
        _operationId = Guid.NewGuid();
    }
    public Guid GetOperationId() => _operationId;
}
