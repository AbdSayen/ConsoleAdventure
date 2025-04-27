using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConsoleAdventure
{
    [Serializable]
    public class AlfisolFloorItem : PlaceableItem
    {
        public AlfisolFloorItem()
        {
            name = Localization.GetTranslation("Transforms", "AlfisolFloor");
            description = GetDescription();
            placeType = (int)VanillaTransforms.alfisolFloor;
            placeLayer = World.FloorLayerId;
            maxCount = 100;
            AddTypeToMap<AlfisolFloorItem>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("`", new Color(54, 43, 32));
        }
    }
}