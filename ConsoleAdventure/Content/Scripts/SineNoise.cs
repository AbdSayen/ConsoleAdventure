using ConsoleAdventure.Content.Scripts.WorldEngine;
using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public struct SineNoise
    {
        public int size = 0;
        public float simplexScale = 1;
        public float height = 1;
        public float width = 1;
        public float xOffset = 1;
        public float yOffset = 1;
        public NoiseBuffer noise = new NoiseBuffer();

        public SineNoise(int size, float simplexScale, float height, float width, float yOffset, float xOffset, NoiseBuffer noise)
        {
            this.size = size;
            this.simplexScale = simplexScale;
            this.height = height;
            this.width = width;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.noise = noise;
        }

        public float GetValue(int x, int y)
        {
            float X = x;
            float Y = ((float)y) / ((float)size) - 0.5f;

            float simplexValue = Math.Abs(noise.FractalSimplex2(0, X * simplexScale));
            float simplex2Value = Math.Abs(noise.FractalSimplex2(16, X * simplexScale));
            float value = Function(Y, height + (simplexValue * xOffset), width + (simplex2Value * yOffset));

            if (value > width * 2) value = 1;
            else if (value < 0) value = 0;

            return value;
        }

        private static float Function(float x, float height, float width)
        {
            if (x * width <= 1f && x * width >= -1f)
                return height * MathF.Cos(MathHelper.Pi * x * width) + height;

            return 0;
        }
    }
}