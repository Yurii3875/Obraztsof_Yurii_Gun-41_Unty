using System;
using FinalTask.Profiles;
using FinalTask.SaveLoad;
using FinalTask.Services;

namespace FinalTask.Casino
{
    public class ProfileManager
    {
        private readonly ISaveLoadService<string> _saveLoadService;
        private readonly IConsoleService _console;

        public ProfileManager(ISaveLoadService<string> saveLoadService, IConsoleService console)
        {
            _saveLoadService = saveLoadService;
            _console = console;
        }

        public PlayerProfile LoadOrCreateProfile()
        {
            string name;
            do
            {
                _console.Write("Введите ваше имя: ");
                name = _console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    _console.WriteLine("Имя не может быть пустым!");
                }
            } while (string.IsNullOrEmpty(name));

            string data = _saveLoadService.LoadData(name);
            if (data != null)
            {
                var parts = data.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int bank))
                {
                    var profile = new PlayerProfile(parts[0], bank);
                    _console.WriteLine($"Добро пожаловать обратно, {profile.Name}! Ваш банк: {profile.Bank}");
                    return profile;
                }
            }

            var newProfile = new PlayerProfile(name, 1000);
            _console.WriteLine($"Создан новый профиль. Ваш начальный банк: {newProfile.Bank}");
            return newProfile;
        }

        public void SaveProfile(PlayerProfile profile)
        {
            string data = $"{profile.Name},{profile.Bank}";
            _saveLoadService.SaveData(data, profile.Name);
        }
    }
}