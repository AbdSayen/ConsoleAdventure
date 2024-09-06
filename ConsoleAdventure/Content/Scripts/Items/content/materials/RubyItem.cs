using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class RubyItem : Item
    {
        public RubyItem()
        {
            name = Localization.GetTranslation("Transforms", "Ruby");
            description = GetDescription();
            placeType = (int)RenderFieldType.ruby;
            AddTypeToMap<RubyItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "♦" },
                    new List<Color>() { new Color(200, 21, 110) }
                   );
        }
    }
}