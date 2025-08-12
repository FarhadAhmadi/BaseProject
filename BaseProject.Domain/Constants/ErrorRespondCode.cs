namespace BaseProject.Domain.Constants
{
    public static class ErrorRespondCode
    {
        public const string NOT_FOUND = "not_found";
        public const string VERSION_CONFLICT = "version_conflict";
        public const string ITEM_ALREADY_EXISTS = "item_exists";
        public const string CONFLICT = "conflict";
        public const string BAD_REQUEST = "bad_request";
        public const string UNAUTHORIZED = "unauthorized";
        public const string INTERNAL_ERROR = "internal_error";
        public const string GENERAL_ERROR = "general_error";
        public const string UNPROCESSABLE_ENTITY = "unprocessable_entity";
        public const string FORBIDDEN = "forbidden";               // Forbidden access
        public const string UNAUTHENTICATED = "unauthenticated";     // Unauthenticated request
        public const string TIMEOUT = "timeout";                     // Request timeout
        public const string SERVICE_UNAVAILABLE = "service_unavailable"; // Service unavailable
        public const string TOO_MANY_REQUESTS = "too_many_requests";  // Too many requests
        public const string INVALID_OPERATION = "invalid_operation";  // Invalid operation
        public const string UNSUPPORTED_MEDIA_TYPE = "unsupported_media_type"; // Unsupported media type
        public const string GATEWAY_TIMEOUT = "gateway_timeout";      // Gateway timeout
        public const string METHOD_NOT_ALLOWED = "method_not_allowed"; // Method not allowed
    }
}
