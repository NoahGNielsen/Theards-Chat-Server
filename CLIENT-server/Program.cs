using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class ProgramClient
{
    static void Main()
    {
        // Define the server's IP address and port number
        string serverIP = "127.0.0.1";  // Localhost (You can change this to any IP)
        int port = 5000;               // Port number
        while (true)
        {

            try
            {
                // Create a TCP client and connect to the server
                TcpClient client = new TcpClient();
                client.Connect(serverIP, port);
                Console.WriteLine("Connected to server.");

                // Get the network stream
                NetworkStream stream = client.GetStream();

                // Prepare a message to send to the server
                string message = "Hello, Server!" + Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Send the message to the server
                stream.Write(data, 0, data.Length);
                Console.WriteLine($"Sent: {message}");

                // Receive a response from the server
                data = new byte[256];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = Encoding.UTF8.GetString(data, 0, bytesRead);
                Console.WriteLine($"Received: {response}");

                //// Close the connection
                stream.Close();
                client.Close();
                Console.WriteLine("Connection closed.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
