using ConsoleAdventure.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace ConsoleAdventure
{
    public static class Localization
    {
        static string[] localizeFiles = new string[2];

        public static void Load()
        {
            
            localizeFiles[0] = File.ReadAllText("Content\\Localization\\en.json");
            localizeFiles[1] = File.ReadAllText("Content\\Localization\\ru.json");

            for (int i = 0; i < localizeFiles.Length; i++)
            {
                //Localizations[i] = JsonSerializer.Deserialize<LocalizationStruct>(localizeFiles[i]);
            }
        }

        public static string GetLanguageName(int id)
        {
            return id == 0 ? GetTranslation("UI", "English")
                   : id == 1 ? GetTranslation("UI", "Russian")
                   : "None";
        }

        public static string GetTranslation(int language, string type, string key)
        {
            string languageName = language == 0 ? "English"
                                : language == 1 ? "Russian"
                                : "None";

            Dictionary<string, Dictionary<string, string>>[] Localizations = new Dictionary<string, Dictionary<string, string>>[2];

            Load();

            for (int i = 0; i < localizeFiles.Length; i++)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };


                Localizations[i] = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(localizeFiles[i], options);
            }

            if (language < 0 || language >= Localizations.Length)
            {
                Console.WriteLine($"Localization: language with index \"{language}\" was not found.");
                return "";
            }

            if (Localizations[language].TryGetValue(type, out var translations))
            {
                if (translations.TryGetValue(key, out var text))
                {
                    return text;
                }
                else
                {
                    Console.WriteLine($"Localization: key \"{key}\" in type \"{type}\" in language \"{languageName}\" was not found.");
                }
            }
            else
            {
                Console.WriteLine($"Localization: type \"{type}\" in language \"{languageName}\" was not found.");
            }

            return "";
        }

        public static string GetTranslation(string type, string key)
        {
            return GetTranslation(SettingsSystem.GetSetting("Options", "Language"), type, key);
        }
    }
}