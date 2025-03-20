using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WallItem : PlaceableItem
    {
        public WallItem()
        {
            name = Localization.GetTranslation("Transforms", "Wall");
            description = GetDescription();
            placeType = (int)VanillaTransforms.wall;
            AddTypeToMap<WallItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("#", Color.White);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new StoneItem(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
