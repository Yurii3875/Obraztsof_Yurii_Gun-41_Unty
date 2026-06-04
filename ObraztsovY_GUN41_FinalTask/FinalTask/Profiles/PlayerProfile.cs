using System;

namespace FinalTask.Profiles
{
    public class PlayerProfile
    {
        public string Name { get; set; }
        public int Bank { get; set; }
        private const int MaxBank = 1000000;

        public PlayerProfile(string name, int initialBank = 1000)
        {
            Name = name;
            Bank = Math.Min(initialBank, MaxBank);
        }

        public bool CanPlaceBet(int bet)
        {
            return bet > 0 && bet <= Bank;
        }

        public void ProcessWin(int winAmount)
        {
            if (Bank + winAmount > MaxBank)
            {
                int excess = Bank + winAmount - MaxBank;
                Bank = MaxBank;
                Console.WriteLine($"Вы разорили казино! На его месте построят новое. Ваш остаток: {excess}");
            }
            else
            {
                Bank += winAmount;
            }
        }

        public void ProcessLose(int loseAmount)
        {
            Bank = Math.Max(0, Bank - loseAmount);
        }

        public bool HasMoney()
        {
            return Bank > 0;
        }
    }
}