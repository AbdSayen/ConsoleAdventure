using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class GranuliteFloorItem : PlaceableItem
    {
        public GranuliteFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "GranuliteFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.granuliteFloor;
            maxCount = 100;
            AddTypeToMap<GranuliteFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("<", new(110, 110, 78));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new GranuliteItem(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);
            return recipe;
        }
    }
}
