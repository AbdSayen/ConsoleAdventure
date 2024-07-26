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
        public EntityColor EntityColor { get; private set; }

        public int life;
        public int maxLife;
        public int damage;

        public Entity(Position position, List<object> parameters = null) : base(position)
        {
            worldLayer = World.MobsLayerId;
            type = (int)RenderFieldType.entity;
            isObstacle = false;

            if (parameters != null)
            {
                SetParams(parameters);
            }

            StateMachine = new StateMachine(this);
            EntityColor = new EntityColor();

            AddTypeToMap<Entity>(type);
            
            ConsoleAdventure.world.Start += Start;
        }

        protected void Start()
        {
            StateMachine?.ChangeState(StatesEnum.Moving);
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
            StateMachine?.CurrentState?.InteractWithWorld();
            //if(life <= 0) Kill();
        }

        /// <summary>
        /// Убивает сущьность 
        /// </summary>
        public void Kill()
        {
            ConsoleAdventure.world.Start -= Start;
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

        public virtual void SetParams(List<object> p) { }

        public virtual List<object> GetParams()
        {
            return new();
        }
    }
}
