
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports System.IO
Imports System.Linq
Imports OfficeOpenXml
Imports OfficeOpenXml.Style

Public Class Form1

    Dim selectedId As Integer = 0

    ' ===== CONTROLS =====
    Dim pnlLeft As GroupBox
    Dim pnlRight As Panel
    Dim lblTitle As Label
    Dim pnlListContainer As Panel

    ' Input fields
    Dim txtNama As TextBox
    Dim txtNim As TextBox
    Dim txtUmur As TextBox
    Dim dtpTglLahir As DateTimePicker
    Dim txtFakultas As TextBox
    Dim txtAlamat As TextBox
    Dim txtJurusan As TextBox
    Dim cboJenjang As ComboBox

    ' Buttons
    Dim btnSave As Button
    Dim btnBatal As Button

    ' Search
    Dim txtSearch As TextBox
    Dim btnSearch As Button
    Dim btnClearSearch As Button

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Sistem Manajemen Data Mahasiswa"
        Me.Size = New Size(1200, 800)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Font = New Font("Segoe UI", 9)
        Me.MinimumSize = New Size(1100, 700)

        BuildUI()
        LoadMahasiswaAsync()
    End Sub

    Private Sub BuildUI()
        ' ===== TITLE BAR =====
        Dim pnlTitle As New Panel()
        pnlTitle.Dock = DockStyle.Top
        pnlTitle.Height = 55
        pnlTitle.BackColor = Color.White

        Dim sepTitle As New Panel()
        sepTitle.Dock = DockStyle.Bottom
        sepTitle.Height = 2
        sepTitle.BackColor = Color.FromArgb(0, 123, 255)
        pnlTitle.Controls.Add(sepTitle)

        lblTitle = New Label()
        lblTitle.Text = "Sistem Manajemen Data Mahasiswa"
        lblTitle.ForeColor = Color.FromArgb(33, 37, 41)
        lblTitle.Font = New Font("Segoe UI", 14, FontStyle.Bold)
        lblTitle.AutoSize = True
        lblTitle.Location = New Point(20, 14)
        pnlTitle.Controls.Add(lblTitle)
        Me.Controls.Add(pnlTitle)

        ' ===== LEFT PANEL =====
        pnlLeft = New GroupBox()
        pnlLeft.Size = New Size(400, 640)
        pnlLeft.Location = New Point(25, 70)
        pnlLeft.BackColor = Color.White
        pnlLeft.Text = "Input Form"
        pnlLeft.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        pnlLeft.ForeColor = Color.FromArgb(0, 123, 255)
        Me.Controls.Add(pnlLeft)

        AddLabel(pnlLeft, "Nama Lengkap", 20, 30)
        txtNama = AddTextBox(pnlLeft, "Masukkan nama...", 20, 52, 170)

        AddLabel(pnlLeft, "NIM", 210, 30)
        txtNim = AddTextBox(pnlLeft, "Masukkan NIM...", 210, 52, 170)

        AddLabel(pnlLeft, "Umur", 20, 95)
        txtUmur = AddTextBox(pnlLeft, "Umur", 20, 117, 170)

        AddLabel(pnlLeft, "Tanggal Lahir", 210, 95)
        dtpTglLahir = New DateTimePicker()
        dtpTglLahir.Location = New Point(210, 117)
        dtpTglLahir.Size = New Size(170, 28)
        dtpTglLahir.Format = DateTimePickerFormat.Short
        pnlLeft.Controls.Add(dtpTglLahir)

        AddLabel(pnlLeft, "Jurusan", 20, 160)
        txtJurusan = AddTextBox(pnlLeft, "Masukkan jurusan...", 20, 182, 170)

        AddLabel(pnlLeft, "Fakultas", 210, 160)
        txtFakultas = AddTextBox(pnlLeft, "Masukkan fakultas...", 210, 182, 170)

        AddLabel(pnlLeft, "Jenjang", 20, 225)
        cboJenjang = New ComboBox()
        cboJenjang.Location = New Point(20, 247)
        cboJenjang.Size = New Size(170, 28)
        cboJenjang.DropDownStyle = ComboBoxStyle.DropDownList
        cboJenjang.FlatStyle = FlatStyle.Flat
        cboJenjang.Items.AddRange({"D1", "D2", "D3", "S1", "S2", "S3"})
        cboJenjang.SelectedIndex = 0
        pnlLeft.Controls.Add(cboJenjang)

        AddLabel(pnlLeft, "Alamat", 20, 290)
        txtAlamat = New TextBox()
        txtAlamat.Location = New Point(20, 312)
        txtAlamat.Size = New Size(360, 80)
        txtAlamat.Multiline = True
        txtAlamat.BorderStyle = BorderStyle.FixedSingle
        pnlLeft.Controls.Add(txtAlamat)

        btnSave = New Button()
        btnSave.Text = "💾 Save"
        btnSave.Location = New Point(20, 410)
        btnSave.Size = New Size(175, 40)
        btnSave.BackColor = Color.FromArgb(40, 167, 69)
        btnSave.ForeColor = Color.White
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.FlatAppearance.BorderSize = 0
        btnSave.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnSave.Cursor = Cursors.Hand
        AddHandler btnSave.Click, AddressOf BtnSave_Click
        pnlLeft.Controls.Add(btnSave)

        btnBatal = New Button()
        btnBatal.Text = "❌ Batal"
        btnBatal.Location = New Point(205, 410)
        btnBatal.Size = New Size(175, 40)
        btnBatal.BackColor = Color.FromArgb(108, 117, 125)
        btnBatal.ForeColor = Color.White
        btnBatal.FlatStyle = FlatStyle.Flat
        btnBatal.FlatAppearance.BorderSize = 0
        btnBatal.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnBatal.Cursor = Cursors.Hand
        btnBatal.Visible = False
        AddHandler btnBatal.Click, AddressOf BtnBatal_Click
        pnlLeft.Controls.Add(btnBatal)
    End Sub

    ' ===== BUILD RIGHT PANEL =====
    Private Sub BuildRightPanel(list As List(Of MahasiswaModel))
        If pnlRight IsNot Nothing Then
            Me.Controls.Remove(pnlRight)
            pnlRight.Dispose()
        End If

        pnlRight = New Panel()
        pnlRight.Size = New Size(730, 700)
        pnlRight.Location = New Point(440, 70)
        pnlRight.BackColor = Color.FromArgb(245, 247, 250)
        Me.Controls.Add(pnlRight)

        Dim lblList As New Label()
        lblList.Text = "List Data Mahasiswa"
        lblList.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblList.ForeColor = Color.FromArgb(0, 123, 255)
        lblList.Location = New Point(0, 0)
        lblList.AutoSize = True
        pnlRight.Controls.Add(lblList)

        ' Search box
        txtSearch = New TextBox()
        txtSearch.Location = New Point(390, 0)
        txtSearch.Size = New Size(180, 28)
        txtSearch.BorderStyle = BorderStyle.FixedSingle
        pnlRight.Controls.Add(txtSearch)

        ' Tombol cari
        btnSearch = New Button()
        btnSearch.Text = "🔍 Cari"
        btnSearch.Location = New Point(575, 0)
        btnSearch.Size = New Size(80, 28)
        btnSearch.BackColor = Color.FromArgb(0, 123, 255)
        btnSearch.ForeColor = Color.White
        btnSearch.FlatStyle = FlatStyle.Flat
        btnSearch.FlatAppearance.BorderSize = 0
        btnSearch.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        pnlRight.Controls.Add(btnSearch)

        ' Tombol X clear search
        btnClearSearch = New Button()
        btnClearSearch.Text = "✕"
        btnClearSearch.Location = New Point(660, 0)
        btnClearSearch.Size = New Size(28, 28)
        btnClearSearch.BackColor = Color.FromArgb(220, 53, 69)
        btnClearSearch.ForeColor = Color.White
        btnClearSearch.FlatStyle = FlatStyle.Flat
        btnClearSearch.FlatAppearance.BorderSize = 0
        btnClearSearch.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        btnClearSearch.Cursor = Cursors.Hand
        btnClearSearch.Visible = False
        AddHandler btnClearSearch.Click, AddressOf BtnClearSearch_Click
        pnlRight.Controls.Add(btnClearSearch)

        ' Scroll panel
        Dim scrollPanel As New Panel()
        scrollPanel.Location = New Point(0, 40)
        scrollPanel.Size = New Size(710, 400)
        scrollPanel.AutoScroll = True
        scrollPanel.BackColor = Color.FromArgb(245, 247, 250)
        pnlRight.Controls.Add(scrollPanel)

        pnlListContainer = New Panel()
        pnlListContainer.Location = New Point(0, 0)
        pnlListContainer.Size = New Size(690, 0)
        pnlListContainer.AutoSize = True
        scrollPanel.Controls.Add(pnlListContainer)

        Dim yPos As Integer = 0
        For Each m In list
            Dim card = BuildMahasiswaCard(m, yPos)
            pnlListContainer.Controls.Add(card)
            yPos += 85
        Next

        ' Export section
        Dim fixedY As Integer = 450

        Dim lblExport As New Label()
        lblExport.Text = "Export Data"
        lblExport.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        lblExport.ForeColor = Color.FromArgb(80, 80, 80)
        lblExport.Location = New Point(0, fixedY)
        lblExport.AutoSize = True
        pnlRight.Controls.Add(lblExport)

        Dim btnExcel = AddExportButton("📊 Excel", 0, fixedY + 30, Color.FromArgb(40, 167, 69))
        Dim btnPDF = AddExportButton("📄 PDF", 110, fixedY + 30, Color.FromArgb(220, 53, 69))
        Dim btnCSV = AddExportButton("📋 CSV", 220, fixedY + 30, Color.FromArgb(108, 117, 125))
        Dim btnJSON = AddExportButton("📁 JSON", 330, fixedY + 30, Color.FromArgb(255, 193, 7))

        AddHandler btnExcel.Click, AddressOf BtnExportExcel_Click
        AddHandler btnPDF.Click, AddressOf BtnExportPDF_Click
        AddHandler btnCSV.Click, AddressOf BtnExportCSV_Click
        AddHandler btnJSON.Click, AddressOf BtnExportJSON_Click

        pnlRight.Controls.Add(btnExcel)
        pnlRight.Controls.Add(btnPDF)
        pnlRight.Controls.Add(btnCSV)
        pnlRight.Controls.Add(btnJSON)

        Dim btnResetAll = New Button()
        btnResetAll.Text = "🔄 Reset All Data"
        btnResetAll.Location = New Point(220, fixedY + 80)
        btnResetAll.Size = New Size(200, 40)
        btnResetAll.BackColor = Color.FromArgb(220, 53, 69)
        btnResetAll.ForeColor = Color.White
        btnResetAll.FlatStyle = FlatStyle.Flat
        btnResetAll.FlatAppearance.BorderSize = 0
        btnResetAll.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        btnResetAll.Cursor = Cursors.Hand
        AddHandler btnResetAll.Click, AddressOf BtnResetAll_Click
        pnlRight.Controls.Add(btnResetAll)

        Dim lblResetInfo As New Label()
        lblResetInfo.Text = "              Menghapus semua list data mahasiswa"
        lblResetInfo.ForeColor = Color.Gray
        lblResetInfo.Font = New Font("Segoe UI", 8)
        lblResetInfo.Location = New Point(175, fixedY + 125)
        lblResetInfo.AutoSize = True
        pnlRight.Controls.Add(lblResetInfo)
    End Sub

    ' ===== BUILD CARD =====
    Private Function BuildMahasiswaCard(m As MahasiswaModel, yPos As Integer) As Panel
        ' Card utama — collapsed by default
        Dim card As New Panel()
        card.Size = New Size(700, 75)
        card.Location = New Point(0, yPos)
        card.BackColor = Color.White
        card.BorderStyle = BorderStyle.FixedSingle
        card.Tag = False ' False = collapsed

        ' Nama (bisa diklik)
        Dim lblNama As New Label()
        lblNama.Text = m.nama
        lblNama.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblNama.ForeColor = Color.FromArgb(0, 123, 255)
        lblNama.Location = New Point(15, 10)
        lblNama.AutoSize = True
        lblNama.Cursor = Cursors.Hand
        card.Controls.Add(lblNama)

        ' Info ringkas
        Dim lblRingkas As New Label()
        lblRingkas.Text = "NIM: " & m.nim &
                          "    Jurusan: " & If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "-") &
                          "    Fakultas: " & If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "-") &
                          "    Jenjang: " & If(m.jurusan IsNot Nothing, m.jurusan.jenjang, "-")
        lblRingkas.Font = New Font("Segoe UI", 9)
        lblRingkas.ForeColor = Color.FromArgb(80, 80, 80)
        lblRingkas.Location = New Point(15, 35)
        lblRingkas.AutoSize = True
        card.Controls.Add(lblRingkas)

        ' Hint klik
        Dim lblHint As New Label()
        lblHint.Text = "▼ klik nama untuk detail"
        lblHint.Font = New Font("Segoe UI", 7, FontStyle.Italic)
        lblHint.ForeColor = Color.LightGray
        lblHint.Location = New Point(15, 55)
        lblHint.AutoSize = True
        card.Controls.Add(lblHint)

        ' Detail panel (hidden by default)
        Dim pnlDetail As New Panel()
        pnlDetail.Location = New Point(0, 75)
        pnlDetail.Size = New Size(700, 70)
        pnlDetail.BackColor = Color.FromArgb(248, 250, 255)
        pnlDetail.Visible = False

        Dim lblDetail1 As New Label()
        lblDetail1.Text = "Umur: " & m.umur & " tahun    Tgl Lahir: " & If(m.tglLahir IsNot Nothing, m.tglLahir, "-")
        lblDetail1.Font = New Font("Segoe UI", 9)
        lblDetail1.ForeColor = Color.FromArgb(60, 60, 60)
        lblDetail1.Location = New Point(15, 10)
        lblDetail1.AutoSize = True
        pnlDetail.Controls.Add(lblDetail1)

        Dim lblDetail2 As New Label()
        lblDetail2.Text = "Alamat: " & If(String.IsNullOrEmpty(m.alamat), "-", m.alamat)
        lblDetail2.Font = New Font("Segoe UI", 9)
        lblDetail2.ForeColor = Color.FromArgb(60, 60, 60)
        lblDetail2.Location = New Point(15, 35)
        lblDetail2.AutoSize = True
        pnlDetail.Controls.Add(lblDetail2)

        card.Controls.Add(pnlDetail)

        ' Tombol Edit & Delete
        Dim btnEdit = New Button()
        btnEdit.Text = "✏️"
        btnEdit.Location = New Point(615, 10)
        btnEdit.Size = New Size(32, 28)
        btnEdit.BackColor = Color.FromArgb(255, 193, 7)
        btnEdit.ForeColor = Color.White
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.FlatAppearance.BorderSize = 0
        btnEdit.Tag = m.id
        btnEdit.Cursor = Cursors.Hand
        AddHandler btnEdit.Click, AddressOf BtnEditCard_Click
        card.Controls.Add(btnEdit)

        Dim btnDel = New Button()
        btnDel.Text = "🗑️"
        btnDel.Location = New Point(652, 10)
        btnDel.Size = New Size(32, 28)
        btnDel.BackColor = Color.FromArgb(220, 53, 69)
        btnDel.ForeColor = Color.White
        btnDel.FlatStyle = FlatStyle.Flat
        btnDel.FlatAppearance.BorderSize = 0
        btnDel.Tag = m.id
        btnDel.Cursor = Cursors.Hand
        AddHandler btnDel.Click, AddressOf BtnDeleteCard_Click
        card.Controls.Add(btnDel)

        ' Klik nama untuk expand/collapse detail
        AddHandler lblNama.Click, Sub(s, ev)
                                      Dim isExpanded = CBool(card.Tag)
                                      If isExpanded Then
                                          ' Collapse
                                          pnlDetail.Visible = False
                                          card.Size = New Size(700, 75)
                                          lblHint.Text = "▼ klik nama untuk detail"
                                          card.Tag = False
                                          ' Geser cards di bawah ke atas
                                          ShiftCardsBelow(card, -70)
                                      Else
                                          ' Expand
                                          pnlDetail.Visible = True
                                          card.Size = New Size(700, 145)
                                          lblHint.Text = "▲ klik nama untuk tutup"
                                          card.Tag = True
                                          ' Geser cards di bawah ke bawah
                                          ShiftCardsBelow(card, 70)
                                      End If
                                  End Sub

        Return card
    End Function

    ' ===== GESER CARDS DI BAWAH =====
    Private Sub ShiftCardsBelow(currentCard As Panel, offset As Integer)
        Dim currentY = currentCard.Location.Y
        For Each ctrl As Control In pnlListContainer.Controls
            If TypeOf ctrl Is Panel AndAlso ctrl.Location.Y > currentY Then
                ctrl.Location = New Point(ctrl.Location.X, ctrl.Location.Y + offset)
            End If
        Next
    End Sub

    ' ===== HELPER UI =====
    Private Function AddLabel(parent As Control, text As String, x As Integer, y As Integer) As Label
        Dim lbl As New Label()
        lbl.Text = text
        lbl.Location = New Point(x, y)
        lbl.AutoSize = True
        lbl.ForeColor = Color.FromArgb(80, 80, 80)
        lbl.Font = New Font("Segoe UI", 9)
        parent.Controls.Add(lbl)
        Return lbl
    End Function

    Private Function AddTextBox(parent As Control, placeholder As String, x As Integer, y As Integer, w As Integer) As TextBox
        Dim txt As New TextBox()
        txt.Location = New Point(x, y)
        txt.Size = New Size(w, 28)
        txt.BorderStyle = BorderStyle.FixedSingle
        parent.Controls.Add(txt)
        Return txt
    End Function

    Private Function AddExportButton(text As String, x As Integer, y As Integer, color As Color) As Button
        Dim btn = New Button()
        btn.Text = text
        btn.Location = New Point(x, y)
        btn.Size = New Size(95, 35)
        btn.BackColor = Color.White
        btn.ForeColor = color
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderColor = color
        btn.FlatAppearance.BorderSize = 1
        btn.Font = New Font("Segoe UI", 9)
        btn.Cursor = Cursors.Hand
        Return btn
    End Function

    ' ===== LOAD DATA =====
    Private Async Sub LoadMahasiswaAsync()
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa")
            Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)
            BuildRightPanel(list)
        Catch ex As Exception
            MessageBox.Show("Gagal load data: " & ex.Message)
        End Try
    End Sub

    ' ===== CARD BUTTONS =====
    Private Sub BtnEditCard_Click(sender As Object, e As EventArgs)
        Dim btn = CType(sender, Button)
        selectedId = CInt(btn.Tag)
        LoadMahasiswaToForm(selectedId)
    End Sub

    Private Async Sub BtnDeleteCard_Click(sender As Object, e As EventArgs)
        Dim btn = CType(sender, Button)
        Dim id = CInt(btn.Tag)
        Dim confirm = MessageBox.Show("Yakin hapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirm = DialogResult.Yes Then
            Try
                Await ApiHelper.DeleteAsync("/mahasiswa/" & id)
                MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ResetForm()
                LoadMahasiswaAsync()
            Catch ex As Exception
                MessageBox.Show("Gagal hapus: " & ex.Message)
            End Try
        End If
    End Sub

    Private Async Sub LoadMahasiswaToForm(id As Integer)
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa/" & id)
            Dim m = JsonConvert.DeserializeObject(Of MahasiswaModel)(json)
            If m IsNot Nothing Then
                pnlLeft.Text = "Edit Form"
                btnSave.Text = "✏️ Update"
                btnSave.BackColor = Color.FromArgb(255, 193, 7)
                btnBatal.Visible = True

                txtNama.Text = m.nama
                txtNim.Text = m.nim
                txtNim.ReadOnly = True
                txtNim.BackColor = Color.FromArgb(240, 240, 240)
                txtUmur.Text = m.umur.ToString()
                txtAlamat.Text = m.alamat
                txtJurusan.Text = If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "")
                txtFakultas.Text = If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "")

                If m.jurusan IsNot Nothing Then
                    Dim idx = cboJenjang.Items.IndexOf(m.jurusan.jenjang)
                    If idx >= 0 Then cboJenjang.SelectedIndex = idx
                End If

                If m.tglLahir IsNot Nothing Then
                    dtpTglLahir.Value = DateTime.Parse(m.tglLahir)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal load data: " & ex.Message)
        End Try
    End Sub

    ' ===== BATAL =====
    Private Sub BtnBatal_Click(sender As Object, e As EventArgs)
        ResetForm()
    End Sub

    ' ===== SEARCH =====
    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs)
        Try
            Dim json As String
            If txtSearch.Text.Trim() = "" Then
                json = Await ApiHelper.GetAsync("/mahasiswa")
                btnClearSearch.Visible = False
            Else
                json = Await ApiHelper.GetAsync("/mahasiswa/search?nama=" & txtSearch.Text.Trim())
                btnClearSearch.Visible = True
            End If
            Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)
            BuildRightPanel(list)
        Catch ex As Exception
            MessageBox.Show("Gagal search: " & ex.Message)
        End Try
    End Sub

    ' ===== CLEAR SEARCH =====
    Private Async Sub BtnClearSearch_Click(sender As Object, e As EventArgs)
        txtSearch.Text = ""
        btnClearSearch.Visible = False
        Dim json = Await ApiHelper.GetAsync("/mahasiswa")
        Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)
        BuildRightPanel(list)
    End Sub

    ' ===== SAVE / UPDATE =====
    Private Async Sub BtnSave_Click(sender As Object, e As EventArgs)
        If Not ValidateInput() Then Return
        Try
            If selectedId = 0 Then
                Dim allJson = Await ApiHelper.GetAsync("/mahasiswa")
                Dim allList = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(allJson)
                If allList IsNot Nothing Then
                    Dim existing = allList.FirstOrDefault(Function(m) m.nim = txtNim.Text.Trim())
                    If existing IsNot Nothing Then
                        MessageBox.Show("NIM sudah terdaftar!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtNim.Focus()
                        Return
                    End If
                End If
            End If

            Dim jurusanId As Integer = 0
            Dim allJurusanJson = Await ApiHelper.GetAsync("/jurusan")
            Dim allJurusan = JsonConvert.DeserializeObject(Of List(Of JurusanModel))(allJurusanJson)
            Dim matchJurusan = allJurusan.FirstOrDefault(Function(j)
                                                             Return j.namaJurusan.ToLower() = txtJurusan.Text.Trim().ToLower() AndAlso
                                                                    j.fakultas.ToLower() = txtFakultas.Text.Trim().ToLower() AndAlso
                                                                    j.jenjang = cboJenjang.SelectedItem.ToString()
                                                         End Function)
            If matchJurusan IsNot Nothing Then
                jurusanId = matchJurusan.idJurusan
            Else
                Dim newJurusan = New With {
                    .namaJurusan = txtJurusan.Text.Trim(),
                    .fakultas = txtFakultas.Text.Trim(),
                    .jenjang = cboJenjang.SelectedItem.ToString()
                }
                Dim jurusanJson = Await ApiHelper.PostAsync("/jurusan", newJurusan)
                Dim createdJurusan = JsonConvert.DeserializeObject(Of JurusanModel)(jurusanJson)
                jurusanId = createdJurusan.idJurusan
            End If

            Dim data = New With {
                .nama = txtNama.Text.Trim(),
                .umur = CInt(txtUmur.Text),
                .nim = txtNim.Text.Trim(),
                .tglLahir = dtpTglLahir.Value.ToString("yyyy-MM-dd"),
                .alamat = txtAlamat.Text.Trim(),
                .jurusan = New With {.idJurusan = jurusanId}
            }

            If selectedId = 0 Then
                Await ApiHelper.PostAsync("/mahasiswa", data)
                MessageBox.Show("Data berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Await ApiHelper.PutAsync("/mahasiswa/" & selectedId, data)
                MessageBox.Show("Data berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ResetForm()
            LoadMahasiswaAsync()
        Catch ex As Exception
            MessageBox.Show("Gagal simpan: " & ex.Message)
        End Try
    End Sub

    ' ===== RESET FORM =====
    Private Sub ResetForm()
        selectedId = 0
        pnlLeft.Text = "Input Form"
        btnSave.Text = "💾 Save"
        btnSave.BackColor = Color.FromArgb(40, 167, 69)
        btnBatal.Visible = False
        txtNama.Text = ""
        txtNim.Text = ""
        txtNim.ReadOnly = False
        txtNim.BackColor = Color.White
        txtUmur.Text = ""
        txtAlamat.Text = ""
        txtJurusan.Text = ""
        txtFakultas.Text = ""
        cboJenjang.SelectedIndex = 0
        dtpTglLahir.Value = DateTime.Now
    End Sub

    ' ===== RESET ALL DATA =====
    Private Async Sub BtnResetAll_Click(sender As Object, e As EventArgs)
        Dim confirm = MessageBox.Show(
            "Yakin hapus SEMUA data mahasiswa? Aksi ini tidak bisa dibatalkan!",
            "Konfirmasi Reset",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)
        If confirm = DialogResult.Yes Then
            Try
                Await ApiHelper.DeleteAsync("/mahasiswa/reset")
                MessageBox.Show("Semua data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ResetForm()
                LoadMahasiswaAsync()
            Catch ex As Exception
                MessageBox.Show("Gagal reset: " & ex.Message)
            End Try
        End If
    End Sub

    ' ===== EXPORT EXCEL =====
    Private Async Sub BtnExportExcel_Click(sender As Object, e As EventArgs)
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa")
            Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)

            Dim sfd As New SaveFileDialog()
            sfd.Filter = "Excel files (*.xlsx)|*.xlsx"
            sfd.FileName = "mahasiswa.xlsx"

            If sfd.ShowDialog() = DialogResult.OK Then
                Using package As New ExcelPackage()
                    Dim ws = package.Workbook.Worksheets.Add("Mahasiswa")
                    Dim headers = {"Nama", "NIM", "Umur", "Tgl Lahir", "Alamat", "Jurusan", "Fakultas", "Jenjang"}
                    For i = 0 To headers.Length - 1
                        ws.Cells(1, i + 1).Value = headers(i)
                    Next
                    Using headerRange = ws.Cells(1, 1, 1, 8)
                        headerRange.Style.Font.Bold = True
                        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid
                        headerRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 123, 255))
                        headerRange.Style.Font.Color.SetColor(Color.White)
                    End Using
                    Dim row = 2
                    For Each m In list
                        ws.Cells(row, 1).Value = m.nama
                        ws.Cells(row, 2).Value = m.nim
                        ws.Cells(row, 3).Value = m.umur
                        ws.Cells(row, 4).Value = If(m.tglLahir IsNot Nothing, m.tglLahir, "")
                        ws.Cells(row, 5).Value = m.alamat
                        ws.Cells(row, 6).Value = If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "")
                        ws.Cells(row, 7).Value = If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "")
                        ws.Cells(row, 8).Value = If(m.jurusan IsNot Nothing, m.jurusan.jenjang, "")
                        row += 1
                    Next
                    ws.Cells.AutoFitColumns()
                    package.SaveAs(New System.IO.FileInfo(sfd.FileName))
                End Using
                MessageBox.Show("Export Excel berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal export Excel: " & ex.Message)
        End Try
    End Sub

    ' ===== EXPORT PDF =====
    Private Async Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa")
            Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)

            Dim sfd As New SaveFileDialog()
            sfd.Filter = "PDF files (*.pdf)|*.pdf"
            sfd.FileName = "mahasiswa.pdf"

            If sfd.ShowDialog() = DialogResult.OK Then
                Using fs As New FileStream(sfd.FileName, FileMode.Create)
                    Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate())
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs)
                    doc.Open()

                    ' ===== JUDUL =====
                    Dim fontJudul = iTextSharp.text.FontFactory.GetFont("Helvetica-Bold", 16)
                    Dim judul As New iTextSharp.text.Paragraph("Data Mahasiswa", fontJudul)
                    judul.Alignment = iTextSharp.text.Element.ALIGN_CENTER
                    judul.SpacingAfter = 15
                    doc.Add(judul)

                    ' ===== TABEL =====
                    Dim table As New iTextSharp.text.pdf.PdfPTable(8)
                    table.WidthPercentage = 100
                    table.SetWidths({15, 15, 7, 12, 18, 13, 12, 8})

                    ' Header style
                    Dim fontHeader = iTextSharp.text.FontFactory.GetFont("Helvetica-Bold", 9, iTextSharp.text.BaseColor.WHITE)
                    Dim headerColor As New iTextSharp.text.BaseColor(0, 123, 255)

                    Dim headers = {"Nama", "NIM", "Umur", "Tgl Lahir", "Alamat", "Jurusan", "Fakultas", "Jenjang"}
                    For Each h In headers
                        Dim cell As New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(h, fontHeader))
                        cell.BackgroundColor = headerColor
                        cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
                        cell.Padding = 6
                        table.AddCell(cell)
                    Next

                    ' Data rows
                    Dim fontData = iTextSharp.text.FontFactory.GetFont("Helvetica", 8)
                    Dim fontDataGray = iTextSharp.text.FontFactory.GetFont("Helvetica", 8)
                    Dim rowCount = 0

                    For Each m In list
                        Dim bgColor = If(rowCount Mod 2 = 0,
                        New iTextSharp.text.BaseColor(245, 248, 255),
                        iTextSharp.text.BaseColor.WHITE)

                        Dim rowData = {
                        m.nama,
                        m.nim,
                        m.umur.ToString(),
                        If(m.tglLahir IsNot Nothing, m.tglLahir, "-"),
                        If(String.IsNullOrEmpty(m.alamat), "-", m.alamat),
                        If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "-"),
                        If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "-"),
                        If(m.jurusan IsNot Nothing, m.jurusan.jenjang, "-")
                    }

                        For Each cellVal In rowData
                            Dim cell As New iTextSharp.text.pdf.PdfPCell(New iTextSharp.text.Phrase(cellVal, fontData))
                            cell.BackgroundColor = bgColor
                            cell.Padding = 5
                            table.AddCell(cell)
                        Next

                        rowCount += 1
                    Next

                    doc.Add(table)

                    ' Footer
                    Dim fontFooter = iTextSharp.text.FontFactory.GetFont("Helvetica", 8, iTextSharp.text.BaseColor.GRAY)
                    Dim footer As New iTextSharp.text.Paragraph(
                    "Dicetak pada: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm") & "    Total data: " & list.Count & " mahasiswa",
                    fontFooter)
                    footer.Alignment = iTextSharp.text.Element.ALIGN_RIGHT
                    footer.SpacingBefore = 10
                    doc.Add(footer)

                    doc.Close()
                End Using

                MessageBox.Show("Export PDF berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal export PDF: " & ex.Message)
        End Try
    End Sub

    ' ===== EXPORT CSV =====
    Private Async Sub BtnExportCSV_Click(sender As Object, e As EventArgs)
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa")
            Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)

            Dim sfd As New SaveFileDialog()
            sfd.Filter = "CSV files (*.csv)|*.csv"
            sfd.FileName = "mahasiswa.csv"

            If sfd.ShowDialog() = DialogResult.OK Then
                Using sw As New StreamWriter(sfd.FileName)
                    sw.WriteLine("Nama,NIM,Umur,Tgl Lahir,Alamat,Jurusan,Fakultas,Jenjang")
                    For Each m In list
                        sw.WriteLine($"{m.nama},{m.nim},{m.umur},{m.tglLahir},{m.alamat}," &
                                     $"{If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "")}," &
                                     $"{If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "")}," &
                                     $"{If(m.jurusan IsNot Nothing, m.jurusan.jenjang, "")}")
                    Next
                End Using
                MessageBox.Show("Export CSV berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal export CSV: " & ex.Message)
        End Try
    End Sub

    ' ===== EXPORT JSON =====
    Private Async Sub BtnExportJSON_Click(sender As Object, e As EventArgs)
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa")
            Dim sfd As New SaveFileDialog()
            sfd.Filter = "JSON files (*.json)|*.json"
            sfd.FileName = "mahasiswa.json"
            If sfd.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(sfd.FileName, json)
                MessageBox.Show("Export JSON berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal export JSON: " & ex.Message)
        End Try
    End Sub

    ' ===== VALIDASI =====
    Private Function ValidateInput() As Boolean
        If txtNama.Text.Trim() = "" Then
            MessageBox.Show("Nama tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNama.Focus()
            Return False
        End If
        If txtNim.Text.Trim() = "" Then
            MessageBox.Show("NIM tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNim.Focus()
            Return False
        End If
        If txtUmur.Text.Trim() = "" OrElse Not Integer.TryParse(txtUmur.Text, Nothing) Then
            MessageBox.Show("Umur harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtUmur.Focus()
            Return False
        End If
        If txtJurusan.Text.Trim() = "" Then
            MessageBox.Show("Jurusan tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtJurusan.Focus()
            Return False
        End If
        If txtFakultas.Text.Trim() = "" Then
            MessageBox.Show("Fakultas tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFakultas.Focus()
            Return False
        End If
        Return True
    End Function

End Class