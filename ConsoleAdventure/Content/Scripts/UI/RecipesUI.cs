using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class RecipesUI : BaseUI
    {
        string[] border;
        string[] border2;
        Texture2D pixel;
        public Point size;
        public int cursorPos = 0;

        public RecipesUI(Point position, Point size) : base(new Rectangle(position, size * new Point(9, 19)), Color.White)
        {
            border = Utils.GetPanel(size);
            border2 = Utils.GetPanel(new(size.X, 3));
            pixel = new Texture2D(ConsoleAdventure._graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.Black });
            this.size = size; 
        }

        int timer;
        public async void Update()
        {
            if (timer % 5 == 0)
            {
                ConsoleAdventure.availableRecipes.Clear();

                for (int i = 0; i < ConsoleAdventure.recipes.Count; i++)
                {
                    if (ConsoleAdventure.recipes[i].IsAvailable())
                    {
                        ConsoleAdventure.availableRecipes.Add(ConsoleAdventure.recipes[i]);
                    }
                }
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && ConsoleAdventure.prekstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && cursorPos > -1 && cursorPos < ConsoleAdventure.availableRecipes.Count)
            {
                ConsoleAdventure.world.GetLocalPlayer().CraftItem(cursorPos);
                await NetworkManager.SendDataAsync(NetworkFuncType.craftItem, ConsoleAdventure.world.GetLocalPlayer().GetPlayerBytes(), NetworkManager.Id);
            }

            timer++;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, size.X * 9, (size.Y + 2) * 19), Color.White);
            spriteBatch.DrawFrame(ConsoleAdventure.Font, border, Position + new Vector2(4, 3), Color.White);

            spriteBatch.DrawFrame(ConsoleAdventure.Font, border2, Position + new Vector2(4, 3 + ((size.Y - 1) * 19)), Color.White);

            int x = 1;
            int y = 1;

            for (int i = 0; i < ConsoleAdventure.availableRecipes.Count; i++)
            {
                ConsoleAdventure.availableRecipes[i].OutItem.item.Draw(ConsoleAdventure._spriteBatch, Position + new Vector2(x * 18, y * 19));

                if (i == cursorPos)
                {
                    spriteBatch.DrawString(ConsoleAdventure.Font, "_", Position + new Vector2(x * 18, y * 19), Color.Yellow);
                }

                if ((i % (size.X - 1)) == (size.X - 1))
                {
                    x = 1;
                    y++;
                }

                x++;
            }

            if (cursorPos >= 0 && cursorPos < ConsoleAdventure.availableRecipes.Count)
            {
                Recipe recipe = ConsoleAdventure.availableRecipes[cursorPos];
                recipe.OutItem.item.Draw(ConsoleAdventure._spriteBatch, Position + new Vector2(18, size.Y * 19));
                string name = recipe.OutItem.item.name + (ConsoleAdventure.availableRecipes[cursorPos].OutItem.count > 1 ? $" ({recipe.OutItem.count})" : "") + " :";
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, name, Position + new Vector2(36, size.Y * 19), color);

                Vector2 nameSize = ConsoleAdventure.Font.MeasureString(name);

                int countWidth = 0;
                for(int i = 0; i < recipe.Ingredients.Count; i++)
                {
                    var ingredient = recipe.Ingredients.ElementAt(i);
                    ingredient.Key.Draw(ConsoleAdventure._spriteBatch, Position + new Vector2(36 + nameSize.X + (i * 18) + countWidth, size.Y * 19));
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, ingredient.Value.ToString(), Position + new Vector2(36 + nameSize.X + ((i + 1) * 18) + countWidth - 9, size.Y * 19), color);

                    countWidth = (int)ConsoleAdventure.Font.MeasureString(ingredient.Value.ToString()).X;
                }
            }
        }
    }
}
