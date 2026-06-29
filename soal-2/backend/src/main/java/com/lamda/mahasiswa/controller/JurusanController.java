package com.lamda.mahasiswa.controller;

import com.lamda.mahasiswa.model.Jurusan;
import com.lamda.mahasiswa.service.MahasiswaService;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/jurusan")
@RequiredArgsConstructor
@CrossOrigin(origins = "*")
@Tag(name = "Jurusan", description = "Data Jurusan")
public class JurusanController {

    private final MahasiswaService service;

    @GetMapping
    public List<Jurusan> getAll() { return service.getAllJurusan(); }
}