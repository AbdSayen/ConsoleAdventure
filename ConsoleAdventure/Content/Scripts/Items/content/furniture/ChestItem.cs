using ConsoleAdventure.Content.Scripts;
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

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("■", new Color(43, 23, 15), new Vector2(0, -1))
                                    .AddLayer("▬", new Color(94, 61, 38), new Vector2(0, 2))
                                    .AddLayer("▬", new Color(94, 61, 38), new Vector2(0, 5))
                                    .AddLayer("─", Color.Gray, new Vector2(0, 1))
                                    .AddLayer("ˈ", Color.Gray, new Vector2(4, 8));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 5);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
