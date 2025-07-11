
# 🌾 CropDeal Backend

CropDeal is a microservices-based platform that connects **Farmers** and **Dealers**, allowing transparent crop transactions and streamlined management. This repository contains the backend code for all microservices in the CropDeal ecosystem.

---

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core (C#)
- **Architecture**: Microservices
- **Database**: SQL Server
- **Communication**: REST APIs
- **Authentication**: JWT-based
- **Frontend**: Angular (separate repository)

---

## 🧱 Microservices Overview

### 1. **Authentication Service**
Handles user registration, login, and role-based JWT token generation.

- **Endpoints**:
  - `POST /api/auth/signup`
  - `POST /api/auth/login`

### 2. **Farmer Service**
Manages farmer profiles and crop publishing.

- **Endpoints**:
  - `GET /api/farmer/{id}`
  - `PUT /api/farmer/update/{id}`
  - `POST /api/farmer/publish-crop`
  - `GET /api/farmer/crops`
  - `GET /api/farmer/transactions?farmerId&startDate&endDate`

### 3. **Dealer Service**
Enables dealers to view and purchase crops and manage their profile.

- **Endpoints**:
  - `GET /api/dealer/{id}`
  - `PUT /api/dealer/update/{id}`
  - `GET /api/dealer/crops`
  - `POST /api/dealer/buy-crop`
  - `GET /api/dealer/transactions?dealerId&startDate&endDate`

### 4. **Admin Service**
Admin-level functionalities (activation, monitoring, etc.) – under development or customizable.

### 5. **Transaction Service**
Handles all crop transaction records between farmers and dealers.

- **Endpoints**:
  - `POST /api/transaction/create`
  - `GET /api/transaction/farmer?farmerId&startDate&endDate`
  - `GET /api/transaction/dealer?dealerId&startDate&endDate`

---

## 📊 Database Schema Summary

### `Transactions` Table (TransactionDB)
| Column            | Type         |
|-------------------|--------------|
| TransactionId     | INT (PK)     |
| DealerId          | INT          |
| FarmerId          | INT          |
| CropId            | INT          |
| CropName          | NVARCHAR(100)|
| Quantity          | INT          |
| TotalAmount       | FLOAT        |
| TransactionStatus | BIT          |
| TransactionDate   | DATETIME     |

---

## 🔐 Authentication Flow

- User signs up with role: `Farmer` or `Dealer`.
- System registers them in:
  - `AuthenticationDB` (for credentials)
  - `FarmerDB` or `DealerDB` (based on role)
- On login, a **JWT token** is issued and used for secure access to protected APIs.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 7+ SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/) or any API testing tool

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/cropdeal-backend.git
   cd cropdeal-backend
   ```

2. **Configure connection strings**
   - Go to `appsettings.json` in each microservice.
   - Update your SQL Server connection string.

3. **Run the services**
   ```bash
   dotnet run --project ./FarmerService/FarmerService.csproj
   dotnet run --project ./DealerService/DealerService.csproj
   dotnet run --project ./AuthService/AuthService.csproj
   dotnet run --project ./TransactionService/TransactionService.csproj
   ```

4. **Test the endpoints**
   - Use Swagger or Postman to interact with each service.

---

## 🧪 Testing

Each microservice includes its own:
- Controllers
- Process layer
- Repository layer

Unit testing and integration testing can be added using:
- xUnit
- Moq
- Entity Framework Core In-Memory DB

---

## 📁 Folder Structure (Example)

```
CropDeal-Backend/
├── AuthService/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── appsettings.json
├── FarmerService/
│   ├── Controllers/
│   ├── Process/
│   ├── Repository/
│   └── Models/
├── DealerService/
├── TransactionService/
└── Shared/
```

---

## 📌 Features Summary

| Role     | Key Features                                         |
|----------|------------------------------------------------------|
| Farmer   | Register, Manage Profile, Publish Crops, View Sales |
| Dealer   | Register, View Crops, Buy Crops, Track Purchases     |
| Admin    | Activate/Deactivate Users, Monitor Activity          |
| Common   | Secure Login/Signup, JWT Auth, Role-based Access     |

---

## 📃 License

This project is licensed under the MIT License.

---

## 🤝 Contributing

Pull requests are welcome! For major changes, open an issue first to discuss what you would like to change.

---

## 📬 Contact

- Created by [Pranav Varshney](https://github.com/claudikt)
- For queries: prnvvarshney@gmail.com
