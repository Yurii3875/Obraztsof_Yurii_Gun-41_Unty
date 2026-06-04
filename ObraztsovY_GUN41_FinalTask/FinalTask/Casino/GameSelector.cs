using System;
using FinalTask.Services;

namespace FinalTask.Casino
{
    public class GameSelector
    {
        private readonly IConsoleService _console;

        public GameSelector(IConsoleService console)
        {
            _console = console;
        }

        public string SelectGame()
        {
            while (true)
            {
                _console.WriteLine("\nВыберите игру:");
                _console.WriteLine("1. Блэкджек (21)");
                _console.WriteLine("2. Игра в кости");
                _console.Write("Ваш выбор (1 или 2): ");

                string choice = _console.ReadLine();

                if (choice == "1" || choice == "2")
                {
                    return choice;
                }

                _console.WriteLine("Неверный выбор! Введите 1 или 2.");
            }
        }
    }
}