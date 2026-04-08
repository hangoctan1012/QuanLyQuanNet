# QuanLyQuanNet

Phần mềm quản lý quán net được xây dựng bằng C# (.NET 10) theo mô hình Client-Server.

## Tổng quan

Workspace này gồm 3 project:

- `ServerAdmin`: ứng dụng WinForms cho admin, TCP server, truy cập dữ liệu SQLite + Dapper.
- `ClientApp`: ứng dụng WinForms máy trạm/kiosk cho người dùng.
- `SharedModels`: thư viện model dùng chung cho cả server và client.

Hệ thống giao tiếp qua TCP bằng thông điệp JSON theo từng dòng, theo mẫu:

```json
{ "Action": "SomeAction", "Payload": "{...}" }
```

## Yêu cầu môi trường

- Windows (chỉ hỗ trợ WinForms)
- .NET 10 SDK

## Build

Chạy từ thư mục gốc repository:

```bash
dotnet restore
dotnet build
```

## Chạy ứng dụng

Chạy server:

```bash
dotnet run --project ServerAdmin/ServerAdmin.csproj
```

Chạy client (ở terminal khác):

```bash
dotnet run --project ClientApp/ClientApp.csproj
```

Cổng TCP mặc định là `5000`.

## Khởi tạo cơ sở dữ liệu

Khi server khởi động, quá trình khởi tạo CSDL được gọi trong `ServerAdmin/Program.cs`:

- `DatabaseHelper.InitializeDatabase()`

File SQLite `QuanLyQuanNet.db` sẽ được tạo tự động ở lần chạy đầu tiên.

## Lưu ý khi thay đổi protocol

Nếu bạn thêm hoặc sửa action message, hãy cập nhật đồng thời cả hai bên để tránh lỗi login/session:

- `ServerAdmin/NetworkServer.cs`
- `ClientApp/NetworkClient.cs`

## Cấu trúc dự án

```text
QuanLyQuanNet/
|-- QuanLyQuanNet.slnx
|-- README.md
|-- SharedModels/
|-- ServerAdmin/
`-- ClientApp/
```

## Tài liệu bổ sung

- `ServerAdmin/UI_DESIGN.md`
- `ClientApp/CLIENTUI_DESIGN.md`

## Các vấn đề thường gặp

- Đây là solution WinForms chỉ dành cho Windows (`net10.0-windows` với app project).
- Thay đổi protocol có thể gây lỗi ngắt quãng nếu chỉ cập nhật một bên.
- SQLite có thể bị tranh chấp ghi nếu có nhiều thao tác ghi đồng thời.

## Trạng thái hiện tại

Dự án đang trong quá trình phát triển.
