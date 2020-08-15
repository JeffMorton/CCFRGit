Imports System.IO

Public Class Handler1
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        System.Diagnostics.Debugger.Launch()
        Dim requestedfile As String = context.Server.MapPath(context.Request.FilePath)
        Dim Finfo As FileInfo = New FileInfo(requestedfile)
        Dim Fextension As String = Finfo.Extension
        Dim fname As String = Finfo.Name
        If context.User.Identity.IsAuthenticated Or fname = "ProspMemberInfo.pdf" Or fname = "Membership form.pdf" Then
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" & fname)
            context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Octet
            context.Response.WriteFile(context.Server.MapPath("~/Documents/" + fname))

            Return
        Else
            context.Response.Redirect(context.Server.MapPath("/Default.aspx"))
        End If


    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class