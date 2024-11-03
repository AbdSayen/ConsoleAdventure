using ConsoleAdventure.Networks;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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

        public string userName;

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
            userName = await Reader.ReadLineAsync();
            try
            {
                string _msg = "";
                if (Id == 0) _msg = "as HOST";
                ConsoleAdventure.logger.AddMessage($"SERVER: client {userName} connected {_msg}");
                await stream.WriteAsync(BitConverter.GetBytes(Id));
                await stream.FlushAsync();

                while (true)
                {
                    byte[] dat = new byte[16];
                    int bytes = await stream.ReadAsync(dat, 0, 16);

                    await server.BroadcastDataAsync(dat, Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
