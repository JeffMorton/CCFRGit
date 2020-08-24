Imports System.Data.SqlClient
Public Class PosititionTable
    Inherits PageBase
    Dim conn As SqlConnection
    'The page allows the administrator to review and make additions to the Offices Table.  The offices table contains the names of the organization offices like priesident, treasurer etc.
    'This table is used to populate the "Position" field in the Member Table.  An entry in this table is  "Member" so all members have a position.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection With {.ConnectionString = GetConnectionString(False, False)}
        conn.Open()
        Me.PositionSource.ConnectionString = conn.ConnectionString
    End Sub
    Protected Sub RowCommands(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Insert" Then
            Dim control As Control
            If (Not (PositionGridView.FooterRow) Is Nothing) Then
                control = PositionGridView.FooterRow
            Else
                control = PositionGridView.Controls(0).Controls(0)
            End If
            Dim OfficeName As String = CType(control.FindControl("OfficeName"), TextBox).Text
            Dim OfficeOrder As String = CType(control.FindControl("OfficeOrder"), TextBox).Text
            Dim strSQL As String = "Insert into Offices (OfficeName,OfficeOrder) values (@OfficeName,@OfficeOrder)"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@OfficeName", OfficeName)
                cmd.Parameters.AddWithValue("@OfficeOrder", OfficeOrder)
                cmd.ExecuteNonQuery()
                PositionGridView.DataBind()
            End Using
        End If
    End Sub
    Protected Sub DeleteRow(sender As Object, e As GridViewDeleteEventArgs)

    End Sub
End Class

