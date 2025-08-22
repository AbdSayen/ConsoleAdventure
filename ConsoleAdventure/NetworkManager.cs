using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Networks;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static string pcId = string.Empty;

        private static Server server = null;

        private static int nextNetID;
        private static Dictionary<int, Entity> netIDMap = new Dictionary<int, Entity>();

        public enum ActionID
        {
            onPlayerConnected,
            onPlayerDisconnected,
            
            requestPlayersData,
            sendPlayerData,

            requestWorldData,
            sendWorldData,

            chatMessage,

            entitySpawned,
            entityKilled,
            entityMoved,
            entityAIvalChanged,
        }

        public static int SetNetID(Entity entity)
        {
            if (!ConsoleAdventure.world.isLoaded) return -1;
            ConsoleAdventure.logger.AddMessage(entity.GetType().Name + " added net id -> " + nextNetID.ToString());
            if (!netIDMap.ContainsKey(nextNetID))
                netIDMap.Add(nextNetID, entity);
            return nextNetID++;
        }

        public static Entity GetEntityFromNetID(int netID)
        {
            return netIDMap[netID];
        }

        public static byte[] GetPlayerDataBytes()
        {
            Dictionary<string, string> myDat = new Dictionary<string, string>
                {
                    {"name", Guid.NewGuid().ToString()},
                    {"pcId", pcId},
                    {"id", Id.ToString()}
                };
            return SerializeData.Serialize(myDat);
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

                byte[] serializedData = GetPlayerDataBytes();
                List<byte> dataToSend = new List<byte>();
                dataToSend.AddRange(BitConverter.GetBytes(serializedData.Length)); // 4 bytes
                dataToSend.AddRange(serializedData);
                await stream.WriteAsync(dataToSend.ToArray());
                await stream.FlushAsync();

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

        public static async Task SendMessage(ActionID act, NetPacket data, short id = -2, bool reverseOwner = false)
        {
            await SendMessage(act, new byte[0], data.GetBytes(), id, reverseOwner);
        }

        public static async void SendChatMessage(String txt, bool isMessage = true, short senderId = -2)
        {
            await SendMessage(ActionID.chatMessage, BitConverter.GetBytes(isMessage), Encoding.UTF8.GetBytes(txt), senderId);
        }

        public static async Task ImConnected()
        {
            await SendMessage(ActionID.onPlayerConnected, BitConverter.GetBytes(Id), GetPlayerDataBytes(), 0, true);
        }

        public static async Task ImDisconnected()
        {
            await SendMessage(ActionID.onPlayerDisconnected, BitConverter.GetBytes(Id), new byte[0], 0, true);
        }

        public static async Task RequestPlayersData()
        {
            await SendMessage(ActionID.requestPlayersData, new byte[0], new byte[0]);
        }

        public static async Task RequestWorldData()
        {
            ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("MProgress", "RequestingServerWorld");
            ConsoleAdventure.progressBar.Progress = 25 + (uint)ConsoleAdventure.rand.Next(0, 50);
            await SendMessage(ActionID.requestWorldData, new byte[0], new byte[0]);
        }

        public static async Task EntitySpawned(Entity entity)
        {
            NetPacket packet = new NetPacket();
            packet.WriteByte(entity.type);
            packet.WritePosition(entity.position);
            packet.WriteByte(entity.w);
            packet.WriteInt(entity.netID);
            await SendMessage(ActionID.entitySpawned, packet);
        }

        public static async Task EntityKilled(Entity entity)
        {
            NetPacket packet = new NetPacket();
            packet.WriteInt(entity.netID);
            await SendMessage(ActionID.entityKilled, packet);
        }
        
        public static async Task EntityMoved(Entity entity, int playerID = -1)
        {
            NetPacket packet = new NetPacket();
            bool isPlayer = playerID >= 0;
            if (isPlayer)
            {
                packet.WriteInt(playerID);
            }
            else
            {
                packet.WriteInt(entity.netID);
            }
            packet.WritePosition(entity.position);
            packet.WriteBool(isPlayer);
            await SendMessage(ActionID.entityMoved, packet);
        }

        public static async Task EntityAIvalChanged(Entity entity)
        {
            NetPacket packet = new NetPacket();
            for (int i = 0; i < 8; i++)
            {
                packet.WriteInt(entity.ai[i]);
            }
            packet.WriteInt(entity.netID);
            await SendMessage(ActionID.entityAIvalChanged, packet);
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
                     * [8] : 1 - reverse owner (byte) (bool)
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

                    NetPacket packet = new NetPacket(buffer);

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
                            if (connectedID == Id) break;
                            Dictionary<string, string> connectedPlayerData = SerializeData.Deserialize<Dictionary<string, string>>(buffer);
                            
                            SendChatMessage(connectedID.ToString() + " has been connected", false, 255);
                            ConsoleAdventure.world.ConnectPlayer(connectedID, "");
                            ConsoleAdventure.world.players[connectedID].LoadPlayerInfo(connectedPlayerData);
                            break;
                        case ActionID.onPlayerDisconnected:
                            short disconnectedID = BitConverter.ToInt16(dat, headerIdx);
                            SendChatMessage(disconnectedID.ToString() + " has been disconnected", false, disconnectedID);
                            ConsoleAdventure.world.DisconnectPlayer(disconnectedID);
                            break;
                        case ActionID.requestPlayersData:
                            if (!isHost) return;
                            List<Dictionary<string, string>> serverPlayersDatas = new List<Dictionary<string, string>>();
                            for (int i = 0; i < server.clients.Count; i++)
                            {
                                serverPlayersDatas.Add(server.clients[i].playerData);
                            }
                            await SendMessage(ActionID.sendPlayerData, new byte[0], SerializeData.Serialize(serverPlayersDatas), senderId, true);
                            break;
                        case ActionID.sendPlayerData:
                            List<Dictionary<string, string>> receivedPlayersDatas = SerializeData.Deserialize<List<Dictionary<string, string>>>(buffer);
                            for (int i = 0; i < receivedPlayersDatas.Count; i++)
                            {
                                short curId = Int16.Parse(receivedPlayersDatas[i]["id"]);
                                if (curId == Id) continue;
                                Loger.AddLog("Creating Player -> " + curId.ToString());
                                ConsoleAdventure.world.ConnectPlayer(curId, "");
                                ConsoleAdventure.world.players[curId].LoadPlayerInfo(receivedPlayersDatas[i]);
                            }
                            break;
                        case ActionID.requestWorldData:
                            if (!isHost) return;
                            Loger.AddLog("Sending world to " + senderId.ToString());
                            await SendMessage(ActionID.sendWorldData, new byte[0], WorldIO.GetWorldBytes(), senderId, true);
                            break;
                        case ActionID.sendWorldData:
                            WorldIO.LoadWorldFromPackedBytes(buffer);
                            break;
                        case ActionID.entitySpawned:
                            int entityNetID = packet.ReadInt();
                            byte entityW = packet.ReadByte();
                            Position entityPosition = packet.ReadPosition();
                            byte entityType = packet.ReadByte();

                            nextNetID = entityNetID;
                            Entity entity = (Entity)Activator.CreateInstance(Entity.TypeMapping[entityType], new object[] { entityPosition, entityW, null });
                            Spawner.Spawn(entity);
                            break;
                        case ActionID.entityKilled:
                            int entitykNetID = packet.ReadInt();
                            GetEntityFromNetID(entitykNetID).Kill();
                            break;
                        case ActionID.entityMoved:
                            bool entitymPlayer = packet.ReadBool();
                            Position entitymPosition = packet.ReadPosition();
                            int entitymNetID = packet.ReadInt();
                            if (!entitymPlayer)
                            {
                                GetEntityFromNetID(entitymNetID).SetPosition(entitymPosition);
                            }
                            else
                            {
                                ConsoleAdventure.world.players[(short)entitymNetID].SetPosition(entitymPosition);
                            }
                            break;
                        case ActionID.entityAIvalChanged:
                            int entityaNetID = packet.ReadInt();
                            Entity entitya = GetEntityFromNetID(entityaNetID);
                            for (int i = 7; i >= 0; i--)
                            {
                                entitya.ai[i] = packet.ReadInt();
                            }
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
