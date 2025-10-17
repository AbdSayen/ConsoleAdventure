using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Web : Transform
    {
        public Web(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.web;

            AddTypeToMap();

            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = 0.1f;
            BurnType[type] = 1;
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new WebItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "¼¼";
        }

        public override Color GetColor()
        {
            return new Color(120, 120, 120);
        }
    }
}