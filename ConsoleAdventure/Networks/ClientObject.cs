using ConsoleAdventure.Networks;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Settings;

namespace ConsoleAdventure.Networks
{
    public class ClientObject
    {
        protected internal short Id { get; } = -1;
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }
        protected internal NetworkStream stream { get; }

        private TcpClient client;
        private Server server;

        public Dictionary<string, string> playerData = new Dictionary<string, string>();

        private int[] position = new int[2];

        public ClientObject(TcpClient tcpClient, Server serverObject, short id)
        {
            client = tcpClient;
            server = serverObject;
            stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
            Id = id;
            if (id == 0) server.hostClient = this;
        }

        public async Task ProcessAsync()
        {
            byte[] lengthBuffer = new byte[4];
            await stream.ReadAsync(lengthBuffer, 0, 4);
            int dataLengthToReceive = BitConverter.ToInt32(lengthBuffer, 0);
            byte[] playerDataBuffer = new byte[dataLengthToReceive];
            await stream.ReadAsync(playerDataBuffer, 0, dataLengthToReceive);

            playerData = SerializeData.Deserialize<Dictionary<string, string>>(playerDataBuffer);
            playerData["id"] = Id.ToString();

            try
            {
                string _msg = "";
                if (Id == 0) _msg = "as HOST";
                ConsoleAdventure.logger.AddMessage($"SERVER: client {playerData["name"]} connected {_msg}");

                await stream.WriteAsync(BitConverter.GetBytes(Id));
                await stream.FlushAsync();

                while (true)
                {
                    byte[] dat = new byte[16];
                    int bytes = await stream.ReadAsync(dat, 0, 16);

                    short id_ = BitConverter.ToInt16(dat, 6);

                    bool reverseOwner = BitConverter.ToBoolean(dat, 8);

                    await server.BroadcastDataAsync(dat, id_, reverseOwner);

                    int additionDataLength = BitConverter.ToInt32(dat, 2);

                    if (additionDataLength > 0)
                    {
                        byte[] buffer = new byte[additionDataLength];
                        await stream.ReadAsync(buffer, 0, additionDataLength);

                        await server.BroadcastDataAsync(buffer, id_, reverseOwner);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(Id);
            }
        }
        protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            stream.Close();
            client.Close();
        }
    }
}
