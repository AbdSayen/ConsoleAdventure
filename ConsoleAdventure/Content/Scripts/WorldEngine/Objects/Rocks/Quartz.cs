using ConsoleAdventure.Content.Scripts.MaterialTypes;
using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdventure.Content.Scripts.MaterialLogic;

namespace ConsoleAdventure.WorldEngine
{
    public class Quartz : Transform
    {
        public Quartz(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.quartz;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (i, t) => new(Color.White);

            var symbol = StoneType.GetModifySymbol("◊", "ƒ", ".", "ƒƒ", new[] { " .", " ,", " ⌐" });
            CreateMaterial(new StoneType(), color, symbol);

            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem(), 1, MaterialSystem.GetMaterial(GetType().Name).Type);

        public override string GetSymbol() => "◊◊";

        public override Color GetColor() => Color.White;

        public override Color? GetBGColor() => Color.Gray;
    }
}
