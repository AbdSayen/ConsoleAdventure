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
                if (NetworkManager.Id == 0 && ConsoleAdventure.world.isLoaded)
                {
                    if (content_ != null)
                        if (content_.GetType().IsSubclassOf(typeof(Entity))) return;

                    List<byte> data = new List<byte>();
                    short x = 0;
                    short y = 0;
                    byte worldLayer = 0;
                    byte w = 0;
                    byte type = 0;
                    byte isNull = 0;
                    if (content_ != null)
                    {
                        x = content_.position.x;
                        y = content_.position.y;
                        worldLayer = content_.worldLayer;
                        w = content_.w;
                        type = content_.type;
                    }
                    else
                    {
                        isNull = 1;
                    }
                    data.AddRange(BitConverter.GetBytes(x));
                    data.AddRange(BitConverter.GetBytes(y));
                    data.Add(worldLayer);
                    data.Add(w);
                    data.Add(type);
                    data.Add(isNull);
                    if (content_ is Loot)
                    {
                        NetworkManager.SendMessage(NetworkManager.ActionID.fieldContent, data.ToArray(), SerializeData.Serialize(((Loot)content_).GetItems()));
                    }
                    else
                    {
                        NetworkManager.SendMessage(NetworkManager.ActionID.fieldContent, data.ToArray(), new byte[0]);
                    }
                }
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
