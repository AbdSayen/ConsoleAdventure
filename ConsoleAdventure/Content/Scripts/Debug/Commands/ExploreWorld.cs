using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
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
            World world = ConsoleAdventure.world;
            Player.Player player = world.GetLocalPlayer();
            Point? curChunk = null;

            for (int y = 0; y < world.size; y++)
            {
                for (int x = 0; x < world.size; x++)
                {
                    for (int w = 0; w < Chunk.maxDeep; w++)
                    {
                        Point? chunk = Map.GetChunkPos(x, y);
                        if (curChunk != chunk && chunk != null)
                        {
                            curChunk = chunk;

                            if (!player.map.data.ContainsKey((Point)curChunk))
                            {
                                player.map.data.Add((Point)curChunk, new MapChunk());
                            }
                        }

                        Transform t = world.GetField(x, y, World.BlocksLayerId, w).content;

                        if (t != null)
                            player.map.data[(Point)curChunk].fields[x % Chunk.Size, y % Chunk.Size, w] = new(t.GetColor(), 255);
                    }
                }
            }
        }
    }
}
