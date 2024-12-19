using ConsoleAdventure.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using static System.Windows.Forms.Design.AxImporter;

namespace ConsoleAdventure
{
    public static class Localization
    {
        private static string[] localizeFiles = new string[2];

        public static Dictionary<string, Dictionary<string, string>>[] Localizations = new Dictionary<string, Dictionary<string, string>>[2];

        public static void Load()
        {
            localizeFiles[(int)Language.english] = File.ReadAllText("Content\\Localization\\en.json");
            localizeFiles[(int)Language.russian] = File.ReadAllText("Content\\Localization\\ru.json");

            Localizations = new Dictionary<string, Dictionary<string, string>>[2];

            for (int i = 0; i < localizeFiles.Length; i++)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };


                Localizations[i] = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(localizeFiles[i], options);
            }
        }

        internal static void LoadModTranslations(string path)
        {
            if (Directory.Exists(path))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string[] modLocalizeFiles = new string[2];

                modLocalizeFiles[(int)Language.english] = TryReadAllText(path + "\\en.json");
                modLocalizeFiles[(int)Language.russian] = TryReadAllText(path + "\\ru.json");

                for (int i = 0; i < localizeFiles.Length; i++)
                {
                    if (modLocalizeFiles[i] != null)
                    {
                        var translation = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(modLocalizeFiles[i], options);

                        if (translation != null)
                        {
                            for (int j = 0; j < translation.Count; j++)
                            {
                                var type = translation.ElementAt(j);
                                if (Localizations[i].ContainsKey(type.Key))
                                {
                                    var vanillaType = Localizations[i][type.Key];
                                    for (int k = 0; k < type.Value.Count; k++)
                                    {
                                        var obj = type.Value.ElementAt(k);
                                        if (vanillaType.ContainsKey(obj.Key))
                                        {
                                            vanillaType[obj.Key] = obj.Value;
                                        }

                                        else
                                        {
                                            vanillaType.Add(obj.Key, obj.Value);
                                        }
                                    }
                                }

                                else
                                {
                                    Localizations[i].Add(type.Key, type.Value);
                                }
                            }
                        }
                    }
                }
            }

            string TryReadAllText(string path)
            {
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }

                return null;
            }
        }

        public static string GetLanguageName(int id)
        {
            return id == (int)Language.english ? GetTranslation("UI", "English")
                   : id == (int)Language.russian ? GetTranslation("UI", "Russian")
                   : "None";
        }

        public static string GetTranslation(int language, string type, string key)
        {
            string languageName = language == (int)Language.english ? "English"
                                : language == (int)Language.russian ? "Russian"
                                : "None";

            if (language < 0 || language >= Localizations.Length)
            {
                ConsoleAdventure.logger.AddMessage($"Localization: language with index \"{language}\" was not found.");
                return "";
            }

            Dictionary<string, Dictionary<string, string>> allTranslations = Localizations[language];

            if (allTranslations != null && allTranslations.TryGetValue(type, out var translations))
            {
                if (translations.TryGetValue(key, out var text))
                {
                    return text;
                }
            }

            return "";
        }

        public static string GetTranslation(string type, string key)
        {
            return GetTranslation(SettingsSystem.GetSetting("Options", "Language"), type, key);
        }
    }
}
