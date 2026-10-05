using System.Net.Sockets;

namespace WinFormsApp1___Forside
{
    public partial class Forside : Form
    {
        // Must match the port the server listens on
        private const int Port = 5000;

        public Forside()
        {
            InitializeComponent();
        }

        private async void buttonForbindIP_Click(object sender, EventArgs e)
        {
            // Empty field = connect to a server running on this PC
            string ip = textBoxIP.Text.Trim();
            if (ip.Length == 0)
            {
                ip = "127.0.0.1";
            }

            buttonForbindIP.Enabled = false;
            TcpClient client = new TcpClient();
            try
            {
                // Give up after 5 seconds instead of hanging if nothing answers
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                await client.ConnectAsync(ip, Port, cts.Token);
            }
            catch (Exception ex)
            {
                client.Dispose();
                string reason = ex is OperationCanceledException ? "Serveren svarede ikke." : ex.Message;
                MessageBox.Show($"Kunne ikke forbinde til {ip}:{Port}\n{reason}", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                buttonForbindIP.Enabled = true;
            }

            // The chat window owns the connection from here on, and closes it when the window closes
            Chat chat = new Chat(client, $"{ip}:{Port}");
            chat.FormClosed += (_, _) => Show();
            chat.Show();
            Hide();
        }
    }
}
