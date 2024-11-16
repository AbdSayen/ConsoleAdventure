using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class OnlineModPanel : ModPanel
    {
        public OnlineModPanel(Rectangle rectangle, IMod modData) : base(rectangle, modData)
        {
            mod = modData;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = ConsoleAdventure.Font;

            if(isHover)
            {
                cursorColor = Color.Yellow;
                spriteBatch.DrawString(font, ">", Position + (new Vector2(9 * (46) - (cursor == 1 ? 9 : 0), 16 * (cursor == 0 ? 1 : 3))), Color.Yellow);
            }

            else
            {
                cursorColor = Color.White;
            }


            spriteBatch.DrawFrame(font, Utils.GetPanel(new(50, 5), 0), Position, cursorColor);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(9, 5), 0), Position, cursorColor);
            //spriteBatch.DrawString(font, " Λ \n╱ ╲", Position + (new Vector2(14, 19) * 1), Color.White);
            
            if(mod == null) return;
            
            mod.modIcon.Draw(spriteBatch, Position + (new Vector2(14, 19) * 1));

            spriteBatch.DrawString(font, TextAssets.Name + mod.modName, Position + (new Vector2(9 * 10, 19 * 1)), Color.White);
            spriteBatch.DrawString(font, TextAssets.Author + mod.modAuthor, Position + (new Vector2(9 * 10, 19 * 2)), Color.White);
            spriteBatch.DrawString(font, TextAssets.Version + mod.modVersion, Position + (new Vector2(9 * 10, 19 * 3)), Color.Gray);

            spriteBatch.DrawString(font, "↓", Position + (new Vector2(9 * 47, 19 * 0.8f)), Color.LightBlue);
            spriteBatch.DrawString(font, "_", Position + (new Vector2(9 * 47, 19 * 0.8f)), Color.LightBlue);
            spriteBatch.DrawString(font, "...", Position + (new Vector2(9 * 45.5f, 19 * 3)), Color.White);
        
            if(mod is Mod)
            {
                Mod _mod = (Mod)mod;
                _mod.PostDrawModPanel(spriteBatch, Position);
            }
        }
    }
}
