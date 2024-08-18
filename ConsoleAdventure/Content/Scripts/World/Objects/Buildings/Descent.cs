using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Descent : Transform
    {
        public Descent(Position position, int w, int worldLayer = 1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.descent;
            isObstacle = false;
            hardness = -1;

            AddTypeToMap<Descent>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "▼▼";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
        
        public override void Interaction()
        {
            if (world.GetLocalPlayer().SetPosition(world.GetLocalPlayer().position, 0))
            {
                ConsoleAdventure.curDeep = 0;
                NetworkManager.SendDataAsync(NetworkFuncType.setPlayerW, (short)ConsoleAdventure.curDeep, NetworkManager.Id);
            }
        }
    }
}
