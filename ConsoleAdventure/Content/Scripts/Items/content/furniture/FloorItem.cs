using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class FloorItem : PlaceableItem
    {
        public FloorItem()
        {
            name = Localization.GetTranslation("Transforms", "Floor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.floor;
            maxCount = 100;
            AddTypeToMap<FloorItem>();
        }

        public override bool CanApplyMaterial(Material material)
        {
            string materialName = Localization.GetTranslation("Transforms", material.Name);

            if (materialName == "")
            {
                materialName = Localization.GetTranslation("Materials", material.Name);
            }

            name = materialName  + " " + Localization.GetTranslation("Transforms", "Floor");
            return true;
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(GetMaterialSymbol("."), GetMaterialColor());
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new Log(), true);
            recipe.AddStation((int)VanillaTransforms.workbench);
            ConsoleAdventure.recipes.Add(recipe);

            recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new StoneItem(), true);
            recipe.AddStation((int)VanillaTransforms.workbench);
            return recipe;
        }
    }
}
