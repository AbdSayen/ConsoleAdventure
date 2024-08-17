using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class AnvilItem : Item
    {
        public AnvilItem()
        {
            name = Localization.GetTranslation("Transforms", "Anvil");
            description = GetDescription();
            placeType = (int)RenderFieldType.anvil;
            AddTypeToMap<AnvilItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() {"■", "σ" },
                    new List<Color>() { new(40, 40, 40), Color.Gray}
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new IronBar(), 8);
            recipe.AddStation((int)RenderFieldType.workbench);
            return recipe;
        }
    }
}
