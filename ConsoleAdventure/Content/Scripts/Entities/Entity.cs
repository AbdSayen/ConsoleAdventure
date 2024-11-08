using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.Entities.StateMachine;
using System;
using System.Linq;
using ConsoleAdventure.Content.Scripts.Player;

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

        public List<Buff> buffs = new();

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
            //ConsoleAdventure.world.Start += SetNetID;
        }

        public void SetNetID()
        {
            netID = NetworkManager.RegisterNetID(this);
        }

        protected void Sync()
        {
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes((short)netID));
            data.AddRange(BitConverter.GetBytes(position.x));
            data.AddRange(BitConverter.GetBytes(position.y));
            data.AddRange(BitConverter.GetBytes((short)life));
            NetworkManager.SendMessage(NetworkManager.ActionID.entitySync, data.ToArray(), new byte[0]);
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
        /// Обновление сущности в мире
        /// </summary>
        public virtual void InteractWithWorld()
        {
            UpdateBuffs();
            StateMachine?.InteractWithWorld();
            AI();

            if (life <= 0 && maxLife > 0)   
                Kill();

            invulnerabilityTime--;
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
            //ConsoleAdventure.world.Start -= SetNetID;
            NetworkManager.RemoveTransformNetID(netID);
            ConsoleAdventure.world.RemoveSubject(this, worldLayer);
            ConsoleAdventure.world.entities.Remove(this);
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
            life -= Math.Abs((int)((float)damage / ((float)defense)));

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
    }
}
