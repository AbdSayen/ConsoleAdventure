using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class ZoisiteItem : PlaceableItem
    {
        public ZoisiteItem()
        {
            name = Localization.GetTranslation("Transforms", "Zoisite");
            description = GetDescription();
            placeType = (int)RenderFieldType.zoisite;
            AddTypeToMap<ZoisiteItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("η", new Color(6, 61, 31));
        }
    }
}