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
        string serverIP = "127.0.0.1";
        int port = 5000;

        TcpListener server = new TcpListener(IPAddress.Parse(serverIP), port);
        server.Start();
        Console.WriteLine($"[Server] : Server started on {serverIP}:{port}");

        while (true)
        {
            try
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine($"[Client] : Client connected.");

                lock (lockObject)
                {
                    clients.Add(client);
                }

                Thread t = new Thread(() => HandleClient(client));
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error] : Error: {e.Message}");
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

            // This loop keeps running as long as the client stays connected - it does NOT disconnect after each message
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"[Client] : Received: {message}");

                BroadcastMessage(message);
            }
            // bytesRead == 0 means the client closed the connection gracefully
        }
        catch (Exception ex)
        {
            // E.g. if the client closes the connection abruptly (connection reset)
            Console.WriteLine($"[Error] : Error in client thread: {ex.Message}");
        }
        finally
        {
            // Regardless of why the loop stopped: remove the client from the list and close the socket.
            // The thread then ends automatically, since the method returns.
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
            try { client.Close(); } catch { /* already closed */ }
            Console.WriteLine($"[Client] : Connection closed. Active clients: {clients.Count}");
        }
    }

    static void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        List<TcpClient> copy;
        List<TcpClient> dead = new List<TcpClient>();

        // Take a copy so we don't hold the lock while writing to sockets (avoids blocking the Accept loop)
        lock (lockObject)
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
                Console.WriteLine($"[Error] : Could not send to a client: {ex.Message}");
                dead.Add(receiver); // The client is no longer responding - clean up right away
            }
        }

        // Remove dead clients now, instead of waiting for their own thread to detect it
        foreach (TcpClient d in dead)
        {
            RemoveClient(d);
        }

        Console.WriteLine($"[Server] : Message broadcasted to {copy.Count - dead.Count} client(s).");
    }
}