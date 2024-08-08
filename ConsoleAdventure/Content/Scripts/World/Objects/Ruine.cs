using System;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Ruine : Transform
    {
        public Ruine(Position position, int w, int worldLayer = -1) : base(position, w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.ruine;
            this.isObstacle = false;

            AddTypeToMap<Ruine>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "::";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}