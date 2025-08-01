using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BasaltItem : PlaceableItem
    {
        public BasaltItem()
        {
            name = Localization.GetTranslation("Transforms", "Basalt");
            description = GetDescription();
            placeType = (int)VanillaTransforms.basalt;
            AddTypeToMap<BasaltItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("‡", new Color(45, 45, 45));
        }
    }
}