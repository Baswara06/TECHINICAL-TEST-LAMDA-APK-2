# TECHINICAL-TEST-LAMDA-APK-2
VB.NET + Spring Boot + PostgreSQL

## Identitas
- **Nama**: Raihan Wendra Baswara
- **Universitas**: Telkom University
- **Program Studi**: S1 INFORMATIKA (2024)

## Deskripsi
Aplikasi desktop CRUD mahasiswa. Backend Spring Boot menyediakan REST API, 
frontend VB.NET hanya consume API tersebut  tidak ada 
query database langsung dari VB.NET.

## Teknologi yang Digunakan
- **Frontend**: Visual Studio (dengan .NET Framework 4.7.2)
- **Backend**: Java 21, Spring Boot 3.x
- **Database**: PostgreSQL 15
- **API Docs**: Swagger (springdoc-openapi)
- **Build Tool**: Maven 3.9+

## Struktur Project
```bash
soal-2/
├── backend/
│   └── src/main/java/com/lamda/mahasiswa/
│       ├── controller/
│       ├── model/
│       ├── repository/
│       ├── service/
│       └── MahasiswaApplication.java
│   └── src/main/resources/
│       └── application.properties
│   └── pom.xml
└── frontend/
    └── Mahasiswa/
```

## Konfigurasi Database
Edit file `backend/src/main/resources/application.properties`:
```
spring.datasource.url=jdbc:postgresql://localhost:5432/mahasiswa_db
spring.datasource.username=postgres
spring.datasource.password=YOUR_PASSWORD
```
## Cara Menjalankan
### 1. Jalankan Backend
```bash
cd backend
mvn spring-boot:run
```
Backend berjalan di port 8080
### 2. Jalankan Frontend
Buka project VB.NET di Visual Studio, lalu jalankan (F5).
### Swagger UI
http://localhost:8080/swagger-ui.html
## Aturan Pengembangan yang Diterapkan
- Tidak ada query database di VB.NET
- VB.NET hanya memanggil endpoint Spring Boot via HttpClient
## Fitur Aplikasi
- CRUD Data Mahasiswa (Create, Read, Update, Delete)
- Search mahasiswa berdasarkan nama
- Validasi input form
- Export data ke Excel, PDF, CSV, JSON
- Reset semua data mahasiswa

