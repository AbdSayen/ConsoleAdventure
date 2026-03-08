using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UITextInputField : UIText
    {
        public string fieldContent = "";

        public static byte UnderliningCursor => 0;
        public static byte VerticalStickCursor => 1;

        public string tooltip;

        private bool isAutoSized = false;

        private int targetWidth;

        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                size.X = value * 9;
            }
        }
        public byte cursor;

        public Point cursorPos;

        public static int cursorTickTime = 30;

        char[] chars;
        bool listType;

        static string[] cursorChars = new string[2]
        {
            "_",
            "|"
        };

        public string[] BorderChars = new string[2]
        {
            "[",
            "]"
        };

        public UITextInputField(Color color, Point screenPosition, int Width, string tooltip = "", byte cursor = 0, char[] chars = null, bool listType = false, bool isAutoSized = false, Anchor anchor = Anchor.Center, int zOrder = 0) : base("", color, screenPosition, new(), Align.Left, anchor, false, zOrder)
        {
            this.Width = Width;
            targetWidth = Width;

            this.cursor = cursor;
            this.chars = chars;
            this.listType = listType;
            this.tooltip = tooltip;
            this.color = color;

            this.isAutoSized = isAutoSized;

            size.Y = 19;
        }

        public void ClearContent()
        {
            fieldContent = "";
            cursorPos = new Point(0, 0);
            text.SetText(fieldContent, new SmartColor(color));
        }

        public override bool IsFocusable()
        {
            return true;
        }

        public override void OnFocus()
        {
            hovered = true;
        }

        public override void OnDefocus()
        {
            hovered = false;
        }

        public override void Update()
        {
            if (!IsHovered()) return;
            string newText = TextInput.GetInputText(fieldContent, cursorPos, out cursorPos, chars, listType);
            if ((newText.Length <= fieldContent.Length || text.BaseString.Length < Width) && !isAutoSized || isAutoSized)
            {
                fieldContent = newText;
                text.SetText(fieldContent, new SmartColor(color));

                if (isAutoSized)
                {
                    if (text.BaseString.Length > targetWidth)
                    {
                        Width = text.BaseString.Length;
                    }
                }
            }
        }

        int timer;
        int offs = 4; // borders offset
        int offsBottom = 5; // between hint and original text
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            if (fieldContent == "" && tooltip != "")
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, tooltip, drawPosition + new Vector2(9, 0), Color.Gray, 0, Vector2.Zero, 1f, 0, 0);
            }

            bool isFormated = text.BaseString != text.String;
            if (hovered)
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, text.BaseString, drawPosition + new Vector2(9, 0), isFormated ? new Color(70, 70, 70) : Color.White); // original

                // Lets draw BG for formated here if isFormated
                if (isFormated)
                {
                    Utils.DrawAnySizeFrame(spriteBatch, (drawPosition + new Vector2(9, -19 - offs - offsBottom) + new Vector2(-offs*2, -offs)).ToPoint(), new Point((int)ConsoleAdventure.Font.MeasureString(text.String).X + offs * 4, 19 + offs * 2), 1, Color.White);
                }
            }
            if (isFormated || !hovered)
            {
                base.Draw(spriteBatch, drawPosition + (hovered ? new Vector2(9, -19 - offs - offsBottom) : new Vector2(9, 0))); // formated
            }

            if (IsHovered() && timer % cursorTickTime > ((float)cursorTickTime / 2))
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, cursorChars[cursor], drawPosition + new Vector2(9, 0) + new Vector2(cursorPos.X * 9, cursorPos.Y * 19), color, 0, Vector2.Zero, 1f, 0, 0); // + new Vector2(-(text.BaseString.Length - text.String.Length) * 9, 0)
            }

            spriteBatch.DrawString(ConsoleAdventure.Font, BorderChars[0], drawPosition, color, 0, Vector2.Zero, 1f, 0, 0);
            spriteBatch.DrawString(ConsoleAdventure.Font, BorderChars[1], drawPosition + new Vector2((Width + 1) * 9, 0), color, 0, Vector2.Zero, 1f, 0, 0);


            timer++;
        }
    }
}
