using System.Text;
using System.Text.Json;

namespace BaseProject.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<string> ReadRequestBodyAsync(this HttpContext context, string[] sensitiveKeys)
        {
            try
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true
                );

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!string.IsNullOrWhiteSpace(body) && body.TrimStart().StartsWith("{"))
                {
                    using var doc = JsonDocument.Parse(body);
                    return doc.RootElement.MaskSensitiveJson();
                }

                return body;
            }
            catch
            {
                return "[Unable to read body]";
            }
        }
    }
}
