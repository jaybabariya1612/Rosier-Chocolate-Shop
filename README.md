Thanks for sharing the folder structure. Based on your ASP.NET MVC project setup (`RosierBars`), here is the updated and **customized `README.md`** content including actual folder structure, key files, and controllers:

---

```markdown
# 🍫 Rosier Chocolate Shop - ASP.NET MVC E-Commerce Web App

**Rosier Chocolate Shop** is a full-featured e-commerce application built with ASP.NET MVC. It provides both a customer-facing storefront and an admin dashboard for managing chocolates, orders, employees, and customers.

## 🏗️ Project Structure

```

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
├── App\_Data/
├── App\_Start/
├── Global.asax
├── Web.config
├── RosierBars.sln

````

## ✅ Key Features

### 🛒 Customer Features
- Product catalog with detail view
- Shopping cart with quantity control
- Wishlist and Save for Later
- Secure registration and login
- Checkout with contact/address form
- Order history and details
- Invoice generation and printing

### 🛠 Admin Features
- Dashboard with overview metrics
- Add/Edit/Delete chocolate products
- Manage customer orders
- View order details and update status
- Employee management system
- Contact form handling

## 💻 Technologies Used

- ASP.NET MVC 5 (C#)
- SQL Server
- Entity Framework
- Bootstrap 4 & jQuery
- Razor View Engine
- HTML5/CSS3
- Font Awesome & Boxicons

## 🔧 Setup Instructions

### Prerequisites
- Visual Studio 2019/2022
- SQL Server
- .NET Framework 4.7.2+

### Steps
1. **Clone the repository**
   ```bash
   git clone https://github.com/jaybabariya1612/Rosier-Chocolate-Shop.git
````

2. **Open the solution**

   * Open `RosierBars.sln` in Visual Studio.

3. **Configure the Database**

   * Edit `Web.config`:

     ```xml
     <connectionStrings>
       <add name="Dbconnection" 
            connectionString="Data Source=.;Initial Catalog=RosierDB;Integrated Security=True" 
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```

4. **Restore NuGet packages**

   * Visual Studio will prompt automatically or use:

     ```
     Tools > NuGet Package Manager > Restore NuGet Packages
     ```

5. **Run the application**

   * Press `F5` or click "Start Debugging".


## 📄 License

MIT License – Use freely for educational or commercial projects with attribution.

---

## 👨‍💻 Developer

**Jay Babariya**
📧 [jaybabariya01@gmail.com](mailto:jaybabariya01@gmail.com)
🌐 [GitHub Profile](https://github.com/jaybabariya1612)

---

```

Let me know if you want this `README.md` as a downloadable file or need to add badges (e.g., GitHub stars, forks, license).
```
