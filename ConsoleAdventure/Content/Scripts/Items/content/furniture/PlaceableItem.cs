using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class PlaceableItem : Item
    {
        public int placeType = -1;
        public int placeLayer = World.BlocksLayerId;

        public new string GetDescription()
        {
            description = Localization.GetTranslation("Generic", "CanBePlaced");
            return description;
        }
    }
}
