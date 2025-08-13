namespace BaseProject.Domain.Constants
{
    public static class DatabaseTableNames
    {
        public static readonly Dictionary<string, string> Tables = new()
    {
        { nameof(Book), "Books" }
    };

        public const string Book = "Books";
        public const string Category = "Categories";
        public const string Author = "Authors";
        public const string Publisher = "Publishers";
    }

}
