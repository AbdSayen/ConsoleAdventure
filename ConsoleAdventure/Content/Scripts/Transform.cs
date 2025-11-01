using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;
using System.Reflection;
using System.Text;
using System.DirectoryServices;
using ConsoleAdventure.Settings;
using System.CodeDom;
using SharpDX.Direct2D1;
using System.Linq;
using ConsoleAdventure.Content.Scripts.IO;
using System.ComponentModel;
using CaModLoaderAPI;

namespace ConsoleAdventure
{
    [Serializable]
    public abstract class Transform
    {
        public static World world { get; protected set; }

        public static Type[] TypeMapping { get { return typeMapping; } }
        private static Type[] typeMapping = new Type[256];

        public byte type;

        public byte degreeDestruction = 0;  

        public byte worldLayer { get; protected set; }
        public Position position;
        public byte w;
        
        internal static bool IsGlobalInit = false;

        /// <summary>
        /// Определяет возможность пройти сквозь блок для каждого типа трансформов.
        /// <br/><br/>Значение по умолчанию: <c>false</c>
        /// </summary>
        public static StaticData<bool> IsObstacle { get; private set; }

        /// <summary>
        /// Определяет твёрдость для каждого типа трансформов
        /// <br/><br/>Значение по умолчанию: <c>1f</c>
        /// </summary>
        public static StaticData<float> Hardness { get; private set; }

        public static StaticData<byte> BurnType { get; private set; }

        /// <summary>
        /// Определяет слой, на котором будет находиться этот трансформ.
        /// <br/><br/>Всего их 4: 
        /// <br/> - <seealso cref="World.FloorLayerId"/>  - полы
        /// <br/> - <seealso cref="World.BlocksLayerId"/> - блоки
        /// <br/> - <seealso cref="World.ItemsLayerId"/>  - предметы
        /// <br/> - <seealso cref="World.MobsLayerId"/>   - сущности
        /// <br/><br/> Если т-форм будет динамически выбирать слой, то 
        /// установите значение на <c>null</c>, тогда вы сможете самостоятельноего 
        /// перезаписать в конструкторе класса.
        /// <br/><br/> При вызове <seealso cref="Initialize"/> или 
        /// <seealso cref="InitializeModTransform"/>, автоматически 
        /// к <seealso cref="worldLayer"/> будет присвоено значение 
        /// отсюда, по типу текущего т-форма, 
        /// если оно <c>null</c>, то ничего не произойдёт.
        /// <br/><br/> Значение по умолчанию: <seealso cref="World.BlocksLayerId"/>
        /// </summary>
        public static StaticData<byte?> DefaultWorldLayer { get; private set; }

        protected Transform(Position position, byte w)
        {
            this.position = position;
            this.w = w;

            world = ConsoleAdventure.world;    
        }

        public void InitializeModTransform()
        {
            type = (byte)Main.GetModTransform(GetType());
            Initialize();
        }

        internal void Initialize()
        {
            if (!IsGlobalInit)
            {
                byte? layer = DefaultWorldLayer[type];
                if (layer.HasValue)
                {
                    worldLayer = Math.Clamp(layer.Value, (byte)0, World.CountOfLayers);
                }

                if (world.GetField(position, worldLayer, w) != null)
                {
                    world.GetField(position, worldLayer, w).content = this;
                }
            }
        }

        public T Copy<T>()
        {
            return (T)MemberwiseClone();
        }
        
        public void AddTypeToMap()
        {
            AddTypeToMap(GetType(), type);
        }

        [Obsolete("Используйте \"AddTypeToMap()\" или \"AddTypeToMap(Type T, int type)\" ")]
        public static void AddTypeToMap<T>(int type)
        {
            AddTypeToMap(typeof(T), type);
        }

        public static void AddTypeToMap(Type T, int type)
        {
            if (type >= TypeMapping.Length)
            {
                List<Type> types = TypeMapping.ToList();
                types.Add(T);

                typeMapping = types.ToArray();

                ConsoleAdventure.logger.AddMessage($"{T.Namespace}.{T.Name} inited with id {type} and resize array");
                return;
            }

            else if (type >= 0)
            {
                if (TypeMapping[type] == null)
                {
                    typeMapping[type] = T;
                    ConsoleAdventure.logger.AddMessage($"{T.Namespace}.{T.Name} inited with id {type}");
                }
            }
        }

