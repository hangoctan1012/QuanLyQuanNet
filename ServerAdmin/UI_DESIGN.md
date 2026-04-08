# ServerAdmin UI Design Documentation

## 📋 Tổng Quan
Giao diện **ServerAdmin** được thiết kế với phong cách **Modern Flat UI**, chia làm 2 khu vực chính:
1. **Sidebar (Trái)**: Navigation menu với các nút điều hướng
2. **Main Content (Phải)**: Khu vực hiển thị nội dung chính, thay đổi theo nút bấm

---

## 🎨 Thành Phần Giao Diện

### 1. Sidebar (Panel Trái)
- **Chiều rộng**: 220px
- **Màu nền**: Xanh dương (`#2196F3`)
- **Thành phần**:
  - **Logo Area** (80px): Tiêu đề "ServerAdmin" với màu nền đậm hơn (`#0D47A1`)
  - **Navigation Buttons**: 5 nút điều hướng
    - 🖥 Quản lý Máy
    - 👤 Tài khoản
    - 📦 Đơn dịch vụ
    - 💬 Chat
    - ⚙ Cài đặt

### 2. Main Content Area (Panel Phải)
- **Header**: Hiển thị tiêu đề của trang hiện tại (70px cao)
- **Content Panel**: Khu vực chính để hiển thị nội dung

---

## 🖥️ Màn hình "Quản lý Máy Trạm" (Computer Management)

### Cấu Trúc
Sử dụng `FlowLayoutPanel` để hiển thị danh sách các máy tính dưới dạng lưới.

### Thẻ Máy (ComputerCard)
Mỗi máy được hiển thị là một Panel vuông (150x150px) với các thông tin:
- **Tên máy**: VD: "Máy 01"
- **Người dùng**: Tên user đang dùng (hoặc "Trống")
- **Số dư**: Hiển thị số tiền còn lại
- **Trạng thái**: Hiển thị status hiện tại

### Màu sắc theo trạng thái
| Trạng Thái | Màu | RGB |
|-----------|-----|-----|
| Available (Trống) | Xanh Lá | #4CAF50 |
| InUse (Đang dùng) | Cam | #FF9800 |
| Offline (Ngoại tuyến) | Xám | #9E9E9E |
| Maintenance (Bảo trì) | Xám Nhạt | #BDBDBD |

### Context Menu (Chuột Phải)
Khi click chuột phải trên một máy, hiển thị menu với các tùy chọn:
- ✓ **Nạp tiền**
- 📧 **Nhắn tin**
- ------- (separator)
- 🔒 **Khóa máy**
- 🔄 **Khởi động lại**

---

## 🎨 Bảng Màu (Color Scheme)

### Sidebar & Navigation
- **Sidebar Primary**: `#2196F3` (Xanh dương)
- **Sidebar Dark**: `#0D47A1` (Xanh dương đậm)
- **Button Normal**: `#42A5F5` (Xanh dương nhạt)
- **Button Hover**: `#64B5F6` (Xanh dương rất nhạt)
- **Text**: `#FFFFFF` (Trắng)

### Computer Status
- **Available**: `#4CAF50` (Xanh lá)
- **InUse**: `#FF9800` (Cam)
- **Offline**: `#9E9E9E` (Xám)
- **Maintenance**: `#BDBDBD` (Xám nhạt)

### Content Area
- **Background**: `#F0F0F0` (Xám sáng)
- **Text (Header)**: `#212121` (Đen)

---

## 🔧 Dữ Liệu Mock

Ứng dụng đi kèm với 10 máy tính giả (Mock Data):
- **Máy 01** - Máy 10
- **Trạng thái**: Xoay vòng giữa Available, InUse, Offline, Maintenance
- **Người dùng**: Khác nhau, một số máy trống
- **Số dư**: Từ 50,000 - 500,000 VNĐ

---

## 📱 Responsive Design

- **Form Size**: 1300x800px
- **Sidebar Width**: 220px
- **AutoScroll**: Enabled trên Main Content Panel
- **Wrap**: FlowLayoutPanel tự động xuống dòng khi không đủ chỗ

---

## 🚀 Cách Sử Dụng

### Khởi chạy
```bash
dotnet run --project ServerAdmin/ServerAdmin.csproj
```

### Điều hướng
1. Click vào các nút ở Sidebar để chuyển đổi giữa các màn hình
2. Màn hình hiện tại sẽ được highlight (màu sáng hơn)

### Tương tác với Máy
- **Click Trái**: Xem thông tin chi tiết
- **Click Phải**: Mở Context Menu với các tùy chọn quản lý

---

## 📝 Cấu Trúc Code

### Files Chính
- **Form1.cs**: Form chính với layout và navigation
- **ComputerCard.cs**: UserControl hiển thị một máy tính
- **Form1.Designer.cs**: Auto-generated designer file (minimal)

### Classes
- `Form1`: MainForm
- `ComputerCard`: Panel tùy chỉnh để hiển thị máy
- `ComputerModel`: Model dữ liệu máy tính
- `ComputerStatus`: Enum trạng thái máy

### Enums
```csharp
public enum ComputerStatus
{
    Available,      // Trống
    InUse,          // Đang dùng
    Offline,        // Ngoại tuyến
    Maintenance     // Bảo trì
}
```

---

## 🎯 Tính Năng Cơ Bản

### ✅ Đã Triển Khai
- [x] Modern Flat UI Design
- [x] Sidebar Navigation
- [x] Computer Grid View
- [x] Color-coded Status
- [x] Context Menu
- [x] Mock Data (10 computers)
- [x] Page Navigation
- [x] Responsive Layout

### ⏳ Chuẩn Bị Phát Triển
- [ ] Account Management
- [ ] Service Order Management
- [ ] Chat System
- [ ] Settings Panel
- [ ] Real Database Integration
- [ ] Network Communication

---

## 📸 Preview

```
┌─────────────────────────────────────────────────┐
│ ServerAdmin ┌───────── Quản lý Máy Trạm ────────│
├──────────┬──────────────────────────────────────┤
│  🖥      │ [Máy01] [Máy02] [Máy03] [Máy04]    │
│Quản lý   │ [Máy05] [Máy06] [Máy07] [Máy08]    │
│Máy       │ [Máy09] [Máy10]                    │
│          │                                      │
│👤       │                                      │
│Tài      │                                      │
│khoản    │                                      │
│          │                                      │
│📦       │                                      │
│Đơn      │                                      │
│dịch     │                                      │
│vụ       │                                      │
│          │                                      │
│💬       │                                      │
│Chat     │                                      │
│          │                                      │
│⚙        │                                      │
│Cài      │                                      │
│đặt      │                                      │
└──────────┴──────────────────────────────────────┘
```

---

## 🔗 Liên Kết & Tham Khảo

- **WinForms Documentation**: https://docs.microsoft.com/en-us/dotnet/desktop/winforms
- **Flat UI Design**: https://en.wikipedia.org/wiki/Flat_design
- **Material Design Colors**: https://material.io/design/color

---

## 📞 Hỗ Trợ

Nếu gặp vấn đề:
1. Kiểm tra .NET version: `dotnet --version`
2. Build lại solution: `dotnet build`
3. Xem message log ở console

