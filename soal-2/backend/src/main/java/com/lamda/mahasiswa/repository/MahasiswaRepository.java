package com.lamda.mahasiswa.repository;

import com.lamda.mahasiswa.model.Mahasiswa;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;

public interface MahasiswaRepository extends JpaRepository<Mahasiswa, Integer> {
    List<Mahasiswa> findByNamaContainingIgnoreCase(String nama);
}