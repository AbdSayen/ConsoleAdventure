using ConsoleAdventure.Content.Scripts.MaterialLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ConsoleAdventure.Content.Scripts.InputLogic;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class RecipesUI : BaseUI
    {
        string border;
        string border2;
        Texture2D pixel;
        public Point size;
        public int cursorPos = 0;
        public byte type = 0;

        public RecipesUI(Point position, Point size) : base(new Rectangle(position, size * new Point(9, 19)), Color.White)
        {
            border = FrameSystem.GetFrame(size, Frame.BaseFrame);
            border2 = FrameSystem.GetFrame(new(size.X, 3), Frame.BaseFrame);
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
                    Recipe recipe = ConsoleAdventure.recipes[i].Copy();

                    Player.Player player = ConsoleAdventure.world.GetLocalPlayer();

                    List<Stack> materialSources = new();

                    for (int j = 0; j < player.inventory.slots.Count; j++)
                    {
                        Stack item = player.inventory.slots[j];
                        int? material = item?.Item?.material;

                        if (material > -1 && material < MaterialSystem.Materials.Count)
                        {
                            materialSources.Add(item);
                        }
                    }

                    if (recipe.IsAvailable() || ConsoleAdventure.GodMode)
                    {
                        List<(Stack item, int ingridient)> usedSources = new();

                        for (int j = 0; j < recipe.Ingredients.Count; j++)
                        {
                            Ingredient ingredient = recipe.Ingredients[j];
                            Item item = ingredient.Item;

                            List<Stack> stacks = materialSources.FindAll(i =>
                            {
                                return i.Item.GetType() == item.GetType(); //&& i.Item.material == item.material;
                            });

                            for (int k = 0; k < stacks.Count; k++)
                            {
                                int index = usedSources.FindIndex(i => 
                                {
                                    return i.item.Item.GetType() == stacks[k].Item.GetType() &&
                                    i.item.Item.material == stacks[k].Item.material;
                                });

                                if (index > -1) usedSources[index].item.count += stacks[k].count;
                                else usedSources.Add((stacks[k].Copy(), j));
                            }
                        }
                        
                        if (usedSources.Count <= 0 || usedSources == null)
                        {
                            Recipe newRecipe = recipe.Copy();
                            newRecipe.IDName = newRecipe.GetNewNameID(i);

                            ConsoleAdventure.availableRecipes.Add(newRecipe);
                        }

                        else
                        {
                            Dictionary<int, List<Stack>> splintUsed = new();
                            
                            for (int j = 0; j < usedSources.Count; j++)
                            {
                                var used = usedSources[j];

                                if (splintUsed.ContainsKey(used.ingridient))
                                {
                                    splintUsed[used.ingridient].Add(used.item);
                                }

                                else
                                {
                                    splintUsed.Add(used.ingridient, new() { used.item });
                                }
                            }

                            try
                            {
                                AddMaterialRecipes(splintUsed, recipe, i);
                            }

                            catch(Exception ex)
                            {
                                ConsoleAdventure.logger.AddException(ex);
                            }
                        }
                    }
                }

                int oldPos = cursorPos;
                
                cursorPos = ConsoleAdventure.availableRecipes.FindIndex(r => r?.IDName == curRecipe?.IDName);

                if(cursorPos == -1)
                {
                    cursorPos = oldPos;
                }
            }

            if (Input.PostClick(Keys.Enter) && cursorPos > -1 && cursorPos < ConsoleAdventure.availableRecipes.Count)
            {
                ConsoleAdventure.world.GetLocalPlayer().CraftItem(cursorPos);
            }

            timer++;
        }

        private void AddMaterialRecipes(Dictionary<int, List<Stack>> splintUsed, Recipe recipe, int recipeIndex, int keyIndex = 0, List<int> curIndexes = null)
        {
            var stacks = splintUsed.ElementAt(keyIndex);

            if (curIndexes == null)
            {
                curIndexes = new();

                for (int i = 0; i < splintUsed.Count - 1; i++)
                {
                    curIndexes.Add(0);
                }
            }

            Recipe newRecipe = recipe.Copy();

            if (keyIndex < splintUsed.Count - 1)
            {
                newRecipe = newRecipe.Copy();
                newRecipe.Ingredients[keyIndex].Item.material = splintUsed[keyIndex][curIndexes[keyIndex]].Item.material;
                
                keyIndex++;
                
                AddMaterialRecipes(splintUsed, newRecipe, recipeIndex, keyIndex, curIndexes);
            }

            else
            {
                for (int i = 0; i < stacks.Value.Count; i++)
                {
                    newRecipe = newRecipe.Copy();
                    newRecipe.Ingredients[keyIndex].Item.material = splintUsed[keyIndex][i].Item.material;

                    if (newRecipe.InheritedMaterial > -1)
                        newRecipe.OutItem.Item.ApplyMaterial(newRecipe.Ingredients[newRecipe.InheritedMaterial].Item.material);

                    newRecipe.IDName = newRecipe.GetNewNameID(recipeIndex);
                    ConsoleAdventure.availableRecipes.Add(newRecipe);
                }

                bool endAddingRecipes = true;

                if (curIndexes.Count > 0)
                {
                    if (curIndexes[0] < splintUsed.ElementAt(0).Value.Count - 1) 
                        endAddingRecipes = false;
                }

                if (!endAddingRecipes)
                {
                    keyIndex = 0;

                    if (curIndexes[keyIndex] >= stacks.Value.Count - 1)
                        curIndexes[keyIndex] = 0;

                    else curIndexes[keyIndex]++;

                    AddMaterialRecipes(splintUsed, newRecipe, recipeIndex, keyIndex, curIndexes);
                }
            }
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
            spriteBatch.DrawString(ConsoleAdventure.Font, border, Position + new Vector2(4, 3) - new Vector2(4, 0), color_);

            spriteBatch.DrawString(ConsoleAdventure.Font, border2, Position + new Vector2(4, 3 + ((size.Y - 1) * 19)) - new Vector2(4, 0), color_);

            string nameUI = Localization.GetTranslation("UI", "RecipeMenu");

            if (type == 1)
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, border2, Position + new Vector2(4, 3 + ((size.Y + 1) * 19)) - new Vector2(4, 0), color_);
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
                    var ingredient = recipe.Ingredients[i];
                    ingredient.Item.Draw(spriteBatch, Position + new Vector2(36 + nameSize.X + (i * 27), size.Y * 19));
                    spriteBatch.DrawString(ConsoleAdventure.Font, $" {ingredient.Count}", Position + new Vector2(36 + nameSize.X + ((i) * 27), size.Y * 19), color);

                    //countWidth = (int)ConsoleAdventure.Font.MeasureString($" {ingredient.Value}").X;
                }

                if (type == 1)
                {
                    int stationWidth = 0;
                    for (int i = 0; i < recipe.CraftStations.Count; i++)
                    {
                        int type = recipe.CraftStations[i];

                        if (type >= 0 && type < Transform.TypeMapping.Length)
                        {
                            Type value = Transform.TypeMapping[type];

                            if (value != null)
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
}
