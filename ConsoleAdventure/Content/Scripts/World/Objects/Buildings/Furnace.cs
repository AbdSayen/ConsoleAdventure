using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Furnace : Transform
    {
        public Furnace(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.furnace;
            isObstacle = false;

            AddTypeToMap<Furnace>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new() { new Stack(new FurnaceItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "[]";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}