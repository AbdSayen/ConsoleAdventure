using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class QuartzItem : PlaceableItem
    {
        public QuartzItem()
        {
            name = Localization.GetTranslation("Transforms", "Quartz");
            description = GetDescription();
            placeType = (int)VanillaTransforms.quartz;
            AddTypeToMap<QuartzItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("◊", Color.White);
        }
    }
}