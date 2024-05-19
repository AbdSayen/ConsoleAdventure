using ConsoleAdventure.Settings;

namespace ConsoleAdventure
{
    public class Position
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}