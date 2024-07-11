namespace ConsoleAdventure
{
    public class Apple : Food
    {
        public Apple()
        {
            satiety = 1;
            name = Localization.GetTranslation("Items", GetType().Name);
            description = GetDescription();
        }

        public new string GetDescription()
        {
            return " " + Localization.GetTranslation("ItemDescs", GetType().Name) + "\n " + base.GetDescription();
        }
    }
}