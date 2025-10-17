using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using System.Reflection;
using System.Threading;
using System.Linq;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public class HighTemperatureFire : Entity
    {
        private static Position[] directions = new Position[]
        {
            new(-1, -1),
            new(0, -1),
            new(1, -1),
            new(1, 0),
            new(1, 1),
            new(0, 1),
            new(-1, 1),
            new(-1, 0)
        };

        public int maxBreed = 20;
        public HighTemperatureFire(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)VanillaTransforms.highTemperatureFire;
            SetMaxLife(-1);

            AddTypeToMap();

            Initialize();
        }

        public override string GetSymbol()
        {
            return "  ";
        }

        public override Color GetColor()
        {
            return Color.White;
        }

        int timer;
        float fadeOut = 0.1f;
        Transform transform;
        float curDegreeDestruction;
        bool outFlag;

        static int maxTime = 600;
        static int delayAttempts = maxTime / 2;

        public override void AI()
        {
            if (timer % 5 == 0)
            {
                Transform t1 = world.GetField(position.x, position.y, World.BlocksLayerId, w)?.content;
                Transform t2 = world.GetField(position.x, position.y, World.FloorLayerId, w)?.content;

                transform = null;

                if (t1 != null)
                {
                    if (BurnType[t1.type] == 1) transform = t1;
                }

                else if (t2 != null)
                {
                    if (BurnType[t2.type] == 1) transform = t2;
                }

                if (CanHitToPlayer(out short id))
                {
                    world.players.ElementAt(id).Value.AddBuff(new InTheFlames(ConsoleAdventure.rand.Next(1, 6) * 60));
                }
            }

            if (transform == null)
            {
                if (timer > maxTime)
                {
                    outFlag = true;
                    fadeOut -= 0.1f;
                }
            }

            else if(timer % 2 == 0 && transform.CanBeDestroyed())
            {
                curDegreeDestruction += Math.Abs(1f / Hardness[transform.type]);
                transform.degreeDestruction = (byte)curDegreeDestruction;

                transform.WhenBurning();

                if (transform.degreeDestruction >= 100)
                {
                    transform.AfterBurning();
                }
            }

            if (fadeOut < 0f)
                Kill();

            if (timer % delayAttempts == delayAttempts - 1)
            {
                if(ConsoleAdventure.rand.Next(0, 2) == 1)
                {
                    for(int i = 0; i < 8; i++)
                    {
                        Position curPosition = new(position.x + directions[i].x, position.y + directions[i].y);

                        Transform t1 = world.GetField(curPosition.x, curPosition.y, World.BlocksLayerId, w)?.content;
                        Transform t2 = world.GetField(curPosition.x, curPosition.y, World.FloorLayerId, w)?.content;

                        if (t1 != null)
                        {
                            if (BurnType[t1.type] <= 2) SpawnFire(curPosition.x, curPosition.y);
                        }

                        else if (t2 != null)
                        {
                            if (BurnType[t2.type] <= 2) SpawnFire(curPosition.x, curPosition.y);
                        }
                    }
                }
            }

            if (fadeOut <= 1 && !outFlag)
            {
                fadeOut += 0.1f;
            }

            if(maxBreed <= 0)
                Kill();

            timer++;

            void SpawnFire(int x, int y)
            {
                if(world.GetField(x, y, World.MobsLayerId, w)?.content == null)
                {
                    HighTemperatureFire fire = new HighTemperatureFire(new Position(x, y), w);
                    fire.maxBreed = maxBreed - 1;
                    Spawner.Spawn(fire);
                }
            }
        }

        float drawTimer;
        public override void OnTheScreen()
        {
            Light.Add(position.x, position.y, w, Color.White, 100f);

            if (CanDraw())
            {
                StringPaint.Draw("●", position, w, new(0.38f, -0.03f), new Color(0, 254, 245) * fadeOut);
                StringPaint.Draw("•", position, w, new(0.42f + (float)(Math.Sin(drawTimer / 30 * Math.PI) / 20), -0.15f), new Color(0, 254, 245) * 0.8f * fadeOut);
                StringPaint.Draw("•", position, w, new(0.42f, -0.03f), new Color(178, 255, 255) * (float)(1 + Math.Sin(drawTimer / 60 * Math.PI) / 4) * fadeOut);
            }

            drawTimer++;
        }
    }
}
