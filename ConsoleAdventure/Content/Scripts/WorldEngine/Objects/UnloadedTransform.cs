using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class UnloadedTransform : Transform
    {
        public object data = null;

        public UnloadedTransform(Position position, int w, int worldLayer, int modType) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (byte)modType;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
        }

        public override string GetSymbol()
        {
            return " ?";
        }

        public override Color? GetBGColor()
        {
            Color color = new Color(255, 0, 255);

            if (worldLayer == 0) color = new Color(90, 0, 90);

            return color;
        }

        public override Color GetColor()
        {
            return new Color(10, 0, 10);
        }

        public override string ModifyTooltip()
        {
            string name = Localization.GetTranslation("Transforms", "Unloaded") + ":" + 
                          world.modTransforms.FirstOrDefault(x => x.Value == type).Key;

            string degree = degreeDestruction > 0 ? $" ({100 - degreeDestruction} / 100)" : "";
            return name + degree;
        }

        public override object SaveData()
        {
            return data;
        }

        public override void LoadData(object data)
        {
            this.data = data;
        }
    }
}
