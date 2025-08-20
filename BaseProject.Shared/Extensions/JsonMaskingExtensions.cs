using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaseProject.Shared.Extensions
{

    public static class JsonMaskingExtensions
    {
        private static readonly string[] SensitiveKeys =
        {
            // Authentication / Tokens
            "password",
            "token",
            "accessToken",
            "refreshToken",
            "secret",
            "apiKey",
            "authorization",
            "credentials",
            "sessionId",
            "authToken",
    
            // Personal Identifiable Information (PII)
            "ssn",
            "socialSecurityNumber",
            "dob",
            "dateOfBirth",
            "passportNumber",
            "nationalId",
            "driverLicense",
            "email",
            "phone",
            "address",
    
            // Payment / Financial Data
            "cardNumber",
            "creditCard",
            "cvv",
            "iban",
            "bankAccount",
            "routingNumber",
            "accountNumber",
    
            // Security / Misc
            "pin",
            "securityAnswer",
            "securityQuestion",
            "privateKey",
            "certificate",
            "jwt",
            "hash"
        };

        /// <summary>
        /// Masks sensitive data in a String.
        /// </summary>
        /// <param name="element">The String element to mask.</param>
        /// <returns>String with sensitive fields masked.</returns>
        public static string MaskSensitiveData(this string json)
        {
            if (string.IsNullOrEmpty(json)) return json;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                using var stream = new MemoryStream();
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false }))
                {
                    WriteMaskedElement(root, writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch
            {
                // Fallback: simple string replacement if JSON parse fails
                return json
                    .Replace("password", "****", StringComparison.OrdinalIgnoreCase)
                    .Replace("token", "****", StringComparison.OrdinalIgnoreCase);
            }
        }


        /// <summary>
        /// Masks sensitive data in a JsonElement recursively.
        /// </summary>
        /// <param name="element">The JSON element to mask.</param>
        /// <returns>JSON string with sensitive fields masked.</returns>
        public static string MaskSensitiveJson(this JsonElement element)
        {
            if (element.ValueKind != JsonValueKind.Object)
                return element.ToString();

            var dict = new Dictionary<string, object>();
            foreach (var prop in element.EnumerateObject())
            {
                if (IsSensitiveKey(prop.Name))
                {
                    dict[prop.Name] = "****";
                }
                else if (prop.Value.ValueKind == JsonValueKind.Object)
                {
                    dict[prop.Name] = MaskSensitiveJson(prop.Value);
                }
                else if (prop.Value.ValueKind == JsonValueKind.Array)
                {
                    var list = new List<object>();
                    foreach (var item in prop.Value.EnumerateArray())
                    {
                        list.Add(item.ValueKind == JsonValueKind.Object
                            ? MaskSensitiveJson(item)
                            : item.GetRawText());
                    }
                    dict[prop.Name] = list;
                }
                else
                {
                    dict[prop.Name] = prop.Value.GetRawText();
                }
            }

            return JsonSerializer.Serialize(dict);
        }

        private static void WriteMaskedElement(JsonElement element, Utf8JsonWriter writer)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    writer.WriteStartObject();
                    foreach (var prop in element.EnumerateObject())
                    {
                        if (IsSensitiveKey(prop.Name))
                        {
                            writer.WriteString(prop.Name, "****");
                        }
                        else
                        {
                            writer.WritePropertyName(prop.Name);
                            WriteMaskedElement(prop.Value, writer);
                        }
                    }
                    writer.WriteEndObject();
                    break;

                case JsonValueKind.Array:
                    writer.WriteStartArray();
                    foreach (var item in element.EnumerateArray())
                    {
                        WriteMaskedElement(item, writer);
                    }
                    writer.WriteEndArray();
                    break;

                default:
                    element.WriteTo(writer);
                    break;
            }
        }

        private static bool IsSensitiveKey(string key)
        {
            return Array.Exists(SensitiveKeys, k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
        }
    }
}
