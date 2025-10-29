using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class ObsidianFloorItem : PlaceableItem
    {
        public ObsidianFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "ObsidianFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.obsidianFloor;
            maxCount = 100;
            AddTypeToMap<ObsidianFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("¬", new(25, 0, 84));
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 2));
            recipe.AddIngredient(new ObsidianItem(), 1);
            recipe.AddStation((int)VanillaTransforms.workbench);
            return recipe;
        }
    }
}
