using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Apple : Food
    {
        public Apple()
        {
            satiety = 1;
            name = Localization.GetTranslation("Items", GetType().Name);
            description = GetDescription();

            AddTypeToMap<Apple>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "o", "`" },
                    new List<Color>() { Color.Red, Color.Green }
                   );
        }

        public new string GetDescription()
        {
            return " " + Localization.GetTranslation("ItemDescription", GetType().Name) + "\n " + base.GetDescription();
        }
    }
}