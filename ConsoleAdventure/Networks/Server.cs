using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using ConsoleAdventure.Settings;

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
            ConsoleAdventure.logger.AddMessage($"SERVER: client {client?.playerData["name"]} disconnected");
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
        protected internal async Task BroadcastDataAsync(byte[] dat, short id, bool reverseOwner = false)
        {
            string s_ = "[ ";
            for (int i = 0; i < dat.Length; i++)
                s_ += dat[i].ToString() + ", ";
            s_ += "]    '" + Encoding.UTF8.GetString(dat).Replace("\a", "alert") + "'";
            ConsoleAdventure.logger.AddMessage($"SERVER: Command: ({dat.Length})\n    " + s_); // {BitConverter.ToInt16(dat, 0)}, {BitConverter.ToInt32(dat, 2)}, {BitConverter.ToInt16(dat, 6)}\n    
            for (int i = 0; i < clients.Count; i++)
            {
                ClientObject client = clients[i];
                bool isSend = false;

                if (reverseOwner)
                    isSend = client.Id == id;
                else
                    isSend = client.Id != id;

                if (isSend)
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
