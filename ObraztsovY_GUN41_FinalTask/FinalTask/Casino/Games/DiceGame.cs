using System;
using System.Collections.Generic;
using DiceStruct = FinalTask.Casino.Games.Dice.Dice;
using DiceException = FinalTask.Casino.Games.Dice.WrongDiceNumberException;

namespace FinalTask.Casino.Games
{
    public class DiceGame : CasinoGameBase
    {
        private readonly int _numberOfDice;
        private readonly int _min;
        private readonly int _max;
        private List<DiceStruct> _dice;

        public DiceGame(int numberOfDice, int min, int max)
            : base()
        {
            if (numberOfDice <= 0)
                throw new ArgumentException("Number of dice must be positive", nameof(numberOfDice));
            if (min < 1 || max > int.MaxValue || min > max)
                throw new DiceException(min < 1 ? min : max, 1, int.MaxValue);

            _numberOfDice = numberOfDice;
            _min = min;
            _max = max;
            FactoryMethod();
        }

        protected override void FactoryMethod()
        {
            _dice = new List<DiceStruct>();
            for (int i = 0; i < _numberOfDice; i++)
            {
                _dice.Add(new DiceStruct(_min, _max));
            }
        }

        private int RollAllDice()
        {
            int total = 0;
            foreach (var dice in _dice)
            {
                int roll = dice.Number;
                total += roll;
            }
            return total;
        }

        public override void PlayGame(int bet)
        {
            Console.WriteLine("Бросаем кости...");

            System.Threading.Thread.Sleep(10);

            int playerRoll = RollAllDice();
            Console.WriteLine($"Ваш бросок: {playerRoll}");

            System.Threading.Thread.Sleep(10);

            int dealerRoll = RollAllDice();
            Console.WriteLine($"Бросок дилера: {dealerRoll}");

            if (playerRoll > dealerRoll)
            {
                Console.WriteLine("Вы выиграли!");
                OnWinInvoke(bet);
            }
            else if (playerRoll < dealerRoll)
            {
                Console.WriteLine("О нет! Дилер выиграл...");
                OnLoseInvoke(bet);
            }
            else
            {
                Console.WriteLine("Ничья!");
                OnDrawInvoke();
            }
        }
    }
}