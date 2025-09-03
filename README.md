````markdown
# Oral and Dental Health Tracking Application

A dental health tracking system built with **ASP.NET Core MVC (.NET 8)** and **Entity Framework Core**.  
Runs on SQL Server using **Code-First Migrations**. Appointment scheduling is integrated with **FullCalendar** for a visual calendar view.

## Table of Contents
- [Technologies](#technologies)
- [Requirements](#requirements)
- [Packages and Installation](#packages-and-installation)
- [Database and First Run](#database-and-first-run)
- [Features](#features)
- [Development Notes](#development-notes)
- [License](#license)

---

## Technologies
- **.NET 8** (ASP.NET Core MVC)
- **Entity Framework Core** (SQL Server, Code-First)
- **Bootstrap 5**, **jQuery**
- **FullCalendar** (visual calendar via CDN)
- Cookie Authentication, Session

## Requirements
- .NET SDK 8.x  
- SQL Server (LocalDB or full edition)
- Visual Studio 2022 (recommended) or `dotnet` CLI
- (Optional) `dotnet-ef` CLI tool

## Packages and Installation

> Packages can be installed via **Package Manager Console (PMC)** or **dotnet CLI**.

### Required Packages
**PMC:**
```powershell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
````

**dotnet CLI:**

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
```

### Client Libraries (Bootstrap/jQuery)

If already available under `wwwroot/lib`, no action is needed.
Otherwise, install with **LibMan** (optional):

```bash
libman install bootstrap@5 --provider unpkg --destination wwwroot/lib/bootstrap
libman install jquery@3 --provider unpkg --destination wwwroot/lib/jquery
```

**FullCalendar** is included via CDN (see `Views/Randevular/Calendar.cshtml`).

### `dotnet-ef` CLI Tool (optional, recommended)

```bash
dotnet tool install --global dotnet-ef
```

## Database and First Run

1. Set the **connection string** in `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=DisSagligiTakipDb;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```
2. Apply **migrations**:

   * PMC:

     ```powershell
     Add-Migration InitialCreate
     Update-Database
     ```
   * CLI:

     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```
3. Run the application:

   * Visual Studio: **F5**
   * CLI: `dotnet run`
4. On startup, **SeedData** runs and creates a default **SuperAdmin** user.
   Credentials can be checked inside the `SeedData` class.

> Default dev URL may vary (e.g. `https://localhost:5028`). Use the URL shown in the console.

## Features

### Authentication & Authorization

* Cookie Authentication with Session
* Roles: **SuperAdmin, Admin, Doctor, Assistant, Patient**
* **SuperAdmin** cannot be deleted or reassigned
* Dynamic navbar menus based on login/role
* Logged-in user’s full name shown via Session

### User & Admin Management

* **Admin Panel**: list users, edit roles
* `UserWithRolesViewModel` for role management
* Email confirmation flags shown in UI

### Notes Module

* `Note` / `NoteViewModel` – **CRUD**
* Image uploads (`wwwroot/uploads`)
* Migration: `Add-NoteFeature`

### Dental Health Records

* `DisSagligiVerisi` – **CRUD**
* Fields: Description, Dentist Name, Image Path, Date, `UserId`, `HastaUserId`
* Doctors can create records for patients
* Migration: `UpdateDisSagligiVerisiSchema`

### Brushing Tracker

* `FircalamaKaydi` / `FircalamaViewModel` – **CRUD**
* Supports image upload
* Migration: `Add-FircalamaFeature`

### Patient Management

* `HastaController` – list and details
* Patient detail shows dental & brushing records
* Future: doctors/assistants can edit records

### Appointments (Oral-Dental Checkups)

* `MuayeneRandevusu` – **CRUD**
* Role-based views: patients see their own, doctors see their own, staff/admin see all
* **Conflict checks** (no double-booking for patients/doctors)
* **Visual Calendar** (FullCalendar):

  * `/Randevular/Takvim` page (month/week/day/list views)
  * Click on a day → quick create (prefilled times)
  * List view includes **Details / Edit / Delete** buttons
  * `/Randevular/Calendar` endpoint for JSON events
* `Randevular` page has **Calendar** and **Calendar (JSON)** buttons

### Common

* Toast notifications (`TempData["Success"]`, `TempData["Error"]`)
* Privacy page
* Clean layered architecture: **Controllers, Entities, ViewModels, Views, DataAccess, Helpers, Migrations**

### Upcoming

* **Health Statistics (Last 7 Days)**
* More advanced appointment filters

## Development Notes

* Razor runtime compilation enabled in `DEBUG`
* File upload limit set to 20MB (`FormOptions`)
* Production config includes `UseExceptionHandler` + `HSTS`

## License

This project is for educational purposes. License it according to your organization’s needs.

```
```
