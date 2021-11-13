Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class EMailReminders
    Inherits PageBase
    Dim conn As SqlConnection
    Dim etype As String
    Dim CntMember As Integer = 0
    Dim Totalcnt As Integer = 0
    'This pages provides a way for the administrator to send an email reminder to all members attending a meeting.
    'This reminder is preformated although the user can add a message.
    'This page includes CKEditor rich text editor and emails are html enabled.

    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        Using cmd As New SqlCommand("SELECT type from event where ID =@EventID", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            etype = cmd.ExecuteScalar.ToString
        End Using
        If Not IsPostBack Then
            Me.EventID.Text = Session("EventID").ToString
            Me.EventDate.Text = Session("EventDate").ToString
            Using cmd As New SqlCommand("select * from Attachments where eventID=@EventID", conn)
                cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader
                If dr.HasRows Then
                    Do While dr.Read()
                        ListAttachments.Items.Add(dr("FileName").ToString)
                    Loop
                End If
                dr.Close()
            End Using
            ReportViewer1.Visible = False
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.ReportPath = "Attendance.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()

            ReportViewer1.ShowExportControls = True


            Dim strSQL As String = "select Firstname + ' ' + lastname as Fullname, Guest from AttendenceReport(@EventID)  where lastname >' ' order by Lastname,Firstname"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@EventID", EventID.Text)
                Dim adp As New SqlDataAdapter(cmd)
                Dim rs As New DataTable
                adp.Fill(rs)
                Dim ds As New DataSet("dsattendence")
                ds.Tables.Add(rs)

                Dim rds As New ReportDataSource With {
                    .Name = "dsattendence",
                    .Value = ds.Tables(0)
                }
                ReportViewer1.LocalReport.DataSources.Add(rds)
                ds.Dispose()
                adp.Dispose()
            End Using

            Dim param As ReportParameter() = New ReportParameter(5) {}
            param(0) = New ReportParameter("DayTimePrinted", CStr(Now()))
            param(1) = New ReportParameter("MeetingDate", CStr(EventDate.Text))
            Dim c1 As Integer = GetScalarInt("Select count(Spouseattend) as cnt from membersignup where eventid = " & EventID.Text & "And spouseattend = 'true'", conn)
            param(3) = New ReportParameter("NumSpouse", CStr(c1))
            Dim c As Integer = GetScalarInt("Select count(Memberattend) as cnt from membersignup where eventid =  " & EventID.Text & " And memberattend = 'true'", conn)
            param(2) = New ReportParameter("NumMembers", CStr(c + c1))
            Dim c2 As Integer = GetScalarInt("Select count(id) as cnt from Guestsignup where eventid = " & EventID.Text, conn)
            param(4) = New ReportParameter("Guests", CStr(c2))
            param(5) = New ReportParameter("Total", CStr(c + c1 + c2))

            ReportViewer1.LocalReport.SetParameters(param)

            RenderReport(ReportViewer1, "Attendance.pdf", Server.MapPath("~/Reports/"))


            Me.ESent.Text = "0"
        End If
    End Sub

    Protected Sub SendEMailReminders(sender As Object, e As EventArgs)
        Dim strsql As String
        Dim LastMemberId As Integer = 0
        Dim FirstEmail As Boolean = True

        Dim Names(20) As String
        Dim Meals(20) As String
        Dim Attachments(20) As String
        Dim i As Integer
        Dim j As Integer
        Dim EMailAddresses As String = ""

        Attachments(0) = Server.MapPath("~/reports/") & "Attendance.pdf"
        Using cmd As New SqlCommand("select * from attachments where eventid =@Eventid", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            j = 1
            If dr.HasRows Then
                Do While dr.Read()
                    Attachments(j) = Server.MapPath("~/documents/") & dr("FileName").ToString
                    j += 1
                Loop
            End If
            dr.Close()
        End Using

        Dim GreetName As String = ""
        strsql = "select * from emailreminderList(@EventID) order by ID,type"
        Using cmd As New SqlCommand(strsql, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))

            Dim dr As SqlDataReader = cmd.ExecuteReader

            If dr.HasRows Then
                Do While dr.Read()
                    If CInt(dr("ID")) <> LastMemberId Then
                        If Not FirstEmail Then SendEmail(Names, Meals, etype, GreetName, Attachments, EMailAddresses)
                        i = 0
                        Array.Clear(Names, 0, Names.Length)
                        EMailAddresses = dr("MEmail").ToString
                        GreetName = dr("NickName").ToString
                        Names(i) = dr("FullName").ToString
                        Meals(i) = dr("Meal1").ToString
                        i = 1
                        LastMemberId = CInt(dr("ID"))
                        FirstEmail = False
                    Else
                        If dr("Type").ToString = "A" Or dr("Type").ToString = "B" Then
                            If Not String.IsNullOrEmpty(dr("MEmail").ToString) Then
                                EMailAddresses += ";" & dr("MEmail").ToString
                            End If
                            GreetName += " & " & CStr(dr("NickName"))
                        End If
                        Names(i) = dr("FullName").ToString
                        Meals(i) = dr("Meal1").ToString
                        i += 1

                    End If
                Loop

                dr.Close()
                If i >= 1 Then SendEmail(Names, Meals, etype, GreetName, Attachments, EMailAddresses)


                ' send email to show all emails sent

                Me.ESent.Text = Totalcnt.ToString()
                SendFinalEmail(Totalcnt, CntMember, "EmailReminders")

                ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('All Emails Sent')", True)


            End If

        End Using

    End Sub
    Protected Sub SendEmail(Names() As String, Meals() As String, etype As String, GreetName As String, Attachments() As String, EmailAddresses As String)
        Dim i As Integer

        Dim MailSubject As String = "CCFR Meeting Reminder"

        Dim msgBody As String = EmailGreeting(GreetName, Session("EventDate").ToString, etype)

        Dim FromEmail As String = "reservations@ccfrcville.org"

        For i = 0 To Names.Length - 1
            If Not String.IsNullOrEmpty(Names(i)) Then
                msgBody += AddName(Names(i), Meals(i))
            Else
                Exit For
            End If
        Next
        msgBody += EndTable(etype)
        msgBody += AddText.Text
        msgBody = Mid(msgBody, 1, Len(msgBody) - 2)
        msgBody += FinishMessage()

        Totalcnt += SendMessage(MailSubject, msgBody, "reservations@ccfrcville.org", FromEmail, EmailAddresses, True, Attachments, CntMember)
        Debug.Print("Totalcnt " & Totalcnt)
        Debug.Print("Member Cnt " & CntMember)
        Me.ESent.Text = Totalcnt.ToString







    End Sub

    Protected Shared Function EmailGreeting(Nam As String, dt As String, etype As String) As String
        If etype = "Dinner" Then


            EmailGreeting = "<p>Dear " & Nam & ",</p>" _
            & "<p>We would like to remind you that the next CCFR meeting is " & CDate(dt).ToShortDateString & "</p>" _
             & "Our records indicate that you have the following" _
            & " reservations:</p> <div style=""margin-left:50px""> <table style=""width: 80%"" >" _
             & "    <tr>" _
             & "         <td style=""width: 150px"" class=""auto-style1""><strong>attendee(s)</strong></td>" _
             & " <td style=""width: 150px"" class=""auto-style1""><strong>meal(s)</strong></td>    </tr>"
        Else
            EmailGreeting = "<p>Dear " & Nam & ",</p>" _
        & " <p>We would like to remind you that the next CCFR meeting is " & dt _
        & "  &nbsp; Our records indicate that you have the following" _
        & " reservations:</p> <div style=""margin-left:50px""> <table style=""width: 80%"" >" _
        & "    <tr>" _
        & "         <td style=""width: 150px"" class=""auto-style1""><strong>Attendee(s)</strong></td>" _
         & " <td style=""width: 150px"" class=""auto-style1""><strong>Meal(s)</strong></td>    </tr>"
        End If

    End Function
    Protected Sub UploadMultipleFiles(sender As Object, e As System.EventArgs) Handles btnUpload.Click
        Dim Files As HttpFileCollection = Request.Files
        Dim fname As String
        Using cmd As New SqlCommand("insert into Attachments (eventid,filename) values (@EventID,@Filename)", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            cmd.Parameters.Add("@FileName", SqlDbType.NVarChar)
            For i As Integer = 0 To Files.Count - 1
                Dim file As HttpPostedFile = Files(i)
                ListAttachments.Items.Add(Files(i).FileName)
                fname = GetFilenameFromPath(Files(i).FileName)
                cmd.Parameters("@FileName").Value = fname
                cmd.ExecuteNonQuery()
                file.SaveAs(Server.MapPath("~/documents/") & fname)
            Next
        End Using
    End Sub
    Protected Sub ClearAttachments() Handles Clear.Click

        Using cmd As New SqlCommand("delete attachments where eventid = @eventid", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            cmd.ExecuteNonQuery()
        End Using
        ListAttachments.Items.Clear()
    End Sub

    Function GetFilenameFromPath(ByVal strPath As String) As String
        ' Returns the rightmost characters of a string up to but not including the rightmost '\'
        ' e.g. 'c:\winnt\win.ini' returns 'win.ini'

        If Right$(strPath, 1) <> "\" And Len(strPath) > 0 Then
            GetFilenameFromPath = GetFilenameFromPath(Left$(strPath, Len(strPath) - 1)) + Right$(strPath, 1)
        Else
            GetFilenameFromPath = Nothing
        End If
    End Function
    Protected Shared Function AddName(Nam As String, Meal As String) As String
        AddName = "<tr> <td style=""width: 150px"" >" & Nam & "</td> <td style=""width: 150px"">" & Meal & "</td>    </tr>"
    End Function
    Protected Function EndTable(etype As String) As String
        Dim hlink As String
        hlink = "https://www.ccfrcville.org/default.aspx?dt=" & CDate(EventDate.Text).ToString("MM/dd/yyyy")
        EndTable = "</table> </div> <p>The attached document contains a list of people attending this meeting.</p>"
        If etype = "Dinner" Then EndTable += "<p>To see the announcement for this meeting, <a href ='https://ccfrcville.org/default.aspx?dt=" & Trim(CDate(EventDate.Text).ToString("MM/dd/yyyy")) & "' > Click here.</a> "
    End Function
    Protected Shared Function FinishMessage() As String

        FinishMessage = " <p>We look forward to seeing you.</p><br/><p>The CCFR Board</p></body></html>"

    End Function

End Class
