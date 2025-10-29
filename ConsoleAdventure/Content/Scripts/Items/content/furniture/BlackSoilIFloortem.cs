using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConsoleAdventure
{
    [Serializable]
    public class BlackSoilFloorItem : PlaceableItem
    {
        public BlackSoilFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "BlackSoilFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.blackSoilFloor;
            maxCount = 100;
            AddTypeToMap<BlackSoilItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("`", new Color(20, 20, 20));
        }
    }
}