using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Quartz : Transform
    {
        public Quartz(Position position, int w, int worldLayer = -1) : base(position, w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.quartz;
            isObstacle = true;


            AddTypeToMap<Quartz>(type);

            Initialize();
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
