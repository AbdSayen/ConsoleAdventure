using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class QuartzItem : Item
    {
        public QuartzItem()
        {
            name = Localization.GetTranslation("Transforms", "Quartz");
            description = GetDescription();
            placeType = (int)RenderFieldType.quartz;
            AddTypeToMap<QuartzItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "◊" },
                    new List<Color>() { Color.White }
                   );
        }
    }
}