# QuanLyQuanNet (Internet Cafe Management)

Dự án phần mềm Quản lý Quán Net mô hình Client-Server, được viết bằng ngôn ngữ C# (.NET 10 WinForms), sử dụng giao thức TCP Sockets để giao tiếp thời gian thực và CSDL SQLite gọn nhẹ. 

Hệ thống bao gồm 2 ứng dụng:
- **ServerAdmin**: Ứng dụng Quản lý Máy chủ dành cho Thu ngân / Kỹ thuật viên.
- **ClientApp**: Ứng dụng Máy trạm dành cho Khách hàng sử dụng.

---

## Tính năng (Features)

### ServerAdmin (Máy chủ Admin)
- Hiển thị danh sách thiết bị dưới dạng lưới (Grid), với màu sắc thể hiện trạng thái (Trống - Đang sử dụng).
- Quản lý tài khoản đăng nhập cho Client.
- Giao tiếp với Client (Nhắn tin, nạp tiền vào tài khoản).
- Điều khiển trạng thái máy Client từ xa (Tắt máy, Khởi động lại).

### ClientApp (Máy trạm Khách chơi)
- Khởi động mặc định vào chế độ khóa màn hình (thay thế Window Lockscreen).
- Yêu cầu xác thực tài khoản để mở máy.
- Floating Widget (chỉ hiển thị góc màn hình hoặc cửa sổ) thông báo: **Tên tài khoản, Số dư (VNĐ), và Thời gian còn lại (Đếm ngược)**.
- Giao diện nạp tiền và nhận tin nhắn trực tiếp từ Admin.
- Tự động khóa màn hình và logout khi hết thời gian nạp.

---

## Hướng dẫn Cài đặt & Sử dụng (How to Run)

### 1. Yêu cầu hệ thống (Prerequisites)
- Đã cài đặt [**.NET 10 SDK**](https://dotnet.microsoft.com/download/dotnet/10.0)
- Hệ điều hành Windows (Dành cho UI WinForms).
- Có kết nối mạng Lan / TCP nội bộ (Mặc định test sẽ dùng `127.0.0.1` trên cùng một máy ảo).

### 2. Hướng dẫn khởi chạy để Test trên 1 Máy tính
Các ứng dụng cần được dịch biên dịch và chạy thông qua Terminal riêng biệt.

**Bước 1: Khởi chạy Máy chủ Thử nghiệm (ServerAdmin)**
Mở Command Prompt/PowerShell hoặc Terminal trên thư mục gốc `QuanLyQuanNet` và chạy:
```bash
dotnet run --project ServerAdmin/ServerAdmin.csproj
```
*(Server sẽ tự tạo Database tên là `QuanLyQuanNet.db` cùng với 1 tài khoản `admin` / mật khẩu `1` và 10 máy tính mẫu giả lập).*

**Bước 2: Khởi chạy Máy trạm (ClientApp)**
Kế tiếp, mở một Terminal thứ hai và chạy:
```bash
dotnet run --project ClientApp/ClientApp.csproj
```

**Bước 3: Màn hình Client App Test**
1. Nhập thông tin: `admin` / Pass: `1`.
2. Trên màn hình Form của Client sẽ thu bé lại dạng Widget nhỏ, báo máy đã mở và đếm lui thời gian.
3. Chuyển lại cửa sổ Server, bạn sẽ thấy máy số hiệu này chuyển thành màu Đỏ (InUse).
4. Bạn có thể Bấm chuột phải vào máy đang hiển thị Đỏ trên Server để Nạp Tiền tự động, Gửi tin nhắn hoặc gửi lệnh Reset Máy. 

---

## Cấu trúc Mã Nguồn (Project Structure)
Dự án được triển khai trên 1 hệ thống Solution `QuanLyQuanNet.sln` bao gồm 3 Module chính:

1. **SharedModels** (Thư viện Class Library)
   - Lưu trữ các Model Database (Người dùng `User`, Máy trạm `Computer`, Đơn hàng `Order`...). Dùng chung cho cả Server lẫn Client.
2. **ServerAdmin** (WinForms)
   - Chứa `NetworkServer.cs` quản lý thư viện nghe gọi TCP Socket và điều khiển thông điệp (Message).
   - Chứa `DatabaseHelper.cs` chuyên xử lý SQLite CRUD (gọi qua Dapper).
   - Giao diện Admin chính `Form1.cs`.
3. **ClientApp** (WinForms)
   - Chứa `NetworkClient.cs` quản lý Socket gửi thông điệp và chờ dữ liệu từ Server.
   - Chứa `Form1.cs` là Màn hình Đăng nhập (Lock Screen).
   - Chứa `WidgetForm.cs` đại diện cho màn hình đếm lùi Desktop.

---
*Dự án đang trong quá trình phát triển (WIP). Sẽ có thêm menu order và chức năng phức tạp khác.*
*(c) 2026 bởi hangoctan1012.*
