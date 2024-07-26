using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace CaModLoaderAPI
{
    public static class Main
    {
        public static int vanillaTypesInitialized = Enum.GetNames(typeof(RenderFieldType)).Length;
        public static Dictionary<Type, int> modTypesInitialized = new Dictionary<Type, int>();

        public static int GetModBlock<T>()
        {
            int id = modTypesInitialized[typeof(T)];
            id = vanillaTypesInitialized + id;
            return id;
        }
    }
}
