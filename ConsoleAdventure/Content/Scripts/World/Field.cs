namespace ConsoleAdventure.WorldEngine
{
    public class Field
    {
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
                case RenderFieldType.wall:
                    return " #";
                case RenderFieldType.tree:
                    return "{}";
                default:
                    return " ^";
            }

        }
    }
}
