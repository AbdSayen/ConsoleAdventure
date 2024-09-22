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
            placeType = (int)RenderFieldType.bomb;
            placeLayer = World.MobsLayerId;
            AddTypeToMap<BombItem>();
        }

        public override (List<string>, List<Color>) GetTexture()
        {
            return (
                    new List<string>() { "B" },
                    new List<Color>() { Color.Red }
                   );
        }
    }
}