using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WoodFloorItem : PlaceableItem
    {
        public WoodFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "WoodFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.woodFloor;
            placeLayer = World.FloorLayerId;
            maxCount = 100;
            AddTypeToMap<WoodFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(".", new(94, 61, 38));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new Log(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);
            return recipe;
        }
    }
}
