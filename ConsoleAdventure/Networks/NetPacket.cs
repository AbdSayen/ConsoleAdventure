using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleAdventure.Networks
{
    public class NetPacket
    {
        private List<byte> data = new();

        public NetPacket() { }
        public NetPacket(List<byte> data)
        {
            this.data = data;
        }
        public NetPacket(byte[] data)
        {
            this.data = data.ToList();
        }

        private byte[] PickLastBytes(int count)
        {
            List<byte> bytes = data.GetRange(data.Count - count, count);
            data.RemoveRange(data.Count - count, count);
            return bytes.ToArray();
        }

        public List<byte> GetBytesList()
        {
            return data;
        }

        public byte[] GetBytes()
        {
            return data.ToArray();
        }

        public void WriteByte(byte n)
        {
            data.Add(n);
        }

        public byte ReadByte()
        {
            byte n;
            n = data.Last();
            data.RemoveAt(data.Count - 1);
            return n;
        }

        public void WriteShort(short n)
        {
            data.AddRange(BitConverter.GetBytes(n));
        }

        public short ReadShort()
        {
            return BitConverter.ToInt16(PickLastBytes(2), 0);
        }

        public void WriteInt(int n)
        {
            data.AddRange(BitConverter.GetBytes(n));
        }

        public int ReadInt()
        {
            return BitConverter.ToInt32(PickLastBytes(4), 0);
        }

        public void WriteLong(long n)
        {
            data.AddRange(BitConverter.GetBytes(n));
        }

        public long ReadLong()
        {
            return BitConverter.ToInt64(PickLastBytes(8), 0);
        }

        public void WriteFloat(float n)
        {
            data.AddRange(BitConverter.GetBytes(n));
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(PickLastBytes(4), 0);
        }

        public void WriteString(string n)
        {
            byte[] dat = Encoding.UTF8.GetBytes(n);
            data.AddRange(dat);
            WriteInt(dat.Length);
        }

        public string ReadString()
        {
            return Encoding.UTF8.GetString(PickLastBytes(ReadInt()));
        }

        public void WritePosition(Position n)
        {
            WriteInt(n.x);
            WriteInt(n.y);
        }

        public Position ReadPosition()
        {
            int x, y;
            y = ReadInt();
            x = ReadInt();
            return new Position(x, y);
        }

        public void WriteBool(bool n)
        {
            data.AddRange(BitConverter.GetBytes(n));
        }

        public bool ReadBool()
        {
            return BitConverter.ToBoolean(PickLastBytes(1), 0);
        }
    }
}
