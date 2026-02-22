# 🐦 Twitter Clone (Clean Architecture & Real-Time WebSockets)

A full-stack Twitter clone built with **.NET 10** (C#) and **Vue 3**. This project demonstrates strict adherence to **Clean Architecture** principles, the CQRS pattern using MediatR, and features a native WebSocket implementation for real-time feed updates.

## ✨ Features

* **User Authentication**: Secure JWT-based registration and login with BCrypt password hashing.
* **Real-Time Feed**: Native WebSockets push new tweets instantly to all connected clients without needing a page refresh.
* **Post Tweets**: Users can compose and publish tweets (max 280 characters).
* **Comment System**: Users can reply to tweets, with replies threaded chronologically under the parent tweet.
* **Optimistic UI**: The Vue 3 frontend instantly updates the UI when adding comments while syncing with the backend in the background.
* **Global Design System**: A clean, reusable, Twitter-inspired CSS architecture.

## 🛠️ Tech Stack

**Backend:**
* C# / .NET 10
* PostgreSQL
* Entity Framework Core (Code-First Migrations)
* MediatR (CQRS Pattern)
* Native WebSockets (`System.Net.WebSockets`)

**Frontend:**
* Vue 3 (Composition API)
* Vite (Build tool)
* Pinia (State Management)
* Vue Router (SPA Navigation)
* Axios (REST API communication)

---

## 🏗️ Architecture (Clean Architecture)

The backend strictly follows Clean Architecture, ensuring that core business logic is isolated from UI, database, and external frameworks. Dependencies only flow **inward**.

* **`TwitterClone.Domain` (Core)**: Contains enterprise-wide logic and types. Includes the primary entities (`User`, `Tweet`, `Comment`) and the `Result` pattern for graceful error handling. This layer has zero external dependencies.
* **`TwitterClone.Application` (Use Cases)**: Contains business logic organized by features (CQRS). It defines the interfaces (`IUserRepository`, `IRealTimeNotifier`) that outer layers must implement. Uses MediatR to handle Commands and Queries.
* **`TwitterClone.Infrastructure` (Implementation)**: Connects to the outside world. Contains the Entity Framework `TwitterDbContext`, database migrations, repository implementations, and the native `WebSocketConnectionManager`.
* **`TwitterClone.API` (Presentation)**: The entry point of the application. Handles HTTP requests/responses, JWT authentication middleware, routing to MediatR handlers, and WebSocket handshake requests.

### 📂 Project Structure

```text
├── backend
│   ├── TwitterClone.API            # REST Controllers, JWT Auth, & WebSocket Middleware
│   │   ├── Controllers
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── TwitterClone.Application    # Use Cases, CQRS (MediatR), DTOs, & Interfaces
│   │   ├── Auth, Comments, Tweets
│   │   └── Interfaces
│   ├── TwitterClone.Domain         # Core entities (User, Tweet, Comment) & Domain logic
│   │   ├── Common
│   │   └── Entities
│   ├── TwitterClone.Infrastructure # EF Core DbContext, Repositories, & WebSocket Manager
│   │   ├── Data
│   │   ├── Migrations
│   │   ├── Repositories
│   │   └── Services
│   └── assets
│       └── twitter-clone-erd.png   # Entity Relationship Diagram
└── frontend
    ├── public
    ├── src
    │   ├── components              # Vue components (HomeFeed, Login, Register)
    │   ├── stores                  # Pinia state management (authStore, feedStore)
    │   ├── utils                   # Helper functions (e.g., Date formatting)
    │   ├── App.vue
    │   ├── main.js
    │   ├── router.js               # Vue Router configuration
    │   └── style.css               # Global Design System
    ├── package.json
    └── vite.config.js
```

## 🚀 Setup & Installation
### Prerequisites
- .NET 10 SDK
- Node.js (v18+ recommended)
- PostgreSQL

### 1. Database Configuration
1. Open PostgreSQL and ensure you have a database server running.
2. Navigate to backend/TwitterClone.API/appsettings.json.
3. Update the DefaultConnection string with your PostgreSQL credentials:
```text
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=twitter_clone_db;Username=postgres;Password=yourpassword"
}
```

### 2. Backend Setup
Open a terminal and navigate to the `backend` folder.
```bash
# Navigate to the backend directory
cd backend

# Restore .NET packages
dotnet restore

# Apply database migrations to create the tables
dotnet ef database update --project TwitterClone.Infrastructure --startup-project TwitterClone.API
```

### 3. Frontend Setup
1. Configure the .env file that follows .env.example 
```bash
cp frontend/.env.example frontend/.env
```
2. Open a new terminal and navigate to the `frontend` folder.
```bash
# Navigate to the frontend directory
cd frontend

# Install NPM dependencies
npm install
```

## 💻 Running the Application
You will need two terminals open to run the backend and frontend simultaneously.
### Start the Backend (Terminal 1):
```bash
cd backend/TwitterClone.API
dotnet run
```
The API will start on http://localhost:5032

### Start the Frontend (Terminal 2):
```bash
cd frontend
npm run dev
```
The Vue app will start on http://localhost:5173.

---
## 📖 API Documentation (OpenAPI)
Once the backend is running, you can access the automatically generated OpenAPI documentation:

- Swagger UI: Navigate to http://localhost:5032/swagger in your browser.
