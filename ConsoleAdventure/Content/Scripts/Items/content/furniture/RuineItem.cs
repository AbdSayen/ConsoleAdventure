using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class RuineItem : PlaceableItem
    {
        public RuineItem()
        {
            name = Localization.GetTranslation("Transforms", "Ruine");
            description = GetDescription();
            placeType = (int)VanillaTransforms.ruine;
            AddTypeToMap<RuineItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(":", Color.Gray);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new StoneItem(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
