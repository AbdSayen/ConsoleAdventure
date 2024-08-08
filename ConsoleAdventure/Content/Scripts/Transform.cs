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
        
        /// <summary>
        /// Ось w, глубина объекта (на коком уровне мира он находится)
        /// </summary>
        public int w;

        protected Transform(Position position, int w)
        {
            this.position = position;
            this.w = w;

            world = ConsoleAdventure.world;    
        }

        public void Initialize()
        {
            if (world.GetField(position.x, position.y, worldLayer, w) != null)
            {
                world.GetField(position.x, position.y, worldLayer, w).content = this;
                world.GetField(position.x, position.y, worldLayer, w).color = GetColor();
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

        public virtual bool SetPosition(Position newPos)
        {
            return world.SetSubjectPosition(this, worldLayer, newPos.x, newPos.y);
        }

        public virtual bool SetPosition(Position newPos, int newW)
        {
            return world.SetSubjectPosition(this, worldLayer, newPos.x, newPos.y, newW);
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

        public static string GetName(Position pos, int layer, int w)
        {
            if (layer < 0) layer = 0;
            if (layer > 3) layer = 3;

            Field field = ConsoleAdventure.world.GetField(pos.x, pos.y, layer, w);

            if (field?.content == null) return Localization.GetTranslation("Transforms", "None");

            return Localization.GetTranslation("Transforms", field.content.GetType().Name);
        }

        public static void SetObject(int type, Position position, int w, int layer = -1, List<Stack> items = null, List<object> parameters = null)
        {
            if (position.x < 0) { position.x = 0; }
            if (position.y < 0) { position.y = 0; }
            if (position.x > ConsoleAdventure.world.size) { position.x = ConsoleAdventure.world.size; }
            if (position.y > ConsoleAdventure.world.size) { position.y = ConsoleAdventure.world.size; }

            if (typeMapping.TryGetValue(type, out Type objectType))
            {
                if (objectType == typeof(Transform))
                {
                    Transform content = ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId, w).content;
                    ConsoleAdventure.world.RemoveSubject(content, layer, false);
                    return;
                }

                //ConstructorInfo constructor = GetConstructor(objectType, items != null, parameters != null);
                Init(objectType, position, w, items, parameters);
            }
        }

        internal static object[] BuildConstructorArgs(Type objectType, Position position, int w, List<Stack> items, List<object> parameters)
        {
            if (objectType.IsSubclassOf(typeof(Loot)) || objectType == typeof(Loot) ||
                objectType.IsSubclassOf(typeof(Storage)) || objectType == typeof(Storage))
                return new object[] { position, w, items, -1 };

            if (objectType.IsSubclassOf(typeof(Entity)) || objectType == typeof(Entity))
                return new object[] { position, w, parameters };

            return new object[] { position, w, -1 };
        }

        internal static void Init(Type type, Position position, int w, List<Stack> items, List<object> parameters)
        {
            if(type == typeof(Storage) || type == typeof(Player))
            {
                return;
            }

            object[] args = BuildConstructorArgs(type, position, w, items, parameters);

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
