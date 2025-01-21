using ConsoleAdventure.Networks;
using ConsoleAdventure.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public static string pcId = string.Empty;

        private static Server server = null;

        public enum ActionID
        {
            chatMessage,

            onPlayerConnected,
            onPlayerDisconnected,
            
            requestPlayersData,
            sendPlayerData,
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

                await Writer.WriteLineAsync(Guid.NewGuid().ToString() + "|_|" + pcId); // name
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

        public static async void DisconectClient()
        {
            await ImDisconnected();

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

        public static async Task SendMessage(ActionID act, byte[] header, byte[] buffer, short id = -2, bool reverseOwner = false)
        {
            if (Id == -1) return;

            if (id == -2)
            {
                id = Id;
            }

            List<byte> dat = new List<byte>();

            dat.AddRange(BitConverter.GetBytes((short)act)); // 2
            dat.AddRange(BitConverter.GetBytes(buffer.Length)); // 4    2 + 4 = 6
            dat.AddRange(BitConverter.GetBytes(id)); // 2    6 + 2 = 8
            dat.AddRange(BitConverter.GetBytes(reverseOwner)); // 1     8 + 1 = 9   16 - 9 = 7

            dat.AddRange(header);
            dat.AddRange(new byte[7 - header.Length]); // 16 bytes header
            

            dat.AddRange(buffer);

            await stream.WriteAsync(dat.ToArray());
            await stream.FlushAsync();
        }

        public static async void SendChatMessage(String txt, bool isMessage = true, short senderId = -2)
        {
            await SendMessage(ActionID.chatMessage, BitConverter.GetBytes(isMessage), Encoding.UTF8.GetBytes(txt), senderId);
        }

        public static async Task ImConnected()
        {
            await SendMessage(ActionID.onPlayerConnected, BitConverter.GetBytes(Id), new byte[0], 0, true);
        }

        public static async Task ImDisconnected()
        {
            await SendMessage(ActionID.onPlayerDisconnected, BitConverter.GetBytes(Id), new byte[0], 0, true);
        }

        public static async Task RequestPlayersData()
        {
            await SendMessage(ActionID.requestPlayersData, new byte[0], new byte[0]);
        }

        public static async Task ReceiveMainDataAsync()
        {
            ConsoleAdventure.logger.AddMessage("Started client listener cycle!");

            while (client.Connected)
            {
                try
                {
                    byte[] dat = new byte[16];
                    /*
                     * [0] : 2 - ActionID (short)
                     * [2] : 4 - next packet data size (int32)
                     * [6] : 2 - sender id (short)
                    */
                    int bytes = await stream.ReadAsync(dat, 0, 16);

                    short act = BitConverter.ToInt16(dat, 0);
                    int nextDatSize = BitConverter.ToInt32(dat, 2);
                    short senderId = BitConverter.ToInt16(dat, 6);

                    int headerIdx = 9;

                    byte[] buffer = new byte[nextDatSize];
                    if (nextDatSize > 0)
                    {
                        bytes = await stream.ReadAsync(buffer, 0, nextDatSize);
                    }

                    switch ((ActionID)act)
                    {
                        case ActionID.chatMessage:
                            string pre_ = "";
                            if (BitConverter.ToBoolean(dat, headerIdx)) {
                                pre_ = senderId.ToString() + ": ";
                            }
                            Loger.AddLog(Utils.StringMaxLengthOnLine(pre_ + Encoding.UTF8.GetString(buffer), 24));
                            break;
                        case ActionID.onPlayerConnected:
                            short connectedID = BitConverter.ToInt16(dat, headerIdx);
                            SendChatMessage(connectedID.ToString() + " has been connected", false, 255);
                            break;
                        case ActionID.onPlayerDisconnected:
                            short disconnectedID = BitConverter.ToInt16(dat, headerIdx);
                            SendChatMessage(disconnectedID.ToString() + " has been disconnected", false, disconnectedID);
                            break;
                        case ActionID.requestPlayersData:
                            if (!isHost) return;
                            await SendMessage(ActionID.sendPlayerData, new byte[0], Encoding.UTF8.GetBytes("Players Data from server translation"), senderId, true);
                            break;
                        case ActionID.sendPlayerData:
                            Loger.AddLog(Encoding.UTF8.GetString(buffer));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleAdventure.logger.AddException(ex);
                    Loger.AddLog("[Exception, please check Logs]");
                    break;
                }
            }
        }
    }
}
