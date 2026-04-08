using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClientApp;

public partial class WidgetForm : Form
{
    // Dark Gaming Theme Colors
    private readonly Color ColorDarkBg = Color.FromArgb(20, 20, 30);
    private readonly Color ColorPanelBg = Color.FromArgb(30, 30, 45);
    private readonly Color ColorAccent = Color.FromArgb(0, 200, 255);
    private readonly Color ColorText = Color.FromArgb(240, 240, 240);
    private readonly Color ColorSuccess = Color.FromArgb(100, 220, 100);
    private readonly Color ColorWarning = Color.FromArgb(255, 180, 80);

    // UI Components
    private Label? lblUsername;
    private Label? lblBalance;
    private Label? lblTimeRemaining;
    private Button? btnService;
    private Button? btnChat;
    private Button? btnLogout;
    private System.Windows.Forms.Timer? timerCountdown;

    // Properties (for compatibility with existing code)
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string Username { get; set; } = "Người dùng";
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public long Balance { get; set; } = 250000;
    
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int TimeRemainingSeconds { get; set; } = 3600; // Default 1 hour

    // Constructor 1: Default (no parameters)
    public WidgetForm()
    {
        InitializeForm();
        SetupUI();
    }

    // Constructor 2: With client and user (for existing code compatibility)
    public WidgetForm(object client, object user)
    {
        InitializeForm();
        SetupUI();
    }

    private void InitializeForm()
    {
        // Form Properties
        this.Text = "Time Widget";
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.ShowInTaskbar = false;
        this.ControlBox = false;
        this.Size = new Size(300, 180);
        this.BackColor = ColorPanelBg;
        this.StartPosition = FormStartPosition.Manual;

        // Fixed position: Bottom Right corner with some margin
        Screen screen = Screen.PrimaryScreen;
        this.Location = new Point(
            screen.WorkingArea.Right - this.Width - 15,
            screen.WorkingArea.Bottom - this.Height - 15
        );

        // Prevent user from moving/resizing
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.SizeGripStyle = SizeGripStyle.Hide;

        // Setup Timer
        timerCountdown = new System.Windows.Forms.Timer();
        timerCountdown.Interval = 1000; // 1 second
        timerCountdown.Tick += TimerCountdown_Tick;
        timerCountdown.Start();

        this.FormClosing += (s, e) =>
        {
            timerCountdown.Stop();
            timerCountdown.Dispose();
        };
    }

    private void SetupUI()
    {
        this.Controls.Clear();
        this.Padding = new Padding(12);

        // Username Label
        lblUsername = new Label
        {
            Text = $"👤 {Username}",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = ColorAccent,
            AutoSize = false,
            Height = 25,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleLeft
        };
        this.Controls.Add(lblUsername);

        // Balance Label
        lblBalance = new Label
        {
            Text = $"💰 Số dư: {Balance:N0} VNĐ",
            Font = new Font("Segoe UI", 9),
            ForeColor = ColorSuccess,
            AutoSize = false,
            Height = 22,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleLeft
        };
        this.Controls.Add(lblBalance);

        // Time Remaining Label
        lblTimeRemaining = new Label
        {
            Text = FormatTime(TimeRemainingSeconds),
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = ColorWarning,
            AutoSize = false,
            Height = 40,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter
        };
        this.Controls.Add(lblTimeRemaining);

        // Button Panel
        Panel pnlButtons = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 35,
            BackColor = Color.Transparent
        };

        // Service Menu Button
        btnService = CreateSmallButton("📋 Menu", 5, 5);
        btnService.Click += (s, e) => OpenServiceMenu();
        pnlButtons.Controls.Add(btnService);

        // Chat Button
        btnChat = CreateSmallButton("💬 Chat", 100, 5);
        btnChat.Click += (s, e) => OpenChat();
        pnlButtons.Controls.Add(btnChat);

        // Logout Button
        btnLogout = CreateSmallButton("🚪 Thoát", 195, 5);
        btnLogout.Click += (s, e) => LogoutUser();
        pnlButtons.Controls.Add(btnLogout);

        this.Controls.Add(pnlButtons);
    }

    private Button CreateSmallButton(string text, int left, int top)
    {
        Button btn = new Button
        {
            Text = text,
            Location = new Point(left, top),
            Size = new Size(90, 30),
            Font = new Font("Segoe UI", 8, FontStyle.Bold),
            BackColor = ColorAccent,
            ForeColor = ColorDarkBg,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 230, 255);
        btn.MouseLeave += (s, e) => btn.BackColor = ColorAccent;
        return btn;
    }

    private void TimerCountdown_Tick(object sender, EventArgs e)
    {
        if (TimeRemainingSeconds > 0)
        {
            TimeRemainingSeconds--;
            lblTimeRemaining.Text = FormatTime(TimeRemainingSeconds);

            // Update balance based on time (example: 10000 VND = 1 hour = 3600 seconds)
            // This is usually handled by the server, but showing the calculation here
        }
        else
        {
            timerCountdown.Stop();
            MessageBox.Show("Hết giờ chơi! Vui lòng nạp thêm tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LogoutUser();
        }
    }

    private string FormatTime(int seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);
        return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
    }

    public void UpdateBalance(long newBalance)
    {
        Balance = newBalance;
        lblBalance.Text = $"💰 Số dư: {Balance:N0} VNĐ";
        lblBalance.Refresh();
    }

    public void UpdateTimeRemaining(int seconds)
    {
        TimeRemainingSeconds = seconds;
        lblTimeRemaining.Text = FormatTime(TimeRemainingSeconds);
        lblTimeRemaining.Refresh();
    }

    private void OpenServiceMenu()
    {
        MessageBox.Show("📋 Menu dịch vụ (Đang phát triển)\n\nChọn các dịch vụ đồ ăn, nước uống tại đây.", "Menu Dịch vụ");
    }

    private void OpenChat()
    {
        string message = InputBox.Show("Gửi tin nhắn tới Admin:", "Chat với Admin", "");
        if (!string.IsNullOrWhiteSpace(message))
        {
            MessageBox.Show("✅ Tin nhắn đã được gửi!", "Thành công");
        }
    }

    private void LogoutUser()
    {
        timerCountdown.Stop();
        this.Hide();
        Application.Exit();
    }

}

