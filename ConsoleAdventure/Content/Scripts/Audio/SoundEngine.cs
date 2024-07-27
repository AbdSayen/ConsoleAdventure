using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Audio
{
    public static class SoundEngine
    {
        public static Wave SineWave => new Sine();
        public static Wave SquareWave => new Square();
        public static Wave TriangleWave => new Triangle();
        public static Wave SawtootWave => new Sawtoot();
        public static Wave WhiteNoiseWave => new WhiteNoise();
        public static Wave FadeOutWhiteNoiseWave => new FadeOutWhiteNoise(); // 500 ms - max
        public static Wave FadeInWhiteNoiseWave => new FadeInWhiteNoise(); // 500 ms - max

        public static Wave BubbleWave => new Bubble(); // 500 ms - max

        static int sampleRate = 44100; // Частота дискретизации 
        static int samplesPerBuffer = 44100; // Количество семплов в одном буфере
        static List<SoundBuffer> activeBuffers = new List<SoundBuffer>();
        static AudioChannels channel;
        static readonly object lockObj = new object(); // объект для блокировки

        public static void Init(int sampleRate, int samplesPerBuffer, AudioChannels channel)
        {
            SoundEngine.sampleRate = sampleRate;
            SoundEngine.samplesPerBuffer = samplesPerBuffer;
            SoundEngine.channel = channel;
            Console.WriteLine($"SoundEngine initialized with sampleRate: {sampleRate}, samplesPerBuffer: {samplesPerBuffer}");
        }

        public static async Task<SoundBuffer> PlaySoundAsync(Wave wave, float frequency = 440f, float amplitude = 0.25f)
        {
            return await Task.Run(() =>
            {
                lock (lockObj)
                {
                    var soundEffectInstance = new DynamicSoundEffectInstance(sampleRate, channel);
                    if (soundEffectInstance == null)
                    {
                        Console.WriteLine("Failed to create DynamicSoundEffectInstance.");
                        return null;
                    }

                    SoundBuffer soundBuffer = new SoundBuffer(soundEffectInstance, sampleRate, samplesPerBuffer, wave, frequency, amplitude);
                    soundEffectInstance.BufferNeeded += soundBuffer.OnBufferNeeded;
                    activeBuffers.Add(soundBuffer);
                    soundEffectInstance.Play();
                    Console.WriteLine($"Playing sound: {wave.GetType().Name}, Frequency: {frequency}, Amplitude: {amplitude}");
                    return soundBuffer;
                }
            });
        }

        public static async Task PlaySound(Wave wave, float frequency, float amplitude, TimeSpan duration)
        {
            var soundBuffer = await PlaySoundAsync(wave, frequency, amplitude);
            if (soundBuffer != null)
            {
                await Task.Delay(duration).ContinueWith(_ => StopSound(soundBuffer));
                Console.WriteLine($"Scheduled to stop sound after {duration.TotalSeconds} seconds");
            }
            else
            {
                Console.WriteLine("SoundBuffer is null. Cannot play sound.");
            }
        }

        public static void StopSound(SoundBuffer soundBuffer)
        {
            lock (lockObj)
            {
                if (soundBuffer != null && soundBuffer.SoundEffectInstance != null && soundBuffer.SoundEffectInstance.State == SoundState.Playing)
                {
                    soundBuffer.SoundEffectInstance.Stop();
                    soundBuffer.SoundEffectInstance.BufferNeeded -= soundBuffer.OnBufferNeeded;
                    activeBuffers.Remove(soundBuffer);
                    Console.WriteLine("Stopped sound");
                }
            }
        }

        public static void StopAllSounds()
        {
            lock (lockObj)
            {
                foreach (var soundBuffer in activeBuffers)
                {
                    if (soundBuffer.SoundEffectInstance != null)
                    {
                        soundBuffer.SoundEffectInstance.Stop();
                        soundBuffer.SoundEffectInstance.BufferNeeded -= soundBuffer.OnBufferNeeded;
                    }
                }
                activeBuffers.Clear();
                Console.WriteLine("Stopped all sounds");
            }
        }

        public static void SetVolume(SoundBuffer soundBuffer, float volume)
        {
            lock (lockObj)
            {
                if (soundBuffer != null && soundBuffer.SoundEffectInstance != null && soundBuffer.SoundEffectInstance.State == SoundState.Playing)
                {
                    soundBuffer.SoundEffectInstance.Volume = Math.Clamp(volume, 0f, 1f);
                    Console.WriteLine($"Set volume to {volume}");
                }
            }
        }

        public static void Dispose()
        {
            lock (lockObj)
            {
                StopAllSounds();
                foreach (var soundBuffer in activeBuffers)
                {
                    if (soundBuffer.SoundEffectInstance != null)
                    {
                        soundBuffer.SoundEffectInstance.Dispose();
                    }
                }
                activeBuffers.Clear();
                Console.WriteLine("Disposed all sound buffers");
            }
        }
    }

    public class SoundBuffer
    {
        internal DynamicSoundEffectInstance SoundEffectInstance { get; private set; }
        int sampleRate; 
        int samplesPerBuffer;
        Wave wave;
        float frequency;
        float amplitude;

        public SoundBuffer(DynamicSoundEffectInstance soundEffectInstance, int sampleRate, int samplesPerBuffer, Wave wave, float frequency, float amplitude)
        {
            this.SoundEffectInstance = soundEffectInstance;
            this.sampleRate = sampleRate;
            this.samplesPerBuffer = samplesPerBuffer;
            this.wave = wave;
            this.frequency = frequency;
            this.amplitude = amplitude;
        }

        internal void OnBufferNeeded(object sender, EventArgs e)
        {
            // Создаем буфер аудиоданных
            byte[] buffer = new byte[samplesPerBuffer * sizeof(short)];
            short[] shortBuffer = new short[samplesPerBuffer];

            // Заполняем буфер нужной волной
            for (int i = 0; i < samplesPerBuffer; i++)
            {
                float time = (float)i / sampleRate;
                shortBuffer[i] = wave.Generate(time, frequency, amplitude);
            }

            // Копируем данные в буфер байтов
            Buffer.BlockCopy(shortBuffer, 0, buffer, 0, buffer.Length);

            // Отправляем буфер на воспроизведение
            SoundEffectInstance.SubmitBuffer(buffer);
        }
    }
}
