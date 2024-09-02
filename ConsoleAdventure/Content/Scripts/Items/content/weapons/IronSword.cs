using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class IronSword : Item
    {
        public IronSword()
        {
            name = Localization.GetTranslation("Items", "IronSword");
            description = GetDescription();
            damage = 1;
            damageClass = 1;
            AddTypeToMap<IronSword>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "̸", "₋" },
                    new List<Color>() { Color.Gray, Color.Gray}
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new IronBar(), 6);
            recipe.AddStation((int)RenderFieldType.anvil);

            return recipe;
        }
    }
}