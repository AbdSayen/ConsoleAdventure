using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class GraniteItem : PlaceableItem
    {
        public GraniteItem()
        {
            name = Localization.GetTranslation("Transforms", "Granite");
            description = GetDescription();
            placeType = (int)RenderFieldType.granite;
            AddTypeToMap<GraniteItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "#" },
                    new List<Color>() { new Color(45, 45, 45) }
                   );
        }
    }
}