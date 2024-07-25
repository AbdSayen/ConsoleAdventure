namespace ConsoleAdventure.CaModLoaderAPI
{
    public abstract class GlobalItem
    {
        public virtual string GetDescription(Item item)
        {
            return null;
        }

        public virtual bool? CanBePickedUp(Item item)
        {
            return null;
        }
    }
}
