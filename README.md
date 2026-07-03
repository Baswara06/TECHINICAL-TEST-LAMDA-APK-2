# TECHINICAL-TEST-LAMDA-APK-2
VB.NET + Spring Boot + PostgreSQL + Minikube

## Identitas
- **Nama**: Raihan Wendra Baswara
- **Universitas**: Telkom University
- **Program Studi**: S1 INFORMATIKA (2024)

## Deskripsi
Aplikasi desktop CRUD mahasiswa. Backend Spring Boot menyediakan REST API, frontend VB.NET hanya consume API tersebut, tidak ada query database langsung dari VB.NET.

## Teknologi yang Digunakan
- **Frontend**: VB.NET (Windows Forms, .NET Framework 4.7.2)
- **Backend**: Java 21, Spring Boot 3.x
- **Database**: PostgreSQL 15
- **API Docs**: Swagger (springdoc-openapi)
- **Build Tool**: Maven 3.9+
- **Deployment**: Docker, Minikube (Kubernetes)

## Struktur Project
```bash
soal-2/
├── backend/
│   ├── src/main/java/com/lamda/mahasiswa/
│   │   ├── controller/
│   │   ├── model/
│   │   ├── repository/
│   │   ├── service/
│   │   └── MahasiswaApplication.java
│   ├── src/main/resources/
│   │   └── application.properties
│   ├── Dockerfile
│   └── pom.xml
├── frontend/
│   └── Mahasiswa/
└── k8s/
    ├── backend.yaml
    └── database.yaml
```

## Cara Menjalankan

### Opsi 1 — Lokal (tanpa Minikube)

#### 1. Jalankan Backend
```bash
cd backend
mvn spring-boot:run
```
Backend berjalan di port 8080.

#### 2. Jalankan Frontend
Buka project VB.NET di Visual Studio, lalu jalankan (F5).

---

### Opsi 2 — Deploy ke Minikube

#### Prerequisites
- Docker Desktop
- Minikube
- kubectl

#### 1. Start Minikube
```bash
minikube start
```

#### 2. Build Docker Image
```bash
cd soal-2
docker build -t backend-soal2:latest ./backend
minikube image load backend-soal2:latest
```

#### 3. Deploy ke Kubernetes
```bash
kubectl apply -f k8s/database.yaml
kubectl apply -f k8s/backend.yaml
```

#### 4. Akses Backend
```bash
minikube service backend-soal2-service --url
```
URL yang muncul digunakan di `ApiHelper.vb` sebagai `BaseUrl`. Terminal harus tetap terbuka.

#### 5. Jalankan Frontend
Ganti `BaseUrl` di `ApiHelper.vb` dengan URL dari step 4, lalu jalankan di Visual Studio (F5).

---

## Konfigurasi Database (Lokal)
Edit `backend/src/main/resources/application.properties`:
```properties
spring.datasource.url=jdbc:postgresql://localhost:5432/mahasiswa_db
spring.datasource.username=postgres
spring.datasource.password=YOUR_PASSWORD
```

## Dokumentasi API

Swagger UI tersedia setelah backend dijalankan:

| Mode | URL |
|------|-----|
| Lokal | http://localhost:8080/swagger-ui.html |
| Minikube | {URL dari minikube service}/swagger-ui.html |

### Endpoint yang Tersedia

| Method | Endpoint | Deskripsi |
|--------|----------|-----------|
| GET | /api/mahasiswa | Ambil semua mahasiswa |
| GET | /api/mahasiswa/{id} | Ambil mahasiswa by ID |
| GET | /api/mahasiswa/search?nama= | Search mahasiswa by nama |
| POST | /api/mahasiswa | Tambah mahasiswa baru |
| PUT | /api/mahasiswa/{id} | Update mahasiswa |
| DELETE | /api/mahasiswa/{id} | Hapus mahasiswa |
| DELETE | /api/mahasiswa/reset | Hapus semua data mahasiswa |
| GET | /api/jurusan | Ambil semua jurusan |
| POST | /api/jurusan | Tambah jurusan baru |

## Aturan Pengembangan yang Diterapkan
- Tidak ada query database di VB.NET
- VB.NET hanya memanggil endpoint Spring Boot via HttpClient

## Fitur Aplikasi
- CRUD Data Mahasiswa (Create, Read, Update, Delete)
- Search mahasiswa berdasarkan nama
- Auto-create jurusan baru saat input data
- Validasi input form
- Export data ke Excel, PDF, CSV, JSON
- Reset semua data mahasiswa
- Tampilan card dengan detail expand/collapse
