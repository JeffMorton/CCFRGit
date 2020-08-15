Imports System.Data.SqlClient
Public Class About
    Inherits PageBase
    Dim conn As New SqlConnection
    ' AAount us page.  Avaialble to Public.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionStringM(True, False))
        conn.Open()

        Using cmd As New SqlCommand("Select PageText  from PageTexts where PageName = 'About Us'", conn)

            Me.MainText.Text = cmd.ExecuteScalar.ToString

        End Using
    End Sub
End Class
