using ConsoleAdventure.Content.Scripts.InputLogic;
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

        public int drawBuffer = 5;
        public int startList = 0;
        public int endList = 5;
        public int height = 38;

        public int waitTime = 5;

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
            int waitTime = Utils.StabilizeTicks(this.waitTime);

            if (elements.Count > 0)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    if (Input.IsKeyDown(InputConfig.NavigationUp) && timer >= waitTime && (i - 1) != -1)
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

                    if (Input.IsKeyDown(InputConfig.NavigationDown) && timer >= waitTime && i < elements.Count - 1)
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

                    if (Input.IsKeyDown(InputConfig.NavigationSelect) && !Input.IsOldKeyDown(InputConfig.NavigationSelect))
                    {
                        if (elements[i].isHover)
                        {
                            ElementActions(elements[i]);
                        }
                    }

                    if (elements[i].isHover)
                        ElementHandleInput(elements[i]);
                }
            }
        }

        public virtual void ElementActions(BaseUI element)
        {

        }

        public virtual void ElementHandleInput(BaseUI element)
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
                        elements[i].Position = Position + new Vector2(0, (number * height));
                        elements[i].Draw(spriteBatch);
                        number++;
                    }
                }
            }
        }
    }
}
