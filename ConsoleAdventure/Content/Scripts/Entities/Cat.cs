using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities
{
    [Serializable]
    public class Cat : Entity
    {
        public Cat(World world, Position position, List<object> parameters = null) : base(world, position, parameters)
        {
            renderFieldType = RenderFieldType.cat;
            SetMaxLife(9);
            Initialize();
            
            ChooseColor();
            world.SetSubjectPosition(this, 1, position.x + 1, position.y + 1);
            //StateMachine.ChangeState(StatesEnum.Moving);
        }

        bool isFree = true;
        int timer;
        int randomTime = 0;
        int rotation = -1;
        int rotation1 = -1;

        protected override void AI()
        {
            ChooseColor();
            Console.WriteLine("cat choose color");
            /*if (isFree)
            {
                randomTime = Utils.StabilizeTicks(ConsoleAdventure.rand.Next(0, 180));
                rotation = ConsoleAdventure.rand.Next(-1, 4);
                rotation1 = ConsoleAdventure.rand.Next(-3, 4);
                isFree = false;

                while (true)
                {
                    if(rotation1 == rotation)
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
                if(rotation > -1 && timer % 10 == 0)
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

            timer++;*/
        }

        public override List<object> GetParams()
        {
            List<object> parameters = new()
            {
                colorIndex
            };

            return parameters;
        }

        public override void SetParams(List <object> p)
        {
            if (p == null)
                return;

            colorIndex = (int)p[0];
        }
    }
}
