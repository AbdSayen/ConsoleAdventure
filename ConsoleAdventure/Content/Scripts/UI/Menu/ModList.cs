using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class ModList : ListUI
    {

        public ModList(string text, Vector2 position, List<BaseUI> elements, Color color) : base(text, position, elements, color)
        {
            height = 34 * 3;
        }

        public override async void ElementHandleInput(BaseUI element, int timer)
        {
            ModPanel modPanel = element as ModPanel;

            if (Input.OnClick(InputConfig.NavigationSelect) && timer >= 5)
            {
                if (modPanel.cursor == 0)
                {
                    if (!ConsoleAdventure.menu.onlineMods)
                    {
                        modPanel.enabled = !modPanel.enabled;

                        if (modPanel.enabled)
                        {
                            CaModLoader.EnableMod(modPanel.mod.dirName);
                        }
                        else
                        {
                            CaModLoader.DisableMod(modPanel.mod.dirName);
                        }
                    }
                    else
                    {
                        // Seting up the http client used to download the data
                        using (var client = new HttpClient())
                        {
                            //client.Timeout = TimeSpan.FromMinutes(5);

                            await CaModLoader.DownloadMod(modPanel.mod.dirName);
                        }
                    }
                    
                    timer = 0;
                }

                else if (modPanel.cursor == 1)
                {
                    StringBuilder description = new();

                    if (modPanel.mod is Mod)
                    {
                        Mod mod = (Mod)modPanel.mod;

                        if (mod.ItemsCount > 0 || mod.TransformsCount > 0 || mod.EntitiesCount > 0 || mod.BuffsCount > 0)
                        {
                            description.Append(Localization.GetTranslation("UI", "ModContentCounts") + "\n");
                            
                            if (mod.ItemsCount > 0) description.Append(GetCountedText(mod.ItemsCount, "Item"));
                            if (mod.TransformsCount > 0) description.Append(GetCountedText(mod.TransformsCount, "Transform"));
                            if (mod.EntitiesCount > 0) description.Append(GetCountedText(mod.EntitiesCount, "Entity", "Entities"));
                            if (mod.BuffsCount > 0) description.Append(GetCountedText(mod.BuffsCount, "Item"));
                            
                            description.Append('\n');
                        }
                    }

                    description.Append(modPanel.mod.modDescription);

                    ConsoleAdventure.menu.OpenModDescription(description.ToString());
                    timer = 0;
                }
            }

            if (isHover)
            {
                if (Input.OnClick(InputConfig.NavigationRight))
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        ((ModPanel)elements[i]).cursor = 1;
                    }
                }                
                
                if (Input.OnClick(InputConfig.NavigationLeft))
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        ((ModPanel)elements[i]).cursor = 0;
                    }
                }
            }
        }

        private string GetCountedText(int count, string baseLocalizeKey, string localizeKeyMany = "")
        {
            
            string text = $"{count} {Localization.GetTranslation("Generic", count > 1 ? "News" : "New")} ";
            Language language = (Language)SettingsSystem.GetSetting("Options", "Language");

            if (language == Language.english)
            {
                if (count > 1) text += Localization.GetTranslation("Generic", localizeKeyMany == "" ? $"{baseLocalizeKey}s" : localizeKeyMany);
                else text += Localization.GetTranslation("Generic", baseLocalizeKey);
            }

            else if (language == Language.russian)
            {
                int firstDigit = count % 10;
                int secondDigit = (count / 10) % 10;

                if (secondDigit == 1 || firstDigit == 0 || firstDigit > 4) 
                    text += Localization.GetTranslation("Generic", (localizeKeyMany == "" ? $"{baseLocalizeKey}s" : localizeKeyMany) + "-genitive");
                
                else if (firstDigit > 1 || firstDigit <= 4) text += Localization.GetTranslation("Generic", baseLocalizeKey + "-genitive");
                else text += Localization.GetTranslation("Generic", baseLocalizeKey);
            }

            return "- "+ text.ToLower() + "\n";
        }
    }
}
