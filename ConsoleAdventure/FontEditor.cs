using CaModLoaderAPI;
using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    public class FontEditor
    {
        public static void ModifyChars(ContentManager content)
        {
            ConsoleAdventure.SetFont(content.Load<SpriteFont>("Fonts/font"));

            Texture2D fontTexture = ConsoleAdventure.Font.Texture;

            Color[] pixelData = new Color[fontTexture.Width * fontTexture.Height];
            fontTexture.GetData(pixelData);

            List<Mod> mods = CaModLoader.GetActiveMods();
            for (int i = -1; i < mods.Count; i++)
            {
                if (i == -1)
                {

                }

                else
                {

                }
            }
        }

        private static void ReplaceChar(Color[] fontColors, Texture2D texture, Vector2 position)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color color = colors[x + y * texture.Width];
                }
            }
        }
    }
}
