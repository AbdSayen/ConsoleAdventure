using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Stalactite : Transform
    {
        static string[] symbolsMap = new string[] //VΛvᴧᵛᶺ
        {
            "VV",
            "ΛΛ",

            "vv",
            "ᴧᴧ",

            "ᵛᵛ",
            "ᶺᶺ",

            " V",
            "Λ ",

            " v",
            "ᴧ ",

            " ᵛ",
            "ᶺ ",

            "Vv",
            "Λᴧ",

            "vᵛ",
            "ᴧᶺ",

            "ᵛV",
            "ᶺΛ",

            "Vᴧ",
            "Λv",

            "vᶺ",
            "ᴧᵛ",

            "ᵛΛ",
            "ᶺV",
        };

        static string[] symbolsDrawsMap = new string[]
        {
            "VV",
            "  ",

            "vv",
            "  ",

            "ᵛᵛ",
            "  ",

            " V",
            "  ",

            " v",
            "  ",

            " ᵛ",
            "  ",

            "Vv",
            "  ",

            "vᵛ",
            "  ",

            "ᵛV",
            "  ",

            "V ",
            " v",

            "v ",
            " ᵛ",

            "ᵛ ",
            " V",
        };

        public Stalactite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.stalactite;
            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = 4f;
        }

        public override void Collapse() => DropItem(new SaltItem(), 3);

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => Color.Gray;

        public override void OnTheScreen()
        {
            if (CanDraw())
            {
                StringPaint.Draw(GetVariation(symbolsMap), position, w, new(), Light.GetColor(Color.Gray, position));
            }
        }
    }
}