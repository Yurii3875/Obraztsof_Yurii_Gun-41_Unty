using System;
using FinalTask.SaveLoad;

namespace FinalTask
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var saveLoadService = new FileSystemSaveLoadService("SaveData");
                var casino = new FinalTask.Casino.Casino(saveLoadService);
                casino.StartGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}