# Inventory Management System - Backend API

A comprehensive inventory management system built with ASP.NET Core Web API, featuring JWT authentication, role-based authorization, and full CRUD operations for managing products, suppliers, users, and stock movements.

## 🚀 Features

- **Authentication & Authorization**: JWT-based authentication with role-based access control
- **User Management**: Admin can create, update, and manage user accounts
- **Product Management**: Complete CRUD operations for inventory products
- **Supplier Management**: Manage supplier information and relationships
- **Stock Movement Tracking**: Track inventory changes with detailed movement history
- **Reporting**: Low stock alerts and movement history reports
- **Database Backup/Restore**: Admin can backup and restore the database
- **API Documentation**: Swagger/OpenAPI documentation

## 🏗️ Database Schema

The system uses SQL Server with Entity Framework Core and includes the following main entities:

### Core Tables

| Table | Description | Key Fields |
|-------|-------------|------------|
| **Users** | User accounts and authentication | Id, Email, FirstName, LastName, PasswordHash, CreatedOn, Deleted |
| **UserRoles** | Available system roles | Id, Name |
| **UserRoleMappings** | Many-to-many relationship between users and roles | UserId, RoleId |
| **Products** | Inventory items | Id, Name, SKU, QuantityInStock, Price, SupplierId, CreatedOn |
| **Suppliers** | Vendor information | Id, Name, ContactName, Phone, Email, Address, CreatedOn |
| **StockMovements** | Inventory transaction history | Id, ProductId, Type, Quantity, Reason, Date, CreatedByUserId |

### Entity Relationships

```
Users ←→ UserRoleMappings ←→ UserRoles
Users → StockMovements
Suppliers → Products → StockMovements
```

### Database Schema Diagram

```
┌─────────────┐    ┌──────────────────┐    ┌─────────────┐
│   Users     │    │ UserRoleMappings │    │  UserRoles  │
├─────────────┤    ├──────────────────┤    ├─────────────┤
│ Id (PK)     │◄──►│ UserId (FK)      │◄──►│ Id (PK)     │
│ Email       │    │ RoleId (FK)      │    │ Name        │
│ FirstName   │    └──────────────────┘    └─────────────┘
│ LastName    │
│ PasswordHash│
│ CreatedOn   │
│ Deleted     │
└─────────────┘
       │
       ▼
┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│StockMovement│    │  Products   │    │  Suppliers  │
├─────────────┤    ├─────────────┤    ├─────────────┤
│ Id (PK)     │    │ Id (PK)     │    │ Id (PK)     │
│ ProductId(FK)│◄───│ Name        │◄───│ Name        │
│ Type        │    │ SKU         │    │ ContactName │
│ Quantity    │    │QuantityStock│    │ Phone       │
│ Reason      │    │ Price       │    │ Email       │
│ Date        │    │ SupplierId  │────┤ Address     │
│CreatedByUser│    │ CreatedOn   │    │ CreatedOn   │
└─────────────┘    └─────────────┘    └─────────────┘
```

## 🔐 Test Accounts

The system comes with pre-seeded test accounts for immediate use:

### Admin Account
- **Email**: `admin@inventory.com`
- **Password**: `Admin123!`
- **Role**: Admin
- **Permissions**: Full access to all features including user management, database backup/restore

### Staff Account
- **Email**: `staff1@inventory.com`
- **Password**: `Staff1`
- **Role**: Staff
- **Permissions**: Limited access to products, suppliers, stock movements, and reports

## 🛠️ Setup Instructions

### Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code with C# extension

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/DeividasDub/Inventory-Management-System-Task-back-end.git
   cd Inventory-Management-System-Task-back-end/InventoryManagementAPI
   ```

2. **Configure Database Connection**
   
   Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=InventoryManagementDB;Integrated Security=True;Persist Security Info=False;Trust Server Certificate=True"
     }
   }
   ```

   For SQL Server LocalDB:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=InventoryManagementDB;Integrated Security=True;Persist Security Info=False;Trust Server Certificate=True"
     }
   }
   ```

3. **Install Dependencies**
   ```bash
   dotnet restore
   ```

4. **Run Database Migrations**
   ```bash
   dotnet ef database update
   ```
   
   This will create the database and seed it with the test accounts.

5. **Start the Application**
   ```bash
   dotnet run
   ```

   The API will be available at:
   - HTTPS: `https://localhost:5001`
   - HTTP: `http://localhost:5000`
   - Swagger UI: `https://localhost:5001/swagger`

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| POST | `/api/auth/register` | Register new user (defaults to Staff role) | Public |
| POST | `/api/auth/login` | User login | Public |

### User Management Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| POST | `/api/usermanagement` | Create new user | Admin |
| GET | `/api/usermanagement/users` | Get all users | Admin |
| PUT | `/api/usermanagement/{userId}` | Update user | Admin |
| PUT | `/api/usermanagement/{userId}/role` | Update user role | Admin |
| DELETE | `/api/usermanagement/{userId}` | Delete user | Admin |

### Product Management Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| POST | `/api/product` | Create product | Admin |
| GET | `/api/product` | Get all products | Admin, Staff |
| GET | `/api/product/{id}` | Get product by ID | Admin, Staff |
| PUT | `/api/product/{id}` | Update product | Admin |
| DELETE | `/api/product/{id}` | Delete product | Admin |
| POST | `/api/product/search` | Search products | Admin, Staff |

