using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WorkbenchItem : Item
    {
        public WorkbenchItem()
        {
            name = Localization.GetTranslation("Transforms", "Workbench");
            description = GetDescription();
            placeType = (int)RenderFieldType.workbench;
            AddTypeToMap<WorkbenchItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "∏" },
                    new List<Color>() { new Color(94, 61, 38) }
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 5);

            return recipe;
        }
    }
}
