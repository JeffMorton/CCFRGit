Imports System.Data.SqlClient

Public Class UpdateWebMenu
    Inherits PageBase
    Dim conn As SqlConnection
    'Allows the administrator to update the webmnus table.  Can be run multiple times without creating a problem.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection With {.ConnectionString = GetConnectionString(False, False)}
        conn.Open()
        Me.webmenuSource.ConnectionString = conn.ConnectionString
    End Sub

End Class
