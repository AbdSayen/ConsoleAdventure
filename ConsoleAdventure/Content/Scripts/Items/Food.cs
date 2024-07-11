using ConsoleAdventure.Settings;

namespace ConsoleAdventure
{
    public abstract class Food : Item
    {
        public int satiety { get; protected set; } = 1;
        public void Eat()
        {
            Loger.AddLog(name.ToString() + Localization.GetTranslation("Events", "Eating"));
        }

        protected new string GetDescription()
        {
            return Localization.GetTranslation("ItemDescs", "BaseFood");
        }
    }
}
