namespace BaseProject.Shared.DTOs.Common
{
    public class ResponseDto<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        private ResponseDto(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ResponseDto<T> SuccessResponse(T data, string message = "Success") =>
            new(true, message, data);

        public static ResponseDto<T> Fail(string message) =>
            new(false, message, default);
    }
    public class ResponseDto
    {
        public bool Success { get; }
        public string Message { get; }

        private ResponseDto(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static ResponseDto Fail(string message) =>
            new(false, message);
    }

}
