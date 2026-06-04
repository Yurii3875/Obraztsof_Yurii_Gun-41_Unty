using System;
using FinalTask.Profiles;
using FinalTask.Services;

namespace FinalTask.Casino
{
    public class BetManager
    {
        private readonly IConsoleService _console;

        public BetManager(IConsoleService console)
        {
            _console = console;
        }

        public bool GetBet(PlayerProfile player, out int bet)
        {
            bet = 0;

            if (!player.HasMoney())
            {
                _console.WriteLine("No money? Kicked!");
                return false;
            }

            _console.WriteLine($"Ваш банк: {player.Bank}");

            while (true)
            {
                _console.Write("Введите вашу ставку: ");
                string input = _console.ReadLine();

                if (int.TryParse(input, out bet) && bet > 0 && bet <= player.Bank)
                {
                    return true;
                }

                _console.WriteLine($"Неверная ставка! Введите число от 1 до {player.Bank}");
            }
        }
    }
}