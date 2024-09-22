using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class StoneItem : PlaceableItem
    {
        public StoneItem()
        {
            name = Localization.GetTranslation("Transforms", "Stone");
            description = GetDescription();
            placeType = (int)RenderFieldType.stone;
            AddTypeToMap<StoneItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "●" },
                    new List<Color>() { Color.Gray }
                   );
        }
    }
}