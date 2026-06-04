using System;
using System.Collections.Generic;
using System.Linq;
using FinalTask.Casino.Games.Cards;
using FinalTask.Services;

namespace FinalTask.Casino.Games
{
    public class BlackjackGame : CasinoGameBase
    {
        private Queue<Card> _deck;
        private readonly int _numberOfCards;
        private readonly IConsoleService _console;
        private readonly IRandomService _random;

        public BlackjackGame(int numberOfCards, IConsoleService console = null, IRandomService random = null)
        {
            if (numberOfCards <= 0)
                throw new ArgumentException("Number of cards must be positive", nameof(numberOfCards));

            _numberOfCards = numberOfCards;
            _console = console ?? new ConsoleService();
            _random = random ?? new RandomService();
            FactoryMethod();
        }

        protected override void FactoryMethod()
        {
            var cards = new List<Card>();
            var suits = (Suit[])Enum.GetValues(typeof(Suit));
            var ranks = (Rank[])Enum.GetValues(typeof(Rank));

            int cardsCreated = 0;
            while (cardsCreated < _numberOfCards)
            {
                foreach (var suit in suits)
                {
                    foreach (var rank in ranks)
                    {
                        if (cardsCreated >= _numberOfCards) break;
                        cards.Add(new Card(suit, rank));
                        cardsCreated++;
                    }
                    if (cardsCreated >= _numberOfCards) break;
                }
            }

            Shuffle(cards);
            _deck = new Queue<Card>(cards);
        }

        private void Shuffle(List<Card> cards)
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(0, n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        private Card DrawCard()
        {
            if (_deck == null || _deck.Count < 10)
            {
                FactoryMethod();
            }

            return _deck.Dequeue();
        }

        private int CalculateScore(List<Card> cards)
        {
            int score = 0;
            int aces = 0;

            foreach (var card in cards)
            {
                if (card.Rank >= Rank.Six && card.Rank <= Rank.Ten)
                {
                    score += (int)card.Rank;
                }
                else if (card.Rank >= Rank.Jack && card.Rank <= Rank.King)
                {
                    score += 10;
                }
                else if (card.Rank == Rank.Ace)
                {
                    aces++;
                    score += 11;
                }
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }

        public override void PlayGame(int bet)
        {
            _console.WriteLine($"Ваши карты: ...");

            var playerCards = new List<Card> { DrawCard(), DrawCard() };
            var dealerCards = new List<Card> { DrawCard(), DrawCard() };

            int playerScore = CalculateScore(playerCards);
            int dealerScore = CalculateScore(dealerCards);

            _console.WriteLine($"Ваши карты: {string.Join(", ", playerCards)}. Сумма: {playerScore}");
            _console.WriteLine($"Карта дилера: {dealerCards[0]}");

            while (playerScore < 21)
            {
                _console.Write("Взять еще карту? (y/n): ");
                string input = _console.ReadLine()?.ToLower();

                if (input == "y")
                {
                    playerCards.Add(DrawCard());
                    playerScore = CalculateScore(playerCards);
                    _console.WriteLine($"Ваши карты: {string.Join(", ", playerCards)}. Сумма: {playerScore}");
                }
                else if (input == "n")
                {
                    break;
                }
                else
                {
                    _console.WriteLine("Введите 'y' или 'n'!");
                }
            }

            while (dealerScore < 17)
            {
                dealerCards.Add(DrawCard());
                dealerScore = CalculateScore(dealerCards);
            }

            _console.WriteLine($"Карты дилера: {string.Join(", ", dealerCards)}. Сумма: {dealerScore}");

            if (playerScore > 21)
            {
                _console.WriteLine("Перебор! Вы проиграли.");
                OnLoseInvoke(bet);
            }
            else if (dealerScore > 21 || playerScore > dealerScore)
            {
                _console.WriteLine("Вы выиграли!");
                OnWinInvoke(bet);
            }
            else if (playerScore < dealerScore)
            {
                _console.WriteLine("Дилер выиграл!");
                OnLoseInvoke(bet);
            }
            else
            {
                _console.WriteLine("Ничья!");
                OnDrawInvoke();
            }
        }
    }
}