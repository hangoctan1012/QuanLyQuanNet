using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClientApp;

public partial class LockScreenForm : Form
{
    private Panel pnlLoginContainer;
    private Label lblTitle;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;
    private Label lblError;
    
    // Dark Mode Colors
    private readonly Color ColorDarkBg = Color.FromArgb(20, 20, 30);
    private readonly Color ColorAccent = Color.FromArgb(0, 150, 255);
    private readonly Color ColorText = Color.FromArgb(240, 240, 240);
    private readonly Color ColorBorder = Color.FromArgb(50, 50, 70);

    public LockScreenForm()
    {
        InitializeForm();
        SetupLoginPanel();
    }

    private void InitializeForm()
    {
        // Form Properties
        this.Text = "Client - Quản lý Quán Net";
        this.WindowState = FormWindowState.Maximized;
        this.FormBorderStyle = FormBorderStyle.None;
        this.TopMost = true;
        this.BackColor = ColorDarkBg;
        this.ControlBox = false; // Remove close button
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = true;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Prevent Alt+F4 and other escape methods
        this.KeyPreview = true;
        this.KeyDown += (s, e) =>
        {
            if (e.Alt && e.KeyCode == Keys.F4)
                e.Handled = true;
            if (e.KeyCode == Keys.Escape)
                e.Handled = true;
        };

        this.Resize += (s, e) => CenterLoginPanel();
    }

    private void SetupLoginPanel()
    {
        // Main Login Container Panel
        pnlLoginContainer = new Panel
        {
            Size = new Size(400, 350),
            BackColor = Color.FromArgb(30, 30, 45),
            Padding = new Padding(30)
        };

        // Title Label
        lblTitle = new Label
        {
            Text = "ĐĂNG NHẬP",
            Font = new Font("Segoe UI", 28, FontStyle.Bold),
            ForeColor = ColorAccent,
            AutoSize = false,
            Height = 60,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top
        };
        pnlLoginContainer.Controls.Add(lblTitle);

        // Spacing Panel
        Panel pnlSpace1 = new Panel
        {
            Height = 20,
            Dock = DockStyle.Top,
            BackColor = Color.Transparent
        };
        pnlLoginContainer.Controls.Add(pnlSpace1);

        // Username Label
        Label lblUser = new Label
        {
            Text = "Tên đăng nhập:",
            Font = new Font("Segoe UI", 11),
            ForeColor = ColorText,
            AutoSize = false,
            Height = 25,
            Dock = DockStyle.Top
        };
        pnlLoginContainer.Controls.Add(lblUser);

        // Username TextBox
        txtUsername = new TextBox
        {
            Font = new Font("Segoe UI", 12),
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = ColorText,
            BorderStyle = BorderStyle.FixedSingle,
            Height = 40,
            Dock = DockStyle.Top,
            Padding = new Padding(10)
        };
        txtUsername.Enter += (s, e) => txtUsername.BackColor = Color.FromArgb(50, 50, 75);
        txtUsername.Leave += (s, e) => txtUsername.BackColor = Color.FromArgb(40, 40, 60);
        pnlLoginContainer.Controls.Add(txtUsername);

        // Spacing
        Panel pnlSpace2 = new Panel
        {
            Height = 15,
            Dock = DockStyle.Top,
            BackColor = Color.Transparent
        };
        pnlLoginContainer.Controls.Add(pnlSpace2);

        // Password Label
        Label lblPass = new Label
        {
            Text = "Mật khẩu:",
            Font = new Font("Segoe UI", 11),
            ForeColor = ColorText,
            AutoSize = false,
            Height = 25,
            Dock = DockStyle.Top
        };
        pnlLoginContainer.Controls.Add(lblPass);

        // Password TextBox
        txtPassword = new TextBox
        {
            Font = new Font("Segoe UI", 12),
            BackColor = Color.FromArgb(40, 40, 60),
            ForeColor = ColorText,
            PasswordChar = '●',
            BorderStyle = BorderStyle.FixedSingle,
            Height = 40,
            Dock = DockStyle.Top,
            Padding = new Padding(10)
        };
        txtPassword.Enter += (s, e) => txtPassword.BackColor = Color.FromArgb(50, 50, 75);
        txtPassword.Leave += (s, e) => txtPassword.BackColor = Color.FromArgb(40, 40, 60);
        txtPassword.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Return)
            {
                btnLogin.PerformClick();
                e.Handled = true;
            }
        };
        pnlLoginContainer.Controls.Add(txtPassword);

        // Spacing
        Panel pnlSpace3 = new Panel
        {
            Height = 15,
            Dock = DockStyle.Top,
            BackColor = Color.Transparent
        };
        pnlLoginContainer.Controls.Add(pnlSpace3);

        // Login Button
        btnLogin = new Button
        {
            Text = "ĐĂNG NHẬP",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            BackColor = ColorAccent,
            ForeColor = ColorDarkBg,
            FlatStyle = FlatStyle.Flat,
            Height = 45,
            Dock = DockStyle.Top,
            Cursor = Cursors.Hand
        };
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(0, 180, 255);
        btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = ColorAccent;
        btnLogin.Click += BtnLogin_Click;
        pnlLoginContainer.Controls.Add(btnLogin);

        // Error Label
        lblError = new Label
        {
            Text = "",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(255, 100, 100),
            AutoSize = false,
            Height = 40,
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.TopCenter,
            Padding = new Padding(5)
        };
        pnlLoginContainer.Controls.Add(lblError);

        this.Controls.Add(pnlLoginContainer);
        CenterLoginPanel();
    }

    private void CenterLoginPanel()
    {
        // Căn giữa panel login bất kể kích thước màn hình
        pnlLoginContainer.Left = (this.ClientSize.Width - pnlLoginContainer.Width) / 2;
        pnlLoginContainer.Top = (this.ClientSize.Height - pnlLoginContainer.Height) / 2;
    }

    private void BtnLogin_Click(object sender, EventArgs e)
    {
        // Xóa lỗi cũ
        lblError.Text = "";

        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            lblError.Text = "Vui lòng nhập tên đăng nhập!";
            return;
        }

        if (string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            lblError.Text = "Vui lòng nhập mật khẩu!";
            return;
        }

        // Giả lập đăng nhập thành công
        // !! Thay thế bằng code gọi server thực tế
        bool loginSuccess = ValidateLogin(txtUsername.Text, txtPassword.Text);

        if (loginSuccess)
        {
            // Ẩn form này
            this.Hide();
            this.Opacity = 0;

            // Hiển thị WidgetForm
            WidgetForm widgetForm = new WidgetForm
            {
                Username = txtUsername.Text,
                Balance = 250000, // Mock balance
                TimeRemainingSeconds = 3600 // Mock 1 hour
            };
            widgetForm.Show();

            // Khóa màn hình
            LockScreen();
        }
        else
        {
            lblError.Text = "Tên đăng nhập hoặc mật khẩu không đúng!";
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }

    private bool ValidateLogin(string username, string password)
    {
        // Mock validation - thay thế bằng code thực tế
        return (username == "user1" && password == "pass123") ||
               (username == "vip1" && password == "vip123");
    }

    private void LockScreen()
    {
        // Lock the screen - disable all windows
        this.ShowInTaskbar = false;
        this.ControlBox = false;
    }

    public void ShowLoginError(string message)
    {
        this.Show();
        lblError.Text = message;
        this.Opacity = 1;
    }
}
