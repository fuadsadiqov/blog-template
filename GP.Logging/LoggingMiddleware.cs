using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Context;

namespace GP.Logging
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly long _start;
        private readonly string[] _ignoredRoutes = new[] { "/api/Files" };
        private readonly string[] _sensitiveRoutsWithPrivateData = new[] { "/api/Account/auth" }; // array of keywords to be masked
        private readonly string[] _logPropertiesToBeMasked = new[] { "password" }; // array of keywords to be masked

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _start = Stopwatch.GetTimestamp();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = httpContext.User.FindFirstValue(ClaimTypes.Name);

            // Get the client's IP address
            var clientIp = httpContext.Connection.RemoteIpAddress;

            // Convert the IP address to IPv4 format if it's an IPv6 address
            if (clientIp.IsIPv4MappedToIPv6)
            {
                clientIp = clientIp.MapToIPv4();
            }
            // Push the user name into the log context so that it is included in all log entries
            LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("username", userName);
            LogContext.PushProperty("ClientIp", clientIp.ToString());

            await RequestLog(httpContext.Request).ConfigureAwait(false);
            await ResponseLog(httpContext).ConfigureAwait(false);
        }

        private async Task RequestLog(HttpRequest request)
        {
            string requestBody = String.Empty;
            var isIgnoredRoute =
                _ignoredRoutes.Any(e => request.Path.StartsWithSegments(new PathString(e)));

            if (!isIgnoredRoute)
            {
                //This line allows us to set the reader for the request back at the beginning of its stream.
                request.EnableBuffering();
                var body = request.Body;

                //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];

                //...Then we copy the entire request stream into the new buffer.
                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                //We convert the byte[] into a string using UTF8 encoding...
                requestBody = Encoding.UTF8.GetString(buffer);


                if (_sensitiveRoutsWithPrivateData.ToList().Contains(request.Path))
                {
                    if (!string.IsNullOrEmpty(requestBody))
                    {
                        var requestBodyJObject = JObject.Parse(requestBody);

                        foreach (var key in _logPropertiesToBeMasked)
                        {
                            if (requestBodyJObject.ContainsKey(key)) requestBodyJObject[key] = "********";
                        }

                        requestBody = requestBodyJObject.ToString();
                        requestBody = requestBody.Replace("\r", "").Replace("\n", "");
                    }
                }

                request.Body.Seek(0, SeekOrigin.Begin);
                //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
                request.Body = body;
            }

            LogContext.PushProperty("Type", "Request");
            Log.ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), true)
                .ForContext("RequestBody", requestBody)
                .ForContext("ClientIp", request.HttpContext.Connection.RemoteIpAddress?.ToString()) // Include the client's IP address
                .Information("Request information {RequestMethod} {RequestPath}", request.Method, request.Path + request.QueryString);
        }

        private async Task ResponseLog(HttpContext httpContext)
        {
            var response = httpContext.Response;


            var originalBody = httpContext.Response.Body;
            string responseBody = "";
            await using var memStream = new MemoryStream();

            try
            {
                response.Body = memStream;

                try
                {
                    await _next(httpContext).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    var errorId = Guid.NewGuid();
                    Log.ForContext("Type", "Error").ForContext("StackTrace", ex.StackTrace)
                        .ForContext("Exception", ex, false)
                        .Error(ex, ex.Message + ". {@errorId}", errorId);

                    if (ex is UnauthorizedAccessException)
                    {
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                    else
                    {
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                    }

                    throw;
                }
            }
            finally
            {
                if (response.Body.CanRead)
                {
                    response.Body.Seek(0, SeekOrigin.Begin);
                    responseBody = await new StreamReader(response.Body).ReadToEndAsync().ConfigureAwait(false);
                    response.Body.Seek(0, SeekOrigin.Begin);
                }

                LogContext.PushProperty("Type", "Response");
                Log.ForContext("ResponseHeaders", response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()))
                    .ForContext("ResponseBody", responseBody)
                    .Information("Response information {RequestMethod} {RequestPath} {statusCode} {ElapsedTime} s",
                        httpContext.Request.Method, httpContext.Request.Path + httpContext.Request.QueryString, response.StatusCode,
                        GetElapsedMilliseconds(_start, Stopwatch.GetTimestamp()));

                await memStream.CopyToAsync(originalBody).ConfigureAwait(false);

                httpContext.Response.Body = originalBody;
            }
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return (double)(((stop - start) * 1000L) / (double)Stopwatch.Frequency);
        }
    }
}
