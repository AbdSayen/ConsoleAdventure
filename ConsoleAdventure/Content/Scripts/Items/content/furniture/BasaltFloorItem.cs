using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BasaltFloorItem : PlaceableItem
    {
        public BasaltFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "BasaltFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.basaltFloor;
            maxCount = 100;
            AddTypeToMap<BasaltFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("“", new(45, 45, 45));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new BasaltItem(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);
            return recipe;
        }
    }
}
