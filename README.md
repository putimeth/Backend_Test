# IT Book Online Shop — Backend API

A RESTful API built with **ASP.NET Core 8.0** and **Entity Framework Core (SQLite)** for managing user authentication and book likes.

> **ภาษาไทย:** โปรเจกต์นี้เป็น RESTful API สำหรับระบบร้านหนังสือออนไลน์ด้านไอที พัฒนาด้วย ASP.NET Core 8.0 และ Entity Framework Core (SQLite) รองรับการจัดการ Authentication และการกดถูกใจหนังสือ

---

## Repository

- **GitHub:** https://github.com/putimeth/Backend_Test.git
- **Swagger UI:** http://localhost:5045/swagger

---

## Prerequisites
## สิ่งที่ต้องติดตั้งก่อน

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

---

## Getting Started / วิธีเริ่มต้นใช้งาน

### 1. Clone the repository / โคลน repository

```bash
git clone https://github.com/putimeth/Backend_Test.git
cd Backend_Test
```

### 2. Restore dependencies / ติดตั้ง dependencies

```bash
dotnet restore
```

### 3. Run the application / รันแอปพลิเคชัน

```bash
dotnet run
```

The database (`app.db`) is created automatically on first startup.

> **ภาษาไทย:** ฐานข้อมูล `app.db` จะถูกสร้างขึ้นโดยอัตโนมัติเมื่อรันครั้งแรก ไม่จำเป็นต้องตั้งค่าฐานข้อมูลเพิ่มเติม

---

## API Endpoints

Swagger UI is available at **http://localhost:5045/swagger** when running in Development mode.
Use the **Authorize** button in Swagger UI to input your Bearer token before calling protected endpoints.

> **ภาษาไทย:** เมื่อรันโปรเจกต์แล้วสามารถทดสอบ API ได้ผ่าน Swagger UI ที่ **http://localhost:5045/swagger** โดยกดปุ่ม **Authorize** เพื่อใส่ JWT Token ก่อนเรียก endpoint ที่ต้องการ Authentication

### Authentication / การยืนยันตัวตน

| Method | Endpoint    | Description (EN)                | คำอธิบาย (TH)                        | Auth Required |
|--------|-------------|---------------------------------|--------------------------------------|---------------|
| POST   | /register   | Create a new user account       | สร้างบัญชีผู้ใช้ใหม่                  | No            |
| POST   | /login      | Login and receive a JWT token   | เข้าสู่ระบบและรับ JWT Token           | No            |

**POST /register** — สร้างบัญชีผู้ใช้ใหม่
```json
{
  "username": "john",
  "password": "password123",
  "fullName": "John Doe"
}
```

**POST /login** — เข้าสู่ระบบ
```json
{
  "username": "john",
  "password": "password123"
}
```
Returns a JWT Bearer token. Use this token in the `Authorization: Bearer <token>` header for protected routes.

> **ภาษาไทย:** หลัง Login สำเร็จจะได้รับ JWT Token กลับมา ให้นำ Token ไปใส่ใน Header `Authorization: Bearer <token>` เพื่อเรียก endpoint ที่ต้องการสิทธิ์

---

### Books / หนังสือ

| Method | Endpoint    | Description (EN)                                    | คำอธิบาย (TH)                              | Auth Required |
|--------|-------------|-----------------------------------------------------|--------------------------------------------|---------------|
| GET    | /books      | Fetch books from IT Book Store, sorted A-Z by title | ดึงรายการหนังสือเรียงตามชื่อ A-Z           | Yes           |
| POST   | /user/like  | Like a book and store it                            | กดถูกใจหนังสือและบันทึกลงฐานข้อมูล        | Yes           |

**POST /user/like** — กดถูกใจหนังสือ
```json
{
  "user_id": 1,
  "book_id": "9781484200087"
}
```

---

## Project Structure / โครงสร้างโปรเจกต์

```
Backend_Test/
├── Controllers/
│   ├── AuthController.cs    # POST /login, POST /register
│   └── BookController.cs    # GET /books, POST /user/like
├── Data/
│   └── ApiDbContext.cs      # EF Core DbContext — จัดการฐานข้อมูล
├── Models/
│   ├── Book.cs              # Book & BookApiResponse models
│   ├── User.cs              # User model + Auth DTOs
│   └── Userlike.cs          # UserLike model + LikeRequest DTO
├── appsettings.json         # Configuration (JWT, DB connection) — ไฟล์ตั้งค่า
└── Program.cs               # App setup and middleware pipeline — จุดเริ่มต้นแอป
```

---

## Configuration / การตั้งค่า

JWT settings are in `appsettings.json`. For production, replace the `Jwt:Key` with a secure secret and manage it via environment variables or a secrets manager.

> **ภาษาไทย:** ค่าตั้งต่างๆ ของ JWT อยู่ในไฟล์ `appsettings.json` หากนำไปใช้งานจริง (Production) ควรเปลี่ยน `Jwt:Key` เป็น Secret ที่ปลอดภัย และจัดการผ่าน Environment Variables แทนการเขียนตรงในไฟล์

```json
{
  "Jwt": {
    "Key": "YourSecretKeyHere",
    "Issuer": "Backend_Test",
    "Audience": "Backend_Test",
    "ExpiresInMinutes": 60
  }
}
```

---

## External API / API ภายนอก

Books are fetched from the [IT Book Store API](https://api.itbook.store).

> **ภาษาไทย:** ข้อมูลหนังสือดึงมาจาก IT Book Store API โดยอัตโนมัติ และเรียงลำดับตามชื่อหนังสือจาก A ถึง Z

```
GET https://api.itbook.store/1.0/search/mysql
```