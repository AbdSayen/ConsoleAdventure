using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;

namespace ConsoleAdventure.Networks
{
    public class Server
    {
        public TcpListener tcpListener = new TcpListener(IPAddress.Any, 2454);
        public List<ClientObject> clients = new List<ClientObject>();

        public ClientObject? hostClient;

        private short currentClientsIds = -1;

        protected internal void RemoveConnection(short id)
        {
            ClientObject? client = clients.FirstOrDefault(c => c.Id == id);
            if (client != null) clients.Remove(client);
            ConsoleAdventure.logger.AddMessage($"SERVER: client {client?.userName} disconnected");
            client?.Close();
        }

        public async Task Start()
        {
            await ListenAsync();
        }

        protected internal async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                ConsoleAdventure.logger.AddMessage("Server started. Waiting connections...");

                while (true)
                {
                    currentClientsIds++;
                    TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                    ClientObject clientObject = new ClientObject(tcpClient, this, currentClientsIds);
                    clients.Add(clientObject);
                    Task.Run(clientObject.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }
        protected internal async Task BroadcastDataAsync(byte[] dat, short id)
        {
            //Console.WriteLine($"Command: ({dat.Length})  {BitConverter.ToInt16(dat, 0)}, {BitConverter.ToInt16(dat, 2)}\n  {BitConverter.ToInt16(dat, 4)}");
            for (int i = 0; i < clients.Count; i++)
            {
                ClientObject client = clients[i];
                if (client.Id != id)
                {
                    await client.stream.WriteAsync(dat, 0, dat.Length);
                    await client.stream.FlushAsync();
                }
            }
        }

        protected internal void Disconnect()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close();
            }
            tcpListener.Stop();
        }
    }
}
