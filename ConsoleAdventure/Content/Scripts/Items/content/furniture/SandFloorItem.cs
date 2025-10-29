using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConsoleAdventure
{
    [Serializable]
    public class SandFloorItem : PlaceableItem
    {
        public SandFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "SandFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.sandFloor;
            maxCount = 100;
            AddTypeToMap<SandFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("≈", new Color(196, 190, 11));
        }
    }
}