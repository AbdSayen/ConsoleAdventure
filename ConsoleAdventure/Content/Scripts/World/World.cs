using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace ConsoleAdventure.World
{
    public class World
    {
        private int worldSize = 50;

        // [Z][Y][X]
        private List<List<List<Field>>> fields = new List<List<List<Field>>>();
        public List<Player> players = new List<Player>();

        private Generator generator;
        private Renderer renderer;

        public static int CountOfLayers = 4;
        public static int FloorLayerId = 0;
        public static int BlocksLayerId = 1;
        public static int MobsLayerId = 2;
        public static int ItemsLayerId = 3;
        public World()
        {
            generator = new Generator(fields, worldSize);
            renderer = new Renderer(fields);
            generator.Generate();
            ConnectPlayer();
        }

        public void ConnectPlayer()
        {
            players.Add(new Player(players.Count));
            players[players.Count - 1].PostInFields(BlocksLayerId, fields);
        }

        public string Render()
        {
            return $"{renderer.Render(players[0])}";
        }
    }
}
