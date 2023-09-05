Public Class Membership
    Inherits PageBase
    'This page provides information to the public about membership.  It is part of the public section of the website
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label1.Text = FormatCurrency(Info("Dues")) & "."
    End Sub

End Class
