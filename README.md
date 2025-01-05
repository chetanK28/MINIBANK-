
# MiniBank 

## Overview
MiniBank is a banking system application that allows users to manage their bank accounts, perform transactions (Deposit, Withdraw, Transfer), and view account and transaction details.

---

## Database Schema

### Users Table
```sql
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role ENUM('Admin', 'User', 'Moderator') NOT NULL
);
```

### BankAccount Table
```sql
CREATE TABLE bankaccount (
    AccountId BIGINT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Balance DOUBLE NOT NULL CHECK (Balance >= 0),
    Type ENUM('Savings', 'Current', 'FixedDeposit') NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
```

### Transactions Table
```sql
CREATE TABLE transactions (
    TransactionId BIGINT AUTO_INCREMENT PRIMARY KEY,
    AccountId BIGINT NOT NULL, -- References the primary account involved
    Amount DOUBLE NOT NULL CHECK (Amount > 0),
    Type ENUM('Deposit', 'Withdraw', 'Transfer') NOT NULL,
    RelatedAccountId BIGINT DEFAULT NULL, -- For transfers, the recipient account ID
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AccountId) REFERENCES bankaccount(AccountId) ON DELETE CASCADE,
    FOREIGN KEY (RelatedAccountId) REFERENCES bankaccount(AccountId) ON DELETE SET NULL
);
```

---

## Project Structure

```
MiniBank/
│
├── Controllers/
│   ├── BankAccountController.cs
│   ├── TransactionController.cs
│   └── UserController.cs
│
├── Models/
│   ├── BankAccount.cs
│   ├── Transaction.cs
│   ├── User.cs
│   └── ErrorViewModel.cs
│
├── Services/
│   ├── Implementation/
│   │   ├── BankAccountService.cs
│   │   ├── TransactionService.cs
│   │   └── UserService.cs
│   ├── Interface/
│   │   ├── IBankAccountService.cs
│   │   ├── ITransactionService.cs
│   │   └── IUserService.cs
│
├── Repositories/
│   ├── Implementation/
│   │   ├── BankAccountRepository.cs
│   │   ├── TransactionRepository.cs
│   │   └── UserRepository.cs
│   ├── Interface/
│   │   ├── IBankAccountRepository.cs
│   │   ├── ITransactionRepository.cs
│   │   └── IUserRepository.cs
│
├── Views/
│   ├── BankAccount/
│   │   ├── ViewBankAccounts.cshtml
│   │   ├── AddBankAccount.cshtml
│   │   └── UpdateBankAccount.cshtml
│   ├── Transaction/
│   │   ├── ViewAllTransactions.cshtml
│   │   ├── ViewTransactionsByAccountId.cshtml
│   │   ├── ViewTransactionsByUserId.cshtml
│   │   ├── AddTransaction.cshtml
│   │   ├── ViewTransactionById.cshtml
│   │   └── DeleteTransaction.cshtml
│   └── Shared/
│       └── Error.cshtml
│
├── DBContext/
│   └── Context.cs
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│
├── Program.cs
├── Startup.cs (if applicable)
├── MiniBank.csproj
└── README.md
```

---

## Steps to Set Up the Database

1. **Create the Database**
   Run the following SQL to create the database:
   ```sql
   CREATE DATABASE MiniBankDB;
   ```

2. **Run the Schema Scripts**
   Execute the SQL scripts provided in the **Database Schema** section above to create the `Users`, `BankAccount`, and `Transactions` tables.

3. **Configure Connection String**
   Update the `appsettings.json` or `Program.cs` file to include the database connection string:
   ```json
   {
       "ConnectionStrings": {
           "MyDatabase": "Server=localhost;Database=MiniBankDB;User=root;Password=yourpassword;"
       }
   }
   ```

4. **Run Entity Framework Migrations**
   Use Entity Framework to apply migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## Features

1. **User Management**
   - Add, update, delete, and view users.
   - Assign roles (Admin, User, Moderator).

2. **Bank Account Management**
   - Create, update, delete, and view bank accounts.
   - Associate accounts with users.

3. **Transactions**
   - Perform deposits, withdrawals, and transfers.
   - View transaction history by account or user.
   - Manage transaction details (view, delete).

4. **Error Handling**
   - Custom error page (`Error.cshtml`).

---

## How to Run the Application

1. **Clone the Repository**
   ```bash
   git clone  https://github.com/chetanK28/MiniBank.git
   cd minibank
   ```

2. **Build the Project**
   ```bash
   dotnet build
   ```

3. **Run the Application**
   ```bash
   dotnet run
   ```

4. **Access the Application**
   Open your browser and navigate to `http://localhost:5000`.

---

## Testing the Application

- Add users via the **User Management** interface.
- Create bank accounts linked to users via the **Bank Account Management** interface.
- Perform transactions (deposit, withdraw, transfer) and view transaction history.
- Trigger an error to view the custom error page.

---
