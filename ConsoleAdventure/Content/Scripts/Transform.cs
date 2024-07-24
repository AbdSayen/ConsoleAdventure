using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.Entities;

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

        protected Transform(World world, Position position)
        {
            this.position = position;

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

        public virtual void Move(int stepSize, Position position)
        {
            world.MoveSubject(this, worldLayer, stepSize, position);
        }

        public virtual void SetPosition(Position newPos)
        {
            world.SetSubjectPosition(this, worldLayer, newPos.x, newPos.y);
        }

        public virtual void Collapse() { }

        public virtual string GetSymbol()
        {
            return "  ";   
        }

        public virtual Color GetColor()
        {
            return Color.Black;
        }

        public object Copy()
        {
            return MemberwiseClone();
        }
        
        public static void SetObject(int type, Position position, int layer = -1, List<Stack> items = null, List<object> parameters = null)
        {
            switch (type)
            {
                case (int)RenderFieldType.empty:
                    ConsoleAdventure.world.RemoveSubject(ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId).content, layer, false);
                    return;
                case (int)RenderFieldType.player:
                    new Player(1, ConsoleAdventure.world, position);
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
                case (int)RenderFieldType.entity:
                    ConsoleAdventure.world.entities.Add(new Entity(ConsoleAdventure.world, position, parameters));
                    return;
                case (int)RenderFieldType.cat:
                    ConsoleAdventure.world.entities.Add(new Cat(ConsoleAdventure.world, position, parameters));
                    return;
                case (int)RenderFieldType.chest:
                    new Chest(ConsoleAdventure.world, position, items);
                    return;
                default:
                    return;
            }
        }

        /*public static void SetObject(int type, Position position, int layer = -1, List<Stack> items = null, List<object> parameters = null)
        {
            if (typeMapping.TryGetValue(type, out Type objectType))
            {
                if (objectType == typeof(EmptyHandler))
                {
                    ConsoleAdventure.world.RemoveSubject(ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId).content, layer, false);
                    return;
                }

                ConstructorInfo constructor = GetConstructor(objectType, items != null, parameters != null);
                object[] args = BuildConstructorArgs(objectType, position, items, parameters);

                if (objectType == typeof(Loot) || objectType == typeof(Chest))
                {
                    Activator.CreateInstance(objectType, args);
                }
                else
                {
                    ConsoleAdventure.world.entites.Add((Entity)Activator.CreateInstance(objectType, args));
                }
            }
        }

        private static ConstructorInfo GetConstructor(Type objectType, bool hasItems, bool hasParameters)
        {
            if (hasItems)
                return objectType.GetConstructor(new Type[] { typeof(World), typeof(Position), typeof(List<Stack>) });
            if (hasParameters)
                return objectType.GetConstructor(new Type[] { typeof(World), typeof(Position), typeof(List<object>) });

            return objectType.GetConstructor(new Type[] { typeof(World), typeof(Position) });
        }

        private static object[] BuildConstructorArgs(Type objectType, Position position, List<Stack> items, List<object> parameters)
        {
            if (objectType == typeof(Loot) || objectType == typeof(Chest))
                return new object[] { ConsoleAdventure.world, position, items };
            if (objectType == typeof(Entity) || objectType == typeof(Cat))
                return new object[] { ConsoleAdventure.world, position, parameters };

            return new object[] { ConsoleAdventure.world, position };
        }*/
    }
}
