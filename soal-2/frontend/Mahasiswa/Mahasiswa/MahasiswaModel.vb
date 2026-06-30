Imports Newtonsoft.Json

Public Class MahasiswaModel
    Public Property id As Integer
    Public Property nama As String
    Public Property umur As Integer
    Public Property nim As String
    Public Property tglLahir As String
    Public Property alamat As String
    Public Property jurusan As JurusanModel
End Class

Public Class JurusanModel
    Public Property idJurusan As Integer
    Public Property namaJurusan As String
    Public Property fakultas As String
    Public Property jenjang As String
End Class