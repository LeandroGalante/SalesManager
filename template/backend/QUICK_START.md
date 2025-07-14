# Sales Manager - Quick Start Guide

This guide will help you get the Sales Manager application up and running quickly using Docker for infrastructure and Visual Studio for the API.

## Prerequisites

- **Docker Desktop** installed and running
- **Visual Studio 2022** or later with .NET 8.0 SDK
- **PostgreSQL** (managed by Docker)
- **MongoDB** (managed by Docker)

## Step 1: Start Infrastructure with Docker

1. **Navigate to the backend directory:**
   ```bash
   cd template/backend
   ```

2. **Start the infrastructure services:**
   ```bash
   docker-compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql
   ```

3. **Clean database (optional - for fresh testing):**
   ```bash
   # Stop and remove volumes (cleans all data)
   docker-compose down -v
   
   # Restart infrastructure
   docker-compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql
   ```

4. **Verify services are running:**
   ```bash
   docker-compose ps
   ```

   You should see:
   - PostgreSQL running on port 5432
   - MongoDB running on port 27017

## Step 2: Run the API

### Option A: Using Visual Studio (Recommended for Development)

1. **Open the solution in Visual Studio:**
   - Open `Ambev.DeveloperEvaluation.sln` in Visual Studio

2. **Set startup project:**
   - Right-click on `Ambev.DeveloperEvaluation.WebApi` project
   - Select "Set as Startup Project"

3. **Configure connection strings:**
   - The API will use these connection strings for local development:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n",
       "MongoDB": "mongodb://developer:ev%40luAt10n@localhost:27017/message_broker?authSource=admin"
     }
   }
   ```

4. **Run the application:**
   - Press `F5` or click "Start" in Visual Studio
   - The API will start on `https://localhost:7181` (HTTPS) or `http://localhost:5119` (HTTP)

### Option B: Using Docker Compose

1. **Start all services with Docker:**
   ```bash
   docker-compose up -d
   ```

2. **Access the API:**
   - The API will be available on `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP)

## Step 3: API Usage with Authentication

> **Note**: Examples below use Visual Studio ports (`https://localhost:7181`). If using Docker, replace with `https://localhost:5001`.

### 1. Create a User

```bash
curl -k -X POST "https://localhost:7181/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john.doe",
    "email": "john.doe@example.com",
    "phone": "+5511999999999",
    "password": "StrongPass123!",
    "status": 1,
    "role": 3
  }'
```

### 2. Authenticate and Get Token

```bash
curl -k -X POST "https://localhost:7181/api/auth" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "StrongPass123!"
  }'
```

### 3. Sales Operations

Use the token from authentication in the `Authorization` header for all operations below.

#### Create a Sale

```bash
curl -k -X POST "https://localhost:7181/api/sales" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "saleNumber": "SALE-001",
    "customerId": "99b88712-b120-40ae-ae51-c6f348da1899",
    "customerName": "John Customer",
    "branchId": "12345678-1234-1234-1234-123456789abc",
    "branchName": "Main Branch",
    "saleDate": "2024-01-15T10:00:00Z",
    "items": [
      {
        "productId": "87654321-4321-4321-4321-abcdef123456",
        "productName": "Premium Product",
        "quantity": 2,
        "unitPrice": 99.99
      }
    ]
  }'
```

#### Get All Sales

```bash
curl -k -X GET "https://localhost:7181/api/sales" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Get a Specific Sale

```bash
curl -k -X GET "https://localhost:7181/api/sales/{sale-id}" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Update a Sale

```bash
curl -k -X PUT "https://localhost:7181/api/sales/{sale-id}" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "customerId": "99b88712-b120-40ae-ae51-c6f348da1899",
    "customerName": "John Customer Updated",
    "branchId": "12345678-1234-1234-1234-123456789abc",
    "branchName": "Main Branch Updated",
    "items": [
      {
        "productId": "87654321-4321-4321-4321-abcdef123456",
        "productName": "Premium Product",
        "quantity": 3,
        "unitPrice": 89.99
      }
    ]
  }'
```

#### Delete a Sale

```bash
curl -k -X DELETE "https://localhost:7181/api/sales/{sale-id}" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Cancel a Sale Item

```bash
curl -k -X POST "https://localhost:7181/api/sales/{sale-id}/items/{item-id}/cancel" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "reason": "Customer requested cancellation"
  }'
```

## Step 4: Explore the API

### Swagger Documentation
Access the interactive API documentation at: `https://localhost:7181/swagger`

### Health Check
```bash
curl -k -X GET "https://localhost:7181/health"
```

## Important Notes

- All Sales endpoints require authentication with JWT tokens
- Use `-k` flag with curl to ignore SSL certificate warnings in development
- GUIDs are required for: `customerId`, `branchId`, `productId`
- User Status: `1` = Active, `2` = Inactive, `3` = Suspended
- User Role: `1` = Customer, `2` = Manager, `3` = Admin
- Automatic discounts: 10% for 4-9 items, 20% for 10-20 items

## Quick Commands

```bash
# Start infrastructure for Visual Studio
docker-compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql

# Start full stack for Docker
docker-compose up -d

# Stop all services
docker-compose down

# Clean environment
docker-compose down -v
``` 