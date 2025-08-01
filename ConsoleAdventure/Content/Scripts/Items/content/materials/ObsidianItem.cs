using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class ObsidianItem : PlaceableItem
    {
        public ObsidianItem()
        {
            name = Localization.GetTranslation("Transforms", "Obsidian");
            description = GetDescription();
            placeType = (int)VanillaTransforms.obsidian;
            AddTypeToMap<ObsidianItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("Ϟ", new Color(25, 0, 84));
        }
    }
}