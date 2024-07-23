using System.Collections.Generic;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities
{
    public class Entity : Transform
    {
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
            //if(life <= 0) Kill();
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

        public void СhooseColor(Color[] colors, Position position, int index)
        {
            SetColor(colors[index], position);
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