### Supplier Management Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| POST | `/api/supplier` | Create supplier | Admin |
| GET | `/api/supplier` | Get all suppliers | Admin, Staff |
| GET | `/api/supplier/{id}` | Get supplier by ID | Admin, Staff |
| PUT | `/api/supplier/{id}` | Update supplier | Admin |
| DELETE | `/api/supplier/{id}` | Delete supplier | Admin |

### Stock Movement Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| POST | `/api/stockmovement` | Create stock movement | Admin, Staff |
| GET | `/api/stockmovement/product/{productId}` | Get product stock movements | Admin, Staff |

### Report Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| GET | `/api/report/low-stock?threshold=10` | Get low stock products | Admin, Staff |
| GET | `/api/report/stock-movements/{productId}` | Get product movement history | Admin, Staff |

### Database Backup Endpoints

| Method | Endpoint | Description | Access |
|--------|----------|-------------|---------|
| POST | `/api/databasebackup/backup` | Create database backup | Admin |
| POST | `/api/databasebackup/restore` | Restore database from backup | Admin |
| GET | `/api/databasebackup/backups` | List available backups | Admin |

## 💾 Database Backup & Restore

### Creating a Backup

```bash
POST /api/databasebackup/backup
Content-Type: application/json
Authorization: Bearer YOUR_ADMIN_JWT_TOKEN

{
  "backupName": "MyBackup"
}
```

### Restoring from Backup

The system includes a pre-configured database backup file: `DatabaseBackups/InventorySystemFinalDataBase2025-07-19`

```bash
POST /api/databasebackup/restore
Content-Type: application/json
Authorization: Bearer YOUR_ADMIN_JWT_TOKEN

{
  "backupFileName": "InventorySystemFinalDataBase2025-07-19"
}
```

### Manual Database Restore

If you prefer to restore the database manually:

1. Stop the application
2. Use SQL Server Management Studio or command line:
   ```sql
   RESTORE DATABASE [InventoryManagementDB] 
   FROM DISK = 'path/to/DatabaseBackups/InventorySystemFinalDataBase2025-07-19' 
   WITH REPLACE
   ```
3. Restart the application

## 🔒 Authentication & Authorization

### JWT Configuration

The API uses JWT tokens for authentication with the following configuration:
- **Secret Key**: Configured in `appsettings.json`
- **Token Expiration**: 24 hours
- **Issuer/Audience**: InventoryManagementAPI

### Role-Based Access Control

- **Admin Role**: Full access to all endpoints including user management and database operations
- **Staff Role**: Limited access to products, suppliers, stock movements, and reports
- **Public**: Only registration and login endpoints

### Using the API

1. **Login** to get a JWT token:
   ```bash
   POST /api/auth/login
   {
     "email": "admin@inventory.com",
     "password": "Admin123!"
   }
   ```

2. **Include the token** in subsequent requests:
   ```bash
   Authorization: Bearer YOUR_JWT_TOKEN_HERE
   ```

## 🧪 Testing the API

### Using Swagger UI

1. Navigate to `https://localhost:5001/swagger`
2. Click "Authorize" and enter: `Bearer YOUR_JWT_TOKEN`
3. Test endpoints directly from the browser

### Using HTTP Client

The repository includes `InventoryManagementAPI.http` file with sample requests for testing all endpoints.

### Sample API Calls

```bash
# Login as Admin
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@inventory.com","password":"Admin123!"}'

# Get all products (requires authentication)
curl -X GET "https://localhost:5001/api/product" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Create a new product (Admin only)
curl -X POST "https://localhost:5001/api/product" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_ADMIN_JWT_TOKEN" \
  -d '{
    "name": "Sample Product",
    "sku": "SP001",
    "quantityInStock": 100,
    "price": 29.99,
    "supplierId": 1
  }'
```

## 🚀 Deployment

### Development Environment

```bash
dotnet run --environment Development
```

### Production Environment

1. Update `appsettings.Production.json` with production database connection
2. Build the application:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
3. Deploy to your hosting environment (IIS, Azure App Service, Docker, etc.)

## 🔧 Configuration

### Key Configuration Settings

- **Connection Strings**: Database connection configuration
- **JWT Settings**: Token signing and validation parameters
- **CORS Policy**: Configured for Angular frontend at `http://localhost:4200`
- **Logging**: Configurable logging levels and providers

### Environment Variables

You can override configuration using environment variables:
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__Secret`
- `JwtSettings__ExpirationHours`

## 📝 Development Notes

### Code Structure

- **Controllers**: API endpoints and request handling
- **Services**: Business logic and data operations
- **DTOs**: Data transfer objects for API requests/responses
- **Models**: Entity Framework models
- **Factories**: Response model preparation
- **Data**: Database context and configurations

### Adding New Features

1. Create DTOs for request/response models
2. Add business logic to appropriate service
3. Create or update controller endpoints
4. Add authorization attributes as needed
5. Update database models if required
6. Run migrations for database changes

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For issues and questions:
1. Check the API documentation at `/swagger`
2. Review the HTTP test file for sample requests
3. Ensure database connection is properly configured
4. Verify JWT tokens are included in authenticated requests

---

**Note**: This system is designed to work with the Angular frontend available at: [Inventory-Management-System-Task-front-end](https://github.com/DeividasDub/Inventory-Management-System-Task-front-end)
