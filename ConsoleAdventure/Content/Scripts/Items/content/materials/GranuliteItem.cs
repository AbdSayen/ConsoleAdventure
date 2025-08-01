using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class GranuliteItem : PlaceableItem
    {
        public GranuliteItem()
        {
            name = Localization.GetTranslation("Transforms", "Granulite");
            description = GetDescription();
            placeType = (int)VanillaTransforms.granulite;
            AddTypeToMap<GranuliteItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("≤", new Color(110, 110, 78));
        }
    }
}