using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using System.Reflection;
using System.Threading;
using ConsoleAdventure.Settings;

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
                                            transform.degreeDestruction = (byte)Math.Abs(transform.degreeDestruction);
                                        }
                                    }
                                }

                                /*if(transform == null)
                                {
                                    int hole = 0;
                                    for (int m = -1; m < 2; m++)
                                    {
                                        for (int n = -1; n < 2; n++)
                                        {
                                            Transform t = world.GetField(destroyPos.x + m, destroyPos.y + n, World.BlocksLayerId, w)?.content;
                                            if (t != null && t.type == (int)RenderFieldType.climb) { hole = 1; }
                                            else if (t != null && t.type == (int)RenderFieldType.descent) { hole = 2; }
                                        }
                                    }

                                    if (hole == 1)
                                    {
                                        new Climb(destroyPos, w, World.BlocksLayerId);
                                        new Descent(destroyPos, w + 1, World.BlocksLayerId);
                                    }

                                    else if (hole == 2)
                                    {
                                        new Climb(destroyPos, w - 1, World.BlocksLayerId);
                                        new Descent(destroyPos, w, World.BlocksLayerId);
                                    }
                                }*/
                                
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

        public override void OnTheScreen()
        {
            Light.Add(position.x, position.y, w, new Color(255, 255, 255), ConsoleAdventure.rand.NextFloat(-0.5f, 0.5f) + 8.5f);
        }
    }
}
