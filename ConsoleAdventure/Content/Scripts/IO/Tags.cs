using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.IO
{
    public class Tags
    {
        public Dictionary<string, object> Data { get; internal set; }

        public Tags() 
        { 
            Data = new(new Dictionary<string, object>());        
        }

        public object this[string key]
        {
            get
            {
                return Data[key];
            }
            set
            {
                Data.Add(key, value);
            }
        }

        public object SafelyGet(string key)
        {
            if (Data.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }
    }
}
