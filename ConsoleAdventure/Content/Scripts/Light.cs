using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdventure.WorldEngine;
using System.ComponentModel;
using System.Xaml.Permissions;

namespace ConsoleAdventure.Content.Scripts
{
    public class Light
    {
        private struct LightSource
        {
            public int x;
            public int y;
            public int w;
            public Color color;
            public float radius;

            public LightSource(int x, int y, int w, Color color, float radius)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.color = color;
                this.radius = radius;
            }
        }

        private static readonly object locker = new object();
        private static List<LightSource> lightSources = new();
        public static Color[,] colors = new Color[61, 31];

        private static int x;
        private static int y;
        private static int w;
        private static int width = 61;
        private static int height = 31;

        public static bool onPlaceLightSource = false;

        public static void Update(Position start)
        {

            x = start.x - 30;
            y = start.y - 15;
            w = ConsoleAdventure.world.GetLocalPlayer().w;

            Color color = Color.Black;

            if (w == ConsoleAdventure.StartDeep)
            {
                color = Color.White;
            }

            Clear();

            for (int i = -11; i < width + 11; i++)
            {
                for (int j = -11; j < height + 11; j++)
                {
                    Field field = ConsoleAdventure.world.GetField(i + (x), j + (y), World.BlocksLayerId, w);
                    Field field1 = ConsoleAdventure.world.GetField(i + (x), j + (y), World.MobsLayerId, w);

                    if (field?.content != null)
                    {
                        field.content.OnTheScreen();
                    }

                    if (field1?.content != null)
                    {
                        field1.content.OnTheScreen();
                    }
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Field field = ConsoleAdventure.world.GetField(i + x, j + y, World.BlocksLayerId, w);

                    Color accumulatedLight = color;

                    if (field != null)
                    {
                        for (int k = 0; k < lightSources.Count; k++)
                        {
                            LightSource source = lightSources[k];

                            if (source.w != w)
                                continue;

                            float distance = Vector2.DistanceSquared(new Vector2(i + x, j + y), new Vector2(source.x, source.y));
                            if (distance < source.radius)
                            {
                                float intensity = MathHelper.Clamp(1f - (distance / source.radius), 0, 1);

                                Color lightColor = source.color * intensity;

                                if (!accumulatedLight.Equals(lightColor))
                                {
                                    if (LightBlock(i + x, j + y, w, source))
                                    {
                                        accumulatedLight = Utils.AddColors(accumulatedLight, lightColor);
                                    }
                                }
                            }

                            accumulatedLight.A = 255;
                        }
                    }

                    colors[i, j] = accumulatedLight;
                }
            }
        }
        

        private static bool LightBlock(int x, int y, int w, LightSource lightSource)
        {
            int maxWallCount = 1;
            int wallCount = 0;
            int xA = lightSource.x;
            int yA = lightSource.y;

            int dx = Math.Abs(x - xA);
            int dy = Math.Abs(y - yA);

            int sx = xA < x ? 1 : -1;
            int sy = yA < y ? 1 : -1;

            int err = dx - dy;

            while (true)
            {
                if (xA == x && yA == y) break;

                Transform transform = ConsoleAdventure.world.GetField(xA, yA, World.BlocksLayerId, w)?.content;
                if (transform != null && transform.isObstacle == true)
                {
                    wallCount++;
                }

                if (wallCount > maxWallCount)
                {
                    return false;
                }

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    xA += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    yA += sy;
                }
            }

            return true;
        }

        public static void Add(int x, int y, int w, Color color, float radius = -1)
        {
            if(radius < 0)
                radius = (float)((color.R + color.G + color.B) / 3);

            lightSources.Add(new LightSource(x, y, w, color, radius));
            onPlaceLightSource = true;
        }

        public static void Add(Position position, int w, Color color)
        {
            Add(position.x, position.y, w, color);
        }

        public static void Clear()
        {
            lightSources.Clear();
        }
    }
}