package com.lamda.mahasiswa.model;

import jakarta.persistence.*;
import lombok.*;

@Entity
@Table(name = "jurusan")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Jurusan {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id_jurusan")
    private Integer idJurusan;

    @Column(name = "nama_jurusan")
    private String namaJurusan;

    private String fakultas;
    private String jenjang;
}