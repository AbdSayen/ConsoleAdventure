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

        private int[] ai = new int[8];

        public List<Buff> buffs = new();

        protected Position oldPos;

        public Entity(Position position, int w, List<object> parameters = null) : base(position, (byte)w)
        {
            worldLayer = World.MobsLayerId;
            type = (int)VanillaTransforms.entity;
            isObstacle = false;

            if (parameters != null)
            {
                SetParams(parameters);
            }

            StateMachine = new StateMachine(this);

            AddTypeToMap<Entity>(type);

            ConsoleAdventure.world.Start += PreStart;
        }

        public void PreStart()
        {
            netID = NetworkManager.SetNetID(this);
            Start();
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

        public override byte[] GetDataBytes()
        {
            List<byte> data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(position.x));             // 2b                   = 0
            data.AddRange(BitConverter.GetBytes(position.y));             // 2b            0 + 2  = 2
            data.Add(w);                                                  // 1b            2 + 2  = 4
            data.AddRange(BitConverter.GetBytes(life));                   // 4b            4 + 1  = 5
            data.AddRange(BitConverter.GetBytes(maxLife));                // 4b            5 + 4  = 9
            data.AddRange(BitConverter.GetBytes(damage));                 // 4b            9 + 4  = 13
            data.AddRange(BitConverter.GetBytes(defense));                // 4b            13 + 4 = 17
            data.AddRange(BitConverter.GetBytes(invulnerabilityTime));    // 4b            17 + 4 = 21
            
            for (int i = 0; i < ai.Length; i++)  // 8                                      21 + 4 = 25
            {
                data.AddRange(BitConverter.GetBytes(ai[i])); // 4b
            } // 8 * 4 = 32b        

            return data.ToArray();
        }

        public override void SetDataFromBytes(byte[] data)
        {
            position.x = BitConverter.ToInt16(data, 0);
            position.y = BitConverter.ToInt16(data, 2);
            w = data[4];
            life = BitConverter.ToInt32(data, 5);
            maxLife = BitConverter.ToInt32(data, 9);
            damage = BitConverter.ToInt32(data, 13);
            defense = BitConverter.ToInt32(data, 17);
            invulnerabilityTime = BitConverter.ToInt32(data, 21);

            for (int i = 0; i < ai.Length; i++)
            {
                ai[i] = BitConverter.ToInt32(data, 25 + i * 4);
            }
        }

        public override string ModifyTooltip()
        {
            return Localization.GetTranslation("Transforms", GetType().Name) + $" ({life} / {maxLife})";
        }
    }
}
