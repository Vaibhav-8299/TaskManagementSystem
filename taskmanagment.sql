-- 1. Create Database
CREATE DATABASE IF NOT EXISTS task_management_db;

-- 2. Use Database
USE task_management_db;

-- 3. Create Tasks Table
CREATE TABLE IF NOT EXISTS Tasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Description TEXT,
    Status ENUM('Pending', 'Completed') DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- 4. Insert Sample Data
INSERT INTO Tasks (Title, Description, Status)
VALUES 
('Buy Milk', 'Buy milk from nearby store', 'Pending'),
('Learn .NET Core', 'Practice Web API and MVC', 'Pending'),
('Complete Assignment', 'Finish hackathon project', 'Completed'),
('Go to Gym', 'Workout for 1 hour', 'Pending'),
('Read Book', 'Read 20 pages of a tech book', 'Completed');

-- 5. Verify Data
SELECT * FROM Tasks;