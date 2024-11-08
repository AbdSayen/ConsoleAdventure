using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class TextListUI : ListUI
    {
        public TextListUI(string text, int maxLength, Vector2 position, Color color) : base("", position, GetTextList(text, maxLength), color)
        {
            if(elements != null || elements.Count != 0)
            {
                elements[0].isHover = true;
            }
        }

        private static List<BaseUI> GetTextList(string text, int maxLength)
        {
            List<BaseUI> list = new();

            int counter = 0;
            int lastSpace = -1;
            int lastSpaceInText = -1;
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                buffer.Append(text[i]);

                if (text[i] == ' ') 
                {
                    lastSpace = buffer.Length;
                    lastSpaceInText = i;
                    if(buffer.Length == 1)
                        buffer.Clear(); 
                }

                if (text[i] == '\n' || counter > maxLength || i >= text.Length - 1)
                {
                    if(lastSpace > -1)
                    {
                        buffer.Remove(lastSpace, buffer.Length - lastSpace);
                        i = lastSpaceInText;
                        lastSpace = -1;
                        lastSpaceInText = -1;
                    }
                    counter = 0;
                    list.Add(new(buffer.ToString(), Vector2.Zero, Color.White));
                    buffer.Clear();
                }

                counter++;
            }

            return list;
        }

        public new void Update(ref int timer)
        {
            int waitTime = Utils.StabilizeTicks(this.waitTime);

            if (elements.Count > 0)
            {
                if (Input.IsKeyDown(InputConfig.NavigationUp) && timer >= waitTime)
                {
                    if (startList > 0) //прокручиваем область видимого списка
                    {
                        startList -= 1;
                        endList = Math.Min(startList + drawBuffer, elements.Count);
                    }

                    timer = 0;
                }

                if (Input.IsKeyDown(InputConfig.NavigationDown) && timer >= waitTime)
                {
                    if (endList < elements.Count - 1)
                    {
                        startList += 1;
                        endList = Math.Min(startList + drawBuffer, elements.Count);
                    }

                    timer = 0;
                }
            }
        }
    }
}
