using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdventure.WorldEngine;
using System.ComponentModel;
using System.Xaml.Permissions;
using System.Threading;
using System.Diagnostics;

namespace ConsoleAdventure.Content.Scripts
{
    public class Light
    {
        private struct LightSource
        {
            public Position position;
            public byte w;
            public Color color;
            public float radius;

            public LightSource(short x, short y, byte w, Color color, float radius)
            {
                position = new Position(x, y);
                this.w = w;
                this.color = color;
                this.radius = radius;
            }
        }

        private struct Wall
        {
            public bool isWall;
            public float absorption;

            public Wall(bool isWall, float absorption)
            {
                this.isWall = isWall;
                this.absorption = absorption;
            }
        }

        public static Stopwatch UpdateTimer { get; private set; }

        private static readonly object locker = new object();

        private static List<LightSource> lightSources = new();

        public static Color[,] colors = new Color[61, 31];
        private static Wall[,] wallsMap = new Wall[61, 31];

        private static int x;
        private static int y;
        private static int w;

        public static int width = 61;
        public static int height = 31;

        public static bool hackLight;

        public static Color GetSunLightColor(Color dayColor, Color nightColor)
        {
            Time time = ConsoleAdventure.world.time;
            Color color = Color.Black;

            if (time.hour >= 4 && time.hour <= 6)
            {
                int seconds = time.GetDaySeconds();
                float amount = ((float)(seconds - (3600 * 4)) / 3) / 3600f;
                color = Color.Lerp(nightColor, dayColor, amount);
            }

            else if (time.hour >= 20 && time.hour <= 22)
            {
                int seconds = time.GetDaySeconds();
                float amount = ((float)(seconds - (3600 * 20)) / 3) / 3600f;
                color = Color.Lerp(dayColor, nightColor, amount);
            }

            else if (time.hour > 6 && time.hour < 20)
                color = dayColor;

            else if (time.hour > 22 || time.hour < 4)
                color = nightColor;

            return color;
        }

        public static void Update(Position start, int w)
        {
            UpdateTimer = new Stopwatch();
            UpdateTimer.Start();

            x = start.x - 30;
            y = start.y - 15;

            Color color = Color.Black;

            WorldLevel level = ConsoleAdventure.world.levels.Levels[w];

            if (level.HasSun)
            {
                color = GetSunLightColor(level.SunLight, level.NightLight);
            }

            if (hackLight)
            {
                color = Color.White;
            }

            /*Light.Clear();
            StringPaint.Clear();

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
            }*/

            colors = new Color[width, height];
            wallsMap = new Wall[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (ConsoleAdventure.world.GetChunk(i + x, j + y, out int v1, out int v2) is LoadedChunk)
                    {
                        Field field = ConsoleAdventure.world.GetField(i + x, j + y, World.BlocksLayerId, w);

                        if (field != null)
                        {
                            int type = field.content?.type ?? 0;
                            bool? canBlockLight = Transform.IsLightingInteractable[type];

                            bool isWall = !canBlockLight.HasValue ? Transform.IsObstacle[type] : canBlockLight.Value;

                            wallsMap[i, j] = new Wall(isWall, Transform.LightAbsorption[type]); 
                        }
                    }
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 accumulatedLight = color.ToVector3();
                    
                    float intersectionsCount = 0;

                    for (int k = 0; k < lightSources.Count; k++)
                    {
                        LightSource source = lightSources[k];

                        if (source.w != w)
                            continue;

                        float distance = Vector2.DistanceSquared(new Vector2(i + x, j + y), source.position.ToVector2());
                        if (distance < source.radius)
                        {
                            float intensity = MathHelper.Clamp(1f - (distance / source.radius), 0, 1);

                            Vector3 lightColor = (source.color * intensity).ToVector3();

                            if (!accumulatedLight.Equals(lightColor))
                            {
                                if (LightBlock(i, j, w, source, out float count))
                                {
                                    lightColor *= 1f - count;

                                    if (lightColor.X > accumulatedLight.X)
                                        accumulatedLight.X = lightColor.X;

                                    if (lightColor.Y > accumulatedLight.Y)
                                        accumulatedLight.Y = lightColor.Y;

                                    if (lightColor.Z > accumulatedLight.Z)
                                        accumulatedLight.Z = lightColor.Z;

                                    intersectionsCount++;
                                }
                            }
                        }
                    }

                    colors[i, j] = (accumulatedLight).ToColor();
                }
            }

            UpdateTimer.Stop();
        }

        private static bool LightBlock(int x, int y, int w, LightSource lightSource, out float wallsCount)
        {
            try
            {
                float wallCounter = 0;

                int xA = Math.Clamp(lightSource.position.x - ConsoleAdventure.startDisplay.x, 0, 60);
                int yA = Math.Clamp(lightSource.position.y - ConsoleAdventure.startDisplay.y, 0, 30);

                int dx = Math.Abs(x - xA);
                int dy = Math.Abs(y - yA);

                int sx = xA < x ? 1 : -1;
                int sy = yA < y ? 1 : -1;

                int err = dx - dy;

                while (true)
                {
                    if (xA == x && yA == y) break;

                    if (wallsMap[xA, yA].isWall)
                    {
                        wallCounter += wallsMap[xA, yA].absorption;
                    }

                    if (wallCounter >= 1)
                    {
                        wallsCount = wallCounter;
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
                wallsCount = wallCounter;
                return true;
            }

            catch (Exception ex)
            {
                
            }

            wallsCount = 0;
            return false;
        }

        public static void Add(short x, short y, byte w, Color color, float radius = -1)
        {
            if(radius < 0)
                radius = (float)((color.R + color.G + color.B) / 3);

            lightSources.Add(new LightSource(x, y, w, color, radius));
        }

        public static void Add(Position position, byte w, Color color)
        {
            Add(position.x, position.y, w, color);
        }

        public static void Clear()
        {
            lightSources.Clear();
        }

        public static Color GetColor(Color color, Position position)
        {
            Position tryColorPos = new Position(Math.Clamp(position.x - ConsoleAdventure.startDisplay.x, 0, 60), Math.Clamp(position.y - ConsoleAdventure.startDisplay.y, 0, 30));
            Color result = (color.ToVector3() * colors[tryColorPos.x, tryColorPos.y].ToVector3()).ToColor();
            return result;
        }
    }
}
