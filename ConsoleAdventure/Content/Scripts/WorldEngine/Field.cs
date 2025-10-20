using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
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
                if (content_ != null)
                    Transform.UpdatedChunk(content_.position, content_.worldLayer);

                if (value != null)
                    Transform.UpdatedChunk(value.position, value.worldLayer);

                content_ = value; 
            }
        }

        public void Destroy()
        {
            if (content != null)
                content.OnDestroy();
            content = null;
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
