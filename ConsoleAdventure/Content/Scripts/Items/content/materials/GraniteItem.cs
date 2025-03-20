using ConsoleAdventure.Content.Scripts;
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
            placeType = (int)VanillaTransforms.granite;
            AddTypeToMap<GraniteItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("#", new Color(45, 45, 45));
        }
    }
}