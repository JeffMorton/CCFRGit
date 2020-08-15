Partial Public Class LunchClosed
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strInfo As String = Request.QueryString("type")
        If strInfo Is Nothing Then
            Me.LunchClosed.Visible = True
            Me.NotAvailable.Visible = False
        Else
            Me.LunchClosed.Visible = False
            Me.NotAvailable.Visible = True
        End If
    End Sub

End Class
