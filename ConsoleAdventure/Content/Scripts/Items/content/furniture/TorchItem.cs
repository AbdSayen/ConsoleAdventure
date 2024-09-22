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

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "/", "*" },
                    new List<Color>() { new(94, 61, 38), Color.OrangeRed}
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 3));
            recipe.AddIngredient(new Log(), 1);

            return recipe;
        }
    }
}
