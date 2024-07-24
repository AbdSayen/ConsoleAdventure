using ConsoleAdventure.WorldEngine;

namespace CaModLoaderAPI
{
    public class Mod
    {
        public string modName = "undefined";
        public string modVersion = "1.0.0";
        public string modDescription = "No description";
        public string modAuthor = "Anonymous";

        public virtual void Init()
        {

        }

        public virtual void Run()
        {

        }

        public virtual void WorldLoaded(World world)
        {
            
        }
    }
}
