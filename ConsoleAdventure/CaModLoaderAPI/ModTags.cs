using ConsoleAdventure.Content.Scripts.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.CaModLoaderAPI
{
    public class ModTags
    {
        string modName;

        Tags tags;

        public ModTags(Tags tags, string modName) 
        {
            this.modName = modName;
            this.tags = tags;
        }

        public object this[string key]
        {
            get
            {
                return tags[modName + "." + key];
            }
            set
            {
                tags[modName + "." + key] = value;
            }
        }

        public T SafelyGet<T>(string key)
        {
            return tags.SafelyGet<T>(modName + "." + key);
        }
    }
}
