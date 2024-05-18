using ConsoleAdventure.Content.Scripts.World;
using ConsoleAdventure.World;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    internal class Generator
    {
        private Random random;
        private readonly int size;
        private readonly World world;
        public Generator(World world, int size)
        {
            this.size = size;
            this.world = world;
        }

        public void Generate(int seed)
        {
            random = new Random(seed);
            Generate();
        }
        public void Generate()
        {
            random = new Random();
            GenerateSpace();
            GenerateBarriers();
            //GenerateTrees();
            //BuildRuine();
        }

        private void GenerateSpace()
        {
            for (int z = 0; z < World.CountOfLayers; z++)
            {
                world.fields.Add(new List<List<Field>>());
                for (int y = 0; y < size; y++)
                {
                    world.fields[z].Add(new List<Field>());
                    for (int x = 0; x < size; x++)
                    {
                        world.fields[z][y].Add(new Field());
                    }
                }
            }
        }

        private void GenerateBarriers()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (y == size - 1 || y == 0 || x == size - 1 || x == 0)
                    {
                        Field field = world.fields[World.BlocksLayerId][x][y];
                        field.content = new Wall(world, World.BlocksLayerId);
                        field.content.renderFieldType = RenderFieldType.wall;
                        field.content.position.SetPosition(x, y);
                    }
                }
            }
        }

        private void GenerateTrees()
        {
            for (int y = 0; y < world.fields[World.BlocksLayerId].Count; y++)
            {
                for (int x = 0; x < world.fields[World.BlocksLayerId][y].Count; x++)
                {
                    Field field = world.fields[World.BlocksLayerId][y][x];

                    if (field.content == null && random.Next(0, 5) == 0)
                    {
                        field.content = new Tree(world, World.BlocksLayerId);
                        field.content.position.SetPosition(x, y);
                    }
                }
            }
        }

        private void BuildRuine()
        {
            for (int y = 0; y < world.fields[World.BlocksLayerId].Count; y++)
            {
                for (int x = 0; x < world.fields[World.BlocksLayerId][y].Count; x++)
                {
                    Field field = world.fields[World.BlocksLayerId][x][y];
                    
                    if (y == 10 || y == 11)
                    {
                        field.content = new Wall(world, World.BlocksLayerId);
                        field.content.position.SetPosition(x, y);
                    }
                }
            }
        }
    }
}