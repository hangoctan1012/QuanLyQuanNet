# ClientApp UI Design Documentation

## 📋 Tổng Quan
Giao diện **ClientApp** được thiết kế với phong cách **Modern Dark Gaming Theme**, tối giản nhưng mạnh mẽ. Ứng dụng hoạt động ở 2 giai đoạn:

1. **Giai đoạn 1: Login** - Màn hình khóa toàn màn hình (LockScreenForm)
2. **Giai đoạn 2: Gaming** - Widget nhỏ đếm thời gian ở góc màn hình (WidgetForm)

---

## 🎨 Bảng Màu (Color Scheme)

### Dark Gaming Theme
| Thành Phần | Màu | RGB | Mô Tả |
|-----------|-----|-----|-------|
| Background Chính | Đen tối | `#141E1E` | Nền rất tối, thân thiện mắt |
| Panel Phụ | Xanh Tối | `#1E1E2D` | Nền các panel |
| Accent (Viền/Text) | Xanh Dương | `#0096FF` hoặc `#00C8FF` | Màu highlight, gaming feel |
| Text Chính | Trắng Sáng | `#F0F0F0` | Chữ sáng dễ đọc |
| Success (Balance) | Xanh Lá | `#64DC64` | Chỉ số tiền |
| Warning (Time) | Cam | `#FFB450` | Thời gian (cảnh báo) |
| Error | Đỏ | `#FF6464` | Lỗi, cảnh báo |

---

## 🔐 Form 1: LockScreenForm (Màn Hình Khóa)

### Thuộc Tính Form
```csharp
WindowState = FormWindowState.Maximized  // Toàn màn hình
FormBorderStyle = FormBorderStyle.None   // Không viền
TopMost = true                           // Luôn ở trên cùng
ControlBox = false                       // Không nút X, Min, Max
ShowInTaskbar = false                    // Ẩn khỏi taskbar
Opacity = 1.0                            // Đầy đủ độ opaque
KeyPreview = true                        // Bắt phím (Alt+F4, Esc)
```

### Bố Cục (Layout)

```
┌──────────────────────────────────────────────────────────┐
│                   Nền Tối (#141E1E)                      │
│                                                          │
│                                                          │
│               ┌────────────────────────┐                │
│               │   ĐĂNG NHẬP (Tiêu đề)  │                │
│               │   Font: Segoe UI 28B   │                │
│               │   Color: #0096FF       │                │
│               ├────────────────────────┤                │
│               │                        │                │
│               │ Tên đăng nhập:         │                │
│               │ [TextBox - 40px]       │                │
│               │                        │                │
│               │ Mật khẩu:              │                │
│               │ [TextBox - 40px]       │                │
│               │ (PasswordChar = '●')   │                │
│               │                        │                │
│               │ [ĐĂNG NHẬP Button]     │                │
│               │  45px height           │                │
│               │                        │                │
│               │ [Error Message]        │                │
│               └────────────────────────┘                │
│                                                          │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

### Các Control

#### 1. Main Container Panel (Login Box)
- **Kích thước**: 400x350px
- **Màu nền**: `#1E1E2D` (xanh đậm)
- **Căn giữa**: Theo chiều ngang & chiều dọc
- **Padding**: 30px

#### 2. Title Label
- **Text**: "ĐĂNG NHẬP"
- **Font**: Segoe UI 28px Bold
- **Color**: `#0096FF` (Accent)
- **Alignment**: Centered

#### 3. Username TextBox
- **Font**: Segoe UI 12px
- **Background**: `#28283C` (nhẹ hơn panel)
- **Text Color**: `#F0F0F0` (Trắng sáng)
- **Border**: Single
- **Height**: 40px
- **On Focus**: Background thay đổi thành `#32324B` (sáng hơn)

#### 4. Password TextBox
- Giống Username nhưng `PasswordChar = '●'`
- Enter key trigger Login button

#### 5. Login Button
- **Text**: "ĐĂNG NHẬP"
- **Font**: Segoe UI 12px Bold
- **Background**: `#0096FF`
- **Text Color**: `#141E1E`
- **Height**: 45px
- **On Hover**: `#00B8FF` (sáng hơn)
- **FlatStyle**: Flat (không 3D)

#### 6. Error Message Label
- **Color**: `#FF6464` (Đỏ cảnh báo)
- **Font**: Segoe UI 10px
- **Alignment**: Centered

### Tính Năng
✅ Căn giữa responsive (tự điều chỉnh khi resize)
✅ Chặn Alt+F4 và Escape
✅ Focus TextBox khi mở form
✅ Phím Enter để đăng nhập
✅ Mock validation (user1/pass123, vip1/vip123)

---

## ⏱️ Form 2: WidgetForm (Widget Đếm Thời Gian)

### Thuộc Tính Form
```csharp
FormBorderStyle = FormBorderStyle.None       // Không viền
TopMost = true                               // Luôn ở trên cùng
ShowInTaskbar = false                        // Ẩn khỏi taskbar
ControlBox = false                           // Không có nút
Size = new Size(300, 180)                    // 300x180px
Location = BottomRight corner                // Góc dưới phải (fixed)
StartPosition = FormStartPosition.Manual      // Manual positioning
```

### Bố Cục (Layout)

```
┌─────────────────────────────┐
│ 👤 Người dùng        (#00FFFF)     │
│ 💰 Số dư: 250,000 VNĐ (#64DC64)  │
│                             │
│     HH:MM:SS            │
│   (Số TO, #FFB450)      │
│                             │
│ [📋 Menu][💬 Chat][🚪 Thoát]  │
└─────────────────────────────┘
```

### Các Control

#### 1. Username Label
- **Text**: `👤 {Username}`
- **Color**: `#00C8FF` (Xanh sáng)
- **Font**: Segoe UI 10px Bold
- **Height**: 25px
- **Alignment**: MiddleLeft

