using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public class Cat : Entity
    {
        public static Color[] Colors = new Color[9]
        {
            new Color(50, 50, 50),
            new Color(131, 105, 44),
            new Color(193, 138, 45),
            new Color(243, 171, 51),
            new Color(140, 147, 153),
            new Color(255, 255, 255),
            new Color(196, 207, 211),
            new Color(250, 194, 45),
            new Color(240, 210, 80),
        };

        int index = -1;

        public Cat(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)RenderFieldType.cat;
            SetMaxLife(9);

            AddTypeToMap<Cat>(type);

            Initialize();

            if (parameters == null)
                index = ConsoleAdventure.rand.Next(0, Colors.Length);
            else
                index = (int)parameters[0];
        }

        protected override void Start()
        {
            base.Start();
            StateMachine?.ChangeState(StatesEnum.Moving);
        }

        public override string GetSymbol()
        {
            return " c";
        }

        public override Color GetColor()
        {
            return Colors[index];
        }

        bool isFree = true;
        int timer;
        int randomTime = 0;
        int rotation = -1;
        int rotation1 = -1;

        public override List<object> GetParams()
        {
            List<object> parameters = new()
            {
                index
            };

            return parameters;
        }

        public override void SetParams(List<object> p)
        {
            if (p == null)
                return;

            index = (int)p[0];
        }
    }
}
