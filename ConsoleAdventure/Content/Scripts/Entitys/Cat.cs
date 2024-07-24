using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public class Cat : Entity
    {
        static Color[] colors = new Color[9]
        {
            new Color(50, 50, 50),
            new Color(131, 105, 44),
            new Color(193, 138, 45),
            new Color(243, 171, 51),
            new Color(140, 147, 153),
            new Color(255, 255, 255),
            new Color(196, 207, 211),
            new Color(250, 194, 45),
            new Color(240, 210, 80),
        };

        int index = -1;

        public Cat(World world, Position position, List<object> parameters = null) : base(world, position, parameters)
        {
            renderFieldType = RenderFieldType.cat;
            SetMaxLife(9);
            Initialize();

            this.index = index == -1 ? ConsoleAdventure.rand.Next(0, colors.Length) : index;
            СhooseColor(colors, this.position, this.index);
        }

        bool isFree = true;
        int timer;
        int randomTime = 0;
        int rotation = -1;
        int rotation1 = -1;

        public override void AI()
        {
            if (isFree)
            {
                randomTime = Utils.StabilizeTicks(ConsoleAdventure.rand.Next(0, 180));
                rotation = ConsoleAdventure.rand.Next(-1, 4);
                rotation1 = ConsoleAdventure.rand.Next(-3, 4);
                isFree = false;

                while (true)
                {
                    if (rotation1 == rotation)
                    {
                        rotation1 = ConsoleAdventure.rand.Next(-3, 4);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (timer < randomTime)
            {
                if (rotation > -1 && timer % 10 == 0)
                {
                    Move(1, (Rotation)(rotation * 2));
                    if (rotation1 > -1)
                    {
                        Move(1, (Rotation)(rotation1 * 2));
                    }

                    СhooseColor(colors, this.position, index);
                }
            }

            else
            {
                isFree = true;
                timer = 0;
            }

            timer++;
        }

        public override List<object> GetParams()
        {
            List<object> parameters = new()
            {
                index
            };

            return parameters;
        }

        public override void SetParams(List<object> p)
        {
            if (p == null)
                return;

            index = (int)p[0];
        }
    }
}
