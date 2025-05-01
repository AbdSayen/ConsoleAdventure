using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine
{
    [Serializable]
    public struct NoiseBuffer
    {
        public long seed;
        public int octaves = 0;
        public double persistence = 0;
        public double lacunarity = 0;

        public NoiseBuffer(long seed) 
        { 
            this.seed = seed;
        }

        public NoiseBuffer(long seed, int octaves, double persistence, double lacunarity)
        {
            this.seed = seed;
            this.octaves = octaves;
            this.persistence = persistence;
            this.lacunarity = lacunarity;
        }

        public float Simplex2(double x, double y)
        {
            return OpenSimplex.noise2(seed, x, y);
        }

        public float Simplex3(double x, double y, double z)
        {
            return OpenSimplex.noise3_Fallback(seed, x, y, z);
        }

        public float FractalSimplex2(double x, double y)
        {
            return OpenSimplex.FractalNoise2D(seed, x, y, octaves, persistence, lacunarity);
        }
    }
}
