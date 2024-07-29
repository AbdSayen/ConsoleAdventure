using ConsoleAdventure.Generate.Structures;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class StructureGenerator
    {
        private World world;
        public void Generate(World world) 
        {
            this.world = world;

            Position startPosition = new Position(30, 30);
            new House(startPosition, 25, 25);
        }

        private bool CheckGeneratePossibility(Position startPosition, int sizeX, int sizeY)
        {
            var layer = world.GetFields(WorldEngine.World.BlocksLayerId);
            int layerHeight = layer.Count;
            int layerWidth = layer[0].Count;
            int worldSize = world.size;

            for (int y = startPosition.y; y < startPosition.y + sizeY; y++)
            {
                if (y >= worldSize || y < 0)
                {
                    return false;
                }
                for (int x = startPosition.x; x < startPosition.x + sizeX; x++)
                {
                    if (x >= worldSize || x < 0 ||
                        world.GetField(x, y, World.BlocksLayerId) == null ||
                        world.GetField(x, y, World.BlocksLayerId).isStructure)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
