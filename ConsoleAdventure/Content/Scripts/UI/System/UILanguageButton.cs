using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UILanguageButton : UIButton
    {
        int selectedLanguage = SettingsSystem.GetSetting("Options", "Language");
        bool relocalized = false;
        public UILanguageButton() : base("", Color.White, Color.Yellow, new())
        {
            SetText(Localization.GetTranslation("UI", "Language") + Localization.GetLanguageName(selectedLanguage));
        }

        private void Relocalize()
        {
            SettingsSystem.SetSetting("Options", "Language", selectedLanguage);
            TextAssets.UpdateLabels();
            MainMenu menu = (MainMenu)GetParent().GetParent();
            menu.Relocalize();
        }

        public override void Update()
        {
            if (!IsHovered()) return;

            if (Input.PostClick(InputConfig.NavigationRight))
            {
                selectedLanguage = ++selectedLanguage % Localization.Localizations.Length;
                Relocalize();
            }

            if (Input.PostClick(InputConfig.NavigationLeft))
            {
                if (--selectedLanguage < 0) selectedLanguage = Localization.Localizations.Length - 1;
                Relocalize();
            }
        }
    }
}
