﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Chest : Storage
    {
        public Chest(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, w, items)
        {
            type = (int)RenderFieldType.chest;

            AddTypeToMap<Chest>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "<>";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }
    }
}
