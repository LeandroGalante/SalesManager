# 🚀 Quick Start - Sales API

Quick guide to run and test the Sales API in minutes.

## ⚡ Setup and Run

### ✅ Visual Studio (Recommended)
```bash
# 1. Open solution in Visual Studio
start Ambev.DeveloperEvaluation.sln

# 2. Set startup project: Ambev.DeveloperEvaluation.WebApi
# 3. Press F5 (Debug) or Ctrl+F5 (Run without debugging)
```

### ⚙️ Command Line (Alternative)
```bash
cd template/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet run
```

**API will be available at:**
- **HTTPS**: `https://localhost:7181`
- **HTTP**: `http://localhost:5119`

**Database**: In-Memory (no setup required, data resets on restart)

## 🎯 Test Endpoints

### Health Check
```bash
# HTTPS (skip SSL verification)
curl -k -X GET "https://localhost:7181/health"

# HTTP (simpler)
curl -X GET "http://localhost:5119/health"
```

### Create Sale
```bash
curl -k -X POST "https://localhost:7181/api/sales" \
-H "Content-Type: application/json" \
-d '{
  "saleNumber": "SALE-001",
  "saleDate": "2024-12-04T10:00:00Z",
  "customerId": "CUST-001",
  "customerName": "John Doe",
  "branchId": "BRANCH-001",
  "branchName": "Main Branch",
  "items": [
    {
      "productId": "PROD-001",
      "productName": "Beer 350ml",
      "quantity": 5,
      "unitPrice": 3.50
    }
  ]
}'
```

### List Sales
```bash
curl -k -X GET "https://localhost:7181/api/sales?page=1&size=10"
```

## 📊 Business Rules

| Quantity | Expected Discount |
|----------|-------------------|
| 1-3 items | 0% (no discount) |
| 4-9 items | 10% discount     |
| 10-20 items | 20% discount   |
| 21+ items | ❌ ERROR         |

## 🔧 Important URLs

- **API**: `https://localhost:7181/api/sales` or `http://localhost:5119/api/sales`
- **Swagger**: `https://localhost:7181/swagger` or `http://localhost:5119/swagger`
- **Health**: `https://localhost:7181/health` or `http://localhost:5119/health`

## 🛠️ If Issues Persist

1. **Use Visual Studio** - Press F5 (most reliable)
2. **Check ports** - Application might start on different ports
3. **Try HTTP instead of HTTPS** - Use port 5119 instead of 7181
4. **Check Visual Studio Output** - Look for actual ports in the output window

**Note**: The application now uses in-memory database, so no database setup is required!

**🎉 Use Visual Studio F5 - it's the most reliable way to run the application!** 