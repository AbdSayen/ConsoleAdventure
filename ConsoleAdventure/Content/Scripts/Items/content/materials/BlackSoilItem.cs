using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BlackSoilItem : PlaceableItem
    {
        public BlackSoilItem()
        {
            name = Localization.GetTranslation("Transforms", "BlackSoil");
            description = GetDescription();
            placeType = (int)VanillaTransforms.blackSoil;
            AddTypeToMap<BlackSoilItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("≈", new Color(20, 20, 20));
        }
    }
}