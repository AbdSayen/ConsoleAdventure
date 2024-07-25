using System;

namespace ConsoleAdventure.CaModLoaderAPI
{
    [Serializable]
    public abstract class ModItem : Item
    {
        public virtual void Init()
        {
            name = "ModItem Name missing";
            description = "ModItem Description missing";
        }

        public ModItem() {
            Init();
        }
    }
}
