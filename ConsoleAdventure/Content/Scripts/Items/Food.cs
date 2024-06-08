using ConsoleAdventure.Settings;

namespace ConsoleAdventure
{
    public abstract class Food : Item
    {
        public int satiety { get; protected set; } = 1;
        public void Eat()
        {
            Loger.AddLog($"{name} было съедено");
        }

        protected new string GetDescription()
        {
            switch (WorldEngine.World.language)
            {
                case Language.english:
                    return $"Can be eaten";
                case Language.russian:
                    return $"Можно съесть";
                default:
                    goto case Language.english;
            }
        }
    }
}
