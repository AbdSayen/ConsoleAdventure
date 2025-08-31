using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.Content.Scripts
{
    public interface IFormatMode
    {
        public Range Range { get; set; }

        /// <summary>
        /// Шаблон форматирования. Используются для определения режима и его аргументов. <br/><br/>
        /// </summary>
        public FormatTemplate Template { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="arguments"></param>
        /// <param name="modifyText"></param>
        /// <returns></returns>
        public virtual int Define(FormatString text, List<string> arguments, string modifyText) { return -1; }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, FormatString text) { }
    }

    public struct FormatTemplate
    {
        public string name;
        public int argumentsCount;
        public bool isTextModifier;

        public FormatTemplate(string name, int argumentsCount, bool isTextModifier)
        {
            if (name == null) throw new ArgumentNullException();
            if (!isTextModifier && argumentsCount < 1) throw new ArgumentException("in template argumentsCount must be at least 0");
            if (isTextModifier && argumentsCount < 0) throw new ArgumentException("in template with isTextModifier argumentsCount must be at least 1");

            this.name = name;
            this.argumentsCount = argumentsCount;
            this.isTextModifier = isTextModifier;
        }
    }
}
