using System;
using System.Collections.Generic;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities
{
    public class Entity : Transform
    {
        protected int colorIndex;
        
        public Color[] colors = new Color[9]
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
        
        //public List<object> Parameters { get; set; } = new List<object>();
        public readonly StateMachine.StateMachine StateMachine;
        
        protected int life;
        protected int maxLife;
        protected int damage;

        public Entity(World world, Position position, List<object> parameters = null) : base(world, position)
        {
            worldLayer = World.MobsLayerId;
            renderFieldType = RenderFieldType.entity;
            isObstacle = false;

            StateMachine = new StateMachine.StateMachine(this);
            colorIndex = ConsoleAdventure.rand.Next(0, colors.Length);

            if(parameters != null)
            {
                SetParams(parameters);
            }    
        }

        /// <summary>
        /// Обновление сущьности в мире
        /// </summary>
        public void InteractWithWorld()
        {
            AI();
            StateMachine?.CurrentState?.InteractWithWorld();
            //if(life <= 0) Kill();
        }

        protected virtual void AI()
        {
            
        }

        public void ChooseColor()
        {
            SetColor(colors[colorIndex], position);
        }
        
        /// <summary>
        /// Убивает сущьность 
        /// </summary>
        public void Kill()
        {
            ConsoleAdventure.world.RemoveSubject(this, worldLayer);
        }

        public void SetMaxLife(int life)
        {
            this.life = life;
            maxLife = life;
        }

        public void SetColor(Color color, Position position)
        {
            world.GetField(position.x, position.y, World.MobsLayerId).color = color;
        }

        public virtual void SetParams(List<object> p)
        { 

        }

        public virtual List<object> GetParams() 
        { 
            return new(); 
        }
    }
}
