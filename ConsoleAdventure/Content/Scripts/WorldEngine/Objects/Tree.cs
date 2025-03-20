using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Tree : Transform
    {
        private static string[,] Сrown = new string[,]
        {
            { "  ", "  ", "¾¾", "¾¾", "¾¾", "",   ""   },
            { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", ""   },
            { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
            { "¾¾", "¾¾", "¾¾", "  ", "¾¾", "¾¾", "¾¾" },
            { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
            { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", ""   },
            { "  ", "¾¾", "¾¾", "¾¾", "¾¾", ""  , "",  },
        };

        static string[] symbolsMap = new string[]
        {
            "-O",
            "O-",
            "Oʹ",
            ">O",
            "O~",
            "~O",
            "()",
            "o-",
            "~o"
        };

        public Tree(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.tree;
            isObstacle = true;
            burnType = 0;

            AddTypeToMap<Tree>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new Log(), 3) });


            int count = ConsoleAdventure.rand.Next(2, 15);
            for (int i = 0; i < count; i++)
            {
                Position pos = new(ConsoleAdventure.rand.Next(position.x - 3, position.x + 4), ConsoleAdventure.rand.Next(position.y - 3, position.y + 4));
                if (ConsoleAdventure.world.GetField(pos.x, pos.y, World.MobsLayerId, w)?.content == null)
                    Spawner.Spawn(new Leaves(pos, w));
            }

            int lootCount = ConsoleAdventure.rand.Next(1, 5);
            for (int i = 0; i < lootCount; i++)
            {
                Position pos = new(ConsoleAdventure.rand.Next(position.x - 3, position.x + 4), ConsoleAdventure.rand.Next(position.y - 3, position.y + 4));

                int lootType = ConsoleAdventure.rand.Next(0, 3);

                if (lootType == 1)
                    new Loot(pos, w, new List<Stack> { new Stack(new Apple(), 1) });

                if (lootType == 2)
                    new Loot(pos, w, new List<Stack> { new Stack(new Log(), 1) });
            }
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, symbolsMap.Length - 1)];
        }

        public override Color GetColor()
        {
            return new (94, 61, 38); //new(13, 152, 20)
        }

        public override void OnTheScreen()
        {
            Position pos = position + new Position(-3, -3);
            StringBuilder crown = new();
            int id = (int)VanillaTransforms.tree;
            int width = Сrown.GetLength(0);
            int height = Сrown.GetLength(1);
            Color color = new Color(13, 152, 20) * 0.5f;
            color.A = 255;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Position curPos = new Position(pos.x + j, pos.y + i);
                    if (ConsoleAdventure.world.GetField(curPos.x, curPos.y, World.BlocksLayerId, w)?.content?.type != id && curPos >= ConsoleAdventure.startDisplay && curPos < ConsoleAdventure.endDisplay)
                    {
                        crown.Append(Сrown[i, j]);
                    }
                    else
                    {
                        crown.Append("  ");
                    }
                }
                crown.Append("\n");
            }

            Position tryColorPos = new Position(Math.Clamp(position.x - ConsoleAdventure.startDisplay.x, 0, 60), Math.Clamp(position.y - ConsoleAdventure.startDisplay.y, 0, 30));
            Color crownColor = (color.ToVector3() * Light.colors[tryColorPos.x, tryColorPos.y].ToVector3()).ToColor();
            if (crownColor.R != 0 && crownColor.G != 0 && crownColor.B != 0) 
            { 
                StringPaint.Draw(crown.ToString(), position + new Position(-3, -3), w, new(), crownColor);
            }
        }

        public override void AfterBurning()
        {
            if (ConsoleAdventure.rand.Next(0, 2) == 1)
            {
                new Charcoal(position, w);
            }

            else
            {
                base.AfterBurning();
            }
        }

        public override void WhenBurning()
        {
            if (ConsoleAdventure.rand.Next(0, 21) == 20)
            {
                Position pos = new Position(ConsoleAdventure.rand.Next(-3, 4) + position.x, ConsoleAdventure.rand.Next(-3, 4) + position.y);
                if (world.GetField(pos.x, pos.y, World.MobsLayerId, w)?.content == null)
                {
                    Spawner.Spawn(new Fire(pos, w));
                }
            }
        }
    }
}