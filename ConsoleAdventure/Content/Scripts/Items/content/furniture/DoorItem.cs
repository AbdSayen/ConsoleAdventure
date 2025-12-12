using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
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

        public override bool CanApplyMaterial(Material material)
        {
            name = Localization.GetTranslation("Materials", material.Name) + " " +
                   Localization.GetTranslation("Transforms", "Door");
            return true;
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("█", GetMaterialColor())
                                    .AddLayer("╴", Color.Gray);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 3, true);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }
    }
}
