Imports Newtonsoft.Json

Public Class MahasiswaModel
    Public Property id As Integer
    Public Property nama As String
    Public Property umur As Integer
    Public Property nim As String

    <JsonProperty("tgl_lahir")>
    Public Property tglLahir As String

    Public Property alamat As String
    Public Property jurusan As JurusanModel
End Class

Public Class JurusanModel
    <JsonProperty("id_jurusan")>
    Public Property idJurusan As Integer

    <JsonProperty("nama_jurusan")>
    Public Property namaJurusan As String

    Public Property fakultas As String
    Public Property jenjang As String
End Class