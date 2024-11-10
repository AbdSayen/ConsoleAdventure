using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Networks;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public class Map
    {
        internal Dictionary<Point, MapChunk> data = new();

        public async void Update()
        {
            World world = ConsoleAdventure.world;
            Player.Player observer = world.GetLocalPlayer();

            Point? curChunk = null;
            int X = 0;
            int Y = 0;

            for (int y = observer.position.y - 30 / 2; y < observer.position.y + 30 / 2; y++)
            {
                if (y >= 0 && y < world.chunks.GetLength(1) * Chunk.Size)
                {
                    for (int x = observer.position.x - 60 / 2; x < observer.position.x + 60 / 2; x++)
                    {
                        if (x >= 0 && x < world.chunks.GetLength(0) * Chunk.Size)
                        {
                            Point? chunk = GetChunkPos(x, y);
                            if (curChunk != chunk && chunk != null)
                            {
                                curChunk = chunk;

                                if (!data.ContainsKey((Point)curChunk))
                                {
                                    data.Add((Point)curChunk, new MapChunk());
                                }
                            }

                            MapField? mapField = data[(Point)curChunk].fields[x % Chunk.Size, y % Chunk.Size, observer.w];

                            Transform t = world.GetField(x, y, World.BlocksLayerId, observer.w).content;

                            if (t != null && (!mapField.HasValue || (mapField.HasValue && (Light.colors[X, Y].R > mapField.Value.color.A))))
                                data[(Point)curChunk].fields[x % Chunk.Size, y % Chunk.Size, observer.w] = new(t.GetColor(), (byte)(Light.colors[X, Y].R));

                            if (t == null && mapField.HasValue && Light.colors[X, Y].R > 0)
                                data[(Point)curChunk].fields[x % Chunk.Size, y % Chunk.Size, observer.w] = null;
                        }
                        X++;
                    }
                }
                Y++;
                X = 0;
            }
        }

        public static Point? GetChunkPos(int x, int y)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            if (chunkX >= 0 && chunkX < Transform.world.chunks.GetLength(0) && chunkY >= 0 && chunkY < Transform.world.chunks.GetLength(1))
            {
                return new(chunkX, chunkY);
            }
            return null;
        }
    }
}
