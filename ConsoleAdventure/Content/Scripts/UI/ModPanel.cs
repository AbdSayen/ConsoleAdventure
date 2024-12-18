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
    internal class ModPanel : BaseUI
    {
        public Color cursorColor = Color.White;

        public int cursor = 0;

        public IMod mod = null;

        internal bool enabled = false;

        public ModPanel(Rectangle rectangle, IMod modData) : base(rectangle, Color.White)
        {
            mod = modData;
            if(mod is Mod)
            {
                enabled = true;
            }
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

            spriteBatch.DrawString(font, enabled ? "√" : "х", Position + (new Vector2(9 * 47, 19 * 0.8f)), enabled ? Color.Green : Color.Red);
            spriteBatch.DrawString(font, "...", Position + (new Vector2(9 * 45.5f, 19 * 3)), Color.White);
        
            if(mod is Mod && enabled)
            {
                Mod trueMod = (Mod)mod;
                trueMod.PostDrawModPanel(spriteBatch, Position);

                StringBuilder addedContent = new();

                if (trueMod.ItemsCount > 0) addedContent.Append("I");
                if (trueMod.TransformsCount > 0) addedContent.Append("T");
                if (trueMod.EntitiesCount > 0) addedContent.Append("E");
                if (trueMod.BuffsCount > 0) addedContent.Append("B");

                spriteBatch.DrawString(font, addedContent, Position + (new Vector2((9 * 43.5f) - font.MeasureString(addedContent).X, 19 * 3)), Color.Gray);
            }
        }
    }
}