        public static void ClearTypeMap()
        {
            typeMapping = new Type[256];
        }

        /// <summary>
        /// Инициализирует все <see cref="StaticData{T}"/> для <see cref="Transform"/>.
        /// Базовая реализация инициализирует базовые <see cref="StaticData{T}"/> (). <br/><br/>
        /// Желательно при перезаписи метода писать <c>base.InitStaticData();</c> в начале метода,
        /// для большей надёжности<br/><br/>
        /// Пример override-а:
        /// <code> 
        /// public static StaticData&lt;bool&gt; Flags { get; private set; } 
        /// 
        /// public override void InitStaticData()
        /// {
        ///     base.InitStaticData();
        ///     Flags = new StaticData&lt;bool&gt;(false);
        /// }
        /// </code>
        /// </summary>
        public virtual void InitStaticData()
        {
            IsObstacle = new StaticData<bool>(false);
            Hardness = new StaticData<float>(1f);
            BurnType = new StaticData<byte>(0);
            DefaultWorldLayer = new StaticData<byte?>(World.BlocksLayerId);
        }

        public virtual void SetStaticData()
        {

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
            return world.SetSubjectPosition(this, worldLayer, newPos.x, newPos.y, (byte)newW);
        }

        public virtual void Collapse() { }

        public virtual void OnDestroy() { }

        public virtual void Interaction() { }

        public virtual string GetSymbol()
        {
            return "  ";   
        }

        public virtual Color GetColor() => Color.Black;

        public virtual Color? GetBGColor() => null;

        public virtual void OnTheScreen()
        {

        }

        public static string GetTooltip(Position pos, int layer, int w)
        {
            if (layer < 0) layer = 0;
            if (layer > 3) layer = 3;

            Position start = ConsoleAdventure.startDisplay;
            Position end = ConsoleAdventure.endDisplay;

            bool inDisplay = pos.x >= start.x && pos.x <= end.x && pos.y >= start.y && pos.y <= end.y;

            if (inDisplay)
            {
                Color lightColor = Light.colors[pos.x - start.x, pos.y - start.y];

                if (lightColor.R > 10 || lightColor.G > 10 || lightColor.B > 10)
                {
                    Field field = ConsoleAdventure.world.GetField(pos.x, pos.y, layer, w);

                    if (field?.content == null)
                        return Localization.GetTranslation("Transforms", "None");

                    string typeText = "";

                    if (ConsoleAdventure.ShowTypes)
                        typeText = $" <{field?.content?.type}:{field?.content?.GetType()?.FullName}>";

                    foreach (var globalTransform in CaModLoader.modGlobalTransforms)
                    {
                        string modTooltip = globalTransform.ModifyTooltip(field?.content);

                        if (modTooltip != null)
                        {
                            return modTooltip + typeText;
                        }
                    }

                    return field?.content.ModifyTooltip() + typeText;
                } 

                return "???";
            }

            return "???";
        }

        public static bool SetObject(int type, Position position, int w, int layer = -1, List<Stack> items = null, List<object> parameters = null)
        {
            if (position.x < 0) { position.x = 0; }
            if (position.y < 0) { position.y = 0; }
            if (position.x > ConsoleAdventure.world.size) { position.x = (short)ConsoleAdventure.world.size; }
            if (position.y > ConsoleAdventure.world.size) { position.y = (short)ConsoleAdventure.world.size; }


            if (type >= 0 && type < TypeMapping.Length) 
            {
                Type objectType = TypeMapping[type];

                if (objectType != null || type == 0)
                {
                    if (objectType == typeof(Transform) || type == 0)
                    {
                        Transform content = ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId, w)?.content;
                        ConsoleAdventure.world.RemoveSubject(content, layer, false);
                        return true;
                    }

                    //ConstructorInfo constructor = GetConstructor(objectType, items != null, parameters != null);
                    Init(objectType, position, w, items, parameters);
                    return true;
                }

                else
                {
                    new UnloadedTransform(position, w, layer, type);
                } 
            }

            return false;
        }

