<div align="center">

# 🪥 Oral & Dental Health Tracking Application

[![](https://img.shields.io/badge/Language-English-blue?style=for-the-badge&logo=google-translate)](#english-version)
&nbsp;&nbsp;&nbsp;&nbsp;
[![](https://img.shields.io/badge/Dil-T%C3%BCrk%C3%A7e-red?style=for-the-badge&logo=google-translate)](#turkish-version)

---

[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core_MVC-8.0-512BD4?style=flat-square&logo=.net&logoColor=white)](https://dotnet.microsoft.com/en-us/apps/aspnet/mvc)
[![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-8.0-512BD4?style=flat-square&logo=.net&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC292B?style=flat-square&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)

</div>

---

<a id="english-version"></a>
# English Version

A comprehensive, full-stack web application designed to track dental hygiene habits, log personal dental health metrics, and manage clinical appointments. Built using **ASP.NET Core MVC** and **Entity Framework Core**, the application serves patients looking to track their hygiene habits and dentists managing appointment schedulers.

## 🚀 Key Features

*   **🔐 Secure Authentication & Roles**: Safe register/login mechanisms with specific user role distributions (e.g., patient, dentist/physician).
*   **📅 Appointment Management System**: Users can book clinical visits with specific physicians. Features multi-index database configurations for faster loading and prevention of scheduling conflicts.
*   **🦷 Dental Health Loggers**: Logs and monitors dental symptoms, treatments, and records. Can also be utilized by administrators/dentists to input data on behalf of patients.
*   **🪥 Brushing habit tracker**: Log daily teeth brushing routines (timestamps, duration, technique) to build healthier hygiene habits.
*   **📊 Interactive Progress Statistics**: Dashboard charts highlighting brushing frequencies and health metrics over time.
*   **📝 Personal Logs & Notes**: Private note-taking capability for users to record dental pain levels, symptom timelines, and questions for upcoming checkups.

---

## 🛠️ Technology Stack

*   **Backend Framework**: ASP.NET Core 8.0 MVC (Model-View-Controller pattern)
*   **Database ORM**: Entity Framework Core (Code-First migration approach)
*   **Database Engine**: Microsoft SQL Server / LocalDB
*   **Frontend Technologies**: Razor Views, HTML5, CSS3, JavaScript, Bootstrap 5

---

## 📁 Project Structure

```text
DisSagligiTakip/
├── Business/        # Domain-driven business logic and services
├── Controllers/     # MVC Controllers handling client requests
├── DataAccess/      # Database context (EF Core) and repository patterns
├── DTOs/            # Data Transfer Objects for decoupled API structures
├── Entities/        # Database models (User, Note, FircalamaKaydi, MuayeneRandevusu, etc.)
├── Helpers/         # Utility methods and encryption helpers
├── Migrations/      # EF Core database migrations
├── Models/          # MVC ViewModels
├── Services/        # Service interfaces and implementations
├── Views/           # Razor markup (.cshtml) templates for the user interface
├── wwwroot/         # Public static files (CSS, JS, images, libraries)
├── Program.cs       # ASP.NET Core web host configuration and service registrations
└── appsettings.json # Connection strings and environment configurations
```

---

## 🧠 Database Architecture

The SQLite/SQL Server schema managed via Entity Framework Core (`AppDbContext`) covers the following database relations:
*   `Users`: Stores account credentials, email, phone, and Enum roles.
*   `Notes`: Relates to users (1:N) with cascade-delete constraints for private logs.
*   `FircalamaKayitlari`: Relates to users (1:N) storing logs of brushing timestamps.
*   `DisSagligiVerileri`: Stores dental metrics, tracking both the patient and the creator (in case a doctor records it on behalf of a patient).
*   `MuayeneRandevulari`: Manages clinical appointments mapping patients, physicians, dates, and status codes. Features speed optimizations via composite indexes on (`HekimUserId`, `BaslangicZamani`) and (`HastaUserId`, `BaslangicZamani`).

---

## ⚙️ Setup & Execution

### Prerequisites
- .NET 8.0 SDK installed on your system.
- LocalDB or MS SQL Server installed and running.

### 1. Clone the Repository
```bash
git clone https://github.com/emirtdede/Oral-and-Dental-Health-Tracking-Application.git
cd Oral-and-Dental-Health-Tracking-Application
```

### 2. Configure the Connection String
Open `appsettings.json` and adjust the connection string to match your SQL Server setup:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DisSagligiTakipDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Update Database
Run EF Core migration commands to initialize the database:
```bash
dotnet ef database update
```

### 4. Build and Run
Execute the application:
```bash
dotnet run
```
Open `https://localhost:5001` or `http://localhost:5000` in your web browser.

---

## ⚖️ License
This project is licensed under the [MIT License](LICENSE).

---

<a id="turkish-version"></a>
# Türkçe Versiyon

Ağız ve diş sağlığı alışkanlıklarını takip etmek, kişisel diş sağlığı metriklerini kaydetmek ve klinik randevuları yönetmek amacıyla tasarlanmış kapsamlı ve tam donanımlı (full-stack) bir web uygulamasıdır. **ASP.NET Core MVC** ve **Entity Framework Core** kullanılarak geliştirilen uygulama, hem hijyen alışkanlıklarını izlemek isteyen hastalara hem de randevu planlamalarını yöneten hekimlere hizmet vermektedir.

## 🚀 Öne Çıkan Özellikler

*   **🔐 Güvenli Kimlik Doğrulama & Rol Yönetimi**: Özel rol dağılımlarına (Örn: Hasta, Hekim/Diş Hekimi) sahip güvenli kayıt ve giriş mekanizmaları.
*   **📅 Randevu Yönetim Sistemi**: Diş hekimlerinden uygun tarihlere muayene randevusu alma. Veritabanı sorgularını hızlandırmak ve çakışmaları önlemek için çoklu indeksleme yapılandırması.
*   **🦷 Diş Sağlığı Kayıt Defteri**: Diş semptomlarının, tedavilerin ve klinik verilerin takibi. Hekimlerin hastaları adına sisteme veri girişi yapabilmesi desteği.
*   **🪥 Diş Fırçalama Takibi**: Sağlıklı hijyen alışkanlıkları oluşturmak amacıyla günlük diş fırçalama rutinlerinin (zaman, süre, teknik) kaydedilmesi.
*   **📊 İnteraktif İstatistikler**: Fırçalama sıklığı ve ağız sağlığı gelişimini zaman serisi grafikleriyle sunan gösterge paneli.
*   **📝 Kişisel Günlük & Notlar**: Kullanıcıların diş ağrılarını, semptom geçmişlerini veya hekimlerine soracakları soruları kaydedebilecekleri özel not alanı.

---

## 🛠️ Kullanılan Teknolojiler

*   **Arkayüz Çerçevesi**: ASP.NET Core 8.0 MVC (Model-View-Controller deseni)
*   **Veritabanı ORM**: Entity Framework Core (Code-First yaklaşımı)
*   **Veritabanı Sunucusu**: Microsoft SQL Server / LocalDB
*   **Arayüz Teknolojileri**: Razor Views, HTML5, CSS3, JavaScript, Bootstrap 5

---

## 📁 Proje Yapısı

```text
DisSagligiTakip/
├── Business/        # İş mantığı ve domain katmanı servisleri
├── Controllers/     # İstemci isteklerini işleyen MVC Kontrolcüleri
├── DataAccess/      # EF Core veritabanı bağlamı ve depo (repository) desenleri
├── DTOs/            # Veri transfer nesneleri (Data Transfer Objects)
├── Entities/        # Veritabanı modelleri (User, Note, FircalamaKaydi, MuayeneRandevusu vb.)
├── Helpers/         # Yardımcı araçlar ve şifreleme metotları
├── Migrations/      # EF Core veritabanı göç (migration) dosyaları
├── Models/          # MVC ViewModels yapıları
├── Services/        # Servis arayüzleri ve uygulamaları
├── Views/           # Arayüz için oluşturulmuş Razor şablonları (.cshtml)
├── wwwroot/         # Statik dosyalar (CSS, JS, resimler, kütüphaneler)
├── Program.cs       # Web sunucusu yapılandırmaları ve servis kayıtları
└── appsettings.json # Veritabanı bağlantı adresleri ve ortam ayarları
```

---

## 🧠 Veri Tabanı Mimarisi

Entity Framework Core (`AppDbContext`) üzerinden yönetilen veritabanı ilişkileri şunları kapsamaktadır:
*   `Users`: Kullanıcı kimlik bilgileri, e-posta, telephone ve Enum tabanlı rolleri saklar.
*   `Notes`: Kullanıcılara (1:N) ilişkisiyle bağlıdır, kullanıcı silindiğinde cascade-delete ile temizlenir.
*   `FircalamaKayitlari`: Kullanıcılara (1:N) ilişkisiyle bağlıdır, fırçalama zaman damgalarını tutar.
*   `DisSagligiVerileri`: Ağız sağlığı verilerini saklar, hekimin hasta adına kayıt oluşturabilmesi için hem hastayı hem de kaydı oluşturan kullanıcıyı takip eder.
*   `MuayeneRandevulari`: Randevu kayıtlarını; hasta, hekim, başlangıç saati ve durum kodlarıyla yönetir. Veritabanı sorgularını hızlandırmak amacıyla (`HekimUserId`, `BaslangicZamani`) ve (`HastaUserId`, `BaslangicZamani`) kolonlarında kompozit indeksleme yapılmıştır.

---

## ⚙️ Kurulum ve Çalıştırma

### Önkoşullar
- Bilgisayarınızda .NET 8.0 SDK yüklü olmalıdır.
- LocalDB veya MS SQL Server kurulu ve çalışır durumda olmalıdır.

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/emirtdede/Oral-and-Dental-Health-Tracking-Application.git
cd Oral-and-Dental-Health-Tracking-Application
```

### 2. Bağlantı Dizesini Yapılandırın
`appsettings.json` dosyasını açarak SQL Server sunucunuza uygun şekilde bağlantı dizesini düzenleyin:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DisSagligiTakipDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 3. Veritabanını Güncelleyin
Veritabanını oluşturmak için EF Core migration komutunu çalıştırın:
```bash
dotnet ef database update
```

### 4. Derleyin ve Çalıştırın
Uygulamayı başlatın:
```bash
dotnet run
```
Tarayıcınızdan `https://localhost:5001` veya `http://localhost:5000` adreslerini ziyaret edin.

---

## ⚖️ Lisans
Bu proje [MIT Lisansı](LICENSE) kapsamında lisanslanmıştır.
