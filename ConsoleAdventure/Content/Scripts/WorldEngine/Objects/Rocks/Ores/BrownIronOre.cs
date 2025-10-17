using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class BrownIronOre : Transform
    {
        public BrownIronOre(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.brownIronOre;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.8f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new BrownIronOreItem());

        public override string GetSymbol() => "§§";

        public override Color GetColor() => new Color(206, 83, 33);

        public override Color? GetBGColor() => new Color(206, 83, 33) * 0.3f;

        public override string ModifyTooltip()
        {
            return base.GetName() + " {" + Localization.GetTranslation("TooltipAdds", GetType().Name) + " [item:ConsoleAdventure.IronBar]}";
        }
    }
}
