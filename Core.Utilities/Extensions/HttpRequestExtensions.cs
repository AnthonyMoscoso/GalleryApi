using Core.Utilities.Ensures;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace Core.Utilities.Extensions
{
    public static class HttpRequestExtensions
    {
        public static TValue? GetValue<TValue>(this HttpRequest httpRequest, string key )
        {
           
            Ensure.That(httpRequest, nameof(httpRequest)).IsNotNull();
            Ensure.That(key,nameof(key)).NotNullOrEmpty();
            string? value = httpRequest.Headers[key];
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            return (TValue)Convert.ChangeType(value, typeof(TValue));

        }
        public static TValue? GetHeaderValue<TValue>(this HttpRequestData httpRequest, string param)
        {
            Ensure.That(httpRequest, nameof(httpRequest)).IsNotNull();
            bool success  = httpRequest.Headers.TryGetValues(param.ToLower(), out IEnumerable<string>? headers);
            if (!success)
            {
                return default;
            }
            if (headers.IsNullOrEmpty())
            {
                return default;
            }
            string? value = headers!.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }

        public static TValue? GetValue<TValue>(this HttpRequestData httpRequest, string param)
        {
            Ensure.That(httpRequest, nameof(httpRequest)).IsNotNull();
            string? value = httpRequest.Query[param.ToLower()];
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }
            if (typeof(TValue) == typeof(DateTimeOffset))
            {
                if (!DateTimeOffset.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                {
                    return default;
                }
                return (TValue)(object)result;
            }
            return (TValue)Convert.ChangeType(value, typeof(TValue));
        }

        public static async Task<HttpResponseData> CreateResponseAsync(this HttpRequestData request, HttpStatusCode statusCode, object? body = null, IDictionary<string, string>? headers = null)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = request.CreateResponse(statusCode);
            if (headers?.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    response.Headers.Add(header.Key, header.Value);
                }
            }
            if (body != null)
            {
                await response.WriteAsJsonAsync(body, statusCode);
            }
            return response;
        }

        public static async Task<HttpResponseData> CreateResponseStringAsync(this HttpRequestData request, HttpStatusCode statusCode, string body, IDictionary<string, string>? headers = null)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = request.CreateResponse(statusCode);

            if (headers?.Count > 0)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    response.Headers.Add(header.Key, header.Value);
                }
            }
            await response.WriteStringAsync(body, Encoding.UTF8);
            return response;
        }

        public static async Task<T?> DeserializeBodyAsync<T>(this HttpRequestData request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Body?.Length == 0)
            {
                return (T)Activator.CreateInstance(typeof(T))!;
            }

            return await JsonSerializer.DeserializeAsync<T>(request.Body!, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
        }

        public static async Task<string> GetBodyJson(this HttpRequestData request)
        {
            string body = string.Empty;
            if (request.Body != null && request.Body.CanRead)
            {

                request.Body.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new(request.Body, Encoding.UTF8);
                body = await reader.ReadToEndAsync();
                request.Body.Seek(0, SeekOrigin.Begin);
            }
            return body;
        }
    }
}
