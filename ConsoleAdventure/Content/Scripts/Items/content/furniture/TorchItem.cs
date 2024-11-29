using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class TorchItem : PlaceableItem
    {
        public TorchItem()
        {
            name = Localization.GetTranslation("Transforms", "Torch");
            description = GetDescription();
            placeType = (int)RenderFieldType.torch;
            AddTypeToMap<TorchItem>();
        }

        int timer;

        public override CharTexture GetTexture()
        {
            timer++;
            return new CharTexture().AddLayer("/", new(94, 61, 38))
                                    .AddLayer("●", Color.OrangeRed, new(-0.58f + 2, -0.03f - 2.5f))
                                    .AddLayer("•", Color.OrangeRed * 0.8f, new Vector2(0.42f + (float)(Math.Sin(timer / 30 * Math.PI) / 2) + 2, -2.4f - 2.5f))
                                    .AddLayer("•", Color.Orange * (float)(1 + Math.Sin(timer / 60 * Math.PI) / 4), new Vector2(0.2f + 2, -0.03f - 2.5f))
;
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 3));
            recipe.AddIngredient(new Log(), 1);

            return recipe;
        }
    }
}
