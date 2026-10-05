using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1___Forside
{
    public partial class Chat : Form
    {
        private readonly TcpClient? client;
        private readonly StreamReader? reader;
        private readonly StreamWriter? writer;

        // Set when the window closes, so the receive loop doesn't report our own disconnect as an error
        private bool closing = false;

        // Only used by the Visual Studio designer
        public Chat()
        {
            InitializeComponent();
        }

        public Chat(TcpClient client, string serverName) : this()
        {
            this.client = client;
            Text = $"Chat - {serverName}";

            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            // Each message ends with a newline, so the receivers know where it stops (same as the console client)
            writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (reader != null)
            {
                _ = ReceiveMessagesAsync(reader);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            closing = true;
            client?.Close();
            base.OnFormClosed(e);
        }

        // Runs on the UI thread: each await frees the UI while waiting, so we can update the controls directly
        private async Task ReceiveMessagesAsync(StreamReader reader)
        {
            try
            {
                // ReadLineAsync returns null when the server closes the connection
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    AddMessage(line);
                }

                if (!closing)
                {
                    AddMessage("[Serveren lukkede forbindelsen]");
                }
            }
            catch (Exception ex)
            {
                // Thrown when the connection is reset, or when we close the client ourselves
                if (!closing)
                {
                    AddMessage($"[Forbindelsen blev afbrudt: {ex.Message}]");
                }
            }

            if (!closing)
            {
                buttonSend.Enabled = false;
                textBoxBesked.Enabled = false;
            }
        }

        private void AddMessage(string message)
        {
            listBoxChat.Items.Add(message);
            listBoxChat.TopIndex = listBoxChat.Items.Count - 1; // Keep the newest message in view
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            if (writer == null || string.IsNullOrWhiteSpace(textBoxBesked.Text))
            {
                return;
            }

            // Not added to the list here: the server broadcasts it back to us too, so it shows up via ReceiveMessagesAsync
            string besked = textBoxBesked.Text;
            textBoxBesked.Clear();
            try
            {
                await writer.WriteLineAsync(besked);
            }
            catch (Exception ex)
            {
                AddMessage($"[Kunne ikke sende: {ex.Message}]");
            }
        }

        private void textBoxBesked_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBoxChat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
