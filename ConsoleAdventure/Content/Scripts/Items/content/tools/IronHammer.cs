using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class IronHammer : Item
    {
        public IronHammer()
        {
            name = Localization.GetTranslation("Items", "IronHammer");
            description = GetDescription();
            hammer = 1;
            maxCount = 1;
            AddTypeToMap<IronHammer>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("/", new(94, 61, 38)).AddLayer("▬", Color.Gray, new(1, -2), 0.25f);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new IronBar(), 2);
            recipe.AddIngredient(new Log(), 1);
            recipe.AddStation((int)RenderFieldType.anvil);

            return recipe;
        }
    }
}