using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class ChestItem : PlaceableItem
    {
        public ChestItem()
        {
            name = Localization.GetTranslation("Transforms", "Chest");
            description = GetDescription();
            placeType = (int)VanillaTransforms.chest;
            AddTypeToMap<ChestItem>();
        }

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Materials", material.Name) + " " +
                   Localization.GetTranslation("Transforms", "Chest");
            return true;
        }

        public override CharTexture GetTexture()
        {
            Color color = GetMaterialColor();

            return new CharTexture().AddLayer("■", color * 0.45f, new Vector2(0, -1)) //new Color(43, 23, 15)
                                    .AddLayer("▬", color, new Vector2(0, 2)) //new Color(94, 61, 38)
                                    .AddLayer("▬", color, new Vector2(0, 5)) //new Color(94, 61, 38)
                                    .AddLayer("─", Color.Gray, new Vector2(0, 1))
                                    .AddLayer("ˈ", Color.Gray, new Vector2(4, 8));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 5, true);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
