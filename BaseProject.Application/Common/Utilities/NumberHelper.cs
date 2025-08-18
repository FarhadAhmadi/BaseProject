namespace BaseProject.Application.Common.Utilities
{
    public static class NumberHelper
    {
        private static readonly Random _random = new();

        /// <summary>
        /// Generates a random integer between min (inclusive) and max (exclusive).
        /// </summary>
        public static int GenerateRandom(int min, int max) => _random.Next(min, max);

        /// <summary>
        /// Generates a random double between min and max.
        /// </summary>
        public static double GenerateRandomDouble(double min, double max)
        {
            if (min > max) throw new ArgumentException("min must be less than or equal to max");
            return _random.NextDouble() * (max - min) + min;
        }
    }
}
