namespace BaseProject.Application.Common.Utilities
{
    #region DateTime Utilities
    #endregion

    public static class Guard
    {
        public static void NotNull(object? obj, string paramName)
        {
            if (obj == null) throw new ArgumentNullException(paramName);
        }

        public static void NotEmpty(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} cannot be empty.", paramName);
        }

        public static void Positive(int value, string paramName)
        {
            if (value <= 0) throw new ArgumentException($"{paramName} must be positive.", paramName);
        }
    }
}
