﻿using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Wall : Transform
    {
        public Wall(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.wall;
            isObstacle = true;
            Initialize();
        }

        public override string GetSymbol()
        {
            return "##";
        }

        public override Color GetColor()
        {
            return Color.White;
        }
    }
}