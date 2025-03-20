using ConsoleAdventure.Content.Scripts;
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
            placeType = (int)VanillaTransforms.brownIronOre;
            AddTypeToMap<BrownIronOreItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("§", new Color(206, 83, 33));
        }
    }
}