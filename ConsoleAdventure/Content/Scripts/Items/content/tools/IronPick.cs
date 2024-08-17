using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class IronPick : Item
    {
        public IronPick()
        {
            name = Localization.GetTranslation("Items", "IronPick");
            description = GetDescription();
            pick = 10;
            AddTypeToMap<IronPick>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "/", "͡" },
                    new List<Color>() { new(94, 61, 38), Color.Gray }
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new IronBar(), 3);
            recipe.AddIngredient(new Log(), 1);
            recipe.AddStation((int)RenderFieldType.anvil);

            return recipe;
        }
    }
}