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
        int index = -1;

        public Cat(Position position, int w, List<object> parameters = null) : base(position, w, parameters)
        {
            type = (int)RenderFieldType.cat;
            SetMaxLife(9);

            AddTypeToMap<Cat>(type);

            Initialize();
            
            EntityColor.ChooseColor(position, w);
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
            return Microsoft.Xna.Framework.Color.White;
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
