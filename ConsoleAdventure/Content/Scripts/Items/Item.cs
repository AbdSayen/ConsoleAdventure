using ConsoleAdventure.CaModLoaderAPI;
using System;
using ConsoleAdventure;

namespace ConsoleAdventure
{
    [Serializable]
    public abstract class Item
    {
        public string name = "Name missing";
        public string description = "Description missing";

        protected string GetDescription()
        {
            foreach (GlobalItem item in CaModLoader.modGlobalItems)
            {
                string customDescription = item.GetDescription(this);
                if (customDescription != null)
                    return customDescription;
            }
            return description;
        }

        public virtual bool CanBePickedUp()
        {
            foreach (GlobalItem glItem in CaModLoader.modGlobalItems)
            {
                bool? customPickUp = glItem.CanBePickedUp(this, ConsoleAdventure.world.players[0]);
                if (customPickUp != null)
                {
                    return (bool)customPickUp;
                }
            }
            return true;
        }
    }
}
