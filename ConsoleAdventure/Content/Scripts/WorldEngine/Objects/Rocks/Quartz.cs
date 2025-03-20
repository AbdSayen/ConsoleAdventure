using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Quartz : Transform
    {
        public Quartz(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.quartz;
            isObstacle = true;
            hardness = 2;
            burnType = 1;

            AddTypeToMap<Quartz>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new QuartzItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "◊◊";
        }

        public override Color GetColor()
        {
            return Color.White;
        }

        public override Color? GetBGColor()
        {
            return Color.Gray;
        }
    }
}
