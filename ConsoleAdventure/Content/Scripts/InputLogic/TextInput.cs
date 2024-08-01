using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.InputLogic
{
    public static class TextInput
    {
        public const bool BlackList = false;
        public const bool WhiteList = true;

        private static KeyboardState preCharInput;
        private static KeyboardState curCharInput;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int ToUnicodeEx(
        uint wVirtKey,
        uint wScanCode,
        byte[] lpKeyState,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] char[] pwszBuff,
        int cchBuff,
        uint wFlags,
        IntPtr dwhkl);

        public static string GetInputText(string oldString, Point oldCursor, out Point newCursor, char[] charList = null, bool listType = false)
        {
            string text = oldString ?? string.Empty;
            Point cursorPos = oldCursor;
            List<string> lines = new();

            preCharInput = curCharInput;
            curCharInput = Keyboard.GetState();

            bool isNew = false;

            Keys[] keys = curCharInput.GetPressedKeys();

            for (int i = 0; i < keys.Length; i++)
            {
                Keys key = keys[i];

                if (!preCharInput.IsKeyDown(key))
                {
                    char? character = GetCharFromKey(key);
                    lines = new List<string>(text.Split(new[] { '\n' }, StringSplitOptions.None));

                    if (character.HasValue)
                    {
                        bool isAvailable = false;
                        for (int j = 0; j < ConsoleAdventure.Font.Characters.Count; j++)
                        {
                            if (character.Value == ConsoleAdventure.Font.Characters[j])
                            {
                                isAvailable = true;
                            }
                        }

                        if (character.Value == '\t' || character.Value == '\r' || character.Value == '\b' || character.Value == ' ')
                        {
                            isAvailable = true;
                        }

                        if (!isAvailable) goto exit; //выход, если символа нету в шрифте

                        if (charList != null)
                        {
                            if (listType == BlackList)
                            {
                                for (int j = 0; j < charList.Length; j++)
                                {
                                    if (character.Value == charList[j])
                                    {
                                        isAvailable = false;
                                    }
                                }
                            }

                            else
                            {
                                for (int j = 0; j < charList.Length; j++)
                                {
                                    if (character.Value != charList[j])
                                    {
                                        isAvailable = false;
                                    }
                                }
                            }

                            if (!isAvailable) goto exit; //выход, если символ запретный
                        }

                        if (cursorPos.X > lines[cursorPos.Y].Length) cursorPos.X = lines[cursorPos.Y].Length;

                        if (character.Value == '\b')
                        {
                            if (text.Length > 0)
                            {
                                if (cursorPos.X > 0)
                                {
                                    lines[cursorPos.Y] = lines[cursorPos.Y].Remove(cursorPos.X - 1, 1);
                                    cursorPos.X--;
                                }
                                else if (cursorPos.Y > 0)
                                {
                                    cursorPos.X = lines[cursorPos.Y - 1].Length;
                                    lines[cursorPos.Y - 1] += lines[cursorPos.Y];
                                    lines.RemoveAt(cursorPos.Y);
                                    cursorPos.Y--;
                                }
                            }
                        }

                        else if (character == '\r')
                        {
                            string newLine = lines[cursorPos.Y].Substring(cursorPos.X);
                            lines[cursorPos.Y] = lines[cursorPos.Y].Substring(0, cursorPos.X);
                            lines.Insert(cursorPos.Y + 1, newLine);
                            cursorPos.X = 0;
                            cursorPos.Y++;
                        }

                        else
                        {
                            if (character.Value != '\t')
                            {      
                                lines.InsertStringToLines(cursorPos, character.Value.ToString());
                                cursorPos.X++;      
                            }
                            else
                            {
                                lines.InsertStringToLines(cursorPos, "    ");
                                cursorPos.X += 4;
                            }
                        }

                        isNew = true;
                    }

                    else
                    {
                        if (key == Keys.Up && cursorPos.Y > 0)
                        {
                            cursorPos.Y--;
                            cursorPos.X = Math.Min(cursorPos.X, lines[cursorPos.Y].Length);
                        }

                        if (key == Keys.Down && cursorPos.Y < lines.Count - 1)
                        {
                            cursorPos.Y++;
                            cursorPos.X = Math.Min(cursorPos.X, lines[cursorPos.Y].Length);
                        }

                        if (key == Keys.Left)
                        {
                            if (cursorPos.X > 0)
                            {
                                cursorPos.X--;
                            }
                            else if (cursorPos.Y > 0)
                            {
                                cursorPos.Y--;
                                cursorPos.X = lines[cursorPos.Y].Length;
                            }
                        }

                        if (key == Keys.Right)
                        {
                            if (cursorPos.X < lines[cursorPos.Y].Length)
                            {
                                cursorPos.X++;
                            }
                            else if (cursorPos.Y < lines.Count - 1)
                            {
                                cursorPos.Y++;
                                cursorPos.X = 0;
                            }
                        }

                        /*if (curCharInput.IsKeyDown(Keys.LeftControl) && curCharInput.IsKeyDown(Keys.C))
                        {
                            if(text != string.Empty)
                                System.Windows.Forms.Clipboard.SetText(text); //не надо создовать using, всё сломается 
                        }

                        if (curCharInput.IsKeyDown(Keys.LeftControl) && curCharInput.IsKeyDown(Keys.V))
                        {
                            lines.InsertStringToLines(cursorPos, System.Windows.Forms.Clipboard.GetText()); //не надо создовать using, всё сломается 
                            isNew = true;
                        }*/
                    }
                }
            }

            if (isNew)
            {
                newCursor = cursorPos;
                return string.Join('\n', lines);
            }

            newCursor = cursorPos;
            return text;


            exit:
                newCursor = oldCursor; 
                return oldString;
        }

        public static void InsertStringToLines(this List<string> lines, Point pos, string text)
        {
            if(pos.X >= lines[pos.Y].Length - 1)
            {
                lines[pos.Y] += text;
            }
            else
            {
                lines[pos.Y] = lines[pos.Y].Insert(pos.X, text);
            }
            
        }

        public static char? GetCharFromKey(Keys key)
        {
            byte[] keyboardState = new byte[256];
            if (!GetKeyboardState(keyboardState))
            {
                return null;
            }

            uint scanCode = MapVirtualKey((uint)key, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            char[] buffer = new char[2];
            int result = ToUnicodeEx((uint)key, scanCode, keyboardState, buffer, buffer.Length, 0, inputLocaleIdentifier);

            if (result > 0)
            {
                return buffer[0];
            }

            return null;
        }
    }
}
