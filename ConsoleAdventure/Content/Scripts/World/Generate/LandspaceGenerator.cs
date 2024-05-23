using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.World.Generate
{
    internal class LandspaceGenerator
    {
        private WorldEngine.World world;
        private Random random;

        public void Generate(WorldEngine.World world, Random random) 
        {
            this.world = world;
            this.random = random;

            GenerateTrees();
        }

        private void GenerateTrees()
        {
            for (int y = 0; y < world.fields[WorldEngine.World.BlocksLayerId].Count; y++)
            {
                for (int x = 0; x < world.fields[WorldEngine.World.BlocksLayerId][y].Count; x++)
                {
                    Field field = world.fields[WorldEngine.World.BlocksLayerId][y][x];
                    Position position = new Position(x, y);

                    if (random.Next(0, 150) == 0 && field.content == null && field.isStructure == false)
                    {
                        new Tree(world, position, WorldEngine.World.BlocksLayerId);
                        
                        
                    }
                    //TEMP TO REMOVE
                    else if (random.Next(0, 300) == 0)
                    {
                        new Loot(world, position, items: new List<Stack> { new Stack(new Apple(), 45) });
                    }
                }
            }
        }
    }
}
