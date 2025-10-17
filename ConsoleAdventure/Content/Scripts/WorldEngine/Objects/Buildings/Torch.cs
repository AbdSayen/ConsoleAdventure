using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Torch : Transform
    {
        public Torch(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.torch;

            Initialize();
        }

        public override void Collapse() => DropItem(new TorchItem());

        public override string GetSymbol() => " |";

        public override Color GetColor() => new(94, 61, 38);

        float timer;
        public override void OnTheScreen()
        {
            Light.Add(position, w, Color.White);

            if(CanDraw())
            {
                StringPaint.Draw("●", position, w, new(0.38f, -0.23f), Color.OrangeRed);
                StringPaint.Draw("•", position, w, new(0.42f + (float)(Math.Sin(timer / 30 * Math.PI) / 20), -0.35f), Color.OrangeRed * 0.8f);
                StringPaint.Draw("•", position, w, new(0.42f, -0.23f), Color.Orange * (float)(1 + Math.Sin(timer / 60 * Math.PI) / 4));
            }

            if(ConsoleAdventure.rand.Next(0, 301) == 300 && world.GetField(position.x, position.y, World.MobsLayerId, w)?.content == null)
            {
                for(int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        Transform transform = world.GetField(position.x + i, position.y + j, worldLayer, w)?.content;
                        Transform mob = world.GetField(position.x + i, position.y + j, World.MobsLayerId, w)?.content;
                        if (BurnType[transform] == 1 && mob == null)
                        {
                            Spawner.Spawn(new Fire(position + new Position(i, j), w));
                        }
                    }
                }
            }

            timer++;
        }
    }
}
