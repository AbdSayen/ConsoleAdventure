using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Anvil : Transform
    {
        public Anvil(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.anvil;
            isObstacle = false;

            AddTypeToMap<Anvil>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new() { new Stack(new AnvilItem(), 1) });
        }

        public override string GetSymbol()
        {
            return " σ";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}