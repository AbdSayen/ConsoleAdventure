using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure
{
    public abstract class Transform
    {
        public World world { get; protected set; }
        public int worldLayer { get; protected set; }
        
        public Position position;
        public RenderFieldType renderFieldType;
        public bool isObstacle;

        protected Transform(World world, Position position = null)
        {
            if (position != null) this.position = position;
            else this.position = new Position(0, 0);

            this.world = world;
        }

        public void Initialize()
        {
            if (world.GetField(position.x, position.y, worldLayer) != null)
            {
                world.GetField(position.x, position.y, worldLayer).content = this;
                world.GetField(position.x, position.y, worldLayer).color = GetColor();
            }
        }

        public virtual void Move(int stepSize, Rotation rotation)
        {
            world.MoveSubject(this, worldLayer, stepSize, rotation);
        }

        public string GetSymbol()
        {
            switch (renderFieldType)
            {
                case RenderFieldType.empty:
                    return "  ";
                case RenderFieldType.player:
                    return " ^";
                case RenderFieldType.ruine:
                    return "::";
                case RenderFieldType.wall:
                    return "##";
                case RenderFieldType.tree:
                    return " *";
                case RenderFieldType.floor:
                    return " .";
                case RenderFieldType.door:
                    return "[]";
                case RenderFieldType.loot:
                    return " $";
                case RenderFieldType.water:
                    return "≈≈";
                default:
                    return "??";
            }

        }

        public Color GetColor()
        {
            switch (renderFieldType)
            {
                case RenderFieldType.empty:
                    return Color.Black;
                case RenderFieldType.player:
                    return Color.Yellow;
                case RenderFieldType.ruine:
                    return Color.Gray;
                case RenderFieldType.wall:
                    return Color.White;
                case RenderFieldType.tree:
                    return new(13, 152, 20);
                case RenderFieldType.floor:
                    return Color.Gray;
                case RenderFieldType.door:
                    return new(94, 61, 38);
                case RenderFieldType.loot:
                    return Color.Yellow;
                case RenderFieldType.water:
                    return new(16, 29, 211);
                default:
                    return Color.Purple;
            }
        }
    }
}
