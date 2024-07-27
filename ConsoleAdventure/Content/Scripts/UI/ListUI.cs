using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace ConsoleAdventure.Content.Scripts.UI
{
    public class ListUI : BaseUI
    {
        public List<BaseUI> elements = new List<BaseUI>();

        private int drawBuffer = 5;

        private int startList = 0;

        private int endList = 5;

        public ListUI(string text, Vector2 position, List<BaseUI> elements, Color color) : base(text, position, color)
        {
            this.elements = elements;
            UpdateElementsPosition(position);
        }

        private void UpdateElementsPosition(Vector2 position)
        {
            int oldH = 0;
            for (int i = 0; i < elements.Count; i++)
            {
                Vector2 pos = position;
                pos.Y += oldH + 38 * i;
                elements[i].Position = pos;
                elements[i].text = elements[i].text.Replace("\n", " ");
                oldH = elements[i].rectangle.Height;
            }
        }

        public void AddElement(BaseUI element)
        {
            elements.Add(element);
            UpdateElementsPosition(Position);
        }

        public void Update(ref int timer)
        {
            int waitTime = Utils.StabilizeTicks(10);

            if (elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Up) && timer >= waitTime && (i - 1) != -1)
                    {
                        if (elements[i].isHover)
                        {
                            elements[i].isHover = false;
                            int newIndex = (i - 1 + elements.Count) % elements.Count;
                            elements[newIndex].isHover = true;

                            if (newIndex < startList) //прокручиваем область видимого списка
                            {
                                startList = newIndex;
                                endList = Math.Min(startList + drawBuffer, elements.Count);
                            }

                            timer = 0;
                            break;
                        }
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Down) && timer >= waitTime && i < elements.Count - 1)
                    {
                        if (elements[i].isHover)
                        {
                            elements[i].isHover = false;
                            int newIndex = (i + 1) % elements.Count;
                            elements[newIndex].isHover = true;

                            if (newIndex >= endList)
                            {
                                startList = (startList + 1) % elements.Count;
                                endList = Math.Min(startList + drawBuffer, elements.Count);
                            }

                            timer = 0;
                            break;
                        }
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Enter) && timer >= Utils.StabilizeTicks(30))
                    {
                        if (elements[i].isHover)
                        {
                            ElementActions(elements[i]);
                        }
                    }
                }
            }
        }

        public virtual void ElementActions(BaseUI element)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (elements.Count > 0)
            {
                int number = 0;
                for (int i = startList; i < endList; i++)
                {
                    if (i < elements.Count)
                    {
                        elements[i].Position = Position + new Vector2(0, (number * 38));
                        elements[i].Draw(spriteBatch);
                        number++;
                    }
                }
            }
        }
    }
}
