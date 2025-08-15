using System.Text.Json.Serialization;

namespace BaseProject.Shared.DTOs.Common
{
    public class ResponseDto<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        [JsonConstructor]
        private ResponseDto(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ResponseDto<T> SuccessResponse(T data, string message = ResponseMessages.DefaultSuccess) =>
            new(true, message, data);

        public static ResponseDto<T> FailResponse(string message, T? data = default) =>
            new(false, message, data);
    }

    public class ResponseDto
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        [JsonConstructor]
        private ResponseDto(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
        }

        public static ResponseDto SuccessResponse(string message = ResponseMessages.DefaultSuccess) =>
            new(true, message);

        public static ResponseDto FailResponse(string message = ResponseMessages.DefaultFailure) =>
            new(false, message);
    }

    public static class ResponseMessages
    {
        public const string DefaultSuccess = "Success";
        public const string DefaultFailure = "Failure";
    }
}
