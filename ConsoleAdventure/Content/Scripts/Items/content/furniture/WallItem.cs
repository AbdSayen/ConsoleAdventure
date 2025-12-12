using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
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

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Transforms", material.Name) + " " +
                   Localization.GetTranslation("Transforms", "Wall");
            return true;
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(GetMaterialSymbol("#"), GetMaterialColor());
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new StoneItem(), true);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
