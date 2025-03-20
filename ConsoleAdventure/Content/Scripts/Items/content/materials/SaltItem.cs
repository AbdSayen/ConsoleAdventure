using ConsoleAdventure.Content.Scripts;
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
            placeType = (int)VanillaTransforms.salt;
            AddTypeToMap<SaltItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("∆", new Color(193, 157, 175));
        }
    }
}