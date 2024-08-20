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
        public static Dictionary<string, Song> Songs {  get; private set; } = new Dictionary<string, Song>();
        public static string currentSong { get; private set; } = string.Empty;

        private static float targetVolume = 1.0f;
        private static float fadeSpeed = 0.01f;

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

        internal static void Start(string song)
        {
            currentSong = song;
            MediaPlayer.Play(Songs[currentSong]);
            MediaPlayer.IsRepeating = true;
        }

        public static async void ChangeSong(string newSong)
        {
            if (currentSong == newSong || !Songs.ContainsKey(newSong))
                return;

            // Плавное затухание текущего трека
            targetVolume = 0.0f;
            while (MediaPlayer.Volume > 0.0f)
            {
                await Task.Delay(10);
            }

            MediaPlayer.Stop();
            Start(newSong);

            targetVolume = 1.0f;
        }
    }
}
