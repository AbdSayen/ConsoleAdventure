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
        private short probability = 0;
        public Range wRange = new(-1, -1);
        public List<short> bioms = new();

        public short Probability
        {
            get { return probability; }
            set
            {
                if (value < 0) probability = 0;
                else if (value > 100) probability = 100;
                else probability = value;
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

        public SpawnCondition(short mobType, Range wRange, List<short> bioms, short probability)
        {
            MobType = mobType;
            this.wRange = wRange; 
            this.bioms = bioms; 
            Probability = probability;
        }
    }
}
