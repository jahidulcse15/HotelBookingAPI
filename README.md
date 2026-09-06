# Hotel Booking API

A RESTful Hotel Booking API built with **ASP.NET Core Web API (.NET 10)**, **Entity Framework Core**, and **SQL Server**.

The project demonstrates clean backend development practices including DTOs, Repository Pattern, Dependency Injection, JWT Authentication, Role-Based Authorization, validation, and Swagger/OpenAPI documentation.

## 🚀 Technologies

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* LINQ
* JWT Authentication
* BCrypt Password Hashing
* Repository Pattern
* Dependency Injection
* DTOs
* Swagger / OpenAPI

##  Features

### Hotel Management

* Create hotel
* Get all hotels
* Get hotel by ID
* Update hotel
* Delete hotel

### Room Management

* Create room for a hotel
* Get rooms
* Get room by ID
* Update room
* Delete room
* Room availability management

### Customer Management

* Create customer
* Get customers
* Get customer by ID
* Update customer
* Delete customer

### Booking Management

* Create booking
* Update booking
* Get booking details
* Booking date validation
* Room availability checking
* Automatic total price calculation
* Customer and room relationship management

### Authentication & Authorization

* User registration
* User login
* BCrypt password hashing
* JWT token generation
* JWT authentication
* Role-based authorization
* Customer and Admin roles
* Protected booking endpoints
* Admin-only hotel and room management

## 🏗️ Project Structure

```text
HotelBookingAPI
│
├── Controllers
│   ├── AuthController.cs
│   ├── HotelController.cs
│   ├── RoomController.cs
│   ├── CustomerController.cs
│   └── BookingController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── DTOs
│   ├── RegisterDto.cs
│   ├── LoginDto.cs
│   ├── LoginResponseDto.cs
│   ├── HotelResponseDto.cs
│   ├── RoomResponseDto.cs
│   ├── CustomerResponseDto.cs
│   └── BookingResponseDto.cs
│
├── Models
│   ├── User.cs
│   ├── Hotel.cs
│   ├── Room.cs
│   ├── Customer.cs
│   └── Booking.cs
│
├── Services
│   └── TokenService.cs
│
├── Repositories
│   └── ...
│
├── Migrations
│
├── Program.cs
└── appsettings.json
```

## 🔐 Authentication Flow

1. User registers using the registration endpoint.
2. Password is securely hashed using BCrypt.
3. User logs in with email and password.
4. Credentials are verified against the stored password hash.
5. A JWT token is generated after successful authentication.
6. The token is used to access protected endpoints.
7. Role-based authorization controls Admin-only operations.

## 📌 Authorization

| Resource         | Customer | Admin |
| ---------------- | :------: | :---: |
| Booking          |     ✅    |   ✅   |
| Hotel Management |     ❌    |   ✅   |
| Room Management  |     ❌    |   ✅   |

## 📖 API Documentation

Swagger / OpenAPI is integrated for API documentation and testing.

After running the project, open the Swagger UI from the application's configured Swagger URL.

Use the **Authorize** button in Swagger to provide the JWT token:

```text
Bearer YOUR_JWT_TOKEN
```

## ⚙️ Database

The application uses **SQL Server** with **Entity Framework Core**.

Database migrations are used to create and update the database schema.

```powershell
Add-Migration InitialCreate
Update-Database
```

## 🎯 Project Purpose

This project was developed as a portfolio project to demonstrate practical knowledge of:

* RESTful API development
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Repository Pattern
* Dependency Injection
* DTO-based API design
* Authentication and Authorization
* JWT
* Password hashing
* API validation
* Swagger/OpenAPI

## 👨‍💻 Author

**Md. Jahidul Islam**

CSE Graduate | ASP.NET Core / .NET Developer
