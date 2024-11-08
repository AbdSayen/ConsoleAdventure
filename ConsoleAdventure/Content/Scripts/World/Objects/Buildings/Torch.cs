using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Torch : Transform
    {
        public Torch(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.torch;
            isObstacle = false;

            AddTypeToMap<Torch>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new TorchItem(), 1) });
        }

        public override string GetSymbol()
        {
            return " |";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }

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
                        if (transform?.burnType == 0 && mob == null)
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