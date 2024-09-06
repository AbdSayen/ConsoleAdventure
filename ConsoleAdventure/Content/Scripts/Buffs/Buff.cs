using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public class Buff
    {
        public int time;
        public int maxTime;

        public void SetMaxTime(int time)
        {
            maxTime = time;
            this.time = time;
        }

        public Buff(int time) 
        { 
            SetMaxTime(time);
        }

        public virtual void Update(Entity entity)
        {
        }

        public virtual CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("B");
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Color color = Color.White * ((float)(time) / (float)(maxTime));
            spriteBatch.DrawFrame(ConsoleAdventure.Font, TextAssets.NormalFrame3x2, position, color);

            GetTexture().Draw(spriteBatch, position + new Vector2(7, 9));
        }

        public virtual string GetName()
        {
            return Localization.GetTranslation("Buffs", GetType().Name);
        }

        public virtual string GetDescription()
        {
            return Localization.GetTranslation("BuffDescription", GetType().Name);
        }
    }
}
