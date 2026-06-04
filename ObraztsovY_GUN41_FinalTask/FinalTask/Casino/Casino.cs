using System;
using FinalTask.Casino.Games;
using FinalTask.Profiles;
using FinalTask.SaveLoad;
using FinalTask.Services;

namespace FinalTask.Casino
{
    public class Casino : IGame
    {
        private PlayerProfile _player;
        private readonly ProfileManager _profileManager;
        private readonly GameSelector _gameSelector;
        private readonly BetManager _betManager;
        private readonly BlackjackGame _blackjackGame;
        private readonly DiceGame _diceGame;
        private const int MaxBank = 1000000;

        public Casino(ISaveLoadService<string> saveLoadService)
        {
            var console = new ConsoleService();
            var random = new RandomService();

            _profileManager = new ProfileManager(saveLoadService, console);
            _gameSelector = new GameSelector(console);
            _betManager = new BetManager(console);

            _blackjackGame = new BlackjackGame(52, console, random);
            _diceGame = new DiceGame(2, 1, 6);

            SetupGameEvents();
        }

        private void SetupGameEvents()
        {
            _blackjackGame.OnWin += (win) => HandleGameResult(true, win);
            _blackjackGame.OnLose += (lose) => HandleGameResult(false, lose);
            _blackjackGame.OnDraw += () => HandleDraw();

            _diceGame.OnWin += (win) => HandleGameResult(true, win);
            _diceGame.OnLose += (lose) => HandleGameResult(false, lose);
            _diceGame.OnDraw += () => HandleDraw();
        }

        private void HandleGameResult(bool isWin, int amount)
        {
            if (isWin)
            {
                _player.ProcessWin(amount);

                if (_player.Bank > MaxBank)
                {
                    _player.Bank = MaxBank / 2;
                    Console.WriteLine("You wasted half of your bank money in casino's bar");
                }
            }
            else
            {
                _player.ProcessLose(amount);
            }
        }

        private void HandleDraw()
        {
            Console.WriteLine("Ничья! Ставка возвращена.");
        }

        public void StartGame()
        {
            Console.WriteLine("Добро пожаловать в казино Final Task!");


            _player = _profileManager.LoadOrCreateProfile();

            if (!_player.HasMoney())
            {
                Console.WriteLine("No money? Kicked!");
                return;
            }

            string choice = _gameSelector.SelectGame();

            if (!_betManager.GetBet(_player, out int bet))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    _blackjackGame.PlayGame(bet);
                    break;
                case "2":
                    _diceGame.PlayGame(bet);
                    break;
            }

            _profileManager.SaveProfile(_player);

            Console.WriteLine($"\nСпасибо за игру, {_player.Name}! Ваш итоговый банк: {_player.Bank}");
            Console.WriteLine("До свидания!");
        }
    }
}