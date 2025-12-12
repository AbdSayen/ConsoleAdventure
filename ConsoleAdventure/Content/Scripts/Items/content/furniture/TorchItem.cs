using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
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
            placeType = (int)VanillaTransforms.torch;
            AddTypeToMap<TorchItem>();
        }

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Materials", material.Name) + " " +
                   Localization.GetTranslation("Transforms", "Torch");
            return true;
        }

        int timer;

        public override CharTexture GetTexture()
        {
            timer++;
            return new CharTexture().AddLayer(GetMaterialSymbol("/"), GetMaterialColor())
                                    .AddLayer("●", Color.OrangeRed, new(-0.58f + 2, -0.03f - 2.5f))
                                    .AddLayer("•", Color.OrangeRed * 0.8f, new Vector2(0.42f + (float)(Math.Sin(timer / 30 * Math.PI) / 2) + 2, -2.4f - 2.5f))
                                    .AddLayer("•", Color.Orange * (float)(1 + Math.Sin(timer / 60 * Math.PI) / 4), new Vector2(0.2f + 2, -0.03f - 2.5f))
;
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 3));
            recipe.AddIngredient(new Log(), 1, true);

            return recipe;
        }
    }
}
