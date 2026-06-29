package com.lamda.mahasiswa.model;

import jakarta.persistence.*;
import lombok.*;
import java.time.LocalDate;

@Entity
@Table(name = "mahasiswa")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class Mahasiswa {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    private String nama;
    private Integer umur;
    private String nim;

    @Column(name = "tgl_lahir")
    private LocalDate tglLahir;

    private String alamat;

    @ManyToOne
    @JoinColumn(name = "id_jurusan")
    private Jurusan jurusan;
}