# 🏨 Hotel Booking API

A RESTful Hotel Booking API built with **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server**.

---

## 🚀 Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- RESTful API
- DTOs
- Dependency Injection
- Repository Pattern
- Swagger / OpenAPI
- LINQ
- CRUD Operations

---

## ✨ Features

- 🏨 Hotel Management
- 🛏️ Room Management
- 👤 Customer Management
- 📅 Booking Management
- 🔍 Room Availability Checking
- 📆 Booking Date Validation
- 💰 Automatic Total Price Calculation
- 🔄 CRUD Operations
- 📡 RESTful API Endpoints
- 📖 Swagger API Documentation

---

## 🏗️ Project Structure

```text
HotelBookingAPI
│
├── Controllers
│   ├── HotelController.cs
│   ├── RoomController.cs
│   ├── CustomerController.cs
│   └── BookingController.cs
│
├── Models
│   ├── Hotel.cs
│   ├── Room.cs
│   ├── Customer.cs
│   └── Booking.cs
│
├── DTOs
│   ├── HotelResponseDto.cs
│   ├── RoomResponseDto.cs
│   ├── CustomerResponseDto.cs
│   └── BookingResponseDto.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Repository
│
├── Interfaces
│
├── Migrations
│
├── Program.cs
└── appsettings.json
