using System.Data;

namespace ConsoleAdventure
{
    public class Player : Transform
    {
        public int id;
        public string Name { get; set; }
    
        public Player(int id)
        {
            this.id = id;
            x = 20;
            y = 20;
        }

        public void Move()
        {

        }
    }
}
