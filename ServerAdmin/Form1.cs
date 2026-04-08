using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharedModels.Models;

namespace ServerAdmin;

public partial class Form1 : Form
{
    private NetworkServer? _server;

    // Sidebar Buttons
    private Button? btnSettings;
    private Button? btnComputerMgmt;
    private Button? btnAccounts;
    private Button? btnServices;
    private Button? btnChat;

    // Layout roots
    private TableLayoutPanel? masterLayout;
    private Panel? sidebarPanel;
    private Panel? rightContentHost;
    private Panel? pnlQuanLyMay;
    private Panel? pnlTaiKhoan;
    private Panel? pnlDichVu;
    private Panel? pnlChat;
    private Panel? headerPanel;
    private Panel? footerPanel;
    private TableLayoutPanel? cardTableLayout;
    private Label? lblTitle;
    private Label? lblFooterStats;

    // Colors
    private readonly Color ColorMainBg = Color.FromArgb(229, 231, 235);    // #E5E7EB
    private readonly Color ColorHeaderBg = Color.White;
    private readonly Color ColorSidebar = Color.FromArgb(44, 62, 80);      // #2C3E50
    private readonly Color ColorButtonNormal = Color.FromArgb(52, 73, 94);
    private readonly Color ColorButtonHover = Color.FromArgb(75, 100, 130);
    private readonly Color ColorButtonActive = Color.FromArgb(26, 188, 156); // #1ABC9C
    private readonly Color ColorText = Color.White;
    private readonly Color ColorAvailable = Color.FromArgb(39, 174, 96);   // #27AE60
    private readonly Color ColorInUse = Color.FromArgb(231, 76, 60);       // #E74C3C
    private readonly Color ColorMaintenance = Color.FromArgb(243, 156, 18); // #F39C12

    public Form1()
    {
        InitializeComponent();
        
        this.Text = "ServerAdmin - Quản Lý Quán Network";
        this.Size = new Size(1024, 768);
        this.WindowState = FormWindowState.Maximized;
        this.AutoScaleMode = AutoScaleMode.Dpi;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Segoe UI", 10);
        this.BackColor = ColorMainBg;
        this.DoubleBuffered = true;

        SetupUI();
        
        this.Load += Form1_Load;
        this.FormClosing += Form1_FormClosing;
    }

