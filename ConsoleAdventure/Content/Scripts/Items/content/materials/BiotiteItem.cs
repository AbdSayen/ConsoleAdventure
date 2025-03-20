using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BiotiteItem : PlaceableItem
    {
        public BiotiteItem()
        {
            name = Localization.GetTranslation("Transforms", "Biotite");
            description = GetDescription();
            placeType = (int)VanillaTransforms.biotite;
            AddTypeToMap<BiotiteItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("≡", new Color(45, 45, 45));
        }
    }
}