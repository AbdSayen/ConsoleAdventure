using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class StoneItem : PlaceableItem
    {
        public StoneItem()
        {
            name = Localization.GetTranslation("Transforms", "Stone");
            description = GetDescription();
            placeType = (int)RenderFieldType.stone;
            AddTypeToMap<StoneItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("●", Color.Gray);
        }
    }
}