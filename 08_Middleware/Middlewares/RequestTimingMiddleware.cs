using System.Diagnostics;

namespace _08_Middleware.Middlewares
{
    /// <summary>
    /// İstek süresi Ölçüm Middleware: 
    /// Nasıl Çalışır? 
    /// 1. İstek gelidğinde bir Stopwatch başlatılır.
    /// 2. context.Response.OnStarting() callback'i ile yanıt istemciye gönderilmeden hemen önce hesaplanan süreyi "X-Response-Time-Ms" 
    /// HTTP yanıt başlığına yazar.
    /// 3. await _next(context) ile bir sonraki middleware çağrılır ve işlem tamamlanır.
    /// 4. Yanıt dönüş yolunca süreyi durdurur ve konsole/debug penceresine loglar.
    /// </summary>
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<RequestTimingMiddleware> _logger; // Middleware için loglama desteği sağlar.

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) // HttpContext parametresi ile gelen isteği temsil eder.
        {
            var stopWatch = Stopwatch.StartNew(); // İstek süresini ölçmek için bir Stopwatch başlatılır.

            context.Response.OnStarting(() =>
            {
                stopWatch.Stop();
                var elapsedMilliseconds = stopWatch.ElapsedMilliseconds; // Ölçülen süreyi milisaniye cinsinden alır.

                // Ysnıt başlığına süreyi ekle(Tarayıcı F12 -> Network sekmesinde de görünür.)
                if (!context.Response.Headers.ContainsKey("X-Response-Time-Ms"))
                {
                    context.Response.Headers.Append("X-Response-Time-Ms", $"{elapsedMilliseconds} ms");
                }

                context.Items["ExecutionTimeMs"] = elapsedMilliseconds; // İSteğin işlenme süresini HttpContext.Items koleksiyonuna ekler.

                return Task.CompletedTask; // Başlık ekleme işlemi tamamlandığında tamamlanmış bir görev döndürür.

            });

            await _next(context); // Bir sonraki middleware'i çağırır.

            _logger.LogInformation("HTTP {Method} {Path} tamamlandı. Süre: {Elapsed} ms", context.Request.Method, context.Request.Path, stopWatch.ElapsedMilliseconds);
        }
    }
}
