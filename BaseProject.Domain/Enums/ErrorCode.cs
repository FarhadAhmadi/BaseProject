namespace BaseProject.Domain.Enums
{
    public enum ErrorCode
    {
        ItemAlreadyExists = 7,
        VersionConflict = 1, // NuGet package versions different
        NotFound = 2,
        BadRequest = 3,
        Conflict = 4,
        Other = 5,
        Unauthorized = 6,
        Internal = 0,
        UnprocessableEntity = 8,
        Forbidden = 9,            // Access forbidden
        Unauthenticated = 10,     // Unauthenticated request
        Timeout = 11,             // Request timeout
        ServiceUnavailable = 12,  // Service unavailable
        TooManyRequests = 13,     // Too many requests
        InvalidOperation = 14,    // Invalid operation
        UnsupportedMediaType = 15, // Unsupported media type
        GatewayTimeout = 16,      // Gateway timeout
        MethodNotAllowed = 17    // Method not allowed
    }
}
