using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class FurnaceItem : Item
    {
        public FurnaceItem()
        {
            name = Localization.GetTranslation("Transforms", "Furnace");
            description = GetDescription();
            placeType = (int)RenderFieldType.furnace;
            AddTypeToMap<FurnaceItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "▄", "■", "⌂" },
                    new List<Color>() { Color.DarkGray, Color.Gray, new Color(50, 50, 50)}
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new StoneItem(), 10);
            recipe.AddIngredient(new Log(), 5);
            recipe.AddStation((int)RenderFieldType.workbench);

            return recipe;
        }
    }
}
