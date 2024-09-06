using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class CharcoalFloor : Transform
    {
        public CharcoalFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.charcoalFloor;
            isObstacle = false;

            AddTypeToMap<CharcoalFloor>(type);

            Initialize();
        }

        public override void Collapse()
        {
            //new Loot(position, w, new List<Stack>() { new Stack(new WoodFloorItem(), 1) });
        }

        public override string GetSymbol()
        {
            return " .";
        }

        public override Color GetColor()
        {
            return new(35, 35, 35);
        }
    }
}