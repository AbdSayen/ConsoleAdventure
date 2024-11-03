using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Debug.Commands;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.Settings;
using ConsoleAdventure.Networks;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using SharpDX.MediaFoundation;
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
    public static class NetworkManager
    {
        private static string host = "127.0.0.1";
        private static int port = 2454;
        private static TcpClient client = new TcpClient();

        private static StreamReader Reader = null;
        private static StreamWriter Writer = null;
        private static NetworkStream stream = null;

        public static short Id = -1;
        public static bool isHost = false;

        private static Position dropItemPos;
        private static short dropItemDeep;
        private static short dropItemPlayerId;

        public static string pcId = string.Empty;

        private static Server server = null;

        public enum RequestTypes
        {
            chatMessage
        }

        public static async Task<bool> ConnectClient()
        {
            if (isHost)
            {
                HostClient();
            }
            ConsoleAdventure.logger.AddMessage("Connecting to server...");
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
                Id = BitConverter.ToInt16(buffer, 0);

                ConsoleAdventure.logger.AddMessage("Connected w ID: " + Id.ToString());

                Task.Run(() => ReceiveMainDataAsync());
            }
            catch (SocketException)
            {
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
            if (isHost)
            {
                server.Disconnect();
            }

            if (client.Client.Connected)
            {
                stream.Close();
                Reader.Close();
                Writer.Close();
                client.Client.Close();
                client.Close();
                client = new TcpClient();
            }
        }

        public static void HostClient()
        {
            ConsoleAdventure.logger.AddMessage("Starting server...");

            server = new Server();
            Task.Run(server.Start);

            ConsoleAdventure.logger.AddMessage("Starting server...OK");
        }

        public static async void SendMessage(RequestTypes rt, byte[] buffer)
        {
            List<byte> dat = new List<byte>();

            dat.AddRange(BitConverter.GetBytes((short)rt)); // 2
            dat.AddRange(BitConverter.GetBytes(buffer.Length)); // 4    2 + 4 = 6       16 - 6 = 10
            dat.AddRange(new byte[10]); // 16 bytes header

            await stream.WriteAsync(dat.ToArray());
            await stream.FlushAsync();

            await stream.WriteAsync(buffer); // other data
            await stream.FlushAsync();
        }

        public static async Task ReceiveMainDataAsync()
        {
            ConsoleAdventure.logger.AddMessage("Started client listener cycle");

            while (true)
            {
                try
                {
                    byte[] dat = new byte[16];
                    /*
                     * [0] : 2 - requestType(short)
                     * [2] : 4 - next packet data size (int32)
                     * [6] : 2 - sender id
                    */
                    int bytes = await stream.ReadAsync(dat, 0, 16);

                    short rt = BitConverter.ToInt16(dat, 0);
                    int nextDatSize = BitConverter.ToInt32(dat, 2);
                    short senderId = BitConverter.ToInt16(dat, 6);

                    ConsoleAdventure.logger.AddMessage($"Getting Request: request info -> nextDatSize : {nextDatSize}, rt : {rt}");

                    byte[] buffer = new byte[nextDatSize];
                    if (nextDatSize > 0)
                    {
                        bytes = await stream.ReadAsync(buffer, 0, nextDatSize);
                    }

                    switch (rt)
                    {
                        case (short)RequestTypes.chatMessage:
                            Loger.AddLog(senderId.ToString() + ": " + Encoding.UTF8.GetString(buffer));
                            return;
                    }
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
