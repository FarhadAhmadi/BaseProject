namespace BaseProject.Domain.Constants
{
    public static class ErrorMessage
    {
        // 1xx Informational
        public const string Continue = "The request has been received and the process is continuing."; // 100
        public const string SwitchingProtocols = "The server is switching protocols as requested by the client."; // 101
        public const string Processing = "The server has received the request but has not yet completed it."; // 102
        public const string EarlyHints = "The server is returning early hints for preloading resources."; // 103

        // 2xx Success
        public const string OK = "The request has succeeded."; // 200
        public const string Created = "The request has succeeded and a new resource has been created."; // 201
        public const string Accepted = "The request has been accepted for processing, but is not yet completed."; // 202
        public const string NonAuthoritativeInformation = "The returned meta-information is from a local or third-party copy."; // 203
        public const string NoContent = "The request has succeeded, but there is no content to return."; // 204
        public const string ResetContent = "The request has succeeded, and the user agent should reset the document view."; // 205
        public const string PartialContent = "The server is delivering only part of the resource due to a range header."; // 206
        public const string MultiStatus = "Multiple responses for multiple independent operations."; // 207
        public const string AlreadyReported = "The members of a DAV binding have already been enumerated in a previous reply."; // 208
        public const string IMUsed = "The server has fulfilled a GET request for the resource, and the response is a representation of the result of one or more instance-manipulations."; // 226

        // 3xx Redirection
        public const string MultipleChoices = "There are multiple options for the resource from which the client may choose."; // 300
        public const string MovedPermanently = "The resource has been permanently moved to a new URI."; // 301
        public const string Found = "The resource is temporarily located at a different URI."; // 302
        public const string SeeOther = "The response to the request can be found under another URI using a GET method."; // 303
        public const string NotModified = "The resource has not been modified since the last request."; // 304
        public const string UseProxy = "The requested resource must be accessed through the proxy given by the Location field."; // 305
        public const string TemporaryRedirect = "The request should be repeated with another URI temporarily."; // 307
        public const string PermanentRedirect = "The request and all future requests should be repeated using another URI."; // 308

        // 4xx Client Errors
        public const string BadRequest = "The server could not understand the request due to invalid syntax."; // 400
        public const string Unauthorized = "Authentication is required to access this resource."; // 401
        public const string PaymentRequired = "Payment is required to access this resource."; // 402
        public const string Forbidden = "The client does not have access rights to the content."; // 403
        public const string NotFound = "The requested resource could not be found."; // 404
        public const string MethodNotAllowed = "The request method is not allowed for the requested resource."; // 405
        public const string NotAcceptable = "The requested resource is not capable of generating content acceptable according to the Accept headers."; // 406
        public const string ProxyAuthenticationRequired = "Authentication with the proxy is required."; // 407
        public const string RequestTimeout = "The server timed out waiting for the request."; // 408
        public const string Conflict = "The request conflicts with the current state of the resource."; // 409
        public const string Gone = "The resource requested is no longer available and will not be available again."; // 410
        public const string LengthRequired = "The request did not specify the length of its content."; // 411
        public const string PreconditionFailed = "One or more preconditions given in the request header fields evaluated to false."; // 412
        public const string PayloadTooLarge = "The request payload is too large."; // 413
        public const string URITooLong = "The URI requested is too long."; // 414
        public const string UnsupportedMediaType = "The request media type is not supported by the server."; // 415
        public const string RangeNotSatisfiable = "The range specified cannot be fulfilled."; // 416
        public const string ExpectationFailed = "The server cannot meet the requirements of the Expect request-header field."; // 417
        public const string ImATeapot = "The server refuses to brew coffee because it is a teapot."; // 418
        public const string MisdirectedRequest = "The request was directed at a server that is not able to produce a response."; // 421
        public const string UnprocessableEntity = "The request was well-formed but could not be followed due to semantic errors."; // 422
        public const string Locked = "The resource that is being accessed is locked."; // 423
        public const string FailedDependency = "The request failed due to failure of a previous request."; // 424
        public const string TooEarly = "The server is unwilling to risk processing a request that might be replayed."; // 425
        public const string UpgradeRequired = "The client should switch to a different protocol."; // 426
        public const string PreconditionRequired = "The server requires the request to be conditional."; // 428
        public const string TooManyRequests = "The user has sent too many requests in a given amount of time."; // 429
        public const string RequestHeaderFieldsTooLarge = "The server is unwilling to process the request because its header fields are too large."; // 431
        public const string UnavailableForLegalReasons = "The resource is unavailable due to legal reasons."; // 451

        // 5xx Server Errors
        public const string InternalServerError = "Something went wrong. Please try again later."; // 500
        public const string NotImplemented = "The server does not support the functionality required to fulfill the request."; // 501
        public const string BadGateway = "The server received an invalid response from the upstream server."; // 502
        public const string ServiceUnavailable = "The server is not ready to handle the request."; // 503
        public const string GatewayTimeout = "The server did not receive a timely response from the upstream server."; // 504
        public const string HTTPVersionNotSupported = "The HTTP version used in the request is not supported by the server."; // 505
        public const string VariantAlsoNegotiates = "The server has an internal configuration error with content negotiation."; // 506
        public const string InsufficientStorage = "The server is unable to store the representation needed to complete the request."; // 507
        public const string LoopDetected = "The server detected an infinite loop while processing the request."; // 508
        public const string NotExtended = "Further extensions to the request are required for the server to fulfill it."; // 510
        public const string NetworkAuthenticationRequired = "The client needs to authenticate to gain network access."; // 511

        // Application-specific codes
        public const string ItemAlreadyExists = "The item already exists and cannot be created again."; // 1000
        public const string VersionConflict = "The item version does not match the current version, causing a conflict."; // 1001
        public const string InvalidOperation = "The requested operation is invalid or cannot be performed."; // 1002
        public const string AppConfigurationMessage = "Unable to retrieve application settings."; // 1003
        public const string TransactionNotCommit = "The transaction could not be committed."; // 1004
        public const string TransactionNotExecute = "The transaction could not be executed."; // 1005
    }
}
