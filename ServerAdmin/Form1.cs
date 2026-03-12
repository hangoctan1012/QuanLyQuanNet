using System;
using System.Drawing;
using System.Windows.Forms;
using SharedModels.Models;
using Dapper;

namespace ServerAdmin;

public partial class Form1 : Form
{
    private NetworkServer _server;
    private FlowLayoutPanel pnlComputers;
    private RichTextBox txtLog;

    public Form1()
    {
        InitializeComponent();
        this.Text = "Server Admin - Quản lý Quán Net";
        this.Size = new Size(1000, 700);
        this.Load += Form1_Load;
        this.FormClosing += Form1_FormClosing;
        
        SetupUI();
    }

    private void SetupUI()
    {
        pnlComputers = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(10)
        };

        txtLog = new RichTextBox
        {
            Dock = DockStyle.Bottom,
            Height = 150,
            ReadOnly = true
        };

        this.Controls.Add(pnlComputers);
        this.Controls.Add(txtLog);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        Log("Initializing Database...");
        LoadComputers();

        _server = new NetworkServer();
        _server.OnLogMessage += Log;
        _server.OnComputerStatusChanged += UpdateComputerStatus;
        
        Log("Starting Network Server...");
        _server.Start();
    }

    private void LoadComputers()
    {
        pnlComputers.Controls.Clear();
        using (var db = DatabaseHelper.GetConnection())
        {
            var computers = Dapper.SqlMapper.Query<SharedModels.Models.Computer>(db, "SELECT * FROM Computers");
            foreach (var comp in computers)
            {
                var btn = new Button
                {
                    Text = $"{comp.Name}\n{comp.Status}",
                    Size = new Size(100, 100),
                    Tag = comp.Id,
                    BackColor = comp.Status == "Available" ? Color.LightGreen : 
                                comp.Status == "InUse" ? Color.LightCoral : Color.LightGray
                };
                btn.Click += ComputerBtn_Click;
                pnlComputers.Controls.Add(btn);
            }
        }
    }

    private void UpdateComputerStatus(int computerId, string status)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdateComputerStatus(computerId, status)));
            return;
        }

        foreach (Control ctrl in pnlComputers.Controls)
        {
            if (ctrl is Button btn && btn.Tag is int id && id == computerId)
            {
                using (var db = DatabaseHelper.GetConnection())
                {
                    var comp = Dapper.SqlMapper.QueryFirstOrDefault<SharedModels.Models.Computer>(db, "SELECT * FROM Computers WHERE Id = @Id", new { Id = id });
                    if (comp != null)
                    {
                        btn.Text = $"{comp.Name}\n{status}";
                        btn.BackColor = status == "Available" ? Color.LightGreen : 
                                        status == "InUse" ? Color.LightCoral : Color.LightGray;
                    }
                }
                break;
            }
        }
    }

    private void ComputerBtn_Click(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id && e is MouseEventArgs me && me.Button == MouseButtons.Right)
        {
            var contextMenu = new ContextMenuStrip();
            
            var addTimeItem = new ToolStripMenuItem("Nạp tiền / Thêm giờ");
            addTimeItem.Click += (s, ev) => AddTime(id);
            
            var turnOffItem = new ToolStripMenuItem("Tắt máy (Shutdown)");
            turnOffItem.Click += (s, ev) => SendCommandToComputer(id, "Shutdown");
            
            var restartItem = new ToolStripMenuItem("Khởi động lại (Restart)");
            restartItem.Click += (s, ev) => SendCommandToComputer(id, "Restart");

            var chatItem = new ToolStripMenuItem("Chat với khách");
            chatItem.Click += (s, ev) => OpenChatWindow(id);

            contextMenu.Items.Add(addTimeItem);
            contextMenu.Items.Add(turnOffItem);
            contextMenu.Items.Add(restartItem);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add(chatItem);

            contextMenu.Show(btn, me.Location);
        }
        else if (sender is Button button && button.Tag is int compId && e is MouseEventArgs leftMe && leftMe.Button == MouseButtons.Left)
        {
            // Just display some basic info
            using (var db = DatabaseHelper.GetConnection())
            {
                var comp = Dapper.SqlMapper.QueryFirstOrDefault<SharedModels.Models.Computer>(db, "SELECT * FROM Computers WHERE Id = @Id", new { Id = compId });
                if (comp != null)
                {
                    string info = $"Tên máy: {comp.Name}\nTrạng thái: {comp.Status}";
                    if (comp.CurrentUserId.HasValue)
                    {
                        var user = Dapper.SqlMapper.QueryFirstOrDefault<User>(db, "SELECT Username, Balance FROM Users WHERE Id = @Id", new { Id = comp.CurrentUserId.Value });
                        if (user != null)
                        {
                            info += $"\nKhách đang chơi: {user.Username}\nSố dư: {user.Balance:N0} VNĐ";
                        }
                    }
                    MessageBox.Show(info, "Thông tin máy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

    private void AddTime(int computerId)
    {
        string input = InputBox.Show("Nhập số tiền nạp (VNĐ):", "Nạp tiền", "10000");
        if (decimal.TryParse(input, out decimal amount) && amount > 0)
        {
            using (var db = DatabaseHelper.GetConnection())
            {
                var comp = Dapper.SqlMapper.QueryFirstOrDefault<SharedModels.Models.Computer>(db, "SELECT CurrentUserId FROM Computers WHERE Id = @Id", new { Id = computerId });
                if (comp != null && comp.CurrentUserId.HasValue)
                {
                    db.Execute("UPDATE Users SET Balance = Balance + @Amount WHERE Id = @UserId", 
                        new { Amount = amount, UserId = comp.CurrentUserId.Value });
                    Log($"Added {amount:N0} VNĐ to Computer {computerId} (User {comp.CurrentUserId.Value})");
                    
                    // Notify client
                    _server.SendMessageToClient(computerId, new NetworkMessage { Action = "BalanceUpdated", Payload = amount.ToString() }).Wait();
                }
                else
                {
                    MessageBox.Show("Máy hiện chưa có khách đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }

    private void SendCommandToComputer(int computerId, string command)
    {
        if (MessageBox.Show($"Bạn có chắc muốn {command} máy {computerId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _server.SendMessageToClient(computerId, new NetworkMessage { Action = command, Payload = "" }).Wait();
            Log($"Sent {command} command to Computer {computerId}.");
        }
    }

    private void OpenChatWindow(int computerId)
    {
        string message = InputBox.Show("Nhập tin nhắn:", "Chat Server -> Client", "");
        if (!string.IsNullOrWhiteSpace(message))
        {
            _server.SendMessageToClient(computerId, new NetworkMessage { Action = "Chat", Payload = message }).Wait();
            Log($"Server -> Computer {computerId}: {message}");
        }
    }

    private void Log(string message)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => Log(message)));
            return;
        }
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
        txtLog.ScrollToCaret();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        _server?.Stop();
    }
}
