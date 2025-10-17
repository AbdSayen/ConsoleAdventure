using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Quartz : Transform
    {
        public Quartz(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.quartz;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new QuartzItem());

        public override string GetSymbol() => "◊◊";

        public override Color GetColor() => Color.White;

        public override Color? GetBGColor() => Color.Gray;
    }
}
