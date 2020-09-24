Imports System.Data.SqlClient
Public Class EMailNotAttending
    Inherits PageBase
    Dim conn As SqlConnection
    Dim Totalcnt As Integer
    Dim CntMember As Integer
    'This pages provides a way for the administrator to send an email to all members not attending a meeting.  
    'This page includes CKEditor rich text editor and emails are html enabled.

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        'Using cmd As New SqlCommand("select EventDate from Event where id = @EventID")
        'cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
        Me.EventDate.Text = Session("Eventdate").ToString
        If Not IsPostBack Then
            Using cmd As New SqlCommand("delete attachments where eventid = @eventid", conn)
                cmd.Parameters.AddWithValue("@EventID", 1)
                cmd.ExecuteNonQuery()
            End Using
        End If
    End Sub
    Protected Sub SendAllEMail(sender As Object, e As EventArgs)
        Dim Attachments(20) As String
        Dim EMailAddresses As String = ""
        Dim cnta As Integer = 0

        Using cmd As New SqlCommand("select * from attachments where eventid =@Eventid", conn)
            cmd.Parameters.AddWithValue("@EventID", 1)
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            If String.IsNullOrEmpty(Subject.Text) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a subject')", True)
                Exit Sub
            End If
            If String.IsNullOrEmpty(MessageText.Text) Then
                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a message')", True)
                Exit Sub

            End If

            If dr.HasRows Then
                Do While dr.Read()
                    Attachments(cnta) = Server.MapPath("~/documents/") & dr("FileName").ToString
                    cnta += 1
                Loop
            End If
            dr.Close()
        End Using
        Dim strSQL As String = "Select orgEMail from information"
        Dim orgEmail As String
        Using cmd As New SqlCommand(strSQL, conn)
            orgEmail = cmd.ExecuteScalar.ToString
        End Using
        Dim cnt As Integer = 0
        strSQL = "SELECT Member.ID, Member.LastName, Member.Nickname, Member.MEmail,Member.Spouseemail From Member " _
            & "WHERE (((Member.ID) Not In (select membersignup.memberid from membersignup where eventid = @EventID AND Member.MEMail >' ' and dontIncludeonReports='false'))) and MEmail >' '"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.HasRows Then
                Do While dr.Read()
                    cnt += 1
                    If EMailAddresses = "" Then
                        EMailAddresses = dr("MEMail").ToString
                    Else
                        EMailAddresses += ";" & dr("MEMail").ToString
                    End If
                    If Not String.IsNullOrEmpty(dr("SpouseEmail").ToString) Then
                        cnt += 1
                        If EMailAddresses = "" Then
                            EMailAddresses = dr("SpouseEmail").ToString
                        Else
                            EMailAddresses += ";" & dr("SpouseEmail").ToString
                        End If
                        If cnt > 96 Then
                            SendEmail(Attachments, EMailAddresses)
                            cnt = 0
                            EMailAddresses = ""
                        End If
                    End If
                Loop
            End If
            dr.Close()
        End Using
        If cnt > 0 Then
            SendEmail(Attachments, EMailAddresses)
        End If


        Me.ESent.Text = Totalcnt.ToString
        SendFinalEmail(Totalcnt, CntMember, "Email Attendees")
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('All Emails Sent')", True)

    End Sub
    Protected Sub SendSampleEmail(sender As Object, e As EventArgs)
        Dim Attachments(20) As String
        Dim EMailAddresses As String
        Dim cnta As Integer = 0
        Using cmd As New SqlCommand("select * from attachments where eventid =@Eventid", conn)
            cmd.Parameters.AddWithValue("@EventID", 1)
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            If dr.HasRows Then
                Do While dr.Read()
                    Attachments(cnta) = Server.MapPath("~/documents/") & dr("FileName").ToString
                    cnta += 1
                Loop
            End If
            dr.Close()
        End Using
        EMailAddresses = "webmaster@ccfr.org;jb_morton@live.com"

        SendEmail(Attachments, EMailAddresses)

        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Sample Email Sent')", True)


    End Sub
    Protected Sub SendEmail(attachments() As String, emailaddresses As String)

        Dim msgBody As String
        Dim msgSubject As String

        If Len(Subject.Text) < 2 Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a subject  longer that 2 characters')", True)
            Exit Sub
        Else
            msgSubject = Subject.Text

        End If
        If Len(MessageText.Text) < 10 Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a message longer that 10 characters')", True)
            Exit Sub
        Else
            msgBody = MessageText.Text
        End If
        Dim FromEmail As String
        Dim strSQL As String = "Select orgEMail from information"
        Using cmd As New SqlCommand(strSQL, conn)
            Dim orgEmail As String = cmd.ExecuteScalar.ToString
            ' Send sampleemail


            FromEmail = orgEmail
        End Using
        Totalcnt += SendMessage(msgSubject, msgBody, "", "reservations@ccfrcville.org", emailaddresses, True, attachments, CntMember)

    End Sub
    Protected Sub OnConfirm(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            SendAllEMail(sender, e)
        End If
    End Sub

    Protected Sub UploadMultipleFiles(sender As Object, e As System.EventArgs) Handles btnUpload.Click
        Dim Files As HttpFileCollection = Request.Files
        Dim fname As String
        Using cmd As New SqlCommand("insert into Attachments (eventid,filename) values (@EventID,@Filename)", conn)
            cmd.Parameters.AddWithValue("@EventID", 1)
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
    Function GetFilenameFromPath(ByVal strPath As String) As String
        ' Returns the rightmost characters of a string upto but not including the rightmost '\'
        ' e.g. 'c:\winnt\win.ini' returns 'win.ini'

        If Right$(strPath, 1) <> "\" And Len(strPath) > 0 Then
            GetFilenameFromPath = GetFilenameFromPath(Left$(strPath, Len(strPath) - 1)) + Right$(strPath, 1)
        Else
            GetFilenameFromPath = Nothing
        End If
    End Function
    Protected Sub ClearAttachments() Handles Clear.Click

        Using cmd As New SqlCommand("delete attachments where eventid = @eventid", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            cmd.ExecuteNonQuery()
        End Using
        ListAttachments.Items.Clear()
    End Sub

End Class
