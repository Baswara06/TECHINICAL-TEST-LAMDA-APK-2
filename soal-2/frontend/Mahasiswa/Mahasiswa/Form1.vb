Imports System.Drawing
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports System.IO
Imports System.Linq

Public Class Form1

    Dim selectedId As Integer = 0
    Dim jurusanList As List(Of JurusanModel) = New List(Of JurusanModel)()

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
    Dim cboJurusan As ComboBox
    Dim txtFakultas As TextBox
    Dim txtJenjang As TextBox
    Dim txtAlamat As TextBox

    ' Buttons
    Dim btnSave As Button
    Dim btnBatal As Button

    ' Search
    Dim txtSearch As TextBox
    Dim btnSearch As Button

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Sistem Manajemen Data Mahasiswa"
        Me.Size = New Size(1200, 800)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(245, 247, 250)
        Me.Font = New Font("Segoe UI", 9)
        Me.MinimumSize = New Size(1100, 700)

        BuildUI()
        LoadJurusanAsync()
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

        ' ===== LEFT PANEL (Input Form) =====
        pnlLeft = New GroupBox()
        pnlLeft.Size = New Size(400, 620)
        pnlLeft.Location = New Point(25, 70)
        pnlLeft.BackColor = Color.White
        pnlLeft.Text = "Input Form"
        pnlLeft.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        pnlLeft.ForeColor = Color.FromArgb(0, 123, 255)
        Me.Controls.Add(pnlLeft)

        ' Nama & NIM row
        AddLabel(pnlLeft, "Nama Lengkap", 20, 30)
        txtNama = AddTextBox(pnlLeft, "Masukkan nama...", 20, 52, 170)

        AddLabel(pnlLeft, "NIM", 210, 30)
        txtNim = AddTextBox(pnlLeft, "Masukkan NIM...", 210, 52, 170)

        ' Umur & Tgl Lahir row
        AddLabel(pnlLeft, "Umur", 20, 95)
        txtUmur = AddTextBox(pnlLeft, "Umur", 20, 117, 170)

        AddLabel(pnlLeft, "Tanggal Lahir", 210, 95)
        dtpTglLahir = New DateTimePicker()
        dtpTglLahir.Location = New Point(210, 117)
        dtpTglLahir.Size = New Size(170, 28)
        dtpTglLahir.Format = DateTimePickerFormat.Short
        pnlLeft.Controls.Add(dtpTglLahir)

        ' Jurusan (ComboBox)
        AddLabel(pnlLeft, "Jurusan", 20, 160)
        cboJurusan = New ComboBox()
        cboJurusan.Location = New Point(20, 182)
        cboJurusan.Size = New Size(170, 28)
        cboJurusan.DropDownStyle = ComboBoxStyle.DropDownList
        cboJurusan.FlatStyle = FlatStyle.Flat
        AddHandler cboJurusan.SelectedIndexChanged, AddressOf CboJurusan_SelectedIndexChanged
        pnlLeft.Controls.Add(cboJurusan)

        AddLabel(pnlLeft, "Fakultas", 210, 160)
        txtFakultas = AddTextBox(pnlLeft, "Fakultas...", 210, 182, 170)
        txtFakultas.ReadOnly = True
        txtFakultas.BackColor = Color.FromArgb(240, 240, 240)

        AddLabel(pnlLeft, "Jenjang", 20, 225)
        txtJenjang = AddTextBox(pnlLeft, "Jenjang...", 20, 247, 170)
        txtJenjang.ReadOnly = True
        txtJenjang.BackColor = Color.FromArgb(240, 240, 240)

        ' Alamat
        AddLabel(pnlLeft, "Alamat", 20, 290)
        txtAlamat = New TextBox()
        txtAlamat.Location = New Point(20, 312)
        txtAlamat.Size = New Size(360, 80)
        txtAlamat.Multiline = True
        txtAlamat.BorderStyle = BorderStyle.FixedSingle
        pnlLeft.Controls.Add(txtAlamat)

        ' Save Button
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

        ' Batal Button (hidden by default)
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

    ' ===== BUILD RIGHT PANEL (List + Export + Reset) =====
    Private Sub BuildRightPanel(list As List(Of MahasiswaModel))
        If pnlRight IsNot Nothing Then
            Me.Controls.Remove(pnlRight)
            pnlRight.Dispose()
        End If

        pnlRight = New Panel()
        pnlRight.Size = New Size(730, 650)
        pnlRight.Location = New Point(440, 70)
        pnlRight.BackColor = Color.FromArgb(245, 247, 250)
        Me.Controls.Add(pnlRight)

        ' Label List
        Dim lblList As New Label()
        lblList.Text = "List Data Mahasiswa"
        lblList.Font = New Font("Segoe UI", 11, FontStyle.Bold)
        lblList.ForeColor = Color.FromArgb(0, 123, 255)
        lblList.Location = New Point(0, 0)
        lblList.AutoSize = True
        pnlRight.Controls.Add(lblList)

        ' Search box
        txtSearch = New TextBox()
        txtSearch.Location = New Point(420, 0)
        txtSearch.Size = New Size(180, 28)
        txtSearch.BorderStyle = BorderStyle.FixedSingle
        pnlRight.Controls.Add(txtSearch)

        btnSearch = New Button()
        btnSearch.Text = "🔍 Cari"
        btnSearch.Location = New Point(605, 0)
        btnSearch.Size = New Size(90, 28)
        btnSearch.BackColor = Color.FromArgb(0, 123, 255)
        btnSearch.ForeColor = Color.White
        btnSearch.FlatStyle = FlatStyle.Flat
        btnSearch.FlatAppearance.BorderSize = 0
        btnSearch.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        pnlRight.Controls.Add(btnSearch)

        ' Scrollable Container untuk cards (MAX 5 card kelihatan)
        Dim scrollPanel As New Panel()
        scrollPanel.Location = New Point(0, 40)
        scrollPanel.Size = New Size(710, 400)
        scrollPanel.AutoScroll = True
        scrollPanel.BackColor = Color.FromArgb(245, 247, 250)
        pnlRight.Controls.Add(scrollPanel)

        ' Inner container untuk cards
        pnlListContainer = New Panel()
        pnlListContainer.Location = New Point(0, 0)
        pnlListContainer.Size = New Size(690, 0)
        pnlListContainer.AutoSize = True
        scrollPanel.Controls.Add(pnlListContainer)

        ' Build cards
        Dim yPos As Integer = 0
        For Each m In list
            Dim card = BuildMahasiswaCard(m, yPos)
            pnlListContainer.Controls.Add(card)
            yPos += 110
        Next

        ' FIXED AREA: Export & Reset (tidak ikut scroll)
        Dim fixedY As Integer = 450

        ' Export Section
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

        ' Reset All Data Button
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
        lblResetInfo.Text = "Kembali ke 0 - Menghapus semua data mahasiswa"
        lblResetInfo.ForeColor = Color.Gray
        lblResetInfo.Font = New Font("Segoe UI", 8)
        lblResetInfo.Location = New Point(180, fixedY + 125)
        lblResetInfo.AutoSize = True
        pnlRight.Controls.Add(lblResetInfo)
    End Sub

    ' ===== BUILD CARD MAHASISWA =====
    Private Function BuildMahasiswaCard(m As MahasiswaModel, yPos As Integer) As Panel
        Dim card As New Panel()
        card.Size = New Size(700, 100)
        card.Location = New Point(0, yPos)
        card.BackColor = Color.White
        card.BorderStyle = BorderStyle.FixedSingle
        card.Padding = New Padding(15)

        Dim lblNama As New Label()
        lblNama.Text = m.nama
        lblNama.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblNama.ForeColor = Color.FromArgb(33, 37, 41)
        lblNama.Location = New Point(15, 10)
        lblNama.AutoSize = True
        card.Controls.Add(lblNama)

        Dim lblInfo1 As New Label()
        lblInfo1.Text = "NIM: " & m.nim & "    Jenjang: " & If(m.jurusan IsNot Nothing, m.jurusan.jenjang, "-")
        lblInfo1.Font = New Font("Segoe UI", 9)
        lblInfo1.ForeColor = Color.FromArgb(80, 80, 80)
        lblInfo1.Location = New Point(15, 35)
        lblInfo1.AutoSize = True
        card.Controls.Add(lblInfo1)

        Dim jurusanName = If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "-")
        Dim fakultasName = If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "-")
        Dim lblInfo2 As New Label()
        lblInfo2.Text = "Jurusan: " & jurusanName & "    Fakultas: " & fakultasName & "    Alamat: " & m.alamat
        lblInfo2.Font = New Font("Segoe UI", 8)
        lblInfo2.ForeColor = Color.Gray
        lblInfo2.Location = New Point(15, 55)
        lblInfo2.AutoSize = True
        card.Controls.Add(lblInfo2)

        Dim lblInfo3 As New Label()
        lblInfo3.Text = "Tgl Lahir: " & If(m.tglLahir IsNot Nothing, m.tglLahir, "-") & "    Umur: " & m.umur
        lblInfo3.Font = New Font("Segoe UI", 8)
        lblInfo3.ForeColor = Color.Gray
        lblInfo3.Location = New Point(15, 72)
        lblInfo3.AutoSize = True
        card.Controls.Add(lblInfo3)

        Dim btnEdit = New Button()
        btnEdit.Text = "✏️"
        btnEdit.Location = New Point(600, 15)
        btnEdit.Size = New Size(35, 30)
        btnEdit.BackColor = Color.FromArgb(255, 193, 7)
        btnEdit.ForeColor = Color.White
        btnEdit.FlatStyle = FlatStyle.Flat
        btnEdit.FlatAppearance.BorderSize = 0
        btnEdit.Font = New Font("Segoe UI", 10)
        btnEdit.Tag = m.id
        AddHandler btnEdit.Click, AddressOf BtnEditCard_Click
        card.Controls.Add(btnEdit)

        Dim btnDelete = New Button()
        btnDelete.Text = "🗑️"
        btnDelete.Location = New Point(640, 15)
        btnDelete.Size = New Size(35, 30)
        btnDelete.BackColor = Color.FromArgb(220, 53, 69)
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.FlatAppearance.BorderSize = 0
        btnDelete.Font = New Font("Segoe UI", 10)
        btnDelete.Tag = m.id
        AddHandler btnDelete.Click, AddressOf BtnDeleteCard_Click
        card.Controls.Add(btnDelete)

        Return card
    End Function

    ' ===== EXPORT BUTTON HELPER =====
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

    Private Async Sub LoadJurusanAsync()
        Try
            Dim json = Await ApiHelper.GetAsync("/jurusan")
            jurusanList = JsonConvert.DeserializeObject(Of List(Of JurusanModel))(json)
            cboJurusan.DataSource = Nothing
            cboJurusan.DataSource = jurusanList
            cboJurusan.DisplayMember = "namaJurusan"
            cboJurusan.ValueMember = "idJurusan"
        Catch ex As Exception
            MessageBox.Show("Gagal load jurusan: " & ex.Message)
        End Try
    End Sub

    ' ===== COMBOBOX JURUSAN =====
    Private Sub CboJurusan_SelectedIndexChanged(sender As Object, e As EventArgs)
        If cboJurusan.SelectedItem IsNot Nothing Then
            Dim j = CType(cboJurusan.SelectedItem, JurusanModel)
            txtFakultas.Text = j.fakultas
            txtJenjang.Text = j.jenjang
        End If
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
                ' Set Edit Mode
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

                If m.tglLahir IsNot Nothing Then
                    dtpTglLahir.Value = DateTime.Parse(m.tglLahir)
                End If

                ' Set ComboBox jurusan
                For Each item As JurusanModel In cboJurusan.Items
                    If item.idJurusan = m.jurusan.idJurusan Then
                        cboJurusan.SelectedItem = item
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal load data: " & ex.Message)
        End Try
    End Sub

    ' ===== BATAL EDIT =====
    Private Sub BtnBatal_Click(sender As Object, e As EventArgs)
        ResetForm()
    End Sub

    ' ===== SEARCH =====
    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs)
        Try
            Dim json As String
            If txtSearch.Text.Trim() = "" Then
                json = Await ApiHelper.GetAsync("/mahasiswa")
            Else
                json = Await ApiHelper.GetAsync("/mahasiswa/search?nama=" & txtSearch.Text.Trim())
            End If
            Dim list = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(json)
            BuildRightPanel(list)
        Catch ex As Exception
            MessageBox.Show("Gagal search: " & ex.Message)
        End Try
    End Sub

    ' ===== SAVE / UPDATE =====
    Private Async Sub BtnSave_Click(sender As Object, e As EventArgs)
        If Not ValidateInput() Then Return

        Try
            ' Cek NIM duplikat untuk mode Save
            If selectedId = 0 Then
                Dim allJson = Await ApiHelper.GetAsync("/mahasiswa")
                Dim allList = JsonConvert.DeserializeObject(Of List(Of MahasiswaModel))(allJson)
                If allList IsNot Nothing Then
                    Dim existing = allList.FirstOrDefault(Function(m) m.nim = txtNim.Text.Trim())
                    If existing IsNot Nothing Then
                        MessageBox.Show("NIM tidak valid (Sudah terdaftar).", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        txtNim.Focus()
                        Return
                    End If
                End If
            End If

            Dim j = CType(cboJurusan.SelectedItem, JurusanModel)
            Dim data = New With {
                .nama = txtNama.Text,
                .umur = CInt(txtUmur.Text),
                .nim = txtNim.Text,
                .tglLahir = dtpTglLahir.Value.ToString("yyyy-MM-dd"),
                .alamat = txtAlamat.Text,
                .jurusan = New With {.idJurusan = j.idJurusan}
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

    ' ===== RESET =====
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
        dtpTglLahir.Value = DateTime.Now
        If cboJurusan.Items.Count > 0 Then cboJurusan.SelectedIndex = 0
    End Sub

    Private Sub BtnResetAll_Click(sender As Object, e As EventArgs)
        Dim confirm = MessageBox.Show("Yakin hapus SEMUA data mahasiswa?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm = DialogResult.Yes Then
            MessageBox.Show("Fitur ini membutuhkan endpoint DELETE /mahasiswa/all di backend")
        End If
    End Sub

    ' ===== EXPORT =====
    Private Sub BtnExportExcel_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Export Excel - perlu tambahan library seperti EPPlus")
    End Sub

    Private Sub BtnExportPDF_Click(sender As Object, e As EventArgs)
        MessageBox.Show("Export PDF - perlu tambahan library seperti iTextSharp")
    End Sub

    Private Sub BtnExportCSV_Click(sender As Object, e As EventArgs)
        ExportToCSV()
    End Sub

    Private Sub BtnExportJSON_Click(sender As Object, e As EventArgs)
        ExportToJSON()
    End Sub

    Private Async Sub ExportToCSV()
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
                        Dim jurusan = If(m.jurusan IsNot Nothing, m.jurusan.namaJurusan, "")
                        Dim fakultas = If(m.jurusan IsNot Nothing, m.jurusan.fakultas, "")
                        Dim jenjang = If(m.jurusan IsNot Nothing, m.jurusan.jenjang, "")
                        sw.WriteLine($"{m.nama},{m.nim},{m.umur},{m.tglLahir},{m.alamat},{jurusan},{fakultas},{jenjang}")
                    Next
                End Using
                MessageBox.Show("Export CSV berhasil!", "Sukses")
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal export: " & ex.Message)
        End Try
    End Sub

    Private Async Sub ExportToJSON()
        Try
            Dim json = Await ApiHelper.GetAsync("/mahasiswa")
            Dim sfd As New SaveFileDialog()
            sfd.Filter = "JSON files (*.json)|*.json"
            sfd.FileName = "mahasiswa.json"

            If sfd.ShowDialog() = DialogResult.OK Then
                File.WriteAllText(sfd.FileName, json)
                MessageBox.Show("Export JSON berhasil!", "Sukses")
            End If
        Catch ex As Exception
            MessageBox.Show("Gagal export: " & ex.Message)
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
        If cboJurusan.SelectedItem Is Nothing Then
            MessageBox.Show("Pilih jurusan dulu!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

End Class