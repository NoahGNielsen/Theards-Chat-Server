using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class BroadcastServerThreaded
{
    static List<TcpClient> clients = new List<TcpClient>();
    static readonly object lockObject = new object();

    static void Main()
    {
        IPAddress serverIP = IPAddress.Any;

        TcpListener server = new TcpListener(serverIP, 5000);
        server.Start();
        Console.WriteLine($"Server ip {serverIP}:{5000}");

        while (true)
        {
            try
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"Clint connected.");

                lock (lockObject)
                {
                    clients.Add(client);
                }

                //starts a new thread for each client, to avoid DOS
                Thread t = new Thread(() => HandleClient(client));
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }

    static void HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            // This loop keeps running as long as the client stays connected
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"[Client] : Received: {message}");

                BroadcastMessage(message);
            }
        }
        catch (Exception ex)
        {
            //If theres an error, it will be caught here, and the client will be removed from the list
            Console.WriteLine($"[Error] : Error in client thread: {ex.Message}");
            RemoveClient(client);
        }
    }

    static void RemoveClient(TcpClient client)
    {
        bool wasRemoved;
        lock (lockObject)
        {
            wasRemoved = clients.Remove(client);
        }

        if (wasRemoved)
        {
            try { client.Close(); } catch {}
            Console.WriteLine($"[Client] : Connection closed. Active clients: {clients.Count}");
        }
    }

    static void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        List<TcpClient> copy;
        List<TcpClient> dead = new List<TcpClient>();

        lock (lockObject) // Copy the list of clients to avoid DOS
        {
            copy = new List<TcpClient>(clients);
        }

        foreach (TcpClient receiver in copy)
        {
            try
            {
                NetworkStream stream = receiver.GetStream();
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not send to a client: {ex.Message}");
                dead.Add(receiver);
            }
        }

        // If a client is dead, remove it from the list of clients
        foreach (TcpClient d in dead)
        {
            RemoveClient(d);
        }

        Console.WriteLine($"Message broadcasted to {copy.Count - dead.Count} clients.");
    }
}