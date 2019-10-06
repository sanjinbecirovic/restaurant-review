using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RestaurantReview.Web.Middleware
{
    /// <summary>
    /// Logger middleware for routes starting with /api.
    /// WARN: Logging of query-string, request- and response-body content is unfiltered and in plain text!
    /// </summary>
    public class ApiLoggingMiddleware
    {
        private readonly ILogger<ApiLoggingMiddleware> logger;

        private readonly RequestDelegate next;

        /// <inheritdoc />
        public ApiLoggingMiddleware(ILogger<ApiLoggingMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger ?? NullLogger<ApiLoggingMiddleware>.Instance;
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Invokes request of pipeline.
        /// </summary>
        /// <param name="httpContext">Context from pipeline.</param>
        /// <returns>Returns async Task execution for next layer.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var context = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            var request = context.Request;
            if (!request.Path.StartsWithSegments(new PathString("/api"), StringComparison.OrdinalIgnoreCase))
            {
                await this.next(context);
                return;
            }

            var requestBodyContent = await ReadRequestBody(request);
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                var response = context.Response;
                response.Body = responseBody;
                await this.next(context);

                var responseBodyContent = await ReadResponseBody(response);
                await responseBody.CopyToAsync(originalBodyStream);

                this.logger.LogInformation(
                    "{@apiLogItem}",
                    new ApiLogItem
                    {
                        QueryString = request.QueryString.ToString(),
                        RequestBody = requestBodyContent,
                        ResponseBody = responseBodyContent
                    });
            }
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength, CultureInfo.InvariantCulture)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private class ApiLogItem
        {
            public string QueryString { get; set; }

            public string RequestBody { get; set; }

            public string ResponseBody { get; set; }
        }
    }
}
