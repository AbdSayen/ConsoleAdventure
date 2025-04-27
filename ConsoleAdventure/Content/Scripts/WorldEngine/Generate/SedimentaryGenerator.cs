using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Generate.Structures;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class SedimentaryGenerator : EmptyGenerator
    {
        Position[] positions =
        {
            new Position(0, 1),
            new Position(0, -1),
            new Position(1, 0),
            new Position(-1, 0),
            new Position(1, 1),
            new Position(-1, 1),
            new Position(1, -1),
            new Position(-1, -1),
        };

        public override async Task Generate(World world)
        {
            await base.Generate(world);
            processHint = "Weathering of rocks...";

            GenerateSedimentary();
        }

        private void GenerateSedimentary()
        {
            Random random = Generator.GenRand;

            IterateWorldChunkByChunk((int x, int y) => //    <- Вот пример чанковой генерации через весь мир
            {
                //Field field = world.GetField(x, y, World.BlocksLayerId, ConsoleAdventure.world.Sedimentary);
                //Field field2 = world.GetField(x, y, World.FloorLayerId, ConsoleAdventure.world.Sedimentary);
                Position position = new Position(x, y);

                new Alfisol(position, ConsoleAdventure.world.Sedimentary);
                new AlfisolFloor(position, ConsoleAdventure.world.Sedimentary);
            });

            processHint = "Weathering of rocks... [Adding holes]";

            for (int i = 0; i < 6; i++)
            {
                Position descentPos = new();
                for (int j = 0; j < 1001; j++)
                {
                    descentPos = new(Generator.GenRand.Next(1, world.size - 1), Generator.GenRand.Next(1, world.size - 1));

                    if (world.GetField(descentPos.x, descentPos.y, World.BlocksLayerId, ConsoleAdventure.world.Sedimentary)?.content == null)
                    {
                        if (world.GetField(descentPos.x, descentPos.y, World.BlocksLayerId, ConsoleAdventure.world.Sedimentary + 1)?.content == null)
                        {
                            if (world.GetField(descentPos.x, descentPos.y, World.BlocksLayerId, ConsoleAdventure.world.Sedimentary - 1)?.content == null)
                            {
                                break;
                            }
                        }
                    }
                }

                new Climb(descentPos, ConsoleAdventure.world.Sedimentary);
                new Descent(descentPos, ConsoleAdventure.world.Sedimentary + 1);

                Position descentPos2 = descentPos + positions[random.Next(0, 9)];

                new Climb(descentPos2, ConsoleAdventure.world.Sedimentary - 1);
                new Descent(descentPos2, ConsoleAdventure.world.Sedimentary);

                processProgress = (int)((float)i / 6 * 100);
            }
        }
    }
}
