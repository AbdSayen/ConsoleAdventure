using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Audio
{
    public static class MusicEngine
    {
        public static Dictionary<string, Song> Songs { get; private set; } = new Dictionary<string, Song>();
        public static string currentSong { get; private set; } = string.Empty;
        private static int currentPriority = -1;

        private static float targetVolume = 1.0f;
        private static float fadeSpeed = 0.01f;

        public static float gameVolume { get; private set; } = 0f;


        public static void Setup()
        {
            gameVolume = (float)SettingsSystem.GetSetting("Options", "MusicVolume") / 100f;
        }
        public static void AddSong(string songName, Song song)
        {
            Songs.Add(songName, song);
        }

        public static void Update()
        {
            if (MediaPlayer.Volume < targetVolume)
            {
                MediaPlayer.Volume += fadeSpeed;
                if (MediaPlayer.Volume > targetVolume)
                    MediaPlayer.Volume = targetVolume;
            }
            else if (MediaPlayer.Volume > targetVolume)
            {
                MediaPlayer.Volume -= fadeSpeed;
                if (MediaPlayer.Volume < targetVolume)
                    MediaPlayer.Volume = targetVolume;
            }
        }

        public static void SetVolume(float volume)
        {
            if (gameVolume > 0)
            {
                targetVolume /= gameVolume;
            }

            gameVolume = volume;
            targetVolume *= gameVolume;
            if (targetVolume == 0 && volume > 0)
            {
                targetVolume = volume;
            }

            SettingsSystem.SetSetting("Options", "MusicVolume", GetVolumePercent());
        }

        public static int GetVolumePercent()
        {
            return (int)(MusicEngine.gameVolume * 100);
        }

        internal static void Start(string song)
        {
            currentSong = song;
            MediaPlayer.Play(Songs[currentSong]);
            MediaPlayer.IsRepeating = true;
        }

        public static async void ChangeSong(string newSong, int priority = 0, float volume = 1f)
        {
            if (currentSong == newSong || !Songs.ContainsKey(newSong) || priority < currentPriority)
                return;

            volume = Math.Clamp(volume, 0f, 1f);

            currentPriority = priority;

            // Плавное затухание текущего трека
            targetVolume = 0.0f;
            while (MediaPlayer.Volume > 0.0f)
            {
                await Task.Delay(10);
            }

            MediaPlayer.Stop();
            Start(newSong);

            targetVolume = volume * gameVolume;
        }

        public static async void StopSong()
        {
            currentPriority = -1;
            targetVolume = 0.0f;
            while (MediaPlayer.Volume > 0.0f)
            {
                await Task.Delay(10);
            }

            MediaPlayer.Stop();
        }
    }
}