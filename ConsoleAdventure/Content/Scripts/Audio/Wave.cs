using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Audio
{
    public abstract class Wave
    {
        public virtual short Generate(float time, float frequency, float amplitude)
        {
            return 0;
        }
    }

    public class Sine : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * Math.Sin(2 * Math.PI * frequency * time));
        }
    }

    public class Square : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * (Math.Sin(2 * Math.PI * frequency * time) >= 0 ? 1 : -1));
        }
    }

    public class Triangle : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * (2.0f * Math.Abs(2.0f * (time * frequency - (float)Math.Floor(time * frequency + 0.5f))) - 1.0f));
        }
    }

    public class Sawtoot : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * (2.0f * (time * frequency - (float)Math.Floor(time * frequency + 0.5f))));
        }
    }

    public class FadeOutWhiteNoise : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * (ConsoleAdventure.rand.NextFloat(-1, 1) * Math.Max(1 - time, 0)));
        }
    }

    public class FadeInWhiteNoise : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * (ConsoleAdventure.rand.NextFloat(-1, 1) * Math.Min(time, 1)));
        }
    }

    public class WhiteNoise : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(amplitude * short.MaxValue * ConsoleAdventure.rand.NextFloat(-1, 1));
        }
    }

    public class Bubble : Wave
    {
        public override short Generate(float time, float frequency, float amplitude)
        {
            return (short)(Math.Max(amplitude - time * amplitude * 1.25f, 0) * short.MaxValue * Math.Sin(2 * Math.PI * Math.Max(frequency - time * frequency * 8, 0) * time));
        }
    }
}
