using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class FloorItem : Item
    {
        public FloorItem()
        {
            name = Localization.GetTranslation("Transforms", "Floor");
            description = GetDescription();
            placeType = (int)RenderFieldType.floor;
            AddTypeToMap<FloorItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "." },
                    new List<Color>() { Color.Gray}
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new StoneItem(), 1);
            recipe.AddStation((int)RenderFieldType.workbench);
            return recipe;
        }
    }
}
