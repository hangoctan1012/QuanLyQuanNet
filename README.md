# QuanLyQuanNet

Phan mem Quan Ly Quan Net theo mo hinh Client-Server, xay dung bang C# (.NET 10) va WinForms.

He thong gom 3 project:
- `ServerAdmin`: ung dung quan ly phong may (WinForms + TCP server + SQLite/Dapper).
- `ClientApp`: ung dung may tram/kiosk cho nguoi choi (WinForms).
- `SharedModels`: thu vien model dung chung.

## Tong Quan Kien Truc

- Giao tiep qua TCP voi thong diep JSON theo dong (`Action` + `Payload`).
- Server khoi tao SQLite khi start (`QuanLyQuanNet.db`).
- Client hien tai dang o giai doan demo UI/login, chua noi full flow xac thuc voi server.

## Trang Thai Hien Tai

### ServerAdmin
- Khoi tao CSDL tu dong khi chay (`DatabaseHelper.InitializeDatabase()`).
- Chay TCP server mac dinh cong `5000`.
- Giao dien admin hien tai su dung du lieu may mau (mock cards) de demo UX.
- Da co xu ly message co ban trong `NetworkServer`:
  - `Identify`
  - `Login`

### ClientApp
- Khoi dong vao `LockScreenForm`.
- Dang nhap dang la mock validation trong client:
  - `user1 / pass123`
  - `vip1 / vip123`
- Sau khi dang nhap se mo `WidgetForm` voi so du/thoi gian mock.

## Yeu Cau Moi Truong

- Windows (WinForms only).
- .NET 10 SDK.

## Chay Du An

Tu thu muc goc repo:

```bash
dotnet restore
dotnet build
```

Chay server:

```bash
dotnet run --project ServerAdmin/ServerAdmin.csproj
```

Chay client:

```bash
dotnet run --project ClientApp/ClientApp.csproj
```

## Du Lieu Mac Dinh

Khi `ServerAdmin` chay lan dau, he thong tao:
- Database: `QuanLyQuanNet.db`
- 1 tai khoan admin:
  - Username: `admin`
  - Password: `1`
- 10 may tinh mac dinh (`May 01` -> `May 10`) trong bang `Computers`

Luu y: thong tin dang nhap demo cua `ClientApp` hien tai la mock trong UI, khong doc tu DB.

## Cau Truc Thu Muc

```text
QuanLyQuanNet/
|-- QuanLyQuanNet.slnx
|-- README.md
|-- FORM1_COMPLETE_SOURCE.cs
|-- SharedModels/
|   |-- Models.cs
|   `-- SharedModels.csproj
|-- ServerAdmin/
|   |-- Program.cs
|   |-- Form1.cs
|   |-- Form1.Designer.cs
|   |-- ComputerCard.cs
|   |-- NetworkServer.cs
|   |-- DatabaseHelper.cs
|   |-- InputBox.cs
|   |-- UI_DESIGN.md
|   `-- ServerAdmin.csproj
`-- ClientApp/
    |-- Program.cs
    |-- Form1.cs
    |-- Form1.Designer.cs
    |-- LockScreenForm.cs
    |-- LockScreenForm.Designer.cs
    |-- WidgetForm.cs
    |-- WidgetForm.Designer.cs
    |-- NetworkClient.cs
    |-- InputBox.cs
    |-- CLIENTUI_DESIGN.md
    `-- ClientApp.csproj
```

## Message Protocol (Hien Tai)

Thong diep theo format:

```json
{ "Action": "Login", "Payload": "{...}" }
```

Mot so action da co trong server:
- `Identify`
- `Login`

Khuyen nghi: khi mo rong protocol, cap nhat dong bo ca `ServerAdmin/NetworkServer.cs` va `ClientApp/NetworkClient.cs`.

## Tai Lieu UI

- Server admin UI: `ServerAdmin/UI_DESIGN.md`
- Client UI: `ClientApp/CLIENTUI_DESIGN.md`

## Su Co Thuong Gap

- Khong ket noi duoc client-server:
  - Dam bao server da chay truoc.
  - Kiem tra firewall cho port `5000`.

- SQLite bi lock:
  - Dong tat ca process dang mo file DB.
  - Chay lai `ServerAdmin`.

## Cong Nghe

- C# / .NET 10
- WinForms
- SQLite (`Microsoft.Data.Sqlite`)
- Dapper
- TCP sockets

## Ghi Chu Phat Trien

- Solution su dung `QuanLyQuanNet.slnx`.
- Huong dan nhanh:
  - `dotnet build`
  - `dotnet run --project ServerAdmin/ServerAdmin.csproj`
  - `dotnet run --project ClientApp/ClientApp.csproj`

## Liên Hệ & Support

Nếu gặp vấn đề hoặc có đề xuất cải tiến:
- Tạo Issue trên GitHub
- Gửi mail: hangoctan1012@example.com (placeholder)
- Chat trực tiếp qua ứng dụng Chat (Đang trong hệ thống)

---

> **Last Updated**: April 2026
> **Current Version**: 1.0 Beta
> ⭐ Nếu thích dự án, vui lòng Star repo này! ⭐