        internal static object[] BuildConstructorArgs(Type objectType, Position position, int w, List<Stack> items, List<object> parameters)
        {
            if (objectType.IsSubclassOf(typeof(Loot)) || objectType == typeof(Loot) ||
                objectType.IsSubclassOf(typeof(Storage)) || objectType == typeof(Storage))
                return new object[] { position, w, items };

            if (objectType.IsSubclassOf(typeof(Entity)) || objectType == typeof(Entity))
                return new object[] { position, w, parameters };

            return new object[] { position, w };
        }

        internal static void Init(Type type, Position position, int w, List<Stack> items, List<object> parameters)
        {
            if(type == typeof(Storage) || type == typeof(Player) || type == typeof(UnloadedTransform))
            {
                return;
            }

            object[] args = BuildConstructorArgs(type, position, w, items, parameters);

            Transform transform = (Transform)Activator.CreateInstance(type, args);

            if (type.IsSubclassOf(typeof(Entity)) || type == typeof(Entity))
                Spawner.Spawn((Entity)transform);

            if (IsGlobalInit) 
            { 
                transform.SetStaticData();
            }
        }

        public virtual void LoadData(object data)
        {

        }

        public virtual object SaveData()
        {
            return null;
        }

        public virtual bool CanBeDestroyed()
        {
            return Hardness[type] > 0;
        }

        public virtual bool CanDraw()
        {
            return position >= ConsoleAdventure.startDisplay && position < ConsoleAdventure.endDisplay;
        }

        public virtual void RandomUpdate()
        {
        }

        public virtual void WhenBurning()
        {
        }

        public virtual void AfterBurning()
        {
            world.RemoveSubject(this, worldLayer, false);
        }

        public virtual byte[] GetDataBytes()
        {
            List<byte> data = new List<byte>();

            /*
            data.AddRange(BitConverter.GetBytes(position.x)); // 2b                  = 0
            data.AddRange(BitConverter.GetBytes(position.y)); // 2b            0 + 2 = 2
            data.AddRange(BitConverter.GetBytes(false)); // 1b                 2 + 2 = 4
            data.AddRange(BitConverter.GetBytes(degreeDestruction)); // 1b     4 + 1 = 5
            data.AddRange(BitConverter.GetBytes(1f)); // 4b                    5 + 1 = 6
            data.Add(w); // 1b                                                 6 + 4 = 10
            */

            data.AddRange(BitConverter.GetBytes(position.x)); // 2b                  = 0
            data.AddRange(BitConverter.GetBytes(position.y)); // 2b            0 + 2 = 2
            data.AddRange(BitConverter.GetBytes(degreeDestruction)); // 1b     2 + 2 = 4
            data.Add(w); // 1b                                                 4 + 1 = 5

            return data.ToArray();
        }

        public virtual void SetDataFromBytes(byte[] data)
        {
            /*
            position.x = BitConverter.ToInt16(data, 0);
            position.y = BitConverter.ToInt16(data, 2);
            degreeDestruction = data[5];
            w = data[10];
            */

            position.x = BitConverter.ToInt16(data, 0);
            position.y = BitConverter.ToInt16(data, 2);
            degreeDestruction = data[4];
            w = data[5];
        }

        public virtual string ModifyTooltip()
        {
            string degree = degreeDestruction > 0 ? $" ({100 - degreeDestruction} / 100)" : "";
            return GetName() + degree;
        }

        public string GetName()
        {
            string name = Localization.GetTranslation("Transforms", GetType().Name);

            if (name == "" || name == null)
                name = GetType().FullName;

            return name;
        }

        public static void UpdatedChunk(Position position, int worldLayer)
        {
            if (worldLayer != World.MobsLayerId)
            {
                Chunk chunk = world.GetChunk(position.x, position.y, out int v1, out int v2);

                if (chunk != null)
                    chunk.IsUpdated = true;
            }
        }

        public void DropItem(Item item, int count = 1)
        {
            new Loot(position, w, new List<Stack>() { new Stack(item, count) });
        }

        protected T GetVariation<T>(T[] valuesMap)
        {
            if (valuesMap == null || valuesMap.Length == 0) 
                return default(T);

            return valuesMap[Utils.HashNoise(position.x, position.y, valuesMap.Length - 1)];
        }
    }
}
