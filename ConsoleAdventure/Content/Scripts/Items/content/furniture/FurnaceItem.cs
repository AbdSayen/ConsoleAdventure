using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
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

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Transforms", material.Name) + " " +
                   Localization.GetTranslation("Transforms", "Furnace");
            return true;
        }

        public override CharTexture GetTexture()
        {
            Color color = GetMaterialColor();

            return new CharTexture().AddLayer("■", color) //Color.Gray
                                    .AddLayer("&", color) //Color.Gray
                                    .AddLayer("┻", color) //Color.Gray
                                    .AddLayer("⌂", (color.ToVector3() * 0.35f).ToColor()); //new Color(50, 50, 50)
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new StoneItem(), 10, inheritedMaterial: true);
            recipe.AddIngredient(new Log(), 5);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
