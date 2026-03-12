using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Text.Json;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        private NetworkClient _client;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblStatus;
        private string _computerName;

        public Form1()
        {
            InitializeComponent();
            _computerName = Environment.MachineName;
            
            // Set up Fullscreen Lock Screen
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = false;
            this.BackColor = Color.FromArgb(30, 30, 30);
            
            SetupUI();
            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;
        }

        private void SetupUI()
        {
            var pnlLogin = new Panel
            {
                Size = new Size(400, 300),
                BackColor = Color.FromArgb(50, 50, 50),
                Location = new Point((Screen.PrimaryScreen.Bounds.Width - 400) / 2, (Screen.PrimaryScreen.Bounds.Height - 300) / 2)
            };

            var lblTitle = new Label { Text = $"Đăng nhập ({_computerName})", ForeColor = Color.White, Font = new Font("Arial", 16, FontStyle.Bold), Top = 20, Left = 0, Width = 400, TextAlign = ContentAlignment.MiddleCenter };
            
            var lblUser = new Label { Text = "Tài khoản:", ForeColor = Color.White, Top = 80, Left = 50 };
            txtUsername = new TextBox { Top = 100, Left = 50, Width = 300, Font = new Font("Arial", 12) };

            var lblPass = new Label { Text = "Mật khẩu:", ForeColor = Color.White, Top = 140, Left = 50 };
            txtPassword = new TextBox { Top = 160, Left = 50, Width = 300, Font = new Font("Arial", 12), PasswordChar = '*' };

            btnLogin = new Button { Text = "ĐĂNG NHẬP", Top = 220, Left = 50, Width = 300, Height = 40, BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 12, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnLogin.Click += BtnLogin_Click;

            lblStatus = new Label { Text = "Đang kết nối đến Server...", ForeColor = Color.Yellow, Top = 270, Left = 0, Width = 400, TextAlign = ContentAlignment.MiddleCenter };

            pnlLogin.Controls.Add(lblTitle);
            pnlLogin.Controls.Add(lblUser);
            pnlLogin.Controls.Add(txtUsername);
            pnlLogin.Controls.Add(lblPass);
            pnlLogin.Controls.Add(txtPassword);
            pnlLogin.Controls.Add(btnLogin);
            pnlLogin.Controls.Add(lblStatus);

            this.Controls.Add(pnlLogin);
            this.AcceptButton = btnLogin;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            _client = new NetworkClient();
            _client.OnMessageReceived += HandleServerMessage;
            _client.OnDisconnected += () => UpdateStatus("Mất kết nối với Server!", Color.Red);

            bool connected = await _client.ConnectAsync("127.0.0.1", 5000);
            if (connected)
            {
                UpdateStatus("Đã kết nối Server", Color.LightGreen);
                // Send identify message
                await _client.SendMessageAsync(new NetworkMessage { Action = "Identify", Payload = _computerName });
            }
            else
            {
                UpdateStatus("Không thể kết nối đến Server!", Color.Red);
            }
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!", "Chú ý");
                return;
            }

            btnLogin.Enabled = false;
            UpdateStatus("Đang xác thực...", Color.Yellow);

            var req = new LoginRequest { Username = txtUsername.Text, Password = txtPassword.Text };
            await _client.SendMessageAsync(new NetworkMessage { Action = "Login", Payload = JsonSerializer.Serialize(req) });
        }

        private void HandleServerMessage(NetworkMessage message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HandleServerMessage(message)));
                return;
            }

            switch (message.Action)
            {
                case "IdentifyResponse":
                    if (message.Payload.StartsWith("Error"))
                        UpdateStatus($"Lỗi: Máy này chưa được thêm trên Server", Color.Red);
                    break;
                case "LoginResponse":
                    if (message.Payload.StartsWith("Error"))
                    {
                        MessageBox.Show(message.Payload.Substring(6), "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateStatus("Lỗi đăng nhập", Color.Red);
                        btnLogin.Enabled = true;
                    }
                    else
                    {
                        // Login success
                        SharedModels.Models.User user = JsonSerializer.Deserialize<SharedModels.Models.User>(message.Payload);
                        this.Hide();
                        
                        // Open Widget
                        var widget = new WidgetForm(_client, user);
                        widget.Show();
                    }
                    break;
            }
        }

        private void UpdateStatus(string text, Color color)
        {
            lblStatus.Text = text;
            lblStatus.ForeColor = color;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _client?.Disconnect();
        }
    }
}
