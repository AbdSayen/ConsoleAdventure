namespace ConsoleAdventure.WorldEngine
{
    public class Field
    {
        public bool isStructure = false;
        public string structureName { get; set; } = "None";
        public Transform content;

        public string GetSymbol()
        {
            if (content == null) return "  ";

            else return content.GetSymbol();
        }
    }
}
