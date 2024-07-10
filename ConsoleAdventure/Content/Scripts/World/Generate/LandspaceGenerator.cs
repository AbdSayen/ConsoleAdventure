using ConsoleAdventure.Settings;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class LandspaceGenerator
    {
        private World world;
        private Random random;

        public void Generate(World world, Random random) 
        {
            this.world = world;
            this.random = random;

            GenerateTrees();
        }

        private void GenerateTrees()
<<<<<<< HEAD
        {
            for (int y = 0; y < world.worldSize; y++)
=======
        {
            for (int y = 0; y < world.GetFields(World.BlocksLayerId).Count; y++)
>>>>>>> 0fa11a56c7e1c5ef353e7fb61295202e73c7eac4
            {
                for (int x = 0; x < world.worldSize; x++)
                {
                    Field field = world.GetField(x, y, World.BlocksLayerId);
                    Position position = new Position(x, y);

                    if (random.Next(0, 150) == 0 && field.content == null && field.isStructure == false)
                    {
                        new Tree(world, position, World.BlocksLayerId);
<<<<<<< HEAD
=======
                        
                        
>>>>>>> 0fa11a56c7e1c5ef353e7fb61295202e73c7eac4
                    }

                    // REMOVE
                    else if (random.Next(0, 300) == 0)
                    {
                        new Loot(world, position, items: new List<Stack> { new Stack(new Apple(), 5) });
                    }

                    if (random.Next(0, 500) == 0 && field.content == null && field.isStructure == false)
                    {
                        for (int i = 0; i < random.Next(1, 3); i++)
                        {
                            for (int j = 0; j < random.Next(1, 3); j++)
                            {
                                new Water(world, position + new Position(i, j), World.BlocksLayerId);
                            }
                        }                      
                    }
                }
            }
        }
    }
}
