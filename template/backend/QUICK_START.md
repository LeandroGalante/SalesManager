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

3. **Verify services are running:**
   ```bash
   docker-compose ps
   ```

   You should see:
   - PostgreSQL running on port 5432
   - MongoDB running on port 27017

## Step 2: Run the API with Visual Studio

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
       "DefaultConnection": "Host=localhost;Port=5432;Database=ambev_evaluation;Username=ambev_user;Password=ambev_pass",
       "MongoDB": "mongodb://developer:ev@luAt10n@localhost:27017/message_broker?authSource=admin"
     }
   }
   ```

4. **Run the application:**
   - Press `F5` or click "Start" in Visual Studio
   - The API will start on `https://localhost:7219` (or the configured port)

## Step 3: API Usage with Authentication

### Authentication Flow

All Sales endpoints require authentication. First, create a user and then authenticate to get a JWT token.

### 1. Create a User

```bash
curl -X POST "https://localhost:7219/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john.doe",
    "email": "john.doe@example.com",
    "phone": "+5511999999999",
    "password": "StrongPass123!",
    "role": "Admin"
  }'
```

### 2. Authenticate and Get Token

```bash
curl -X POST "https://localhost:7219/api/auth" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "StrongPass123!"
  }'
```

**Response:**
```json
{
  "success": true,
  "message": "User authenticated successfully",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "john.doe@example.com",
    "name": "john.doe",
    "role": "Admin"
  }
}
```

### 3. Use Token for Sales Operations

Copy the token from the authentication response and use it in the `Authorization` header:

#### Create a Sale

```bash
curl -X POST "https://localhost:7219/api/sales" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "saleNumber": "SALE-001",
    "saleDate": "2024-01-15T10:00:00Z",
    "customerId": "customer-123",
    "customerName": "John Customer",
    "branchId": "branch-456",
    "branchName": "Main Branch",
    "items": [
      {
        "productId": "product-789",
        "productName": "Premium Product",
        "quantity": 2,
        "unitPrice": 99.99
      }
    ]
  }'
```

#### Get All Sales

```bash
curl -X GET "https://localhost:7219/api/sales" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Get a Specific Sale

```bash
curl -X GET "https://localhost:7219/api/sales/{sale-id}" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Update a Sale

```bash
curl -X PUT "https://localhost:7219/api/sales/{sale-id}" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "customerId": "customer-123",
    "customerName": "John Customer Updated",
    "branchId": "branch-456",
    "branchName": "Main Branch",
    "items": [
      {
        "productId": "product-789",
        "productName": "Premium Product",
        "quantity": 3,
        "unitPrice": 89.99
      }
    ]
  }'
```

#### Delete a Sale

```bash
curl -X DELETE "https://localhost:7219/api/sales/{sale-id}" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

#### Cancel a Sale Item

```bash
curl -X POST "https://localhost:7219/api/sales/{sale-id}/items/{item-id}/cancel" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "reason": "Customer requested cancellation"
  }'
```

## Step 4: Explore the API

### Swagger Documentation

Once the API is running, you can access the interactive API documentation at:
- **Swagger UI**: `https://localhost:7219/swagger`

### Health Check

Verify the API is running:
```bash
curl -X GET "https://localhost:7219/health"
```

## Important Notes

### Security
- All Sales endpoints require authentication
- JWT tokens expire after 8 hours
- Use HTTPS in production environments

### Database
- PostgreSQL stores application data
- MongoDB stores message broker events
- Both databases are automatically created when the application starts

### Troubleshooting

**Docker Issues:**
```bash
# Stop all services
docker-compose down

# Remove volumes and start fresh
docker-compose down -v
docker-compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql
```

**Connection Issues:**
- Ensure Docker containers are running
- Check if ports 5432 and 27017 are available
- Verify connection strings in `appsettings.json`

## Architecture Overview

- **API Layer**: ASP.NET Core Web API
- **Application Layer**: Business logic and CQRS handlers
- **Domain Layer**: Entities and domain rules
- **Infrastructure Layer**: Data access and external services
- **Database**: PostgreSQL for relational data
- **Message Broker**: MongoDB for event storage

## Next Steps

1. Explore the Swagger documentation
2. Test all endpoints with different data
3. Check the MongoDB collections for stored events
4. Review the application logs for detailed information

For more detailed information, refer to the individual project documentation files. 