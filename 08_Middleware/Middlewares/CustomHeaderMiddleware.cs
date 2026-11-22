namespace _08_Middleware.Middlewares
{
    /// <summary>
    /// Özel Başlık Middleware:
    /// Her HTTP yanıtına kurumsal güvenlik ve ortam bilgisi başlıkları ekler.
    /// </summary>
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next; // Bir sonraki middleware'i temsil eder.

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append("X-Powered-By-Course", "Ucuncu Binyil MVC Akademi");
                context.Response.Headers.Append("X-Server-Time", DateTime.UtcNow.ToString("O")); // ISO 8601 formatında UTC zamanını ekler.
                return Task.CompletedTask; // Başlık ekleme işlemi tamamlandığında tamamlanmış bir görev döndürür. 
            });

            await _next(context); // Bir sonraki middleware'i çağırır.
        }
    }
}
