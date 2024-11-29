using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public class InTheFlames : Buff
    {
        public InTheFlames(int time) : base(time) 
        { 
        }

        float timer = -1;
        public override CharTexture GetTexture()
        {
            timer++;
            return new CharTexture()
                        .AddLayer("●", Color.OrangeRed, new(-0.58f, -0.03f))
                        .AddLayer("•", Color.OrangeRed * 0.8f, new Vector2(0.42f + (float)(Math.Sin(timer / 30 * Math.PI) / 2), -2.4f))
                        .AddLayer("•", Color.Orange * (float)(1 + Math.Sin(timer / 60 * Math.PI) / 4), new(0.2f, -0.03f));
        }

        public override void Update(Entity entity)
        {
            if(time % 30 == 0)
            {
                entity.life -= 1;
            }
        }
    }
}
