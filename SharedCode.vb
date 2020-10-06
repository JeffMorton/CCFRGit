

Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Imports System.Data
Imports System.IO
Imports System.Net.Mail
'All of the shared code is in this module.
Public Module SharedCode
    Public Sub RenderReport(rv As ReportViewer, reportname As String, Folderlocation As String)
        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing
        Dim bytes As Byte()

        'First delete existing file
        Dim filepath As String = Folderlocation & reportname
        File.Delete(filepath)
        Dim deviceInfo As String
        deviceInfo = "<DeviceInfo>" +
            " <OutputFormat>EMF</OutputFormat>" +
            " <PageWidth>8.5in</PageWidth>" +
            " <PageHeight>11in</PageHeight>" +
            " <MarginTop>0.25in</MarginTop>" +
            " <MarginLeft>0.50in</MarginLeft>" +
            " <MarginRight>0.25in</MarginRight>" +
            " <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>"
        'Then create new pdf file
        Dim type As String = Mid(reportname, (Len(reportname) - 2))
        bytes = rv.LocalReport.Render(type, deviceInfo, Nothing, mimeType,
            encoding, extension, streamids, warnings)

        Dim fs As New FileStream(Folderlocation & reportname, FileMode.Create)
        fs.Write(bytes, 0, bytes.Length)
        fs.Close()

    End Sub
    Public Function Info(ByVal st As String) As Object
        Dim strConnection As String = ConfigurationManager.ConnectionStrings("DB_25784_ccfrsqlConnectionString").ToString
        Dim sqlConnection As New SqlConnection(strConnection)

        Dim SQLQuery As String = "SELECT " & st & " from Information"
        Dim command As New SqlCommand(SQLQuery, sqlConnection)

        Dim Dr As SqlDataReader
        sqlConnection.Open()
        Dr = command.ExecuteReader()
        Dr.Read()
        Info = Dr(0)

    End Function
    Public Function GetScalarInt(strSQL As String, conn As SqlConnection) As Integer
        Using command As SqlCommand = New SqlCommand(strSQL, conn)
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then
                reader.Read()
                GetScalarInt = CInt(reader("cnt").ToString)

                reader.Close()
            Else
                GetScalarInt = 0
            End If
        End Using
    End Function
    Public Sub RecordError(message As String, errinner As String, loc As String, conn As SqlConnection)
        Try
            Dim strSQL As String = "insert into ErrorLog(Memberid,EventID,ErrorMessage,Date,Errorinner,loc) values (@memid,@evID,@msg,@dat,@errinner,@loc)"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.Add("@memid", SqlDbType.Int)
                cmd.Parameters("@memid").Value = System.Web.HttpContext.Current.Session("UserID")
                cmd.Parameters.Add("@dat", SqlDbType.Date)
                cmd.Parameters("@dat").Value = Now.ToLongDateString
                cmd.Parameters.Add("@evID", SqlDbType.Int)
                cmd.Parameters("@evID").Value = System.Web.HttpContext.Current.Session("EventID")
                cmd.Parameters.Add("@msg", SqlDbType.NVarChar)
                cmd.Parameters("@msg").Value = message
                cmd.Parameters.Add("@loc", SqlDbType.NVarChar)
                cmd.Parameters("@loc").Value = loc
                cmd.Parameters.Add("@errinner", SqlDbType.NVarChar)
                cmd.Parameters("@errinner").Value = errinner
                cmd.ExecuteNonQuery()
            End Using
        Catch
        End Try
    End Sub
    Public Sub GetEventID(type As String)
        Dim conn As New SqlConnection(GetConnectionString(True, False))
        conn.Open()
        Dim strSQL As String
        If type = "Dinner" Then
            strSQL = "select ID,eventdate from Event where EventDate >= @Now and Type =  'Dinner' order by EventDate "
        Else
            strSQL = "Select [ID], [Cost], [EventDate], [Meal1], [Meal2], [Meal3], [Cost] FROM [Event] where [eventdate] >= @Now And Type =  'Lunch/Discussion' order by eventdate"
        End If
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@Now", Now().ToShortDateString)
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                System.Web.HttpContext.Current.Session("EventDate") = dr("Eventdate")
                System.Web.HttpContext.Current.Session("EventID") = dr("ID").ToString
            Else
                System.Web.HttpContext.Current.Session("EventDate") = "1/1/1900"
                System.Web.HttpContext.Current.Session("EventID") = "0"
            End If
            dr.Close()
        End Using

    End Sub


    Public Function GetConnectionString(PrimarySource As Boolean, xDebug As Boolean) As String
        'used on adinistrative part of website
        'If Primary source is true, the page can be run without loggin in.  Fakllse requires logiging in
        'if xDebug is true, the connetion string returned points to the development database other wise, it points to the production database
        If HttpContext.Current.Request.IsLocal = True And CBool(System.Web.HttpContext.Current.Session("Debug")) = True Then
            xDebug = True
            System.Web.HttpContext.Current.Session("Loggedin") = True
            If CBool(System.Web.HttpContext.Current.Session("Admin")) = False Then
                System.Web.HttpContext.Current.Session("UserID") = 3487
                System.Web.HttpContext.Current.Session("FullName") = "Test Member"
                System.Web.HttpContext.Current.Session("UserName") = "Test Member"
            End If
        End If
            If xDebug Then
            GetConnectionString = ConfigurationManager.ConnectionStrings("CCFRDataConnectionString").ToString()
            System.Web.HttpContext.Current.Session("conn") = GetConnectionString
        Else
            If PrimarySource Then
                GetConnectionString = ConfigurationManager.ConnectionStrings("DB_25784_ccfrsqlConnectionString").ToString
                System.Web.HttpContext.Current.Session("conn") = GetConnectionString
            Else
                If CBool(System.Web.HttpContext.Current.Session("loggedin")) Then
                    GetConnectionString = ConfigurationManager.ConnectionStrings("DB_25784_ccfrsqlConnectionString").ToString
                    System.Web.HttpContext.Current.Session("conn") = GetConnectionString
                Else
                    System.Web.HttpContext.Current.Response.Redirect("http://ccfrcville.org/default.aspx")
                    GetConnectionString = ""
                End If
            End If
        End If
    End Function
    Public Function ReportDataS(strSQL As String, dsName As String, conn As SqlConnection) As ReportDataSource
        Dim adp As New SqlDataAdapter(strSQL, conn)
        Dim rs As New DataTable
        adp.Fill(rs)
        Dim ds As New DataSet(dsName)
        ds.Tables.Add(rs)

        Dim rds As New ReportDataSource With {
                .Name = dsName,
                .Value = ds.Tables(0)
            }
        ReportDataS = rds
        ds.Dispose()
        adp.Dispose()

    End Function
    Public Function ReportDataSC(cmd As SqlCommand, dsName As String) As ReportDataSource
        Dim adp As New SqlDataAdapter(cmd)
        Dim rs As New DataTable
        adp.Fill(rs)
        Dim ds As New DataSet(dsName)
        ds.Tables.Add(rs)
        Dim rds As New ReportDataSource With {
                .Name = dsName,
                .Value = ds.Tables(0)
            }
        ReportDataSC = rds
        ds.Dispose()
        adp.Dispose()
    End Function
    Public Function GridViewDataS(cmd As SqlCommand) As DataTable
        Using ada As SqlDataAdapter = New SqlDataAdapter(cmd)
            Using dt As DataTable = New DataTable()
                ada.Fill(dt)

                GridViewDataS = dt
            End Using
        End Using
    End Function
    Public Function SendMessage(MessageSubject As String, MessageBody As String, ToEmail As String, FromEmail As String, BccEmail As String, IsHtml As Boolean, Attach() As String, ByRef CntMember As Integer) As Integer
        ' If AttaachementCode Is 0, there are no attachments.  If non-zero, it identifies the email meesage page sending the email.
        Dim BccM() As String
        Dim ToEM() As String
        Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        Dim i As Integer
        myMessage.From = New System.Net.Mail.MailAddress(FromEmail)
        myMessage.Subject = MessageSubject
        If Not String.IsNullOrEmpty(ToEmail) Then
            ToEM = Split(ToEmail, ";")
            For i = 0 To ToEM.Length - 1
                myMessage.To.Add(ToEM(i))
            Next
        Else
            Exit Function
        End If

        If Not String.IsNullOrEmpty(BccEmail) Then
            BccM = Split(BccEmail, ";")

            For i = 0 To BccM.Length - 1
                myMessage.Bcc.Add(BccM(i))
            Next
        End If

        For i = 0 To Attach.Length - 1
            If Not String.IsNullOrEmpty(Attach(i)) Then
                Dim attch As New Attachment(Attach(i))
                myMessage.Attachments.Add(attch)
            Else
                Exit For
            End If
        Next

        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr@2020"

        myMessage.IsBodyHtml = IsHtml
        myMessage.Body = MessageBody

        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)

        Dim MailObj As New System.Net.Mail.SmtpClient(smtpMailServer) With {
            .Credentials = basicAuthenticationInfo,
            .Host = "m04.internetmailserver.net"
        }
        Try
            MailObj.Send(myMessage)
            If Not String.IsNullOrEmpty(BccEmail) Then
                CntMember += BccM.Length
            Else
                CntMember += ToEM.Length
            End If
            SendMessage = 1
        Catch
            SendMessage = 0
        End Try

    End Function

    Public Function SendMessageNA(MessageSubject As String, MessageBody As String, ToEmail As String, FromEmail As String, BccEmail As String, IsHtml As Boolean) As Integer
        ' If AttaachementCode Is 0, there are no attachments.  If non-zero, it identifies the email meesage page sending the email.
        Dim BccM() As String
        Dim ToEM() As String
        Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        Dim i As Integer
        myMessage.From = New System.Net.Mail.MailAddress(FromEmail)
        myMessage.Subject = MessageSubject
        If Not String.IsNullOrEmpty(ToEmail) Then
            ToEM = Split(ToEmail, ";")
            For i = 0 To ToEM.Length - 1
                myMessage.To.Add(ToEM(i))
            Next
        End If

        If Not String.IsNullOrEmpty(BccEmail) Then
            BccM = Split(BccEmail, ";")

            For i = 0 To BccM.Length - 1
                myMessage.Bcc.Add(BccM(i))
            Next
        End If



        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr@2020"

        myMessage.IsBodyHtml = IsHtml
        myMessage.Body = MessageBody

        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)

        Dim MailObj As New System.Net.Mail.SmtpClient(smtpMailServer) With {
            .Credentials = basicAuthenticationInfo,
            .Host = "m04.internetmailserver.net"
        }
        Try
            MailObj.Send(myMessage)

            SendMessageNA = 1
        Catch
            SendMessageNA = 0
        End Try

    End Function
    Public Sub SendFinalEmail(CntEmails As Integer, CntMember As Integer, EmailSource As String)
        Dim Attach(1) As String

        Attach(1) = ""
        Dim MsgBody As String = "<p>The  " & EmailSource & " option just send " & CStr(CntEmails) & " Emails. " & CStr(CntMember) & " members received these emails.<p>"
        SendMessage("Final Email from " & EmailSource, MsgBody, "Webmaster@ccfrcville.org", "reservatation@ccfrcville.org", "", True, Attach, 0)

    End Sub
End Module
