using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public abstract class Tree : Transform
    {
        public static StaticData<string[,]> Crowns { get; private set; }

        public static StaticData<Color> CrownColors { get; private set; }

        public static StaticData<Type> LogTypes { get; private set; }

        public static StaticData<Type> FruitTypes { get; private set; }

        public Tree(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;
        }

        protected void SetDefaultStaticData()
        {
            IsObstacle[type] = true;
            BurnType[type] = 1;
        }

        public override void InitStaticData()
        {
            base.InitStaticData();
            Crowns = new StaticData<string[,]>(new string[0, 0]);
            CrownColors = new StaticData<Color>(Color.White);
            LogTypes = new StaticData<Type>(null);
            FruitTypes = new StaticData<Type>(null);
        }

        public override void Collapse()
        {
            string[,] crown = Crowns[type];
            int xOffset = (crown.GetLength(0) - 1) / 2;
            int yOffset = (crown.GetLength(1) - 1) / 2;

            Type log = LogTypes[type];

            if (log != null)
            {
                Item item = (Item)Activator.CreateInstance(log);
                new Loot(position, w, new List<Stack> { new Stack(item, 3) });
            }

            int count = ConsoleAdventure.rand.Next(2, 15);
            for (int i = 0; i < count; i++)
            {
                Position pos = new(ConsoleAdventure.rand.Next(position.x - xOffset, position.x + xOffset + 1), ConsoleAdventure.rand.Next(position.y - yOffset, position.y + yOffset + 1));
                if (ConsoleAdventure.world.GetField(pos.x, pos.y, World.MobsLayerId, w)?.content == null)
                    Spawner.Spawn(new Leaves(pos, w));
            }

            int lootCount = ConsoleAdventure.rand.Next(1, 5);
            for (int i = 0; i < lootCount; i++)
            {
                Position pos = new(ConsoleAdventure.rand.Next(position.x - xOffset, position.x + xOffset + 1), ConsoleAdventure.rand.Next(position.y - yOffset, position.y + yOffset + 1));

                int lootType = ConsoleAdventure.rand.Next(0, 3);

                Type fruit = FruitTypes[type];

                if (lootType == 1 && fruit != null)
                {
                    Item item = (Item)Activator.CreateInstance(fruit);
                    new Loot(pos, w, new List<Stack> { new Stack(item, 1) });
                }

                if (lootType == 2 && log != null)
                {
                    Item item = (Item)Activator.CreateInstance(log);
                    new Loot(pos, w, new List<Stack> { new Stack(item, 1) });
                }
            }
        }

        public override void OnTheScreen()
        {
            StringBuilder crown = new();
            string[,] Crown = Crowns[type];
            int width = Crown.GetLength(0);
            int height = Crown.GetLength(1);
            int xOffset = width / 2;
            int yOffset = height / 2;
            Position pos = position + new Position(-xOffset, -yOffset);
            Color color = CrownColors[type];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Position curPos = new Position(pos.x + j, pos.y + i);
                    if (ConsoleAdventure.world.GetField(curPos.x, curPos.y, World.BlocksLayerId, w)?.content?.type != type && curPos >= ConsoleAdventure.startDisplay && curPos < ConsoleAdventure.endDisplay)
                        crown.Append(Crown[i, j]);
                    else 
                        crown.Append("  ");
                }
                crown.Append("\n");
            }

            Position tryColorPos = new Position(Math.Clamp(position.x - ConsoleAdventure.startDisplay.x, 0, 60), Math.Clamp(position.y - ConsoleAdventure.startDisplay.y, 0, 30));
            Color crownColor = (color.ToVector3() * Light.colors[tryColorPos.x, tryColorPos.y].ToVector3()).ToColor();
            if (crownColor.R != 0 && crownColor.G != 0 && crownColor.B != 0) 
            { 
                StringPaint.Draw(crown.ToString(), pos, w, new(), crownColor);
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
            string[,] crown = Crowns[type];
            int xOffset = (crown.GetLength(0) - 1) / 2;
            int yOffset = (crown.GetLength(1) - 1) / 2;

            if (ConsoleAdventure.rand.Next(0, 21) == 20)
            {
                Position pos = new Position(ConsoleAdventure.rand.Next(-xOffset, xOffset + 1) + position.x, ConsoleAdventure.rand.Next(yOffset, yOffset + 1) + position.y);
                if (world.GetField(pos.x, pos.y, World.MobsLayerId, w)?.content == null)
                {
                    Spawner.Spawn(new Fire(pos, w));
                }
            }
        }
    }
}