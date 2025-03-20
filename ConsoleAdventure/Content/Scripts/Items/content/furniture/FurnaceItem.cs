using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class FurnaceItem : PlaceableItem
    {
        public FurnaceItem()
        {
            name = Localization.GetTranslation("Transforms", "Furnace");
            description = GetDescription();
            placeType = (int)VanillaTransforms.furnace;
            AddTypeToMap<FurnaceItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("■", Color.Gray)
                                    .AddLayer("&", Color.Gray)
                                    .AddLayer("┻", Color.Gray)
                                    .AddLayer("⌂", new Color(50, 50, 50));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new StoneItem(), 10);
            recipe.AddIngredient(new Log(), 5);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
