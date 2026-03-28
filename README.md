# 📋 Full-Stack Task Management System

A production-ready, clean, and modern Task Management application built with **.NET 8 (ASP.NET Core Web API)**, **Angular 19**, and **MySQL**.

This project provides a complete end-to-end flow allowing users to Create, Read, Update, and Delete (CRUD) tasks, alongside instantly toggling task statuses between 'Pending' and 'Completed'.

---

## 🏗️ Tech Stack

### Backend
* **Framework**: .NET 8 (C#)
* **Architecture**: 3-Layer Architecture (Controller → Service → DbContext)
* **ORM**: Entity Framework Core (Database First Approach)
* **Database**: MySQL
* **Key Features**: Global Exception Middleware, Dependency Injection, Auto-timestamp triggers, ENUM Status Validation, Swagger UI.

### Frontend
* **Framework**: Angular 19+ (Zoneless Setup configured)
* **Styling**: Vanilla CSS (Modern, Responsive, Glass-like UI)
* **Key Features**: Reactive API handling (HttpClient), Real-time Change Detection, Routing, Smart Error/Success handling.

---

## 🗄️ Database Setup (MySQL)

1. Open your MySQL client (e.g., MySQL Workbench).
2. Run the `taskmanagment.sql` file provided in the root directory to create the database, tables, and seed initial data:

```sql
CREATE DATABASE IF NOT EXISTS task_management_db;
USE task_management_db;

CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description TEXT,
    Status ENUM('Pending', 'Completed') DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);
```

3. Ensure your MySQL credentials match what is in the backend: `user=root;password=Gaurav9519@$`.

---

## 🚀 How to Run the Project

### 1. Start the Backend API (.NET Core)

1. Open a terminal.
2. Navigate to the API folder:
   ```bash
   cd TaskManagement.Api
   ```
3. Run the application:
   ```bash
   dotnet run
   ```
4. The backend will start on **http://localhost:5222**. To test the endpoints directly, go to **http://localhost:5222/swagger** in your browser.

### 2. Start the Frontend (Angular)

1. Open a new, separate terminal.
2. Navigate to the frontend folder:
   ```bash
   cd task-management-frontend
   ```
3. Install dependencies (if this is your first time):
   ```bash
   npm install
   ```
4. Start the development server:
   ```bash
   npm start
   ```
5. Open your browser and navigate to: **http://localhost:4200**

---

## 🧩 Architectural Decisions made in this project

1. **No AutoMapper / No Repository Pattern**: The backend avoids bloated boilerplate. Services map directly to DTOs and talk directly to the `DbContext`. This keeps the API extremely fast, readable, and easy to maintain.
2. **Global Error Handling Middleware**: Instead of wrapping every Controller method in `try/catch` blocks, a central `ExceptionMiddleware` catches any failures and returns clean, uniform `500` JSON errors to the frontend without crashing the application.
3. **Database-Managed Timestamps**: Both `CreatedAt` and `UpdatedAt` are heavily managed by the database using `CURRENT_TIMESTAMP ON UPDATE`. EF Core is configured simply to ignore those columns on updates to prevent tracking conflicts.
4. **Zoneless Angular Strategy**: The frontend leverages manual `ChangeDetectorRef` triggering inside Observable callbacks, optimizing rendering speeds and eliminating `zone.js` bloat tracking.

---

## 📸 Endpoints Reference

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/tasks` | Fetches all tasks |
| `GET` | `/api/tasks/{id}` | Fetches a single task by ID |
| `POST` | `/api/tasks` | Creates a new task |
| `PUT` | `/api/tasks/{id}` | Updates an entire task |
| `PATCH` | `/api/tasks/{id}/status` | Toggles the completion status instantly |
| `DELETE` | `/api/tasks/{id}` | Deletes the task from DB entirely |

---

*Project built successfully using Angular CLI & .NET 8 Scaffoldings.*
