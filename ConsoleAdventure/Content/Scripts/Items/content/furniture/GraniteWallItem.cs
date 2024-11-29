using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class GraniteWallItem : PlaceableItem
    {
        public GraniteWallItem()
        {
            name = Localization.GetTranslation("Transforms", "GraniteWall");
            description = GetDescription();
            placeType = (int)RenderFieldType.graniteWall;
            AddTypeToMap<GraniteWallItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("∫", new Color(45, 45, 45));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new GraniteItem(), 1);
            recipe.AddStation((int)RenderFieldType.workbench);
            return recipe;
        }
    }
}