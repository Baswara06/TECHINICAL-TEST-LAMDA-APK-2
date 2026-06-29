package com.lamda.mahasiswa.controller;

import com.lamda.mahasiswa.model.Mahasiswa;
import com.lamda.mahasiswa.service.MahasiswaService;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/mahasiswa")
@RequiredArgsConstructor
@CrossOrigin(origins = "*")
@Tag(name = "Mahasiswa", description = "CRUD Mahasiswa")
public class MahasiswaController {

    private final MahasiswaService service;

    @GetMapping
    public List<Mahasiswa> getAll() {
        return service.getAll();
    }

    @GetMapping("/{id}")
    public ResponseEntity<Mahasiswa> getById(@PathVariable Integer id) {
        return service.getById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @GetMapping("/search")
    public List<Mahasiswa> search(@RequestParam String nama) {
        return service.search(nama);
    }

    @PostMapping
    public Mahasiswa create(@RequestBody Mahasiswa m) {
        return service.save(m);
    }

    @PutMapping("/{id}")
    public ResponseEntity<Mahasiswa> update(@PathVariable Integer id, @RequestBody Mahasiswa m) {
        return service.getById(id).map(existing -> {
            existing.setNama(m.getNama());
            existing.setUmur(m.getUmur());
            existing.setNim(m.getNim());
            existing.setTglLahir(m.getTglLahir());
            existing.setAlamat(m.getAlamat());
            existing.setJurusan(m.getJurusan());
            return ResponseEntity.ok(service.save(existing));
        }).orElse(ResponseEntity.notFound().build());
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable Integer id) {
        service.delete(id);
        return ResponseEntity.noContent().build();
    }
}