using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BrokenLogItem : PlaceableItem
    {
        public BrokenLogItem()
        {
            name = Localization.GetTranslation("Transforms", "BrokenLog");
            description = GetDescription();
            placeType = (int)VanillaTransforms.brokenLog;
            AddTypeToMap<BrokenLogItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(":", ColorAssets.woodenColor);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
