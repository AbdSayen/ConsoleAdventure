using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using System.Reflection;
using System.Threading;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public class Bomb : Entity
    {
        public Bomb(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)RenderFieldType.bomb;
            SetMaxLife(-1);

            AddTypeToMap<Bomb>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return " B";
        }

        public override Color GetColor()
        {
            return Color.Red;
        }


        int timer;
        int rayCount = 20;
        int radius = 8;

        public override void AI()
        {
            if (timer > 300)
            {
                Vector2[] positions = new Vector2[rayCount];
                byte[] powers = new byte[rayCount];

                for (int j = 0; j < rayCount; j++)
                {
                    positions[j] = position.ToVector2();
                    powers[j] = (byte)(10 * radius - 1);
                }

                for (int i = 0; i < radius; i++)
                {
                    float curAngle = 0;

                    for(int j = 0; j < rayCount; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            for (int l = 0; l < 3; l++)
                            {
                                Position destroyPos = new(positions[j].ToPosition().x - 1 + k, positions[j].ToPosition().y - 1 + l);
                                Transform transform = ConsoleAdventure.world.GetField(destroyPos.x, destroyPos.y, World.BlocksLayerId, w)?.content;

                                if (transform != null && transform.CanBeDestroyed())
                                {
                                    transform.degreeDestruction += (byte)(powers[j] - (int)transform.hardness);
                                    powers[j] -= (byte)transform.hardness;

                                    if (transform.degreeDestruction > 100)
                                    {
                                        if (ConsoleAdventure.rand.Next(0, powers[j]) > powers[j] / 2)
                                        {
                                            ConsoleAdventure.world.RemoveSubject(transform, World.BlocksLayerId);
                                        }

                                        else
                                        {
                                            transform.degreeDestruction /= 2;
                                        }
                                    }
                                }
                                
                                if(ConsoleAdventure.world.GetField(destroyPos.x, destroyPos.y, World.MobsLayerId, w)?.content == null)
                                    ConsoleAdventure.world.entities.Add(new Explosion(destroyPos, w));
                            }
                        }

                        positions[j] = positions[j].Move(curAngle);
                        curAngle += rayCount;
                    }
                }

                Kill();
            }

            timer++;
        }
    }
}
