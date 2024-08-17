using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class IronBar : Item
    {
        public IronBar()
        {
            name = Localization.GetTranslation("Items", "IronBar");
            description = GetDescription();
            AddTypeToMap<IronBar>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "■", "▬" },
                    new List<Color>() { Color.Gray, Color.White }
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new BrownIronOreItem(), 3);
            recipe.AddStation((int)RenderFieldType.furnace);

            return recipe;
        }
    }
}