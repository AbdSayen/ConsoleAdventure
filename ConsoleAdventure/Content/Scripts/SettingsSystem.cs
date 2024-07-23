using SharpDX.MediaFoundation;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace ConsoleAdventure
{
    public class SettingsSystem
    {
        private static Dictionary<string, Dictionary<string, int>> settings = new Dictionary<string, Dictionary<string, int>>(); // Настройки

        public static void InitSetting(string type, string key)
        {
            if (!settings.ContainsKey(type)) // Если еще нет типа настроек то создать его
                settings.Add(type, new Dictionary<string, int>());

            if (!settings[type].ContainsKey(key)) // Если еще нет ключа настроек то создать его
                settings[type].Add(key, 0);
                
        }

        public static void SetSetting(string type, string key, int value)
        {
            settings[type][key] = value; // По типу и ключу установить значение
        }

        public static dynamic GetSetting(string type, string key)
        {
            if (settings.ContainsKey(type)) // Если есть тип
                if (settings[type].ContainsKey(key)) //      и ключ
                    return settings[type][key]; // Вернуть значение

            return null; // Иначе вернуть NULL
        }

        public static void SaveSettings()
        {
            string fileName = Program.savePath + "settings.json"; // Путь к файлу настроек
            string jsonString = JsonSerializer.Serialize(settings); // Делаем жисон :)
            File.WriteAllText(fileName, jsonString); // Сохранить жисон :)
        }

        public static void LoadSettings()
        {
            string fileName = Program.savePath + "settings.json"; // Путь к файлу настроек
            string json = File.ReadAllText(fileName); // Читаем жисон
            var settingsLoaded = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(json); // Делаем из жисона наш словарь
            settings = settingsLoaded; // И востанавливаем настройки
        }
    }
}
