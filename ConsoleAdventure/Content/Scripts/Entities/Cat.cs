﻿using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities
{
    [Serializable]
    public class Cat : Entity
    {
        public Cat(World world, Position position, List<object> parameters = null) : base(world, position, parameters)
        {
            renderFieldType = RenderFieldType.cat;
            SetMaxLife(9);
            Initialize();
            
            ChooseColor();
            //world.SetSubjectPosition(this, 1, position.x + 1, position.y + 1);
            World.instance.Start += Start;
        }

        private void Start()
        {
            StateMachine.ChangeState(StatesEnum.Moving);
            Console.WriteLine("start");
        }
        
        public override List<object> GetParams()
        {
            List<object> parameters = new()
            {
                colorIndex
            };

            return parameters;
        }

        public override void SetParams(List <object> p)
        {
            if (p == null)
                return;

            colorIndex = (int)p[0];
        }
    }
}