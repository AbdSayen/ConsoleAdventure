using ConsoleAdventure.Content.Scripts.WorldEngine.ChunkManagement.Tasks;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.WorldEngine.Generate;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.ChunkManagement
{
    public class ChunkManager
    {
        #region Threed

        private static readonly object locking = new object();

        private static bool ThreadState { get; set; }

        private static Thread Thread { get; set; }

        private static readonly AutoResetEvent waitEvent = new(false);

        #endregion

        public static World world; 

        public static Dictionary<Position, int> InProgress { get; private set; } = new();

        /// <summary>
        /// Очередь выполнения задач, каждый запрос идёт сюда и ждёт выполнения.
        /// </summary>
        public static Queue<IChunkTask> TaskQueue { get; private set; } = new();

        public static void Start(World world)
        {
            ChunkManager.world = world;

            lock (locking)
                TaskQueue.Clear();

            ThreadState = true;

            waitEvent.Set();

            Thread = new Thread(() =>
            {
                while (ThreadState)
                {
                    Update();
                }
            });

            Thread.Start();
        }

        public static void End()
        {
            ThreadState = false;
        }

        private static void Update()
        {
            try
            {
                IChunkTask task = null;

                lock (locking)
                {
                    if (TaskQueue == null)
                        throw new NullReferenceException();
                    if (TaskQueue.Count <= 0)
                    {
                        //waitEvent.WaitOne();
                        return;
                    }

                    task = TaskQueue.Dequeue();
                }

                if (task != null)
                    task.Execute();
            }

            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Добавляет задание в очередь выполнения.
        /// </summary>
        /// <param name="task">задание, которое надо выполнить с чанком по позиции: <seealso cref="IChunkTask.Chunk"/></param>
        public static void Request(IChunkTask task)
        {
            lock (locking)
                TaskQueue.Enqueue(task);

            waitEvent.Set();
        }

        /// <summary>
        /// Добавляет задание "Загрузить чанк" в очередь выполнения.
        /// </summary>
        /// <param name="position">координата чанка</param>
        public static void RequestLoad(Position position)
        {
            IChunkTask task = new LoadChunkTask();
            task.Chunk = position;

            Request(task);
        }

        /// <summary>
        /// Добавляет задание "Выгрузить чанк" в очередь выполнения
        /// </summary>
        /// <param name="position">координата чанка</param>
        public static void RequestUnload(Position position)
        {
            IChunkTask task = new UnloadChunkTask();
            task.Chunk = position;

            Request(task);
        }

        #region ChunkManipulations

        public static void LoadChunk(int xChunk, int yChunk, bool playerArea)
        {
            int chunkStartX = xChunk * Chunk.Size;
            int chunkStartY = yChunk * Chunk.Size;

            Chunk chunk = world.chunks[xChunk, yChunk];

            if (chunk != null)
            {
                if (chunk is UnloadedChunk)
                {
                    UnloadedChunk uchunk = (UnloadedChunk)chunk;
                    short[,,,] types = uchunk.fields;
                    List<TransformDataInChunk> data = uchunk.data;
                    List<TransformMaterialInChunk> materials = uchunk.materials;

                    lock (world.chunks[xChunk, yChunk].Locking)
                    {
                        world.chunks[xChunk, yChunk] = new LoadedChunk() { IsUpdated = true };
                    }

                    for (int w = 0; w < Chunk.maxDeep; w++)
                    {
                        for (int x = 0; x < Chunk.Size; x++)
                        {
                            for (int y = 0; y < Chunk.Size; y++)
                            {
                                for (int z = 0; z < 3; z++)
                                {
                                    int X = x + chunkStartX;
                                    int Y = y + chunkStartY;

                                    lock (world.chunks[xChunk, yChunk].Locking)
                                    {
                                        Transform.SetObject(types[x, y, z, w], new(X, Y), w, z);
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < data.Count; i++)
                    {
                        lock (world.chunks[xChunk, yChunk].Locking)
                        {
                            var currentData = data[i];

                            Transform transform = world.GetField(currentData.position.x, currentData.position.y, currentData.z, currentData.w)?.content;

                            if (transform != null)
                            {
                                transform.LoadData(currentData.data);
                            }
                        }
                    }

                    for (int i = 0; i < materials.Count; i++)
                    {
                        lock (world.chunks[xChunk, yChunk].Locking)
                        {
                            var currentMaterial = materials[i];

                            Transform transform = world.GetField(currentMaterial.position.x, currentMaterial.position.y, currentMaterial.z, currentMaterial.w)?.content;

                            if (transform != null)
                            {
                                transform.ApplyMaterial(currentMaterial.material);
                            }
                        }
                    }
                }
            }

            else
            {
                GenerateChunk(xChunk, yChunk);
            }

            for (short i = 0; i < world.players.Count; i++)
            {
                if (i == NetworkManager.Id) continue;
                if (world.players[i].position.x > chunkStartX && world.players[i].position.x < (chunkStartX + Chunk.Size) && world.players[i].position.y > chunkStartY && world.players[i].position.y < (chunkStartY + Chunk.Size))
                {
                    world.players[i].SetPosition(world.players[i].position);
                }
            }
        }

        public static void UnloadChunk(int xChunk, int yChunk)
        {
            Chunk chunk = world.chunks[xChunk, yChunk];

            if (chunk != null)
            {
                if (chunk is LoadedChunk)
                {
                    if (chunk.IsUpdated)
                    {
                        LoadedChunk lchunk = (LoadedChunk)chunk;
                        Field[,,,] fields = lchunk.GetFields();

                        lock (world.chunks[xChunk, yChunk].Locking)
                        {
                            world.chunks[xChunk, yChunk] = new UnloadedChunk() { IsUpdated = true };
                        }

                        for (int w = 0; w < Chunk.maxDeep; w++)
                        {
                            for (int x = 0; x < Chunk.Size; x++)
                            {
                                for (int y = 0; y < Chunk.Size; y++)
                                {
                                    for (int z = 0; z < 3; z++)
                                    {
                                        short? type = (short?)fields[x, y, z, w]?.content?.type;

                                        if (!type.HasValue)
                                            type = 0;

                                        ((UnloadedChunk)world.chunks[xChunk, yChunk]).fields[x, y, z, w] = type.Value;

                                        object data = fields[x, y, z, w]?.content?.SaveData();
                                        int? material = fields[x, y, z, w]?.content?.material;

                                        short X = (short)(x + xChunk * Chunk.Size);
                                        short Y = (short)(y + yChunk * Chunk.Size);

                                        if (data != null)
                                            ((UnloadedChunk)world.chunks[xChunk, yChunk]).data.Add(new(X, Y, (byte)z, (byte)w, data));

                                        if (material.HasValue && material != -1)
                                            ((UnloadedChunk)world.chunks[xChunk, yChunk]).materials.Add(new(X, Y, (byte)z, (byte)w, material.Value));
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        world.chunks[xChunk, yChunk] = null;
                    }
                }
            }
        }

        public static void GenerateChunk(int x, int y)
        {
            world.chunks[x, y] = new LoadedChunk();

            world.generator.Generate(world, new Position(x, y));

            if (world.chunks[x, y] != null)
                world.chunks[x, y].IsUpdated = false;
        }

        #endregion
    }
}
