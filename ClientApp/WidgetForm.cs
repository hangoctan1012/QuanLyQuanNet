using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.Json;
using System.Runtime.InteropServices;
using SharedModels.Models;

namespace ClientApp
{
    public class WidgetForm : Form
    {
        // Drag window constants
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private NetworkClient _client;
        private User _currentUser;
        private Label lblTime;
        private Label lblBalance;
        private System.Windows.Forms.Timer _timer;
        private int _timeRemainingSeconds;
        private decimal _hourlyRate = 5000; // 5000 VND / hour

        public WidgetForm(NetworkClient client, User user)
        {
            _client = client;
            _currentUser = user;
            _client.OnMessageReceived += HandleServerMessage;
            
            // Calculate initial time
            _timeRemainingSeconds = (int)((_currentUser.Balance / _hourlyRate) * 3600);

            // Setup UI
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(200, 150);
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 20, 20); // Top Right corner

            // Drag handle
            this.MouseDown += WidgetForm_MouseDown;

            var lblUser = new Label { Text = $"Xin chào: {_currentUser.Username}", ForeColor = Color.White, Top = 10, Left = 10, Width = 180 };
            
            lblBalance = new Label { Text = $"Số dư: {_currentUser.Balance:N0}đ", ForeColor = Color.LightGreen, Top = 35, Left = 10, Width = 180, Font = new Font("Arial", 10, FontStyle.Bold) };
            
            lblTime = new Label { Text = "00:00:00", ForeColor = Color.Cyan, Top = 60, Left = 10, Width = 180, Font = new Font("Arial", 18, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };

            var btnMenu = new Button { Text = "Dịch vụ", Top = 100, Left = 10, Width = 80, BackColor = Color.Orange, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnMenu.Click += (s, e) => MessageBox.Show("Tính năng Dịch vụ đang phát triển", "Dịch vụ");

            var btnChat = new Button { Text = "Chat", Top = 100, Left = 100, Width = 80, BackColor = Color.Teal, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            btnChat.Click += BtnChat_Click;

            this.Controls.Add(lblUser);
            this.Controls.Add(lblBalance);
            this.Controls.Add(lblTime);
            this.Controls.Add(btnMenu);
            this.Controls.Add(btnChat);

            this.FormClosing += (s, e) => Application.Exit();

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();

            UpdateTimeDisplay();
        }

        private void WidgetForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_timeRemainingSeconds > 0)
            {
                _timeRemainingSeconds--;
                UpdateTimeDisplay();
            }
            else
            {
                _timer.Stop();
                MessageBox.Show("Khách hàng đã hết giờ chơi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Send Logout / Lock command to server or self lock
                Application.Restart(); // Quick hack to show Lock Screen again
            }
        }

        private void UpdateTimeDisplay()
        {
            TimeSpan time = TimeSpan.FromSeconds(_timeRemainingSeconds);
            lblTime.Text = time.ToString(@"hh\:mm\:ss");
        }

        private void RefreshBalance(decimal newBalance)
        {
            _currentUser.Balance = newBalance;
            _timeRemainingSeconds = (int)((_currentUser.Balance / _hourlyRate) * 3600);
            lblBalance.Text = $"Số dư: {_currentUser.Balance:N0}đ";
            UpdateTimeDisplay();
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
                case "BalanceUpdated":
                    if (decimal.TryParse(message.Payload, out decimal addedAmount))
                    {
                        RefreshBalance(_currentUser.Balance + addedAmount);
                        MessageBox.Show($"Tài khoản vừa được cộng {addedAmount:N0} VNĐ", "Nạp tiền thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (!_timer.Enabled && _timeRemainingSeconds > 0) _timer.Start();
                    }
                    break;
                case "Shutdown":
                    System.Diagnostics.Process.Start("shutdown", "-s -t 0");
                    break;
                case "Restart":
                    System.Diagnostics.Process.Start("shutdown", "-r -t 0");
                    break;
                case "Chat":
                    MessageBox.Show($"Server Admin: {message.Payload}", "Tin nhắn từ Máy Chủ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void BtnChat_Click(object sender, EventArgs e)
        {
            // Similar to InputBox on client side
            string msg = InputBox.Show("Nhắn tin cho ADMIN:", "Hỗ trợ", "");
            if (!string.IsNullOrWhiteSpace(msg))
            {
                _client.SendMessageAsync(new NetworkMessage { Action = "Chat", Payload = msg }).Wait();
            }
        }
    }
}
