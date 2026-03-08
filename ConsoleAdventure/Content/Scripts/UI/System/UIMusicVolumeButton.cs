using ConsoleAdventure.Content.Scripts.Audio;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIMusicVolumeButton : UIButton
    {
        int selectedLanguage = SettingsSystem.GetSetting("Options", "Language");
        bool relocalized = false;
        public UIMusicVolumeButton() : base("", Color.White, Color.Yellow, new())
        {
            SetText(Localization.GetTranslation("UI", "MusicVolume") + MusicEngine.GetVolumePercent().ToString() + "%");
        }

        public override void Update()
        {
            if (!IsHovered()) return;

            if (Input.PostClick(InputConfig.NavigationRight))
            {
                if (MusicEngine.gameVolume < 1f)
                {
                    MusicEngine.SetVolume((MusicEngine.GetVolumePercent() + 5) / 100f);
                    SetText(Localization.GetTranslation("UI", "MusicVolume") + MusicEngine.GetVolumePercent().ToString() + "%");
                }
            }

            if (Input.PostClick(InputConfig.NavigationLeft))
            {
                if (MusicEngine.gameVolume > 0f)
                {
                    MusicEngine.SetVolume((MusicEngine.GetVolumePercent() - 5) / 100f);
                    SetText(Localization.GetTranslation("UI", "MusicVolume") + MusicEngine.GetVolumePercent().ToString() + "%");
                }
            }
        }
    }
}
