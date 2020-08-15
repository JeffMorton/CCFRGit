Imports System.Data.SqlClient
Public Class Account
    Inherits PageBase
    Dim conn As SqlConnection
    'provides a list of financial transactions involving payments from members. Part of Admin section.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        Using cmd As New SqlCommand("Select [tID], [tType], [tCheckNumber], [tCheckDate], [tCheckAmount], [tPayee], [tCategory], [tBalance], [tEventDate] FROM [dbo].[Account] order by tid desc", conn)
            AccountGridview.DataSource = GridViewDataS(cmd)
            AccountGridview.DataBind()
            AccountGridview.SelectedIndex = AccountGridview.Rows.Count
        End Using

    End Sub
    Protected Sub Accountgridview_rowcreated(sender As Object, e As GridViewRowEventArgs)
        AccountGridview.PageIndex = AccountGridview.PageCount - 1
    End Sub

End Class
