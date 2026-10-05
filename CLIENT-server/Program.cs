using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ProgramClient
{
    // Set when the user quits, so the receive thread doesn't report our own disconnect as an error
    static volatile bool quitting = false;

    static void Main()
    {
        // Define the server's IP address and port number
        string serverIP = "127.0.0.1";  // Localhost (change this to the server's LAN IP to connect from another PC)
        int port = 5000;               // Port number

        try
        {
            // Connect ONCE and keep the connection open for the whole session
            using TcpClient client = new TcpClient();
            client.Connect(serverIP, port);
            Console.WriteLine($"Connected to server {serverIP}:{port}. Type a message and press Enter (/quit to exit).");

            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };

            // Background thread: prints every message the server broadcasts, while the main thread waits for input
            Thread receiveThread = new Thread(() => ReceiveMessages(reader));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            // Main thread: read from the console and send. Each message ends with a newline so the receiver knows where it stops.
            while (true)
            {
                string? message = Console.ReadLine();
                if (message == null || message == "/quit")
                {
                    break;
                }

                if (message.Length > 0)
                {
                    writer.WriteLine(message);
                }
            }

            quitting = true;
            Console.WriteLine("Connection closed.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    static void ReceiveMessages(StreamReader reader)
    {
        try
        {
            // ReadLine returns null when the server closes the connection
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine($"Received: {line}");
            }
            Console.WriteLine("Server closed the connection.");
        }
        catch (Exception e)
        {
            // Thrown when the connection is reset, or when we close the client ourselves
            if (!quitting)
            {
                Console.WriteLine($"Disconnected: {e.Message}");
            }
        }
    }
}
