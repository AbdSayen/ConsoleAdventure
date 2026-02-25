using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
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
            placeType = (int)VanillaTransforms.workbench;
            AddTypeToMap<WorkbenchItem>();
        }

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Materials", material.Name) + " " +
                   Localization.GetTranslation("Transforms", "Workbench");
            return true;
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(GetMaterialSymbol("∏"), GetMaterialColor());
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 5, true);

            return recipe;
        }
    }
}
