using CaModLoaderAPI;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using ModifyColor = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, ConsoleAdventure.Content.Scripts.SmartColor>;
using ModifySymbol = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, string>;

namespace ConsoleAdventure.Content.Scripts.MaterialLogic
{
    public static class MaterialSystem
    {
        public static List<Material> Materials { get; private set; }

        private static Dictionary<string, int> Types { get; set; }

        public static void Init(World world)
        {
            Materials = new List<Material>();
            Types = new Dictionary<string, int>();

            foreach (var unloadMaterial in world.materials)
            {
                string name = unloadMaterial.Key;
                int type = unloadMaterial.Value;

                if (type >= Materials.Count)
                {
                    for (int i = Materials.Count; i < type + 1; i++)
                    {
                        Materials.Add(null);
                    }
                }

                Types.Add(name, type);
                Materials[type] = new UnloadMaterial(name);
            }
        }

        public static Material AddMaterial(Mod mod, MaterialType materialType, string name, ModifyColor modifyColor, ModifySymbol modifySymbol)
        {
            string modName = mod?.modName ?? "ConsoleAdventure";

            if (Materials.FindIndex(m => m.Name == name && m.ModName == modName && !(m is UnloadMaterial)) > -1)
                throw new Exception("This material already exists in this mod!");

            Material material = new Material(mod, materialType, name);
            material.ModifyColor = modifyColor;
            material.ModifySymbol = modifySymbol;

            if (Types.TryGetValue(material.Name, out int type))
            {
                material.SetType(type);
                Materials[type] = material;
            }

            else
            {
                material.SetType(Materials.Count);

                Types.Add(material.Name, Materials.Count);
                Materials.Add(material);
            }

            return material;
        }

        public static Material GetMaterial(int type)
        {
            if (type < 0 || type >= Materials.Count)
                return null;

            Material material = Materials[type];

            if (material?.Type != type)
                return null;

            return material;
        }

        public static Material GetMaterial(string name)
        {
            if (name == null || name == "")
                return null;

            if (Types.TryGetValue(name, out int type))
                return GetMaterial(type);

            return null;
        }
    }
}
