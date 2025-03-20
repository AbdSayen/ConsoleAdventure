using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class DoorItem : PlaceableItem
    {
        public DoorItem()
        {
            name = Localization.GetTranslation("Transforms", "Door");
            description = GetDescription();
            placeType = (int)VanillaTransforms.door;
            AddTypeToMap<DoorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("█", new(94, 61, 38))
                                    .AddLayer("╴", Color.Gray);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 3);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
