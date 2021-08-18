Imports System.Data.SqlClient
Public Class CommitteeList
    Inherits PageBase
    Dim conn As SqlConnection
    'This page allows the administrator to view and edit the meal category table.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection With {
            .ConnectionString = GetConnectionString(False, False)
        }
        conn.Open()

        Me.CommListSource.ConnectionString = conn.ConnectionString
    End Sub
    Protected Sub RowCommands(sender As Object, e As GridViewCommandEventArgs)

        If e.CommandName = "Insert" Then
            Dim control As Control
            If (CommListGridView.FooterRow) IsNot Nothing Then
                control = CommListGridView.FooterRow
            Else
                control = CommListGridView.Controls(0).Controls(0)
            End If
            Dim ComName As String = CType(control.FindControl("ComName"), TextBox).Text
            Dim strSQL As String = "INSERT into CommitteeList (ComName) VALUES (@ComName)"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@ComName", ComName)
                cmd.ExecuteNonQuery()
                CommListGridView.DataBind()
            End Using
        End If
    End Sub
    Protected Sub DeleteRow(sender As Object, e As GridViewDeleteEventArgs)
        Dim row As Integer = e.RowIndex
        Dim rID As Integer = CInt(CType(CommListGridView.Rows(row).FindControl("ID"), Label).Text)
        Using cmd1 As New SqlCommand("Delete CommitteeList where ID=@ID", conn)
            cmd1.Parameters.AddWithValue("@ID", rID)
            cmd1.ExecuteNonQuery()
        End Using

    End Sub
End Class
