using System;

namespace ConsoleAdventure.World
{
    public class Field
    {
        private string description;
        public FieldType type;
        public Transform content;

        public string GetSymbol()
        {
            switch (type) {
                case FieldType.empty:
                    return " .";
                case FieldType.player:
                    return " @";
                case FieldType.wall:
                    return " #";
                default:
                    return " ^";
            }

        }
    }
}
