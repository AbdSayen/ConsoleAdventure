namespace ConsoleAdventure.WorldEngine
{
    public class Field
    {
        public bool isStructure = false;
        public string structureName { get; set; } = "None";
        private string description;
        public Transform content;

        public string GetSymbol()
        {
            if (content == null) return "  ";

            switch (content.renderFieldType) {
                case RenderFieldType.empty:
                    return "  ";
                case RenderFieldType.player:
                    return " ^";
                case RenderFieldType.ruine:
                    return "::";
                case RenderFieldType.wall:
                    return "##";
                case RenderFieldType.tree:
                    return " *";
                case RenderFieldType.floor:
                    return " .";
                case RenderFieldType.door:
                    return "[]";
                default:
                    return "??";
            }

        }
    }
}
