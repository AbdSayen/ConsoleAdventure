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

        private static float maxVolume = 1.0f;

        public static float MaxVolume 
        {
            get
            {
                return maxVolume;
            }

            set
            {
                if(value > 1) maxVolume = 1;
                else if (value < 0) maxVolume = 0;
                else maxVolume = value;
            }
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

        internal static void Start(string song)
        {
            currentSong = song;
            MediaPlayer.Play(Songs[currentSong]);
            MediaPlayer.IsRepeating = true;
        }

        public static async void ChangeSong(string newSong, int priority = 0)
        {
            if (currentSong == newSong || !Songs.ContainsKey(newSong) || priority < currentPriority)
                return;

            currentPriority = priority;

            // Плавное затухание текущего трека
            targetVolume = 0.0f;
            while (MediaPlayer.Volume > 0.0f)
            {
                await Task.Delay(10);
            }

            MediaPlayer.Stop();
            Start(newSong);

            targetVolume = maxVolume;
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
