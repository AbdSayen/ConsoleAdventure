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
    internal class RecipesUI : BaseUI
    {
        string[] border;
        string[] border2;
        Texture2D pixel;
        public Point size;
        public int cursorPos = 0;
        public byte type = 0;

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
            if (type == 1)
                return;

            if (timer % 5 == 0)
            {
                Recipe curRecipe = null;
                if (cursorPos > -1 && ConsoleAdventure.availableRecipes.Count > 0)
                {
                    curRecipe = ConsoleAdventure.availableRecipes[cursorPos];
                }

                ConsoleAdventure.availableRecipes.Clear();

                for (int i = 0; i < ConsoleAdventure.recipes.Count; i++)
                {
                    if (ConsoleAdventure.recipes[i].IsAvailable() || ConsoleAdventure.GodMode)
                    {
                        ConsoleAdventure.availableRecipes.Add(ConsoleAdventure.recipes[i]);
                    }
                }

                int oldPos = cursorPos;
                
                cursorPos = ConsoleAdventure.availableRecipes.IndexOf(curRecipe);

                if(cursorPos == -1)
                {
                    cursorPos = oldPos;
                }
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && ConsoleAdventure.prekstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && cursorPos > -1 && cursorPos < ConsoleAdventure.availableRecipes.Count)
            {
                ConsoleAdventure.world.GetLocalPlayer().CraftItem(cursorPos);
            }

            timer++;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(type == 0)
                DrawRecipes(spriteBatch, ConsoleAdventure.availableRecipes, Color.White);

            else if (type == 1)
                DrawRecipes(spriteBatch, ConsoleAdventure.recipes, Color.DarkGreen);
        }

        private void DrawRecipes(SpriteBatch spriteBatch, List<Recipe> recipes, Color color_)
        {
            spriteBatch.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, size.X * 9, (size.Y + (type == 0 ? 2 : 4)) * 19), Color.White);
            spriteBatch.DrawFrame(ConsoleAdventure.Font, border, Position + new Vector2(4, 3), color_);

            spriteBatch.DrawFrame(ConsoleAdventure.Font, border2, Position + new Vector2(4, 3 + ((size.Y - 1) * 19)), color_);

            string nameUI = Localization.GetTranslation("UI", "RecipeMenu");

            if (type == 1)
            {
                spriteBatch.DrawFrame(ConsoleAdventure.Font, border2, Position + new Vector2(4, 3 + ((size.Y + 1) * 19)), color_);
                nameUI = Localization.GetTranslation("UI", "RecipeBook");
            }

            spriteBatch.DrawString(ConsoleAdventure.Font, nameUI, Position + new Vector2(9, -15), color_);

            int x = 1;
            int y = 1;

            for (int i = 0; i < recipes.Count; i++)
            {
                recipes[i].OutItem.Item.Draw(spriteBatch, Position + new Vector2(x * 18, y * 19));

                if (i == cursorPos)
                {
                    spriteBatch.DrawString(ConsoleAdventure.Font, "_", Position + new Vector2(x * 18, y * 19), Color.Yellow);
                }

                if ((i % (29)) == (29 - 1))
                {
                    x = 0;
                    y++;
                }

                x++;
            }

            if (cursorPos >= 0 && cursorPos < recipes.Count)
            {
                Recipe recipe = recipes[cursorPos];
                recipe.OutItem.Item.Draw(spriteBatch, Position + new Vector2(18, size.Y * 19));
                string name = recipe.OutItem.Item.name + (recipes[cursorPos].OutItem.count > 1 ? $" ({recipe.OutItem.count})" : "") + " :";
                spriteBatch.DrawString(ConsoleAdventure.Font, name, Position + new Vector2(36, size.Y * 19), color);

                Vector2 nameSize = ConsoleAdventure.Font.MeasureString(name);

                //int countWidth = 0;
                for (int i = 0; i < recipe.Ingredients.Count; i++)
                {
                    var ingredient = recipe.Ingredients.ElementAt(i);
                    ingredient.Key.Draw(spriteBatch, Position + new Vector2(36 + nameSize.X + (i * 27), size.Y * 19));
                    spriteBatch.DrawString(ConsoleAdventure.Font, $" {ingredient.Value}", Position + new Vector2(36 + nameSize.X + ((i) * 27), size.Y * 19), color);

                    //countWidth = (int)ConsoleAdventure.Font.MeasureString($" {ingredient.Value}").X;
                }

                if (type == 1)
                {
                    int stationWidth = 0;
                    for (int i = 0; i < recipe.CraftStations.Count; i++)
                    {
                        if (Transform.TypeMapping.TryGetValue(recipe.CraftStations[i], out Type value))
                        {
                            string transformName = Localization.GetTranslation("Transforms", value.Name) + (i < recipe.CraftStations.Count - 1 ? "," : "");
                            spriteBatch.DrawString(ConsoleAdventure.Font, transformName, Position + new Vector2(9 + ((i + 1) * 18) + stationWidth - 9, (size.Y + 2) * 19), color);
                            stationWidth = (int)ConsoleAdventure.Font.MeasureString(transformName).X;
                        }
                    }
                }
            }
        }
    }
}
