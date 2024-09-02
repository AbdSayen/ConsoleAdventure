using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using System.Reflection;
using System.Threading;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public class Leaves : Entity
    {
        static string[] Symbols = new string[]
        {
            "  ",
            "¾¾",
            "÷÷",
            "\\\\",
            "⁄⁄",
            "··"
        };

        int delay;
        int frame;

        public Leaves(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)RenderFieldType.leaves;
            SetMaxLife(-1);

            AddTypeToMap<Leaves>(type);

            delay = ConsoleAdventure.rand.Next(3, 20);

            Initialize();
        }

        public override string GetSymbol()
        {
            return Symbols[frame];
        }

        public override Color GetColor()
        {
            Color color = new Color(13, 152, 20) * 0.5f;
            color.A = 255;
            return color;
        }

        int timer;

        public override void AI()
        {
            if (timer % delay == 0 && frame < Symbols.Length)
            {
                frame++;
                Position newPos = new(ConsoleAdventure.rand.Next(position.x - 1, position.x + 2), ConsoleAdventure.rand.Next(position.y - 1, position.y + 2));
                SetPosition(newPos);
            }

            if (frame >= Symbols.Length) frame = Symbols.Length - 1;
            if (frame < 0) frame = 0;

            if (timer >= (Symbols.Length - 1) * delay) 
                Kill();

            timer++;
        }
    }
}
