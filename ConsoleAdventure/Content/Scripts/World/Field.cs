using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Field
    {
        public bool isStructure = false;

        [NonSerialized]
        public Color color = Color.White;

        public string structureName { get; set; } = "None";
        public Transform content;

        public string GetSymbol()
        {
            if (content == null) return "  ";

            else return content.GetSymbol();
        }

        internal void Deconstruct()
        {
            throw new NotImplementedException();
        }
    }
}
