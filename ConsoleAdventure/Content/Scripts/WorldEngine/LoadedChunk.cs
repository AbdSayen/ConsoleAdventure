using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class LoadedChunk : Chunk
    {
        private readonly Field[,,,] fields;

        public bool IsPlayer { get; internal set; }
        public int OherUpdCount { get; internal set; }

        public LoadedChunk()
        {
            fields = InitializeFields();
        }

        private Field[,,,] InitializeFields()
        {
            var initializedFields = new Field[Size, Size, World.CountOfLayers, maxDeep];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < World.CountOfLayers; k++)
                    {
                        for (int l = 0; l < maxDeep; l++)
                        {
                            initializedFields[i, j, k, l] = new Field();
                        }
                    }
                }
            }

            return initializedFields;
        }

        public override Field GetField(int x, int y, int z, int w)
        {
            if (IsValidCoordinate(x, y, z, w))
            {
                Field field = fields[x, y, z, w];
                if (field == null)
                {
                    field = new Field();
                }
                return field;
            }
            return null;
        }

        public override void SetField(int x, int y, int layer, int w, Field field)
        {
            if (IsValidCoordinate(x, y, layer, w))
            {
                fields[x, y, layer, w] = field;
            }
        }

        public override Field[,,,] GetFields()
        {
            return fields;
        }
    }
}