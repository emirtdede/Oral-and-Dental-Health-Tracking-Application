<div align="center">

# 🦷 Oral and Dental Health Tracking Application

[![](https://img.shields.io/badge/Language-English-blue?style=for-the-badge&logo=google-translate)](#english-version)
&nbsp;&nbsp;&nbsp;&nbsp;
[![](https://img.shields.io/badge/Dil-T%C3%BCrk%C3%A7e-red?style=for-the-badge&logo=google-translate)](#turkish-version)

---

[![.NET Core](https://img.shields.io/badge/.NET_Core-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/)
[![ASP.NET MVC](https://img.shields.io/badge/ASP.NET_Core_MVC-Web_App-blue?style=flat-square&logo=microsoft&logoColor=white)](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview)
[![EF Core](https://img.shields.io/badge/EF_Core-SQL_Server-purple?style=flat-square&logo=microsoft&logoColor=white)](#)
[![Cookie Auth](https://img.shields.io/badge/Authentication-Cookie-orange?style=flat-square)](#)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](https://opensource.org/licenses/MIT)

</div>

---

<a id="english-version"></a>
# English Version

A comprehensive full-stack **Oral and Dental Health Tracking Web Application** designed for patient file management, appointment scheduling, and personal dental hygiene tracking. It provides specialized access layers for patients, dentists, and clinic assistants.

## 🚀 Key Features

*   **👥 Role-based Access Control**: Cookie-based authentication system supporting distinct portals for Patients, Dentists, and Clinic Assistants.
*   **📅 Appointment Scheduler**: Interactive booking and management system enabling clinics to organize doctor shifts, patients to view available slots, and prescribe medical notes.
*   **🦷 Detailed Dental Records**: Systematically logs dental data (decay, filling, sensitivity, gum conditions, pain rating) with physical image uploads (configured with a safe 20MB limit).
*   **🪥 Brushing Log Tracker**: Encourages patients to log daily oral hygiene habits (timer, brushing techniques, gum bleeding tracking).
*   **✉️ SMTP Email Integration**: Automatic notification engine configured to send reminders and confirmation emails through Gmail SMTP.
*   **📝 Secure Session & Local Logging**: Session state management and structured diagnostic logging via `FileLogService`.

---

## 🛠️ Technology Stack

*   **Core**: .NET Core 8 (ASP.NET Core MVC architecture)
*   **Database**: Entity Framework Core with SQL Server integration
*   **Authentication**: Cookie Authentication middleware (sliding expiration, HttpOnly, secure cookies)
*   **Development Utilities**: Razor Runtime Compilation enabled under debug environments for hot-reloading views

---

## 💾 Core Entities & Data Model

The application architecture utilizes 5 main relational database entities:
1.  **User**: Stores names, secure password credentials, email contacts, and roles (Patient, Dentist, Assistant).
2.  **DisSagligiVerisi**: Records specific dental health updates, descriptive details, prescribing doctor names, and visual uploads.
3.  **MuayeneRandevusu**: Schedules appointments containing clinic, slot, doctor, and status metadata.
4.  **FircalamaKaydi**: Tracks personal habits including brushing duration, bleeding flags, and techniques.
5.  **Note**: Stores personal reminders, category mappings, and urgency levels.

---

## 📁 Project Structure

```text
├── Business/           # Business logic service implementations
├── Controllers/        # MVC request flow control (Auth, Home, Randevu, Notes)
├── DataAccess/         # DB Context definitions, database schema initializers (SeedData)
├── DTOs/               # Data Transfer Objects
├── Entities/           # Database tables and custom HSL enum definitions
├── Views/              # Razor layouts and template interfaces
├── ViewModels/         # Composite ViewModels mapped to presentation layers
├── Migrations/         # EF Core migration files
├── Program.cs          # Pipeline configurations and Dependency Injection mapping
└── appsettings.json    # Connection strings and SMTP parameters
```

---

## ⚙️ Setup & Execution Guide

### 1. Clone the Repository
```bash
git clone https://github.com/emirtdede/Oral-and-Dental-Health-Tracking-Application.git
cd Oral-and-Dental-Health-Tracking-Application
```

### 2. Configure Database & Connection Strings
Open `appsettings.json` and configure your local SQL Server instance:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=DisSagligiDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Update Database Schema
Ensure the latest migrations are applied to your database:
```bash
dotnet ef database update
```

### 4. Configure SMTP Server (Optional)
Specify your email credentials in `appsettings.json` for mail integrations:
```json
"Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSsl": true,
    "User": "your-email@gmail.com",
    "Password": "your-app-password"
}
```

### 5. Launch the Application
Run the project using dotNET CLI:
```bash
dotnet run
```
Open the localhost address shown in your terminal (typically `https://localhost:5001`).

---

## ⚖️ License
This project is licensed under the [MIT License](LICENSE).

---

<a id="turkish-version"></a>
# Türkçe Versiyon

Hasta kaydı yönetimi, randevu çizelgeleri ve kişisel ağız hijyeni takibi için geliştirilmiş, tam donanımlı (full-stack) bir **Ağız ve Diş Sağlığı Takip Web Uygulamasıdır**. Hastalar, diş hekimleri ve klinik asistanları için özel olarak yetkilendirilmiş erişim panelleri sunar.

## 🚀 Öne Çıkan Özellikler

*   **👥 Rol Tabanlı Erişim Yönetimi**: Cookie tabanlı kimlik doğrulama sistemi ile Hasta, Diş Hekimi ve Klinik Asistanı rolleri için ayrılmış ekranlar.
*   **📅 Muayene Randevu Sistemi**: Kliniklerin hekim mesailerini planlamasına, hastaların boş randevu slotlarını seçip randevu almasına ve reçete/hekim notu eklenmesine olanak tanıyan etkileşimli yapı.
*   **🦷 Detaylı Ağız ve Diş Kaydı**: Çürük, dolgu, hassasiyet, diş eti kanaması gibi diş bazlı durumları, güvenli 20MB dosya yükleme desteğiyle görsel ekleyerek kaydetme.
*   **🪥 Fırçalama Takip Günlüğü**: Hastaların günlük diş fırçalama alışkanlıklarını (süre, fırçalama teknikleri, diş eti kanaması durumu) kayıt altına almasını teşvik eden modül.
*   **✉️ SMTP E-posta Entegrasyonu**: Gmail SMTP altyapısı üzerinden randevu onayları, hatırlatmalar ve bildirim e-postaları gönderen otomatik servis.
*   **📝 Güvenli Oturum ve Dosya Günlüğü**: Oturum yönetimi desteği ve `FileLogService` ile sunucu üzerinde yapılandırılmış günlük (log) kaydı.

---

## 🛠️ Kullanılan Teknolojiler

*   **Çekirdek**: .NET Core 8 (ASP.NET Core MVC mimarisi)
*   **Veritabanı**: Entity Framework Core ile SQL Server ilişkisel veritabanı
*   **Kimlik Doğrulama**: Cookie Kimlik Doğrulama ara katmanı (sliding expiration, HttpOnly, secure çerezler)
*   **Geliştirme Araçları**: Arayüzlerin hızlı düzenlenebilmesi için geliştirme ortamında Razor Runtime Compilation aktif edilmiştir.

---

## 💾 Temel Veri Modelleri (Entities)

Uygulama ilişkisel veritabanında 5 ana model üzerinden çalışmaktadır:
1.  **User**: Kullanıcı bilgileri, şifre hash'leri, e-posta adresleri ve rolleri tutar.
2.  **DisSagligiVerisi**: Diş sağlık durumu güncellemelerini, açıklamaları, hekim adını ve yüklenen görselleri saklar.
3.  **MuayeneRandevusu**: Randevu tarihi, klinik, hekim ve durum bilgilerini içerir.
4.  **FircalamaKaydi**: Fırçalama süresi, kanama durumu ve fırçalama tekniklerini kaydeder.
5.  **Note**: Kişisel notları, önem düzeylerini ve not kategorilerini barındırır.

---

## 📁 Proje Yapısı

```text
├── Business/           # İş mantığı (Business logic) servisleri ve kodları
├── Controllers/        # MVC istek yönlendiricileri (Giriş, Randevu, Notlar vb.)
├── DataAccess/         # DB Context tanımları ve başlangıç verileri (SeedData)
├── DTOs/               # Veri Taşıma Nesneleri (Data Transfer Objects)
├── Entities/           # Veritabanı tablo modelleri ve Enum tanımları
├── Views/              # Razor arayüz şablonları ve sayfaları
├── ViewModels/         # Arayüze veri taşımak için özelleştirilmiş modeller
├── Migrations/         # EF Core veritabanı göç (migration) dosyaları
├── Program.cs          # Pipeline ayarları ve Servis/Repository DI tanımları
└── appsettings.json    # Veritabanı bağlantı dizeleri ve SMTP parametreleri
```

---

## ⚙️ Kurulum ve Çalıştırma

### 1. Depoyu Klonlayın
```bash
git clone https://github.com/emirtdede/Oral-and-Dental-Health-Tracking-Application.git
cd Oral-and-Dental-Health-Tracking-Application
```

### 2. Veritabanını Yapılandırın
`appsettings.json` dosyasını açarak yerel SQL Server bağlantı dizenizi tanımlayın:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=YEREL_SUNUCU;Database=DisSagligiDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Veritabanını Güncelleyin
Gerekli tabloları veritabanında oluşturmak için aşağıdaki komutu çalıştırın:
```bash
dotnet ef database update
```

### 4. SMTP Sunucusunu Yapılandırın (Opsiyonel)
E-posta bildirimleri için `appsettings.json` altından mail bilgilerinizi tanımlayın:
```json
"Smtp": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSsl": true,
    "User": "e-postaniz@gmail.com",
    "Password": "uygulama-sifreniz"
}
```

### 5. Uygulamayı Başlatın
Projeyi dotNET CLI kullanarak çalıştırın:
```bash
dotnet run
```
Konsolda belirtilen adresi tarayıcınızda açın (Genellikle `https://localhost:5001`).

---

## ⚖️ Lisans
Bu proje [MIT Lisansı](LICENSE) kapsamında lisanslanmıştır.