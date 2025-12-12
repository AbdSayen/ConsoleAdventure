using ConsoleAdventure.CaModLoaderAPI;
using System;
using ConsoleAdventure;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;

namespace ConsoleAdventure
{
    [Serializable]
    public abstract class Item
    {
        public string name = "Name missing";
        public string description = "Description missing";

        public int material = -1;

        public int pick = 0;
        public int hammer = 0;
        public int damage;
        public bool canUse;
        public byte damageClass;
        public bool consume;
        public int maxCount = 50;

        public static int lastTypeId = 0;

        private static Dictionary<int, Type> typeMapping = new Dictionary<int, Type>()
        {
            { 0, typeof(Item) },
        };

        public Material Material => MaterialSystem.GetMaterial(material);

        protected string GetDescription()
        {
            foreach (GlobalItem item in CaModLoader.modGlobalItems)
            {
                string customDescription = item.GetDescription(this);
                if (customDescription != null)
                    return customDescription;
            }
            return description;
        }

        public virtual CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("I", Color.White);
        }

        protected Color GetMaterialColor()
        {
            return (Material?.ModifyColor?.Invoke(null, this))?.Color ?? Color.Magenta;
        }

        protected string GetMaterialSymbol(string defaultSymbol)
        {
            return Material?.ModifySymbol?.Invoke(null, this) ?? defaultSymbol;
        }


        public static void AddTypeToMap<T>(int type)
        {
            if (!typeMapping.ContainsKey(type))
            {
                typeMapping.Add(type, typeof(T));
                lastTypeId++;
            }
        }

        public static void AddTypeToMap<T>()
        {
            do
            {
                lastTypeId++;
            } while (typeMapping.ContainsKey(lastTypeId));
            typeMapping.Add(lastTypeId, typeof(T));
        }

        public static int GetTypeFromMap(Type t)
        {
            return typeMapping.FirstOrDefault(x => x.Value == t).Key;
        }

        public static Stack GetItem(int type, int count)
        {
            if (typeMapping.TryGetValue(type, out Type item))
            {
                return new Stack((Item)Activator.CreateInstance(item), count);
            }

            return null;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            GetTexture().Draw(spriteBatch, position);
        }

        public bool ApplyMaterial(int type)
        {
            Material material = MaterialSystem.GetMaterial(type);
            if (material == null) return false;

            if (CanApplyMaterial(material))
                this.material = type;
            
            return true;
        }

        public bool ApplyMaterial(string name)
        {
            Material material = MaterialSystem.GetMaterial(name);
            if (material == null) return false;

            if (CanApplyMaterial(material))
                this.material = material.Type;

            return true;
        }

        public Item Copy() 
        { 
            return (Item)MemberwiseClone(); 
        }

        public virtual bool CanBePickedUp()
        {
            foreach (GlobalItem glItem in CaModLoader.modGlobalItems)
            {
                bool? customPickUp = glItem.CanBePickedUp(this, ConsoleAdventure.world.GetLocalPlayer());
                if (customPickUp != null)
                {
                    return (bool)customPickUp;
                }
            }
            return true;
        }

        public virtual Recipe AddRecipe()
        {
            return null;
        }

        public virtual void UseItem()
        {

        }
        public virtual void Droped()
        {

        }
        public virtual void PickedUp()
        {

        }

        /// <summary>
        /// Вызывается при применении материала к предмету. Нужно для изменения 
        /// параметров предмета в зависимости от материала и для кастомных правил 
        /// ограничия применения их. 
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public virtual bool CanApplyMaterial(Material material)
        {
            return true;
        }
    }
}
