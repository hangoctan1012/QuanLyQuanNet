using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SharedModels.Models;

namespace ClientApp
{
    public class NetworkClient
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;
        private bool _isConnected;

        public event Action<NetworkMessage> OnMessageReceived;
        public event Action OnDisconnected;

        public async Task<bool> ConnectAsync(string ipAddress, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ipAddress, port);
                
                var stream = _client.GetStream();
                _reader = new StreamReader(stream, Encoding.UTF8);
                _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                _isConnected = true;

                _ = Task.Run(() => ReceiveMessagesAsync());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            _isConnected = false;
            _client?.Close();
            OnDisconnected?.Invoke();
        }

        public async Task SendMessageAsync(NetworkMessage message)
        {
            if (_isConnected && _writer != null)
            {
                string json = JsonSerializer.Serialize(message);
                await _writer.WriteLineAsync(json);
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            try
            {
                while (_isConnected && _client.Connected)
                {
                    string line = await _reader.ReadLineAsync();
                    if (line == null) break;

                    var message = JsonSerializer.Deserialize<NetworkMessage>(line);
                    if (message != null)
                    {
                        OnMessageReceived?.Invoke(message);
                    }
                }
            }
            catch
            {
                // Disconnected
            }
            finally
            {
                Disconnect();
            }
        }
    }

    public class NetworkMessage
    {
        public string Action { get; set; }
        public string Payload { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
