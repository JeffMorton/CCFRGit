Imports System.Web

Public Class PageBase
    Inherits System.Web.UI.Page
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Dim str As String
        str = Request.ServerVariables("http_user_agent")
        If UCase(str).Contains("SAFARI") = True Then
            ClientTarget = "uplevel"
        End If
    End Sub
End Class
