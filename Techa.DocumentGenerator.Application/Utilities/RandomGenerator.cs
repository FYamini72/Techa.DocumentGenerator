namespace Techa.DocumentGenerator.Application.Utilities
{
    public static class RandomGenerator
    {
        public static int GenerateRandomInt(int min = 1000, int max = 9999)
        {
            var random = new Random();
            return random.Next(min, max);
        }
    }
}
