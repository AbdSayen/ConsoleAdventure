using ConsoleAdventure.Settings;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class LandspaceGenerator
    {
        private World world;

        public void Generate(World world)
        {
            this.world = world;

            new Chest(new(0, 0), new List<Stack>() { new Stack(new Apple(), 50) });
            GenerateTrees();
        }

        private void GenerateTrees()
        {
            Random random = ConsoleAdventure.rand;

            for (int y = 0; y < world.size; y++)
            {
                for (int x = 0; x < world.size; x++)
                {
                    Field field = world.GetField(x, y, World.BlocksLayerId);
                    Position position = new Position(x, y);

                    if (random.Next(0, 150) == 0 && field.content == null && field.isStructure == false)
                    {
                        new Tree(position);
                    }
                    else if (random.Next(0, 300) == 0)
                    {
                        new Loot(position, new List<Stack> { new Stack(new Apple(), 5) });
                    }

                    if (random.Next(0, 1500) == 0 && field.content == null && field.isStructure == false)
                    {
                        for (int i = 0; i < random.Next(1, 3); i++)
                        {
                            for (int j = 0; j < random.Next(1, 3); j++)
                            {
                                new Water(position + new Position(i, j), World.BlocksLayerId);
                            }
                        }
                    }

                    if (random.Next(0, 500) == 0 && field.content == null && field.isStructure == false)
                    {
                        for (int i = 0; i < random.Next(1, 4); i++)
                        {
                            for (int j = 0; j < random.Next(1, 4); j++)
                            {
                                new Grass(position + new Position(i, j));
                            }
                        }
                    }
                }
            }
        }
    }
}
