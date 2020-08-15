Partial Public Class DenyEvent
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        label1.Text = Session("UserID").ToString
    End Sub

End Class
