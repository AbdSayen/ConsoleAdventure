using CaModLoaderAPI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModifyColor = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, ConsoleAdventure.Content.Scripts.SmartColor>;
using ModifySymbol = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, string>;

namespace ConsoleAdventure.Content.Scripts.MaterialLogic
{
    public class Material
    {
        public int Type { get; private set; } = 0;

        public Mod Mod { get; private set; } = null;

        public string ModName => Mod?.modName ?? "ConsoleAdventure";
        
        public string Name { get; private set; }

        public MaterialType MaterialType { get; private set; }

        /// <summary>
        /// Изменяет цвет предмета или т-форма к которому был применён материал. 
        /// </summary>
        public ModifyColor ModifyColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ModifySymbol ModifySymbol { get; set; }

        /// <summary>
        /// Айди т-форма, создавшего этот материал. Если он был 
        /// создан иным спосабом, то значение будет равно 0
        /// </summary>
        public int FromTransformType { get; set; } = 0;

        internal Material(MaterialType materialType, string name, int type)
        {
            if (materialType == null) throw new ArgumentNullException(nameof(materialType));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name == "") throw new ArgumentException(nameof(name));

            MaterialType = materialType;
            Name = name;
            Type = type;
        }
    }
}
