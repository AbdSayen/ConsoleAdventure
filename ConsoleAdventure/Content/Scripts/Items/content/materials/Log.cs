using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
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
            placeType = (int)VanillaTransforms.log;
            AddTypeToMap<Log>();
        }

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Materials", material.Name);

            return true;
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(GetMaterialSymbol("⸗"), GetMaterialColor()); //new Color(94, 61, 38)
        }

        public new string GetDescription()
        {
            return Localization.GetTranslation("ItemDescription", GetType().Name);
        }
    }
}