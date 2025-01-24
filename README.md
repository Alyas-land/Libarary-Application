# Library Management System

This project is a Library Management System built using **ASP.NET Core 8** for the backend and **Bootstrap** for the frontend.

## Features
- Manage books (add, edit, delete)
- Advanced book search
- User management (registration and login)
- Responsive and user-friendly interface using Bootstrap

---

## Technologies Used
- **Backend**: ASP.NET Core 8
- **Frontend**: HTML, CSS, Bootstrap
- **Database**: SQL Server
- **Version Control**: Git and GitHub

---

## Installation and Setup

### Prerequisites
- **.NET SDK 8.0** or higher
- **SQL Server**
- **Visual Studio 2022** (or later)

### Steps to Install
1. **Clone the repository**
   ```bash
   git clone https://github.com/YourUsername/LibraryManagement.git
   cd LibraryManagement

2. **Configure the Database**
     - Open the appsettings.json file and update the database connection string:
  ```
  "ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LibraryDB;Trusted_Connection=True;"
}
```
3. Apply Migrations Run the following command in the project terminal:
   ```
   dotnet ef database update
   
4. Run the Application

5. Open your browser and navigate to:
```
http://localhost:5000
```

## Usage
 - Once the project is running, the homepage of the Library Management System will be displayed.
 - You can add new books, manage users, and use the search functionality.

## Project Structure
```
LibraryManagement/
├── Controllers/
├── Models/
├── Views/
│   ├── Shared/
│   ├── Books/
│   ├── Users/
├── wwwroot/
│   ├── css/
│   ├── js/
│   ├── lib/ (Bootstrap)
└── appsettings.json
```

## License
This project is licensed under the MIT License.

## Contact
For questions or suggestions, feel free to contact me:
Email: a.mashayekhi90@example.com
GitHub: YourUsername + RepositoryName
