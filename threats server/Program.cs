using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class EchoServerThreaded
{
    static void Main()
    {
        string serverIP = "127.0.0.1";
        int port = 5000;

        TcpListener server = new TcpListener(IPAddress.Parse(serverIP), port);
        server.Start();
        Console.WriteLine($"Server startet på {serverIP}:{port}");

        while (true)
        {
            try
            {
                TcpClient klient = server.AcceptTcpClient();
                Console.WriteLine("Klient forbundet.");

                // Start en ny tråd til hver klient
                Thread t = new Thread(() => HåndterKlient(klient));
                t.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fejl: {e.Message}");
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
                Console.WriteLine($"[Klient] Modtaget: {besked}");

                // Echo tilbage
                stream.Write(buffer, 0, bytesLæst);
                Console.WriteLine($"[Klient] Echo sendt: {besked}");
            }

            klient.Close();
            Console.WriteLine("[Klient] Forbindelse lukket.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl i klient-tråd: {ex.Message}");
        }
    }
}