#### 2. Balance Label
- **Text**: `💰 Số dư: {Balance:N0} VNĐ`
- **Color**: `#64DC64` (Xanh lá)
- **Font**: Segoe UI 9px
- **Height**: 22px

#### 3. Time Remaining Label (Quan Trọng!)
- **Text**: Format `HH:MM:SS` (ví dụ: `01:30:45`)
- **Color**: `#FFB450` (Cam - Warning)
- **Font**: Segoe UI 16px Bold (SỐ LỚN!)
- **Height**: 40px
- **Alignment**: Centered

#### 4. Các Button (Nhỏ)
- **Size**: 90x30px mỗi cái
- **Font**: Segoe UI 8px Bold
- **Background**: `#00C8FF`
- **Text Color**: `#141E1E`
- **On Hover**: `#00E6FF` (sáng hơn)

**Buttons**:
- 📋 **Menu** - Mở menu dịch vụ
- 💬 **Chat** - Chat với Admin
- 🚪 **Thoát** - Đăng xuất

### Timer (Đếm Ngược)

```csharp
// Timer cơ bản
System.Windows.Forms.Timer timerCountdown = new();
timerCountdown.Interval = 1000;  // 1 second
timerCountdown.Tick += (s, e) =>
{
    if (TimeRemainingSeconds > 0)
    {
        TimeRemainingSeconds--;
        lblTimeRemaining.Text = FormatTime(TimeRemainingSeconds);
    }
);
timerCountdown.Start();

// Format time function
private string FormatTime(int seconds)
{
    TimeSpan ts = TimeSpan.FromSeconds(seconds);
    return $"{ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
}
```

### Cảnh Báo
- Khi thời gian < 5 phút: Đổi màu thành đỏ (`#FF6464`)
- Khi hết giờ: Hiển thị message > Logout

---

## 🔄 Luồng Hoạt Động

```
1. App khởi chạy
   ↓
2. LockScreenForm hiển thị (fullscreen, khóa)
   ↓
3. Người dùng nhập Tên/Mật khẩu → Click Đăng nhập
   ↓
4. Validation thành công
   ↓
5. LockScreenForm ẩn
   ↓
6. WidgetForm hiển thị ở góc dưới phải
   ↓
7. Timer bắt đầu đếm ngược
   ↓
8. User click "Thoát" → Logout
   ↓
9. Quay lại LockScreenForm
```

---

## 🎮 Gaming Feel Features

✅ **Dark Mode**: Màu tối, chữ sáng (không mỏi mắt)
✅ **Accent Colors**: Xanh dương gaming
✅ **Large Time Display**: Dễ nhìn, dễ theo dõi
✅ **Emoji Icons**: 👤 💰 ⏱️ 📋 💬 🚪
✅ **Smooth Transitions**: Button hover effects
✅ **Minimal UI**: Chỉ hiển thị cần thiết

---

## 📝 Code Highlights

### Căn Giữa Responsive (LockScreenForm)
```csharp
private void CenterLoginPanel()
{
    pnlLoginContainer.Left = (this.ClientSize.Width - pnlLoginContainer.Width) / 2;
    pnlLoginContainer.Top = (this.ClientSize.Height - pnlLoginContainer.Height) / 2;
}

// Gọi khi form resize
this.Resize += (s, e) => CenterLoginPanel();
```

### Timer Đếm Ngược (WidgetForm)
```csharp
private void TimerCountdown_Tick(object sender, EventArgs e)
{
    if (TimeRemainingSeconds > 0)
    {
        TimeRemainingSeconds--;
        lblTimeRemaining.Text = FormatTime(TimeRemainingSeconds);
        
        // Cảnh báo khi < 5 phút
        if (TimeRemainingSeconds < 300)
        {
            lblTimeRemaining.ForeColor = Color.FromArgb(255, 100, 100);
        }
    }
    else
    {
        timerCountdown.Stop();
        MessageBox.Show("Hết giờ chơi!");
        LogoutUser();
    }
}
```

---

## 🚀 Chạy Ứng Dụng

```bash
cd d:\QuanLyQuanNet\ClientApp
dotnet run

# Hoặc debug
dotnet run --debug
```

### Test Scenarios

1. **Normal Login**:
   - Username: `user1`
   - Password: `pass123`

2. **VIP Login**:
   - Username: `vip1`
   - Password: `vip123`

3. **Error Handling**:
   - Để các field trống → Error message
   - Sai tên/mật khẩu → Error message

4. **Timer Test**:
   - Sau login → Widget hiển thị với timer đếm ngược
   - Timer cứ mỗi giây trừ đi 1 giây
   - Click "Thoát" → Logout

---

## 🎨 Customization

### Để thay đổi màu sắc

Chỉnh sửa các `Color` constants ở đầu Form:

```csharp
private readonly Color ColorDarkBg = Color.FromArgb(20, 20, 30);      // Nền
private readonly Color ColorAccent = Color.FromArgb(0, 150, 255);     // Accent
private readonly Color ColorText = Color.FromArgb(240, 240, 240);     // Chữ
```

### Để thay đổi kích thước widget

```csharp
this.Size = new Size(300, 180);  // Chiều rộng x chiều cao
```

### Để thay đổi thứ tự buttons

Chỉnh sửa trong `SetupUI()` method của WidgetForm.

---

## 📞 Support

Các tính năng có thể phát triển thêm:
- [ ] Remember Me checkbox trên login
- [ ] Keyboard shortcuts (Ex: Alt+L = Logout)
- [ ] Sound effects on events
- [ ] Themes switcher (Dark/Light)
- [ ] Multilanguage support
- [ ] Network integration (connect to server)

