using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class GraniteFloorItem : PlaceableItem
    {
        public GraniteFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "GraniteFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.graniteFloor;
            placeLayer = World.FloorLayerId;
            maxCount = 100;
            AddTypeToMap<GraniteFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("~", new(45, 45, 45));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new GraniteItem(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);
            return recipe;
        }
    }
}
