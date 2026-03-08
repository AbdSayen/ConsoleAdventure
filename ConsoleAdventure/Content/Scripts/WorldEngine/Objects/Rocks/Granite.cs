using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.Content.Scripts.MaterialTypes;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    public class Granite : Transform
    {
        public Granite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.granite;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (i, t) =>
            {
                return new(40, 40, 40);
            };

            var symbol = StoneType.GetModifySymbol("#", "∫", "~", "∫∫", new[] { " .", " ~", " ," });
            CreateMaterial(new StoneType(), color, symbol);
            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem(), 1, MaterialSystem.GetMaterial(GetType().Name).Type);

        public override string GetSymbol() => "##";

        public override Color GetColor() => new Color(10, 10, 10);

        public override Color? GetBGColor() => new Color(75, 75, 75);
    }
}
