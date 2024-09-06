using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class ZoisiteItem : Item
    {
        public ZoisiteItem()
        {
            name = Localization.GetTranslation("Transforms", "Zoisite");
            description = GetDescription();
            placeType = (int)RenderFieldType.zoisite;
            AddTypeToMap<ZoisiteItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "η" },
                    new List<Color>() { new Color(6, 61, 31) }
                   );
        }
    }
}