
# Vertex Test API

This is an ASP.NET Core Web API project that implements **JWT authentication** and integrates with a **MySQL** database. 
It allows users to authenticate and manage items through protected endpoints.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Setup Instructions](#setup-instructions)
3. [Database Configuration](#database-configuration)
4. [Running the Application in Visual Studio 2022](#running-the-application-in-visual-studio-2022)
5. [API Testing with Swagger](#api-testing-with-swagger)
6. [API Endpoints](#api-endpoints)

## Prerequisites

Ensure you have the following installed:
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with the **ASP.NET and web development** workload
- [MySQL Workbench (optional)](https://dev.mysql.com/downloads/workbench/)

## Setup Instructions

1. **Clone the repository:**

   ```bash
   git clone https://github.com/yourusername/VertexTestApi.git
   cd VertexTestApi
   ```

2. **Configure the database connection:**

   Open `appsettings.json` and make sure the MySQL connection string looks like as follows:

   ```json
   "ConnectionStrings": {
     "MySQLConnection": "server=localhost;database=vertex_test;user=test;password=Vert3xt3$T!"
   }
   ```

3. **Make sure the JWT and credentials are as follows:**

   ```json
   "Jwt_Key": "super secrete unguessable key super secrete unguessable key super secrete unguessable key",
   "Credentials_Username": "admin",
   "Credentials_Password": "@dm!n123"
   ```
## Database Configuration

Use the following SQL script in **MySQL Workbench**:

```sql
--To create the database
CREATE DATABASE IF NOT EXISTS vertex_test;
USE vertex_test;

--To create items table
CREATE TABLE items (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(100) NOT NULL,
  description TEXT,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

--Stored procedure: To get all items
DELIMITER $$
CREATE PROCEDURE sp_get_items()
BEGIN
  SELECT * FROM items;
END$$

--Stored procedure: To add new item
CREATE PROCEDURE sp_add_item(
  IN p_name VARCHAR(100),
  IN p_description TEXT
)
BEGIN
  INSERT INTO items (name, description)
  VALUES (p_name, p_description);
END$$
DELIMITER ;

--Sample data
INSERT INTO items (name, description) VALUES
  ('Tablet', 'iPad Pro 12.9" with Apple Pencil'),
  ('Smartwatch', 'Apple Watch Series 9'),
  ('Printer', 'Epson EcoTank ET-4850'),
  ('External SSD', 'Samsung T7 Shield 1TB'),
  ('Webcam', 'Logitech Brio 4K'),
  ('Mouse', 'Logitech MX Master 3S'),
  ('Docking Station', 'CalDigit TS4 Thunderbolt 4'),
  ('Router', 'TP-Link Archer AX6000'),
  ('Microphone', 'Shure MV7 USB/XLR'),
  ('Backpack', 'Peak Design Everyday Backpack 30L');
```

## Running the Application in Visual Studio 2022

1. Open the solution file `VertexTestApi.sln` in Visual Studio 2022.

2. In **Solution Explorer**, right-click on `VertexTestApi` and select **Set as Startup Project**.

3. Press `Ctrl + F5` or click the green **Start Without Debugging** button to run the application.

4. The app should launch in your browser, and the Swagger UI will be available at:

   ```
   https://localhost:5001/swagger
   ```

## API Testing with Swagger

1. Navigate to `/swagger` after the app starts.

2. Use the `/api/Auth/login` endpoint to get a JWT token.  Credentials:

   ```json
   {
     "username": "admin",
     "password": "@dm!n123"
   }
   ```

3. Copy the token returned.

4. Click the **Authorize** button in Swagger UI and paste the token.

5. You can now access the secured endpoints.



## API Endpoints

Note that All `Items` endpoints require JWT authentication.

### Authentication

| Method | Endpoint             | Description                            |
|--------|----------------------|----------------------------------------|
| POST   | `/api/Auth/login`    | Get JWT token by providing credentials |

### Items

| Method | Endpoint             | Description                        |
|--------|----------------------|------------------------------------|
| GET    | `/api/Items`         | Retrieve all items                 |
| POST   | `/api/Items`         | Create a new item                  |

