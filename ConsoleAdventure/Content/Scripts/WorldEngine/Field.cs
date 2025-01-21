using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Field
    {
        public bool isStructure = false;

        private Transform content_;

        public Transform content
        {
            get
            {
                return content_;
            }
            set
            {
                content_ = value;
            }
        }

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
