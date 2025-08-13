namespace BaseProject.Shared.DTOs.Common
{
    public class ErrorDto(string code = null, string message = null)
    {
        public string Code { get; set; } = code;
        public string Message { get; set; } = message;
        public string Property { get; set; }

        public void AddErrorProperty(ErrorProperty property)
        {
            Property = property.Property;
        }
    }
    public class ErrorProperty
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public ErrorProperty()
        {

        }
        public ErrorProperty(string property, string value)
        {
            Property = property;
            Value = value;
        }
    }
    public class ErrorResponse
    {
        public IEnumerable<ErrorDto> Errors { get; set; } = [];
        public ErrorResponse()
        {

        }
        public ErrorResponse(IEnumerable<ErrorDto> errors)
        {
            Errors = errors;
        }
    }
}