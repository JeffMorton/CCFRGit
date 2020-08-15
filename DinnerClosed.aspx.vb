Public Class DinnerClosed
    Inherits PageBase
    'There is a maximum number of people who can attend CCFRdinners.  Yhis nummber is stored in the Information table in the database'
    'When this number is reach, this page is display instead of the membersignup.aspx page.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strInfo As String = Request.QueryString("type")
        If strInfo Is Nothing Then
            Me.DinnerClose.Visible = True
            Me.NotAvailable.Visible = False
        Else
            Me.DinnerClose.Visible = False
            Me.NotAvailable.Visible = True
        End If
    End Sub

End Class
