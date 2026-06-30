package com.lamda.mahasiswa.service;

import com.lamda.mahasiswa.model.Jurusan;
import com.lamda.mahasiswa.model.Mahasiswa;
import com.lamda.mahasiswa.repository.JurusanRepository;
import com.lamda.mahasiswa.repository.MahasiswaRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import java.util.List;
import java.util.Optional;

@Service
@RequiredArgsConstructor
public class MahasiswaService {

    private final MahasiswaRepository mahasiswaRepo;
    private final JurusanRepository jurusanRepo;

    // ===== MAHASISWA =====
    public List<Mahasiswa> getAll() { return mahasiswaRepo.findAll(); }

    public Optional<Mahasiswa> getById(Integer id) { return mahasiswaRepo.findById(id); }

    public List<Mahasiswa> search(String nama) {
        return mahasiswaRepo.findByNamaContainingIgnoreCase(nama);
    }

    public Mahasiswa save(Mahasiswa m) { return mahasiswaRepo.save(m); }

    public void delete(Integer id) { mahasiswaRepo.deleteById(id); }
    public void deleteAll() { mahasiswaRepo.deleteAll(); }

    // ===== JURUSAN =====
    public List<Jurusan> getAllJurusan() { return jurusanRepo.findAll(); }
    public Jurusan saveJurusan(Jurusan j) { return jurusanRepo.save(j); }
}