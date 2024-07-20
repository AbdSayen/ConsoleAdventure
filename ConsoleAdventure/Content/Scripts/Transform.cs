using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
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

        public virtual void Move(int stepSize, Position direction)
        {
            world.MoveSubject(this, worldLayer, stepSize, direction);
        }

        public virtual void Collapse() { }

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
                case RenderFieldType.log:
                    return "≡≡";
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
                case RenderFieldType.log:
                    return new(94, 61, 38);
                default:
                    return Color.Purple;
            }
        }

        public static void SetObject(int type, Position position, int layer = -1, List<Stack> items = null)
        {
            switch (type)
            {
                case (int)RenderFieldType.empty:
                    ConsoleAdventure.world.RemoveSubject(ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId).content, layer, false);
                    return;
                case (int)RenderFieldType.player:
                    //new Player(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.ruine:
                    new Ruine(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.wall:
                    new Wall(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.tree:
                    new Tree(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.floor:
                    new Floor(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.door:
                    new Door(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.loot:
                    new Loot(ConsoleAdventure.world, position, items);
                    return;
                case (int)RenderFieldType.water:
                    new Water(ConsoleAdventure.world, position);
                    return;
                case (int)RenderFieldType.log:
                    new Plank(ConsoleAdventure.world, position);
                    return;
                default:
                    return;
            }
        }
    }
}
