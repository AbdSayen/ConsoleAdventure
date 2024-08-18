using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;

namespace ConsoleAdventure.Content.Scripts
{
    public class Entity : Transform
    {
        //public List<object> Parameters { get; set; } = new List<object>();

        public StateMachine StateMachine { get; private set; }

        public int life;
        public int maxLife;
        public int damage;

        protected Position oldPos;

        public Entity(Position position, int w, List<object> parameters = null) : base(position, (byte)w)
        {
            worldLayer = World.MobsLayerId;
            type = (int)RenderFieldType.entity;
            isObstacle = false;

            if (parameters != null)
            {
                SetParams(parameters);
            }

            StateMachine = new StateMachine(this);

            AddTypeToMap<Entity>(type);
            
            ConsoleAdventure.world.Start += Start;
        }

        protected virtual void Start()
        {
            
        }

        public override string GetSymbol()
        {
            return "AE";
        }

        public override Color GetColor()
        {
            return Color.Yellow;
        }


        /// <summary>
        /// Обновление сущьности в мире
        /// </summary>
        public virtual void InteractWithWorld()
        {
            StateMachine?.InteractWithWorld();
            AI();
            //if(life <= 0) Kill();
        }

        /// <summary>
        /// Искуственный интилект сущьности
        /// </summary>
        public virtual void AI() 
        { 
        
        }

        /// <summary>
        /// Убивает сущьность 
        /// </summary>
        public void Kill()
        {
            ConsoleAdventure.world.Start -= Start;
            ConsoleAdventure.world.RemoveSubject(this, worldLayer);
            ConsoleAdventure.world.entities.Remove(this);
        }
        
        public void SetMaxLife(int life)
        {
            this.life = life;
            maxLife = life;
        }

        public virtual void SetParams(List<object> p) { }

        public virtual List<object> GetParams()
        {
            return new();
        }
    }
}
