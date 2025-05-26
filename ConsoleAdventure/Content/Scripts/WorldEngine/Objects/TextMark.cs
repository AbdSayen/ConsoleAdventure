using Microsoft.Xna.Framework;
using System;
using System.Text;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class TextMark : Transform
    {
        string text;

        public TextMark(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.textMark;

            AddTypeToMap<TextMark>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "__";
        }

        public override Color GetColor()
        {
            return Color.White;
        }

        public override void OnTheScreen()
        {
            int offset = text.Length / 4;
            Position pos = new Position(position.x - offset, position.y);

            if (position.y >= ConsoleAdventure.startDisplay.y && position.y < ConsoleAdventure.endDisplay.y)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < text.Length; i++)
                {
                    Position position = pos + new Position(i / 2, 0);

                    if (position.x < ConsoleAdventure.startDisplay.x || position.x >= ConsoleAdventure.endDisplay.x)
                    {
                        sb.Append(" ");
                    }

                    else
                    {
                        sb.Append(text[i]);
                    }
                }

                StringPaint.Draw(sb.ToString(), pos, w, new Vector2(), Color.White);
            }
        }

        public override object SaveData()
        {
            return text;
        }

        public override void LoadData(object data)
        {
            text = (string)data;
        }
    }
}