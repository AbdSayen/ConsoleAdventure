using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;
using System.Reflection;

namespace ConsoleAdventure
{
    [Serializable]
    public abstract class Transform
    {
        private static Dictionary<int, Type> typeMapping = new Dictionary<int, Type>() 
        {
            { 0, typeof(Transform) },
        };

        public World world { get; protected set; }
        public int worldLayer { get; protected set; }

        public Position position;
        public byte type;
        public bool isObstacle;

        protected Transform(Position position)
        {
            this.position = position;

            world = ConsoleAdventure.world;
        }

        public void Initialize()
        {
            if (world.GetField(position.x, position.y, worldLayer) != null)
            {
                world.GetField(position.x, position.y, worldLayer).content = this;
                world.GetField(position.x, position.y, worldLayer).color = GetColor();
            }
        }

        public T Copy<T>()
        {
            return (T)MemberwiseClone();
        }
        
        public static void AddTypeToMap<T>(int type)
        {
            if(!typeMapping.ContainsKey(type))
                typeMapping.Add(type, typeof(T));
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

        public virtual Color? GetBGColor()
        {
            return null;
        }

        public static string GetName(Position pos, int layer)
        {
            if (layer < 0) layer = 0;
            if (layer > 3) layer = 3;

            Field field = ConsoleAdventure.world.GetField(pos.x, pos.y, layer);

            if (field?.content == null) return Localization.GetTranslation("Transforms", "None");

            return Localization.GetTranslation("Transforms", field.content.GetType().Name);
        }

        public static void SetObject(int type, Position position, int layer = -1, List<Stack> items = null, List<object> parameters = null)
        {
            if (typeMapping.TryGetValue(type, out Type objectType))
            {
                if (objectType == typeof(Transform))
                {
                    ConsoleAdventure.world.RemoveSubject(ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId).content, layer, false);
                    return;
                }

                //ConstructorInfo constructor = GetConstructor(objectType, items != null, parameters != null);
                Init(objectType, position, items, parameters);
            }
        }

        internal static object[] BuildConstructorArgs(Type objectType, Position position, List<Stack> items, List<object> parameters)
        {
            if (objectType.IsSubclassOf(typeof(Loot)) || objectType == typeof(Loot) ||
                objectType.IsSubclassOf(typeof(Storage)) || objectType == typeof(Storage))
                return new object[] { position, items, -1 };

            if (objectType.IsSubclassOf(typeof(Entity)) || objectType == typeof(Entity))
                return new object[] { position, parameters };

            return new object[] { position, -1 };
        }

        internal static void Init(Type type, Position position, List<Stack> items, List<object> parameters)
        {
            if(type == typeof(Storage) || type == typeof(Player))
            {
                return;
            }

            object[] args = BuildConstructorArgs(type, position, items, parameters);

            if (type.IsSubclassOf(typeof(Entity)) || type == typeof(Entity))
                ConsoleAdventure.world.entities.Add((Entity)Activator.CreateInstance(type, args));
            else
                Activator.CreateInstance(type, args);
        }

        public virtual bool CanBeDestroyed()
        {
            return true;
        }
    }
}
