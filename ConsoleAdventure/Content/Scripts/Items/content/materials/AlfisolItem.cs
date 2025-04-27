using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class AlfisolItem : PlaceableItem
    {
        public AlfisolItem()
        {
            name = Localization.GetTranslation("Transforms", "Alfisol");
            description = GetDescription();
            placeType = (int)VanillaTransforms.alfisol;
            AddTypeToMap<AlfisolItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("≈", new Color(54, 43, 32));
        }
    }
}