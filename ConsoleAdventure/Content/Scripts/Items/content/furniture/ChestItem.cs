using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class ChestItem : Item
    {
        public ChestItem()
        {
            name = Localization.GetTranslation("Transforms", "Chest");
            description = GetDescription();
            placeType = (int)RenderFieldType.chest;
            placeLayer = World.ItemsLayerId;
            AddTypeToMap<ChestItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "■", "+"/*"̷"*/ },
                    new List<Color>() { new Color(94, 61, 38), new Color(43, 23, 15)}
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 5);
            recipe.AddStation((int)RenderFieldType.workbench);

            return recipe;
        }
    }
}
