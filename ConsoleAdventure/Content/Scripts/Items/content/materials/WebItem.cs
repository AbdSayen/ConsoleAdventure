using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WebItem : PlaceableItem
    {
        public WebItem()
        {
            name = Localization.GetTranslation("Transforms", "Web");
            description = GetDescription();
            placeType = (int)RenderFieldType.web;
            AddTypeToMap<WebItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "¼" },
                    new List<Color>() { new Color(120, 120, 120) }
                   );
        }
    }
}