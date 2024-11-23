using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public struct SpawnCondition
    {
        private short mobType = (short)RenderFieldType.entity;
        private double probability = 0;
        public Range wRange = new(0, 0);
        public List<short> biomes = new();

        public double Probability
        {
            get { return probability; }
            set
            {
                probability = Math.Clamp(value, 0, 1);
            }
        }

        public short MobType
        {
            get { return mobType; }
            set
            {
                if (Transform.TypeMapping.TryGetValue(value, out Type type))
                {
                    if (type.IsSubclassOf(typeof(Entity)))
                    {
                        mobType = value;
                        return;
                    }
                }
            }
        }

        public SpawnCondition(short mobType, Range wRange, List<short> biomes, double probability)
        {
            MobType = mobType;
            this.wRange = wRange; 
            this.biomes = biomes; 
            Probability = probability;
        }
    }
}
