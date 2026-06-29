Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class ApiHelper

    Private Shared ReadOnly client As New HttpClient()
    Private Const BaseUrl As String = "http://localhost:8080/api"

    Public Shared Async Function GetAsync(endpoint As String) As Task(Of String)
        Dim response = Await client.GetAsync(BaseUrl & endpoint)
        response.EnsureSuccessStatusCode()
        Return Await response.Content.ReadAsStringAsync()
    End Function

    Public Shared Async Function PostAsync(endpoint As String, data As Object) As Task(Of String)
        Dim json = JsonConvert.SerializeObject(data)
        Dim content = New StringContent(json, Encoding.UTF8, "application/json")
        Dim response = Await client.PostAsync(BaseUrl & endpoint, content)
        Return Await response.Content.ReadAsStringAsync()
    End Function

    Public Shared Async Function PutAsync(endpoint As String, data As Object) As Task(Of String)
        Dim json = JsonConvert.SerializeObject(data)
        Dim content = New StringContent(json, Encoding.UTF8, "application/json")
        Dim response = Await client.PutAsync(BaseUrl & endpoint, content)
        Return Await response.Content.ReadAsStringAsync()
    End Function

    Public Shared Async Function DeleteAsync(endpoint As String) As Task(Of String)
        Dim response = Await client.DeleteAsync(BaseUrl & endpoint)
        Return Await response.Content.ReadAsStringAsync()
    End Function

End Class