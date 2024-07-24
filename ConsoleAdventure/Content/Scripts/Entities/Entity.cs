using System.Collections.Generic;
using ConsoleAdventure.WorldEngine;

namespace ConsoleAdventure.Content.Scripts.Entities
{
    public class Entity : Transform
    {
        public StateMachine.StateMachine StateMachine { get; private set; }
        public EntityColor Color { get; private set; }
        
        //public List<object> Parameters { get; set; } = new List<object>();

        public int life;
        public int maxLife;
        public int damage;
        
        public Entity(World world, Position position, List<object> parameters = null) : base(world, position)
        {
            worldLayer = World.MobsLayerId;
            renderFieldType = RenderFieldType.entity;
            isObstacle = false;

            Color = new EntityColor();
            
            if (parameters != null)
            {
                SetParams(parameters);
            }
            
            world.Start += Start;
        }

        protected virtual void Start()
        {
            StateMachine = new StateMachine.StateMachine(this);
        }

        public override string GetSymbol()
        {
            return "AE";
        }
        
        /// <summary>
        /// Обновление сущьности в мире
        /// </summary>
        public virtual void InteractWithWorld()
        {
            StateMachine?.CurrentState?.InteractWithWorld();
            //if(life <= 0) Kill();
        }

        /// <summary>
        /// Убивает сущьность 
        /// </summary>
        public void Kill()
        {
            world.Start -= Start;
            ConsoleAdventure.world.RemoveSubject(this, worldLayer);
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
