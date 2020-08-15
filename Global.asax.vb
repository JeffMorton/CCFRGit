

Imports System.Data.SqlClient
Public Class Global_asax
    Inherits System.Web.HttpApplication


    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        'AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim lastErrorWrapper As HttpException =
            CType(Server.GetLastError(), HttpException)

        Dim lastError As Exception = lastErrorWrapper
        If lastErrorWrapper.InnerException IsNot Nothing Then
            lastError = lastErrorWrapper.InnerException
        End If
        Dim lastErrorTypeName As String = lastError.GetType().ToString()
        Dim lastErrorMessage As String = lastError.Message
        Dim lastErrorStackTrace As String = lastError.StackTrace

        'insert into WebErrorLog
        Dim conn As New SqlConnection(GetConnectionStringM(True, False))

        conn.Open()
        Dim strSQL As String = "insert into WebErrorLog(ErrorMessage,StackTrace,Typename,ErrorTime) values (@Message,@trace,@type,@dTime)"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@message", lastErrorMessage)
            cmd.Parameters.AddWithValue("@trace", lastErrorStackTrace)
            cmd.Parameters.AddWithValue("@type", lastErrorTypeName)
            cmd.Parameters.AddWithValue("@dTime", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
            cmd.ExecuteNonQuery()
        End Using
        Dim uname As String =User.Identity.Name
        If String.IsNullOrEmpty(uname) Then
            uname="Name Not Available"
        End If
        Dim strMessage As String = ""
        If Session("EventID") Is Nothing Then
            strMessage += "Session(EvemtID) Is nothing"
        Else
            strMessage += "Session(EvemtID) Is " & Session("EventID").ToString
        End If
        strMessage += "<br />"
        If CBool(Session("LoggedIn")) = True Then
            strMessage += "Logged In"
        Else
            strMessage += "Not Logged In"
        End If
        strMessage += "<br />"
        If Session("conn") Is Nothing Then
            strMessage += "No Connection String"
        Else
            strMessage += Mid(Session("conn").ToString, 1, 70)
        End If

        strMessage += " <br /> " & uname & "<br /><br />"


        If Session("UserID") Is Nothing Then
            Session("UserID") = "User ID Not Available"
        End If

        strMessage = strMessage & "<br />" & Request.RawUrl & "<br /><br />" &
        Session("UserID").ToString & "<br />" &
  lastErrorTypeName & "<br />" &
  lastErrorMessage & "<br />" &
  lastErrorStackTrace.Replace(Environment.NewLine, "<br />") & "<br />"
        Dim cnt As Integer = 0
        Dim attach(1) As String
        Response.Redirect("Error.aspx")
        SendMessage(lastErrorMessage, strMessage, "webmaster@ccfrcville.org;ccfrwm@gmail.com", "webmaster@ccfrcville.org", "", True, attach, cnt)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class
