using System.Text.Json.Serialization;

namespace BaseProject.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Admin,
        User
    }
}
