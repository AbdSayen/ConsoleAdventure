using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Workbench : Transform
    {
        public Workbench(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.workbench;

            AddTypeToMap();

            Initialize();
        }

        public override void SetStaticData()
        {
            BurnType[type] = 1;
        }

        public override void Collapse()
        {
            new Loot(position, w, new() { new Stack(new WorkbenchItem(), 1) });
        }

        public override string GetSymbol()
        {
            return " ∏";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
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
    }
}