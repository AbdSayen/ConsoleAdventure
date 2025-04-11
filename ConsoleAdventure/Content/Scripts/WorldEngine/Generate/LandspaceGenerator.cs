using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Generate.Structures;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class LandspaceGenerator : EmptyGenerator
    {
        public override async Task Generate(World world)
        {
            await base.Generate(world);

            processHint = "Generating trees...";

            //new Chest(new(1, 1), ConsoleAdventure.StartDeep, new List<Stack>() { new Stack(new Apple(), 50) });

            GenerateTrees();
        }

        private void GenerateTrees()
        {
            Random random = Generator.GenRand;

            iterateWorldChunkByChunk((int x, int y) => //    <- Вот пример чанковой генерации через весь мир
            {
                Field field = world.GetField(x, y, World.BlocksLayerId, ConsoleAdventure.world.Surface);
                Position position = new Position(x, y);

                if (random.Next(0, 150) == 0 && field.content == null && field.isStructure == false)
                {
                    new Tree(position, ConsoleAdventure.world.Surface);
                }

                if (random.Next(0, 1500) == 0 && field.content == null && field.isStructure == false)
                {
                    for (int i = 0; i < random.Next(1, 3); i++)
                    {
                        for (int j = 0; j < random.Next(1, 3); j++)
                        {
                            new Water(position + new Position(i, j), ConsoleAdventure.world.Surface);
                        }
                    }
                }

                if (random.Next(0, 500) == 0 && field.content == null && field.isStructure == false)
                {
                    for (int i = 0; i < random.Next(1, 4); i++)
                    {
                        for (int j = 0; j < random.Next(1, 4); j++)
                        {
                            new Grass(position + new Position(i, j), ConsoleAdventure.world.Surface);
                        }
                    }
                }
            });
        }
    }
}