    private void SetupUI()
    {
        this.Controls.Clear();

        // [1] Master layout: chia ranh giới sidebar/content tuyệt đối
        masterLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = ColorMainBg
        };
        masterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
        masterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        masterLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        // [2] Sidebar panel (cot 0)
        sidebarPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = ColorSidebar
        };

        // Nut cai dat rieng, dock bottom (khong nam trong flow)
        btnSettings = CreateSidebarButton("⚙ Cài Đặt");
        btnSettings.Dock = DockStyle.Bottom;

        // Flow chua logo + menu theo dung thu tu
        var sidebarFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = false,
            BackColor = ColorSidebar
        };

        var logoPanel = new Panel
        {
            Width = 230,
            Height = 80,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = ColorSidebar,
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
        };

        var lblLogo = new Label
        {
            Text = "ServerAdmin",
            Dock = DockStyle.Fill,
            ForeColor = ColorText,
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        logoPanel.Controls.Add(lblLogo);

        btnComputerMgmt = CreateSidebarButton("🖥 Quản Lý Máy Trạm");
        btnAccounts = CreateSidebarButton("👤 Tài Khoản");
        btnServices = CreateSidebarButton("📦 Dịch Vụ");
        btnChat = CreateSidebarButton("💬 Chat");

        btnComputerMgmt.Click += (s, e) => ShowPage(pnlQuanLyMay, btnComputerMgmt);
        btnAccounts.Click += (s, e) => ShowPage(pnlTaiKhoan, btnAccounts);
        btnServices.Click += (s, e) => ShowPage(pnlDichVu, btnServices);
        btnChat.Click += (s, e) => ShowPage(pnlChat, btnChat);

        sidebarFlow.Controls.Add(logoPanel);
        sidebarFlow.Controls.Add(btnComputerMgmt);
        sidebarFlow.Controls.Add(btnAccounts);
        sidebarFlow.Controls.Add(btnServices);
        sidebarFlow.Controls.Add(btnChat);

        sidebarPanel.Controls.Add(sidebarFlow);
        sidebarPanel.Controls.Add(btnSettings);

        // [3] Host cot 1 de quan ly panel swapping on dinh
        rightContentHost = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            BackColor = ColorMainBg
        };

        // 4 trang chuc nang nam trong rightContentHost
        pnlQuanLyMay = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = ColorMainBg,
            Padding = Padding.Empty
        };

        pnlTaiKhoan = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = ColorMainBg
        };

        pnlDichVu = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = ColorMainBg
        };

        pnlChat = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = ColorMainBg
        };

        // Chi ve UI quan ly may tren pnlQuanLyMay
        headerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Height = 88,
            Margin = Padding.Empty,
            BackColor = ColorHeaderBg,
            Padding = new Padding(20, 12, 20, 12),
            MinimumSize = new Size(0, 88)
        };

        lblTitle = new Label
        {
            Text = "Quản Lý Máy Trạm",
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = ColorTranslator.FromHtml("#0984E3"),
            Font = new Font("Segoe UI", 18, FontStyle.Bold)
        };
        headerPanel.Controls.Add(lblTitle);

        footerPanel = new Panel
        {
            Dock = DockStyle.Fill,
            Height = 44,
            Margin = Padding.Empty,
            BackColor = ColorHeaderBg,
            Padding = new Padding(16, 10, 16, 10),
            MinimumSize = new Size(0, 44)
        };

        lblFooterStats = new Label
        {
            Text = "Đang sử dụng: 0 | Sẵn sàng: 0 | Bảo trì: 0",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(107, 114, 128),
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
        footerPanel.Controls.Add(lblFooterStats);

        // [4] Luoi may tram 2 cot
        cardTableLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = new Padding(20, 20, 20, 64),
            AutoScroll = true,
            ColumnCount = 3,
            RowCount = 0,
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize
        };
        cardTableLayout.AutoScrollMargin = new Size(0, 32);

        cardTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        cardTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        cardTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

        var computerPageLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = Padding.Empty,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = ColorMainBg
        };
        computerPageLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        computerPageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        computerPageLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        computerPageLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

        computerPageLayout.Controls.Add(headerPanel, 0, 0);
        computerPageLayout.Controls.Add(cardTableLayout, 0, 1);
        computerPageLayout.Controls.Add(footerPanel, 0, 2);

        pnlQuanLyMay.Controls.Add(computerPageLayout);

        rightContentHost.Controls.Add(pnlQuanLyMay);
        rightContentHost.Controls.Add(pnlTaiKhoan);
        rightContentHost.Controls.Add(pnlDichVu);
        rightContentHost.Controls.Add(pnlChat);

        masterLayout.Controls.Add(sidebarPanel, 0, 0);
        masterLayout.Controls.Add(rightContentHost, 1, 0);
        this.Controls.Add(masterLayout);

        // Mac dinh mo trang quan ly may
        ShowComputerManagement();
        ShowPage(pnlQuanLyMay, btnComputerMgmt);
    }

    private void ShowPage(Panel? panel, Button? activeButton)
    {
        panel?.BringToFront();
        SetActiveMenu(activeButton);
    }

    private void SetActiveMenu(Button? activeButton)
    {
        var menuButtons = new[] { btnComputerMgmt, btnAccounts, btnServices, btnChat };

        foreach (var button in menuButtons)
        {
            if (button == null) continue;
            button.BackColor = button == activeButton ? ColorButtonActive : ColorButtonNormal;
        }
    }

    private Button CreateSidebarButton(string text)
    {
        Button btn = new Button
        {
            Text = text,
            AutoSize = false,
            Width = 230,
            Height = 50,
            Margin = Padding.Empty,
            BackColor = ColorButtonNormal,
            ForeColor = ColorText,
            Font = new Font("Segoe UI", 10),
            FlatStyle = FlatStyle.Flat,
            FlatAppearance = { BorderSize = 0 },
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0)
        };

        btn.MouseEnter += (s, e) => btn.BackColor = ColorButtonHover;
        btn.MouseLeave += (s, e) =>
        {
            if (btn.BackColor != ColorButtonActive)
            {
                btn.BackColor = ColorButtonNormal;
            }
        };

        return btn;
    }

    private void ShowComputerManagement()
    {
        if (cardTableLayout == null) return;

        int inUseCount = 0;
        int availableCount = 0;
        int maintenanceCount = 0;

        cardTableLayout.SuspendLayout();
        cardTableLayout.Controls.Clear();
        cardTableLayout.RowStyles.Clear();
        cardTableLayout.RowCount = 0;

        for (int i = 0; i < 24; i++)
        {
            int row = i / 3;
            int col = i % 3;
            int machineNumber = i + 1;
            int statusType = machineNumber % 3;

            if (statusType == 1)
            {
                availableCount++;
            }
            else if (statusType == 2)
            {
                inUseCount++;
            }
            else
            {
                maintenanceCount++;
            }

            if (col == 0)
            {
                cardTableLayout.RowCount++;
                cardTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
            }

            var cardPanel = CreateComputerCardPanel(machineNumber);
            cardTableLayout.Controls.Add(cardPanel, col, row);
        }

        int spacerRow = cardTableLayout.RowCount;
        cardTableLayout.RowCount++;
        cardTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));

        var bottomSpacer = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = Color.Transparent
        };
        cardTableLayout.Controls.Add(bottomSpacer, 0, spacerRow);
        cardTableLayout.SetColumnSpan(bottomSpacer, 3);

        cardTableLayout.ResumeLayout();
        cardTableLayout.PerformLayout();

        if (lblFooterStats != null)
        {
            lblFooterStats.Text = $"Đang sử dụng: {inUseCount} | Sẵn sàng: {availableCount} | Bảo trì: {maintenanceCount}";
        }
    }

    private Panel CreateComputerCardPanel(int machineNumber)
    {
        int padX = ScaleByDpi(10);
        int padY = ScaleByDpi(12);

        var statusType = machineNumber % 3;
        string statusText;
        Color statusColor;

        if (statusType == 1)
        {
            statusText = "Sẵn sàng";
            statusColor = ColorAvailable;
        }
        else if (statusType == 2)
        {
            statusText = "Đang sử dụng";
            statusColor = ColorInUse;
        }
        else
        {
            statusText = "Đang bảo trì";
            statusColor = ColorMaintenance;
        }

        var cardHost = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            Padding = new Padding(padX, padY, padX, padY),
            BackColor = ColorMainBg
        };

        var card = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = Padding.Empty,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var statusBorder = new Panel
        {
            Width = 5,
            Dock = DockStyle.Left,
            BackColor = statusColor
        };

        var lblMachine = new Label
        {
            Text = $"Máy {machineNumber:00}",
            AutoSize = true,
            Location = new Point(20, 15),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 51, 51)
        };

        var lblStatus = new Label
        {
            Text = statusText,
            AutoSize = true,
            Location = new Point(20, 45),
            Font = new Font("Segoe UI", 9, FontStyle.Regular),
            ForeColor = statusColor
        };

        var balance = statusType == 2 ? 120000 : 350000;
        var lblBalance = new Label
        {
            Text = $"Số dư: {balance:N0} VNĐ",
            AutoSize = true,
            Location = new Point(20, 75),
            Font = new Font("Segoe UI", 9, FontStyle.Regular),
            ForeColor = Color.FromArgb(80, 80, 80)
        };

        card.Controls.Add(statusBorder);
        card.Controls.Add(lblMachine);
        card.Controls.Add(lblStatus);
        card.Controls.Add(lblBalance);

        cardHost.Controls.Add(card);
        return cardHost;
    }

    private int ScaleByDpi(int px)
    {
        var scale = DeviceDpi / 96f;
        return Math.Max(1, (int)Math.Round(px * scale));
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        try
        {
            _server = new NetworkServer();
            _server.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khởi động server: {ex.Message}");
        }
    }

    private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _server?.Stop();
    }
}

// Computer Model for UI display
public class ComputerModel
{
    public int ComputerId { get; set; }
    public string? ComputerName { get; set; }
    public string? IpAddress { get; set; }
    public ComputerStatus Status { get; set; }
    public string? CurrentUser { get; set; }
    public long Balance { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastSeen { get; set; }
}

public enum ComputerStatus
{
    Available,
    InUse,
    Offline,
    Maintenance
}
