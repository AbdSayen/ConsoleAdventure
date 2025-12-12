using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class StoneItem : PlaceableItem
    {
        public StoneItem()
        {
            name = Localization.GetTranslation("Transforms", "Stone");
            description = GetDescription();
            placeType = -1; 

            AddTypeToMap<StoneItem>();
        }

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Transforms", material.Name);

            placeType = material?.FromTransformType ?? -1;
            if (placeType == 0) placeType = -1;

            return true;
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(GetMaterialSymbol("●"), GetMaterialColor());
        }
    }
}