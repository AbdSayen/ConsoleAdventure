using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BrownIronOreItem : PlaceableItem
    {
        public BrownIronOreItem()
        {
            name = Localization.GetTranslation("Transforms", "BrownIronOre");
            description = GetDescription();
            placeType = (int)RenderFieldType.brownIronOre;
            AddTypeToMap<BrownIronOreItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "§" },
                    new List<Color>() { new Color(206, 83, 33) }
                   );
        }
    }
}