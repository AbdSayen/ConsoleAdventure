using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Debug.Commands;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleAdventure
{
    public enum NetworkDataType
    {
        vec2_,
        short_,
        bytes_
    }

    public enum NetworkFuncType
    {
        setPlayerPos,
        sendPlayerConnectedId,
        setPlayerW,
        sendPlayerDisconnectedId,
        breakTransform,
        buildTransform,
        pickUpItem,
        sendCommand,
        sendWorld,
        worldRequest
    }

    public static class NetworkManager
    {
        private static string host = "127.0.0.1";//"26.8.244.156";
        private static int port = 2454;
        private static TcpClient client = new TcpClient();

        private static StreamReader? Reader = null;
        private static StreamWriter? Writer = null;
        private static NetworkStream stream = null;

        public static short Id = -1;

        private static Position dropItemPos;
        private static short dropItemDeep;
        private static short dropItemPlayerId;

        public static async Task<bool> ConnectClient()
        {
            try
            {
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("MProgress", "Connecting");
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream();
                Reader = new StreamReader(stream);
                Writer = new StreamWriter(stream);
                if (Writer is null || Reader is null) return false;
                await Writer.WriteLineAsync(Guid.NewGuid().ToString()); // name
                await Writer.FlushAsync();
                byte[] buffer = new byte[2];
                await stream.ReadAsync(buffer, 0, 2);
                Id = BitConverter.ToInt16(buffer);

                for (int i = Id - 1; i >= 0; i--)
                {
                    ConsoleAdventure.world.ConnectPlayer((short)i);
                }
                await SendDataAsync(NetworkFuncType.sendPlayerConnectedId, Id);
                Task.Run(() => ReceiveMainDataAsync());
                await SendDataAsync(NetworkFuncType.worldRequest, Id);
            }
            catch (SocketException) {
                ConsoleAdventure.menu.serverNotFoundTimer = 90;
                ConsoleAdventure.InWorld = false;
                ConsoleAdventure.menu.CloseAllPages();
                ConsoleAdventure.world = null;
                return false;
            }

            return true;
        }

        public static void DisconectClient()
        {
            if (client.Client.Connected)
            {
                SendDataAsync(NetworkFuncType.sendPlayerDisconnectedId, Id);

                if (Id == 0)
                    stream.Write(BitConverter.GetBytes(254));
                else
                    stream.Write(BitConverter.GetBytes(255));
                stream.Flush();

                stream.Close();
                Reader.Close();
                Writer.Close();
                client.Client.Close();
                client.Close();
                client = new TcpClient();
            }
        }

        public static async Task SendDataAsync(NetworkFuncType type, Position pos, short a = 0, short b = 0)
        {
            if (Id == -1) return;
            List<byte> dat = new List<byte>();
            dat.AddRange(BitConverter.GetBytes((short)NetworkDataType.vec2_));
            dat.AddRange(BitConverter.GetBytes((short)type));

            dat.AddRange(BitConverter.GetBytes((short)pos.x));
            dat.AddRange(BitConverter.GetBytes((short)pos.y));

            dat.AddRange(BitConverter.GetBytes((short)a));
            dat.AddRange(BitConverter.GetBytes((short)b));

            await stream.WriteAsync(dat.ToArray());
            await stream.FlushAsync();
        }

        public static async Task SendDataAsync(NetworkFuncType type, short num, short a = 0, short b = 0, short c = 0)
        {
            if (Id == -1) return;
            List<byte> dat = new List<byte>();
            dat.AddRange(BitConverter.GetBytes((short)NetworkDataType.short_));
            dat.AddRange(BitConverter.GetBytes((short)type));

            dat.AddRange(BitConverter.GetBytes((short)num));
            dat.AddRange(BitConverter.GetBytes((short)a));

            dat.AddRange(BitConverter.GetBytes((short)b));
            dat.AddRange(BitConverter.GetBytes((short)c));

            await stream.WriteAsync(dat.ToArray());
            await stream.FlushAsync();
        }

        public static async Task SendDataAsync(NetworkFuncType type, byte[] data, short a = 0, short b = 0)
        {
            if (Id == -1) return;
            List<byte> dat = new List<byte>();
            dat.AddRange(BitConverter.GetBytes((short)NetworkDataType.bytes_));
            dat.AddRange(BitConverter.GetBytes((short)type));

            int dataLength = data.Length;
            if (dataLength < 6) dataLength = 6;
            dat.AddRange(BitConverter.GetBytes(dataLength));

            dat.AddRange(BitConverter.GetBytes((short)a));
            dat.AddRange(BitConverter.GetBytes((short)b));

            dat.AddRange(data);

            if (data.Length < 6)
                dat.AddRange(new byte[6 - data.Length]);

            Console.WriteLine(dat);

            await stream.WriteAsync(dat.ToArray());
            await stream.FlushAsync();
        }

        public static async Task SendDataAsync(NetworkFuncType type, string data, short a = 0, short b = 0)
        {
            await SendDataAsync(type, Encoding.UTF8.GetBytes(data), a, b);
        }

        public static async Task ReceiveMainDataAsync()
        {
            while (true)
            {
                try
                {
                    byte[] dat = new byte[12];
                    int bytes = await stream.ReadAsync(dat, 0, 12);

                    if (dat[0] == 255)
                    {
                        DisconectClient();
                        ConsoleAdventure.InWorld = false;
                        ConsoleAdventure.menu.CloseAllPages();
                        break;
                    }

                    NetworkDataType dataType = (NetworkDataType)BitConverter.ToInt16(dat, 0);
                    NetworkFuncType type = (NetworkFuncType)BitConverter.ToInt16(dat, 2);

                    Position pos;
                    short a, b;

                    switch (dataType)
                    {
                        case NetworkDataType.vec2_:
                            pos = new Position(BitConverter.ToInt16(dat, 4), BitConverter.ToInt16(dat, 6));
                            a = BitConverter.ToInt16(dat, 8);
                            b = BitConverter.ToInt16(dat, 10);
                            ReceiveDataAsync(type, pos, a, b);
                            break;
                        case NetworkDataType.short_:
                            ReceiveDataAsync(type, BitConverter.ToInt16(dat, 4), BitConverter.ToInt16(dat, 6), BitConverter.ToInt16(dat, 8), BitConverter.ToInt16(dat, 10));
                            break;
                        case NetworkDataType.bytes_:
                            int dataLength = BitConverter.ToInt32(dat, 4);
                            a = BitConverter.ToInt16(dat, 8);
                            b = BitConverter.ToInt16(dat, 10);
                            byte[] data = new byte[dataLength];
                            bytes = await stream.ReadAsync(data, 0, dataLength);
                            ReceiveDataAsync(type, data, a, b);
                            break;
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        private static void ReceiveDataAsync(NetworkFuncType type, Position pos, short a, short b)
        {
            switch (type)
            {
                case NetworkFuncType.setPlayerPos:
                    ConsoleAdventure.world.players[a].SetPosition(pos);
                    break;
                case NetworkFuncType.breakTransform:
                    Transform t = ConsoleAdventure.world.GetField(pos.x, pos.y, World.BlocksLayerId, a).content;
                    if (t != null)
                    {
                        if (t.CanBeDestroyed())
                        {
                            ConsoleAdventure.world.RemoveSubject(t, World.BlocksLayerId);
                            ConsoleAdventure.world.time.PassTime(60);
                        }
                    }
                    break;
                case NetworkFuncType.buildTransform:
                    Inventory inv = ConsoleAdventure.world.players[b].inventory;
                    if (inv.HasItems(new Log(), 1))
                    {
                        new Plank(pos, a);
                        inv.RemoveItems(new Log(), 1);
                        ConsoleAdventure.world.time.PassTime(120);
                    }
                    break;
            }
        }

        private static void ReceiveDataAsync(NetworkFuncType type, short num, short a = 0, short b = 0, short c = 0)
        {
            switch (type)
            {
                case NetworkFuncType.sendPlayerConnectedId:
                    ConsoleAdventure.world.ConnectPlayer(num);
                    break;
                case NetworkFuncType.sendPlayerDisconnectedId:
                    ConsoleAdventure.world.DisconnectPlayer(num);
                    break;
                case NetworkFuncType.setPlayerW:
                    Player pl = ConsoleAdventure.world.players[a];
                    pl.SetPosition(pl.position, num);
                    break;
                case NetworkFuncType.pickUpItem:
                    ConsoleAdventure.world.players[num].TryPickUp();
                    break;
                case NetworkFuncType.worldRequest:
                    if (Id == 0)
                        SendDataAsync(NetworkFuncType.sendWorld, WorldIO.GetWorldBytes(), num);
                    break;
            }
        }

        private static void ReceiveDataAsync(NetworkFuncType type, byte[] data, short a = 0, short b = 0)
        {
            switch (type)
            {
                case NetworkFuncType.sendCommand:
                    string command = Encoding.UTF8.GetString(data);
                    Command.Find(command, a);
                    break;
                case NetworkFuncType.sendWorld:
                    if (a == Id)
                    {
                        WorldIO.SetWorldFromBytes(data);
                        WorldIO.LoadTags();
                    }
                    break;
            }
        }
    }
}
