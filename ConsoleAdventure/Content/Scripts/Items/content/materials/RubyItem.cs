using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class RubyItem : PlaceableItem
    {
        public RubyItem()
        {
            name = Localization.GetTranslation("Transforms", "Ruby");
            description = GetDescription();
            placeType = (int)RenderFieldType.ruby;
            AddTypeToMap<RubyItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("♦", new Color(200, 21, 110));
        }
    }
}