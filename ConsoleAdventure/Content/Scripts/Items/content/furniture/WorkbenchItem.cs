using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WorkbenchItem : PlaceableItem
    {
        public WorkbenchItem()
        {
            name = Localization.GetTranslation("Transforms", "Workbench");
            description = GetDescription();
            placeType = (int)RenderFieldType.workbench;
            AddTypeToMap<WorkbenchItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("∏", new Color(94, 61, 38));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 5);

            return recipe;
        }
    }
}
