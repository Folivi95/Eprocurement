using Microsoft.AspNetCore.Http;
using System.Linq;

namespace EGPS.Application.Helpers
{
    public static class GetHttpRequestHeadersExtension
    {
        public static string GetHeader(this HttpRequest request, string key)
        {
            request.Headers.TryGetValue(key, out var userIpAddress);

            return userIpAddress;
        }
    }
}
