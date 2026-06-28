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
- **Frontend**: VB.NET (Windows Forms Desktop Application)
- **Backend**: Java 17, Spring Boot 3.x
- **Database**: PostgreSQL 15
- **API Docs**: Swagger (springdoc-openapi)
- **Build Tool**: Maven

## Struktur Project
```bash
soal-2/
├── backend-springboot/
│   └── src/main/java/com/lamda/mahasiswa/
│       ├── controller/    
│       ├── model/
│       ├── repository/
│       ├── service/
│       └── MahasiswaApplication.java
│   └── src/main/resources/
│       └── application.properties
└── frontend-vbnet/
└── MahasiswaApp/       
```
## Cara Menjalankan
### 1. Jalankan Backend
```bash
cd backend-springboot
./mvnw spring-boot:run
```
Backend berjalan di: http://localhost:8080
### 2. Jalankan Frontend
Buka project VB.NET di Visual Studio, lalu jalankan (F5).
### Swagger UI
http://localhost:8080/swagger-ui.html
## Aturan Pengembangan yang Diterapkan
- Tidak ada query database di VB.NET
- VB.NET hanya memanggil endpoint Spring Boot via HttpClient
