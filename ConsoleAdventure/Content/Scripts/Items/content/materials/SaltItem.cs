using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class SaltItem : PlaceableItem
    {
        public SaltItem()
        {
            name = Localization.GetTranslation("Transforms", "Salt");
            description = GetDescription();
            placeType = (int)RenderFieldType.salt;
            AddTypeToMap<SaltItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "∆" },
                    new List<Color>() { new Color(193, 157, 175) }
                   );
        }
    }
}