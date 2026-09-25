using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class BroadcastServerThreaded
{
    static List<TcpClient> klienter = new List<TcpClient>();
    static readonly object låsObjekt = new object();

    static void Main()
    {
        string serverIP = "127.0.0.1";
        int port = 5000;

        TcpListener server = new TcpListener(IPAddress.Parse(serverIP), port);
        server.Start();
        Console.WriteLine($"[Server] : Server startet på {serverIP}:{port}");

        while (true)
        {
            try
            {
                TcpClient klient = server.AcceptTcpClient();
                Console.WriteLine($"[Klient] : Klient forbundet.");

                // Tilføj klienten til den fælles liste, så den kan modtage beskeder
                lock (låsObjekt)
                {
                    klienter.Add(klient);
                }

                // Start en ny tråd til hver klient
                Thread t = new Thread(() => HåndterKlient(klient));
                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error] : Fejl: {e.Message}");
            }
        }
    }

    static void HåndterKlient(TcpClient klient)
    {
        try
        {
            NetworkStream stream = klient.GetStream();
            byte[] buffer = new byte[1024];
            int bytesLæst;

            while ((bytesLæst = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string besked = Encoding.UTF8.GetString(buffer, 0, bytesLæst);
                Console.WriteLine($"[Klient] : Modtaget: {besked}");

                // Send beskeden videre til alle tilsluttede klienter
                BroadcastBesked(besked);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] : Fejl i klient-tråd: {ex.Message}");
        }
        finally
        {
            // Fjern klienten fra listen igen, når forbindelsen lukker
            lock (låsObjekt)
            {
                klienter.Remove(klient);
            }
            klient.Close();
            Console.WriteLine($"[Klient] : Forbindelse lukket.");
        }
    }

    static void BroadcastBesked(string besked)
    {
        byte[] data = Encoding.UTF8.GetBytes(besked);

        lock (låsObjekt)
        {
            foreach (TcpClient modtager in klienter)
            {
                try
                {
                    NetworkStream stream = modtager.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] : Kunne ikke sende til en klient: {ex.Message}");
                }
            }
        }

        Console.WriteLine($"[Server] : Besked broadcastet til {klienter.Count} klient(er).");
    }
}