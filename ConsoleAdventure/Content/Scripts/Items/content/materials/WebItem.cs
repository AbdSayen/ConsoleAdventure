using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WebItem : PlaceableItem
    {
        public WebItem()
        {
            name = Localization.GetTranslation("Transforms", "Web");
            description = GetDescription();
            placeType = (int)VanillaTransforms.web;
            AddTypeToMap<WebItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("¼", new Color(120, 120, 120));
        }
    }
}