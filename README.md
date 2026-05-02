# IT Book Online Shop — Backend API

A RESTful API built with **ASP.NET Core 8.0** and **Entity Framework Core (SQLite)** for managing user authentication and book likes.

---

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

---

## Getting Started

### 1. Clone the repository

```bash
git clone <your-repository-url>
cd Backend_Test
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Run the application

```bash
dotnet run
```

The database (`app.db`) is created automatically on first startup.

---

## API Endpoints

Swagger UI is available at `https://localhost:<port>/swagger` when running in Development mode.
Use the **Authorize** button in Swagger UI to input your Bearer token before calling protected endpoints.

### Authentication

| Method | Endpoint    | Description                     | Auth Required |
|--------|-------------|---------------------------------|---------------|
| POST   | /register   | Create a new user account       | No            |
| POST   | /login      | Login and receive a JWT token   | No            |

**POST /register**
```json
{
  "username": "john",
  "password": "password123",
  "fullName": "John Doe"
}
```

**POST /login**
```json
{
  "username": "john",
  "password": "password123"
}
```
Returns a JWT Bearer token. Use this token in the `Authorization: Bearer <token>` header for protected routes.

---

### Books

| Method | Endpoint    | Description                                         | Auth Required |
|--------|-------------|-----------------------------------------------------|---------------|
| GET    | /books      | Fetch books from IT Book Store, sorted A-Z by title | Yes           |
| POST   | /user/like  | Like a book and store it                            | Yes           |

**POST /user/like**
```json
{
  "user_id": 1,
  "book_id": "9781484200087"
}
```

---

## Project Structure

```
Backend_Test/
├── Controllers/
│   ├── AuthController.cs    # POST /login, POST /register
│   └── BookController.cs    # GET /books, POST /user/like
├── Data/
│   └── ApiDbContext.cs      # EF Core DbContext
├── Models/
│   ├── Book.cs              # Book & BookApiResponse models
│   ├── User.cs              # User model + Auth DTOs
│   └── Userlike.cs          # UserLike model + LikeRequest DTO
├── appsettings.json         # Configuration (JWT, DB connection)
└── Program.cs               # App setup and middleware pipeline
```

---

## Configuration

JWT settings are in `appsettings.json`. For production, replace the `Jwt:Key` with a secure secret and manage it via environment variables or a secrets manager.

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

## External API

Books are fetched from the [IT Book Store API](https://api.itbook.store):

```
GET https://api.itbook.store/1.0/search/mysql
```

