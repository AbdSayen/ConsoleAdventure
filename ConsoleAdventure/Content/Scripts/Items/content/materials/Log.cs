using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Log : PlaceableItem
    {
        public Log()
        {
            name = Localization.GetTranslation("Items", GetType().Name);
            description = GetDescription();
            placeType = (int)RenderFieldType.log;
            AddTypeToMap<Log>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("⸗", new Color(94, 61, 38));
        }

        public new string GetDescription()
        {
            return Localization.GetTranslation("ItemDescription", GetType().Name);
        }
    }
}