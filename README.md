Certainly! Here's the complete polished **README.md** for your **Rosier Chocolate Shop** ASP.NET MVC project, including the cleaned-up folder structure, key features, setup instructions, and contact info:

````markdown
# 🍫 Rosier Chocolate Shop - ASP.NET MVC E-Commerce Web App

**Rosier Chocolate Shop** is a full-featured e-commerce application built with ASP.NET MVC. It provides both a customer-facing storefront and an admin dashboard for managing chocolates, orders, employees, and customers.

---

## 🏗️ Project Structure

```plaintext
RosierBars/
├── Controllers/            # All MVC Controllers
│   ├── DashboardController.cs
│   ├── EmployeeController.cs
│   ├── HomeController.cs
│   ├── ProductController.cs
│   ├── RegisterController.cs
│   └── ShopController.cs
│
├── Models/                 # Data models and view models
│   ├── ContactForm.cs
│   ├── DashboardViewModel.cs
│   ├── EmployeeForm.cs
│   ├── LoginModel.cs
│   ├── OrderDetailModel.cs
│   ├── OrderModel.cs
│   ├── ProductModel.cs
│   └── RegisterModel.cs
│
├── Views/                  # Razor Views
│   ├── Dashboard/
│   ├── Default/
│   ├── Employee/
│   ├── Product/
│   ├── Register/
│   ├── Shop/
│   └── Shared/
│
├── Scripts/                # JavaScript files
├── Content/                # CSS files and static content
├── App_Data/
├── App_Start/
├── Global.asax
├── Web.config
├── RosierBars.sln
````

---

## ✅ Key Features

### 🛒 Customer Features

* Product catalog with detailed views
* Shopping cart with quantity controls
* Wishlist and Save for Later functionality
* Secure registration and login
* Checkout with contact and shipping address forms
* Order history and detailed views
* Invoice generation and printing support

### 🛠 Admin Features

* Dashboard with key metrics overview
* Add, edit, and delete chocolate products
* Manage customer orders and update status
* View detailed order information
* Employee management system
* Contact form submission handling

---

## 💻 Technologies Used

* ASP.NET MVC 5 (C#)
* SQL Server
* Entity Framework
* Bootstrap 4 & jQuery
* Razor View Engine
* HTML5 / CSS3
* Font Awesome & Boxicons

---

## 🔧 Setup Instructions

### Prerequisites

* Visual Studio 2019 or 2022
* SQL Server
* .NET Framework 4.7.2 or higher

### Steps

1. **Clone the repository**

   ```bash
   git clone https://github.com/jaybabariya1612/Rosier-Chocolate-Shop.git
   ```

2. **Open the solution**

   * Open `RosierBars.sln` in Visual Studio.

3. **Configure the Database**

   * Update the connection string in `Web.config`:

     ```xml
     <connectionStrings>
       <add name="Dbconnection"
            connectionString="Data Source=.;Initial Catalog=RosierDB;Integrated Security=True"
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

4. **Restore NuGet Packages**

   * Visual Studio usually restores packages automatically, or manually via:

     ```
     Tools > NuGet Package Manager > Restore NuGet Packages
     ```

5. **Run the application**

   * Press `F5` or click "Start Debugging" to launch the app.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE). You are free to use and modify it with proper attribution.

---

## 👨‍💻 Developer

**Jay Babariya**
📧 [jaybabariya01@gmail.com](mailto:jaybabariya01@gmail.com)
🌐 [GitHub Profile](https://github.com/jaybabariya1612)

---
