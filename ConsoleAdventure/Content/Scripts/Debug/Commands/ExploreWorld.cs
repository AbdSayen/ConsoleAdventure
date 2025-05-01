using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.WorldEngine.Generate;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class ExploreWorld : Command
    {
        public ExploreWorld()
        {
            Name = "exploreworld";
            Description = "explores the whole world";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            ExploreAllWorld();
        }

        private void ExploreAllWorld()
        {
            World world = ConsoleAdventure.world;
            Player.Player player = world.GetLocalPlayer();

            player.map.data.Clear();

            for (int x = 0; x < world.GetChunkCounts().X; x++)
            {
                for (int y = 0; y < world.GetChunkCounts().Y; y++)
                {
                    Point chunk = new Point(x, y);
                    player.map.data.Add(chunk, new MapChunk());
                    world.GenerateChunk(x, y);

                    for (int x1 = 0; x1 < Chunk.Size; x1++)
                    {
                        for (int y1 = 0; y1 < Chunk.Size; y1++)
                        {
                            for (int w = 0; w < Chunk.maxDeep; w++)
                            {
                                Position position = new Position(x1 + (x * Chunk.Size), y1 + (y * Chunk.Size));

                                Transform t = world.GetField(position.x, position.y, World.BlocksLayerId, w).content;

                                if (t != null)
                                    player.map.data[chunk].fields[x1, y1, w] = new(t.GetColor(), 255);
                            }
                        }
                    }

                    world.chunks[x, y] = null;
                }
            }
        }
        private void ExploreWorldForChunk()
        {
            World world = ConsoleAdventure.world;
            Player.Player player = world.GetLocalPlayer();
            Point curChunk = new();

            player.map.data.Clear();

            for (int x = 0; x < world.GetChunkCounts().X; x++)
            {
                for (int y = 0; y < world.GetChunkCounts().Y; y++)
                {
                    Point chunk = new Point(x, y);
                    world.GenerateChunk(x, y);

                    if (x % Chunk.Size == 0 && y % Chunk.Size == 0)
                    {
                        player.map.data.Add(chunk, new MapChunk());
                        curChunk = chunk;
                    }

                    for (int w = 0; w < Chunk.maxDeep; w++)
                    {
                        Position position = new Position((x * Chunk.Size), (y * Chunk.Size));

                        Transform t = world.GetField(position.x, position.y, World.BlocksLayerId, w).content;

                        if (t != null)
                            player.map.data[curChunk].fields[chunk.X % Chunk.Size, chunk.Y % Chunk.Size, w] = new(t.GetColor(), 255);
                    }

                    world.chunks[x, y] = null;
                }
            }
        }
    }
}
