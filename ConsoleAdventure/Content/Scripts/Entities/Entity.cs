using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using System;
using System.Linq;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Settings;
using SharpDX.Direct2D1;
using System.Text;

namespace ConsoleAdventure.Content.Scripts
{
    public class Entity : Transform
    {
        //public List<object> Parameters { get; set; } = new List<object>();

        public StateMachine StateMachine { get; private set; }

        public int life;
        public int maxLife;
        public int damage;
        public int defense;
        public int invulnerabilityTime;

        public int netID = -1;

        internal int[] ai = new int[8];
        private int[] oldAi = new int[8];

        public List<Buff> buffs = new();

        protected Position oldPos;

        public Entity(Position position, int w, List<object> parameters = null) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.entity;

            if (parameters != null)
            {
                SetParams(parameters);
            }

            StateMachine = new StateMachine(this);

            if (!ConsoleAdventure.InWorld) return;
            ConsoleAdventure.world.Start += PreStart;
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.MobsLayerId;
        }

        public void PreStart()
        {
            netID = NetworkManager.SetNetID(this);
            Start();
        }

        protected virtual void Start()
        {
            
        }

        public override string GetSymbol() => "AE";

        public override Color GetColor() => Color.Yellow;

        public void UpdateEntityInWorld()
        {
            if (oldAi != ai)
            {
                if (NetworkManager.isHost)
                {
                    NetworkManager.EntityAIvalChanged(this);
                }
            }
            if (this is Player.Player || NetworkManager.isHost)
            {
                oldAi = ai;
                oldPos = position;
                InteractWithWorld();
            }
                
            if (oldPos != position)
            {
                if (this is Player.Player)
                {
                    Player.Player pl = (Player.Player)this;
                    NetworkManager.EntityMoved(this, pl.info.Id);
                }
                else if (NetworkManager.isHost)
                {
                    NetworkManager.EntityMoved(this);
                }
            }
        }

        /// <summary>
        /// Обновление сущности в мире
        /// </summary>
        public virtual void InteractWithWorld()
        {
            if (world.GetUnloadedChunk(position.x, position.y, out int v1, out int v2) == null) 
            {
                UpdateBuffs();
                StateMachine?.InteractWithWorld();
                AI();

                if (life <= 0 && maxLife > 0)
                    Kill();

                invulnerabilityTime--; 
            }
        }

        /// <summary>
        /// Искуственный интеллект сущности
        /// </summary>
        public virtual void AI() 
        { 
        
        }

        /// <summary>
        /// Убивает сущность 
        /// </summary>
        public void Kill()
        {
            ConsoleAdventure.world.Start -= Start;
            ConsoleAdventure.world.RemoveSubject(this, worldLayer);
            ConsoleAdventure.world.entities.Remove(this);

            if (NetworkManager.isHost)
            {
                NetworkManager.EntityKilled(this);
            }
        }
        
        public void SetMaxLife(int life)
        {
            this.life = life;
            maxLife = life;
        }

        public void Hit(int damage)
        {
            if (maxLife <= 0 && invulnerabilityTime > 0)
                return;

            if (defense == 0) 
                defense = 1;

            damage = Math.Abs(damage);
            life -= Math.Max((int)((float)damage * (1f - (float)defense / 100f)), 1);

            invulnerabilityTime = 60;
        }

        public virtual void SetParams(List<object> p) { }

        public virtual List<object> GetParams()
        {
            return new();
        }


        public bool CanHitToPlayer(out short id)
        {
            for (int i = 0; i < world.players.Count; i++)
            {
                Player.Player player = world.players.ElementAt(i).Value;

                if (player.maxLife > 0)
                {
                    Position pos = player.position;
                    if (pos >= position + new Position(-1, -1) && pos <= position + new Position(1, 1) && pos != position && player.w == w)
                    {
                        id = world.players.ElementAt(i).Key;
                        return true;
                    }
                }
            }
            id = 0;
            return false;
        }

        protected void UpdateBuffs()
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].Update(this);
                buffs[i].time--;

                if (buffs[i].time < 0)
                {
                    buffs.RemoveAt(i);
                    i--;
                }
            }
        }

        public bool AddBuff(Buff buff)
        {
            Type curType = buff.GetType();
            int buffIndex = -1;

            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].GetType() == curType)
                {
                    buffIndex = i;
                }
            }

            if( buffIndex > -1)
            {
                //buffs[buffIndex].time = buff.time;
                return false;
            }

            buffs.Add(buff);
            return true;
        }

        public override string ModifyTooltip()
        {
            return Localization.GetTranslation("Transforms", GetType().Name) + $" ({life} / {maxLife})";
        }
    }
}
