Imports System.Data.SqlClient
Public Class History
    Inherits PageBase
    Dim conn As New SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionStringM(True, False))
        conn.Open()
        If Not IsPostBack Then
            Using cmd As New SqlCommand("Select PageText  from PageTexts where PageName = 'History'", conn)

                Me.MainText.Text = cmd.ExecuteScalar.ToString

            End Using
        End If
    End Sub

End Class