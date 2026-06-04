using System;

namespace FinalTask.Casino.Games.Dice
{
    public class WrongDiceNumberException : Exception
    {
        public WrongDiceNumberException(int number, int min, int max)
            : base($"Number {number} is out of range. Allowed range: from {min} to {max}.")
        {
        }
    }
}