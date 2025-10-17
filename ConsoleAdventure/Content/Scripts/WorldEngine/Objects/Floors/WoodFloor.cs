using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class WoodFloor : Transform
    {
        public WoodFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.woodFloor;

            AddTypeToMap();

            Initialize();
        }

        public override void SetStaticData()
        {
            BurnType[type] = 1;
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new WoodFloorItem(), 1) });
        }

        public override string GetSymbol()
        {
            return " .";
        }

        public override Color GetColor()
        {
            return new(74, 41, 18);
        }

        public override void AfterBurning()
        {
            if (ConsoleAdventure.rand.Next(0, 2) == 1)
            {
                new CharcoalFloor(position, w);
            }

            else
            {
                base.AfterBurning();
            }
        }
    }
}