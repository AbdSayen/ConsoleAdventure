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

            type = (int)RenderFieldType.workbench;
            isObstacle = false;

            AddTypeToMap<Workbench>(type);

            Initialize();
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
    }
}