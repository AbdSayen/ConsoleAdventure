using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.MaterialTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using ModifyColor = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, ConsoleAdventure.Content.Scripts.SmartColor>;
using ModifySymbol = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, string>;

namespace ConsoleAdventure.Content.Scripts.MaterialLogic
{
    public static class MaterialSystem
    {
        public static Dictionary<string, Material> Materials { get; private set; }

        public static void Init()
        {
            Materials = new Dictionary<string, Material>();
        }

        public static Material AddMaterial(Mod mod, MaterialType materialType, string name, ModifyColor modifyColor, ModifySymbol modifySymbol)
        {
            string modName = mod?.modName ?? "ConsoleAdventure";

            if (Materials.ToList().FindIndex(m => m.Value.Name == name && m.Value.ModName == modName) > -1)
                throw new Exception("This material already exists in this mod!");

            int newType = Materials.Count;

            Material material = new Material(materialType, name, newType);
            material.ModifyColor = modifyColor;
            material.ModifySymbol = modifySymbol;

            Materials.Add(material.Name, material);
            return material;
        }

        public static Material GetMaterial(int type)
        {
            if (type < 0 || type >= Materials.Count)
                return null;

            var materials = Materials.ToList();

            return materials[type].Value;
        }

        public static Material GetMaterial(string name)
        {
            if (name == null || name == "")
                return null;

            if (Materials.TryGetValue(name, out Material material))
                return material;

            return null;
        }
    }
}
