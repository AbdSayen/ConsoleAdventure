using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace CaModLoaderAPI
{
    public static class Main
    {
        public static int vanillaTypesInitialized = Enum.GetNames(typeof(RenderFieldType)).Length;
        public static Dictionary<string, Dictionary<Type, int>> modTypesInitialized = new Dictionary<string, Dictionary<Type, int>>();

        public static Dictionary<string, byte> modTransformTypesOffset = new Dictionary<string, byte>();


        public static Mod GetModInstance<T>()
        {
            if (CaModLoader.modsIdsMap.ContainsKey(typeof(T)))
            {
                int modId = CaModLoader.modsIdsMap[typeof(T)];
                return CaModLoader.GetActiveMods()[modId];
            }
            return null;
        }

        public static int GetModTransform<T>(string mod)
        {
            int id = modTypesInitialized[mod][typeof(T)];
            id = vanillaTypesInitialized + modTransformTypesOffset[mod] + id;
            return id;
        }
    }
}
