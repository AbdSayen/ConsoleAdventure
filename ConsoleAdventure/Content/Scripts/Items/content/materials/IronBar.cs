using ConsoleAdventure.Content.Scripts;
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

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("▬", new(50, 50, 50), new(0, 4f), -0.2f)
                                    .AddLayer("▬", new(50, 50, 50), new(-1, 3), -0.2f)
                                    .AddLayer("▬", new(180, 180, 180), new(0, 1f), -0.2f)
                                    .AddLayer("▬", new(180, 180, 180), new(-1, 0), -0.2f)
                                    .AddLayer("─", Color.White, new(0, 2), -0.2f)
                                    .AddLayer("ˈ", Color.White, new(2, 8));
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