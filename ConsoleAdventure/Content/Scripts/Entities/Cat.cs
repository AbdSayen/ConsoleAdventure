using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;

namespace ConsoleAdventure.Content.Scripts
{ 
    public class Cat : Entity
    {
        public static Color[] Colors = new Color[10]
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
            ColorAssets.theLionColor
        };

        int index
        {
            get
            {
                return ai[0];
            }
            set
            {
                ai[0] = value;
            }
        }

        public Cat(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)VanillaTransforms.cat;
            SetMaxLife(9);

            Initialize();

            if (parameters == null)
            {
                index = ConsoleAdventure.rand.Next(0, Colors.Length - 1);

                if(ConsoleAdventure.rand.Next(0, 101) < 2)
                {
                    index = 9;
                }
            }
            else
                index = (int)parameters[0];
        }

        protected override void Start()
        {
            base.Start();
            StateMachine?.ChangeState(StatesEnum.Moving);
        }

        public override string GetSymbol() => " c";

        public override Color GetColor() => Colors[index];

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
