using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class KeyPanel : BaseUI
    {
        public Color cursorColor = Color.White;

        public Key key;

        public bool flag;

        public KeyPanel(Rectangle rectangle, Key key) : base(rectangle, Color.White)
        {
            this.key = key;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (flag && isHover) 
            {
                Keys[] ks = ConsoleAdventure.kstate.GetPressedKeys();
                if (ks.Length > 0)
                {
                    Keys k = ks[0];
                    if (k != Keys.None)
                    {
                        key.key = k;
                        SettingsSystem.SetSetting("Control", key.name, (int)key.key);
                        flag = false;
                    }
                }
            }

            SpriteFont font = ConsoleAdventure.Font;

            if (isHover)
            {
                cursorColor = Color.Yellow;
                spriteBatch.DrawString(font, ">", Position + (new Vector2(-9, 8)), Color.Yellow);
            }

            else cursorColor = Color.White;

            spriteBatch.DrawFrame(font, Utils.GetPanel(new(80, 2), 0), Position, cursorColor);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(55, 2), 0), Position, cursorColor);

            spriteBatch.DrawString(font, Localization.GetTranslation("KeysConfig", key.name), Position + (new Vector2(9, 8)), Color.White);

            if (!flag)
            {
                spriteBatch.DrawString(font, Localization.GetTranslation("Keys", key.key.ToString()), Position + (new Vector2((55 * 9), 8)), ((int)key.key) == 0 ? Color.Gray : Color.White);
            }
        }
    }
}
