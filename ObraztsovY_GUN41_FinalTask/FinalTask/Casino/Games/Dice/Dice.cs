using System;

namespace FinalTask.Casino.Games.Dice
{
    public struct Dice
    {
        private readonly int _min;
        private readonly int _max;
        private static readonly Random _random = new Random();

        public int Number => _random.Next(_min, _max + 1);

        public Dice(int min, int max)
        {
            if (min < 1 || max > int.MaxValue || min > max)
                throw new WrongDiceNumberException(min < 1 ? min : max, 1, int.MaxValue);

            _min = min;
            _max = max;
        }
    }
}