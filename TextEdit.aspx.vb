Imports System.Data.SqlClient
Public Class TextEdit
    Inherits PageBase

    Dim conn As New SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        If Not CBool(Session("Loggedin")) = True Then Response.Redirect("Admin.aspx")
        Dim type As String = Request.QueryString("type")
        If type Is Nothing Then Exit Sub
        Using cmd As New SqlCommand("Select *  from PageTexts where PageName = @Pname", conn)
            cmd.Parameters.AddWithValue("@Pname", type)
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            If dr.Read Then
                Label1.Text = dr("PageName").ToString & " Page"
                PTextEd.Text = dr("PageText").ToString
                Session("PageID") = dr("ID")
            Else
                Response.Redirect("Default.aspx")
            End If
            dr.Close()
        End Using




    End Sub
    Protected Sub PSave(sender As Object, e As EventArgs) Handles Save.Click
        Using cmd As New SqlCommand("Update PageTexts set PageText = @NewText where ID = @ID", conn)
            cmd.Parameters.AddWithValue("@NewText", PTextEd.Text)
            cmd.Parameters.AddWithValue("@ID", Session("PageID"))
            cmd.ExecuteNonQuery()
        End Using
    End Sub
End Class
