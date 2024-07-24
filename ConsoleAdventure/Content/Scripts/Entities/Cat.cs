using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;

namespace ConsoleAdventure.Content.Scripts
{
    [Serializable]
    public class Cat : Entity
    {
        public Cat(World world, Position position, List<object> parameters = null) : base(world, position, parameters)
        {
            renderFieldType = RenderFieldType.cat;
            SetMaxLife(9);
            Initialize();

            Color.ChooseColor(position);
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

        public override List<object> GetParams()
        {
            List<object> parameters = new()
            {
                Color.ColorIndex
            };

            return parameters;
        }

        public override void SetParams(List<object> p)
        {
            if (p == null)
                return;

            Color.ColorIndex = (int)p[0];
        }
    }
}
