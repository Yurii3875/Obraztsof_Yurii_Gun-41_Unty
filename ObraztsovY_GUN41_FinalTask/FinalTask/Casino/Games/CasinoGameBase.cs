using System;

namespace FinalTask.Casino.Games
{
    public abstract class CasinoGameBase
    {
        public event Action<int> OnWin;
        public event Action<int> OnLose;
        public event Action OnDraw;

        protected CasinoGameBase()
        {
            FactoryMethod();
        }

        protected abstract void FactoryMethod();

        protected void OnWinInvoke(int winAmount)
        {
            OnWin?.Invoke(winAmount);
        }

        protected void OnLoseInvoke(int loseAmount)
        {
            OnLose?.Invoke(loseAmount);
        }

        protected void OnDrawInvoke()
        {
            OnDraw?.Invoke();
        }

        public abstract void PlayGame(int bet);
    }
}