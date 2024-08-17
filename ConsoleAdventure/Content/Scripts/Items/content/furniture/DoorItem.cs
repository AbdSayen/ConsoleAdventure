using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class DoorItem : Item
    {
        public DoorItem()
        {
            name = Localization.GetTranslation("Transforms", "Door");
            description = GetDescription();
            placeType = (int)RenderFieldType.door;
            AddTypeToMap<DoorItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "█", "╴" },
                    new List<Color>() { new(94, 61, 38), Color.Gray }
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 3);
            recipe.AddStation((int)RenderFieldType.workbench);

            return recipe;
        }
    }
}
