using ConsoleAdventure.Content.Scripts.Player;

namespace ConsoleAdventure.CaModLoaderAPI
{
    public abstract class GlobalItem
    {
        public virtual string GetDescription(Item item)
        {
            return null;
        }

        public virtual bool? CanBePickedUp(Item item, Player player)
        {
            return null;
        }
    }
}
