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
            name = Localization.GetTranslation("Transforms", "GraniteFloorItem");
            description = GetDescription();
            placeType = (int)RenderFieldType.graniteFloor;
            placeLayer = World.FloorLayerId;
            maxCount = 100;
            AddTypeToMap<GraniteFloorItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "~" },
                    new List<Color>() { new(45, 45, 45) }
                   );
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new GraniteItem(), 1);
            recipe.AddStation((int)RenderFieldType.workbench);
            return recipe;
        }
    }
}
