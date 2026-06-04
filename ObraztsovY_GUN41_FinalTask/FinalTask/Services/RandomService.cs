using System;

namespace FinalTask.Services
{
    public class RandomService : IRandomService
    {
        private readonly Random _random = new Random();

        public int Next(int min, int max) => _random.Next(min, max);
    }
}