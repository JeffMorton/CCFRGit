Partial Public Class LunchClosed
    Inherits PageBase
    'There are limits to the number of members who can signup for luncheons.  If that number is reached, this page is displayed instead of the signup page.  It also handles the case when no future luncheon is scheduled.
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
