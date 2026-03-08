using CaModLoaderAPI;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModifyColor = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, ConsoleAdventure.Content.Scripts.SmartColor>;
using ModifySymbol = System.Func<ConsoleAdventure.Transform, ConsoleAdventure.Item, string>;

namespace ConsoleAdventure.Content.Scripts.MaterialLogic
{
    public class UnloadMaterial : Material
    {
        internal UnloadMaterial(string name) : base(null, null, name)
        {
        }
    }
}
