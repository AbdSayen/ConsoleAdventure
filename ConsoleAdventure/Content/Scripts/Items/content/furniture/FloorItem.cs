using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class FloorItem : PlaceableItem
    {
        public FloorItem()
        {
            name = Localization.GetTranslation("Transforms", "Floor");
            description = GetDescription();
            placeType = (int)RenderFieldType.floor;
            placeLayer = World.FloorLayerId;
            maxCount = 100;
            AddTypeToMap<FloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer(".", Color.Gray);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new StoneItem(), 1);
            recipe.AddStation((int)RenderFieldType.workbench);
            return recipe;
        }
    }
}
