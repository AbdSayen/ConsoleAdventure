using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class AnvilItem : PlaceableItem
    {
        public AnvilItem()
        {
            name = Localization.GetTranslation("Transforms", "Anvil");
            description = GetDescription();
            placeType = (int)RenderFieldType.anvil;
            AddTypeToMap<AnvilItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("■", new(40, 40, 40)).AddLayer("σ", Color.Gray);
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
