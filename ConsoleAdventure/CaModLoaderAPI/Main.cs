using ConsoleAdventure;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAdventureMain = ConsoleAdventure.ConsoleAdventure;

namespace CaModLoaderAPI
{
    public static class Main
    {
        public static int vanillaTypesInitialized = Enum.GetNames(typeof(VanillaTransforms)).Length;
        public static Dictionary<string, Dictionary<Type, int>> modTypesInitialized = new Dictionary<string, Dictionary<Type, int>>();

        //public static Dictionary<string, byte> modTransformTypesOffset = new Dictionary<string, byte>();
        public static Dictionary<Type, byte> modTransformTypes = new Dictionary<Type, byte>();

        public static int AllTransformCount { get; internal set; } = vanillaTypesInitialized;

        public static Mod GetModInstance<T>()
        {
            if (CaModLoader.modsIdsMap.ContainsKey(typeof(T)))
            {
                int modId = CaModLoader.modsIdsMap[typeof(T)];
                return CaModLoader.GetActiveMods()[modId];
            }
            return null;
        }

        public static GlobalPlayer GetModGlobalPlayer<T>()
        {
            if (CaModLoader.modGlobalPlayers.Where((item) => item.GetType() == typeof(T)).Count() > 0)
            {
                return CaModLoader.modGlobalPlayers.Where((item) => item.GetType() == typeof(T)).FirstOrDefault();
            }
            return null;
        }

        public static GlobalItem GetModGlobalItem<T>()
        {
            if (CaModLoader.modGlobalItems.Where((item) => item.GetType() == typeof(T)).Count() > 0)
            {
                return CaModLoader.modGlobalItems.Where((item) => item.GetType() == typeof(T)).FirstOrDefault();
            }
            return null;
        }

        public static int GetModTransform<T>()
        {
            return modTransformTypes[typeof(T)];
        }

        public static int GetModTransform(Type type)
        {
            return modTransformTypes[type];
        }

        public static void InitTransformsTypes(int lastVanillaTransformCount)
        {
            if (ConsoleAdventureMain.world == null) return;

            AllTransformCount = vanillaTypesInitialized;
            modTransformTypes.Clear();

            List<Type> newTypes = new List<Type>();
           
            if (ConsoleAdventureMain.world.modTransforms != null)
            {
                foreach (Type transform in CaModLoader.modTransforms)
                {
                    if (ConsoleAdventureMain.world.modTransforms.TryGetValue(transform.Name, out int type))
                    {
                        modTransformTypes.Add(transform, (byte)(type));
                        AllTransformCount++;
                    }

                    else
                    {
                        newTypes.Add(transform);
                    }
                }
            }

            else
            {
                newTypes = CaModLoader.modTransforms;
                ConsoleAdventureMain.world.modTransforms = new();
            }

            foreach (Type transform in newTypes)
            {
                modTransformTypes.Add(transform, (byte)AllTransformCount);
                ConsoleAdventureMain.world.modTransforms.Add(transform.Name, AllTransformCount);
                AllTransformCount++;
            }
        }
    }
}
