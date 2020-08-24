Public Class PageBase
    Inherits System.Web.UI.Page
    'This was written to fix a problem running the website on Safari.  It may no longer be needed but does no harm.
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim str As String
        str = Request.ServerVariables("http_user_agent")
        If UCase(str).Contains("SAFARI") = True Then
            ClientTarget = "uplevel"
        End If
    End Sub
End Class
