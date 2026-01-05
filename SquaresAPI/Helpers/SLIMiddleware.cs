using System.Diagnostics;

namespace SquaresAPI.Helpers
{
    public class SLIMiddleware
    {
        private readonly RequestDelegate _next;
        private static long totalRequests, successfulRequests, failedRequests, totalResponseTime;

        public SLIMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                Interlocked.Increment(ref totalRequests);
                var sw = Stopwatch.StartNew();
                await _next(context);
                sw.Stop();

                Interlocked.Add(ref totalResponseTime, sw.ElapsedMilliseconds);

                var statusCode = context.Response.StatusCode;
                if (statusCode >= 200 && statusCode < 300)
                    Interlocked.Increment(ref successfulRequests);
                else
                    Interlocked.Increment(ref failedRequests);

                if (totalRequests % 10 == 0)
                {
                    var avgTime = totalRequests > 0 ? totalResponseTime / totalRequests : 0;
                    Console.WriteLine($"--- SLI Metrics ---");
                    Console.WriteLine($"Total Requests: {totalRequests}");
                    Console.WriteLine($"Successful Requests (2xx): {successfulRequests}");
                    Console.WriteLine($"Failed Requests (4xx/5xx): {failedRequests}");
                    Console.WriteLine($"Average Response Time: {avgTime} ms");
                    Console.WriteLine("-------------------");
                }
            }
            else
            {
                await _next(context); // skip Swagger/static requests
            }
        }
    }
   
        
}
