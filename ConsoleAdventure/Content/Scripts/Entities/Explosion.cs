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
    public class Explosion : Entity
    {
        static string[] Symbols = new string[]
        {
            "  ",
            "@@",
            "¼¼",
            "÷÷",
            "\\\\",
            "~~",
            "··"
        };

        int frame;

        public Explosion(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)RenderFieldType.explosion;
            SetMaxLife(-1);

            AddTypeToMap<Explosion>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return Symbols[frame];
        }

        public override Color GetColor()
        {
            return new Color(255, 211, 51);
        }

        int timer;
        int delay = 5;
        public override void AI()
        {
            if (timer % delay == 0 && frame < Symbols.Length)
                frame++;

            if (timer >= (Symbols.Length - 1) * 5) 
                Kill();

            timer++;
        }

        public override void OnTheScreen()
        {
            Light.Add(position.x, position.y, w, new Color(255, 255, 255), 8.5f);
        }
    }
}
