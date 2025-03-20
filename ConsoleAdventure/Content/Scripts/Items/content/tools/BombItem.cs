using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class BombItem : PlaceableItem
    {
        public BombItem()
        {
            name = Localization.GetTranslation("Transforms", "Bomb");
            description = GetDescription();
            placeType = (int)VanillaTransforms.bomb;
            placeLayer = World.MobsLayerId;
            AddTypeToMap<BombItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("B", Color.Red);
        }
    }
}