namespace ConsoleAdventure
{
    public class Apple : Food
    {
        public Apple()
        {
            satiety = 1;
            name = "Яблоко";
            description = GetDescription();
        }

        public string GetDescription()
        {
            switch (WorldEngine.World.language)
            {
                case Language.english:
                    return $"Apple fruit, {base.GetDescription()}";
                case Language.russian:
                    return $"Плод яблони, {base.GetDescription()}";
                default:
                    goto case Language.english;
            }
        }
    }
}