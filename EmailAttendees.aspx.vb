Imports System.Data.SqlClient

Public Class EmailAttendees
    Inherits PageBase
    Dim conn As SqlConnection
    Dim CntMember As Integer = 0
    Dim Totalcnt As Integer = 0
    'This pages provides a way for the administrator to send an email to all attendees for a meeting.  
    'This page includes CKEditor rich text editor and emails are html enabled.

    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        Using cmd As New SqlCommand("SELECT type from event where ID =@EventID", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
        End Using
        If Not IsPostBack Then
            Me.EventID.Text = Session("EventID").ToString
            Me.EventDate.Text = CDate(Session("EventDate")).ToString("MMMM dd, yyyy")
            Using cmd As New SqlCommand("delete attachments where eventid = @eventid", conn)
                cmd.Parameters.AddWithValue("@EventID", 1)
                cmd.ExecuteNonQuery()
            End Using
            Using cmd As New SqlCommand("delete attachments where eventid = @eventid", conn)
                cmd.Parameters.AddWithValue("@EventID", 1)
                cmd.ExecuteNonQuery()
            End Using
            Me.ESent.Text = "0"
        End If
    End Sub
    Protected Sub SendEMailAttendees(sender As Object, e As EventArgs)
        Dim strsql As String

        Dim Attachments(20) As String
        Dim j As Integer
        Dim EMailAddresses As String
        If String.IsNullOrEmpty(mSubject.Text) Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a subject')", True)
            Exit Sub
        End If
        If String.IsNullOrEmpty(MessageText.Text) Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a message')", True)
            Exit Sub

        End If

        Using cmd As New SqlCommand("select * from attachments where eventid =@Eventid", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            j = 0
            If dr.HasRows Then
                Do While dr.Read()
                    Attachments(j) = Server.MapPath("~/documents/") & dr("FileName").ToString
                    j += 1
                Loop
            End If
            dr.Close()
        End Using

        Dim GreetName As String
        strsql = "select * from emailreminderlist(@EventID) where type<>'C'"
        Using cmd As New SqlCommand(strsql, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            Dim dr As SqlDataReader = cmd.ExecuteReader

            If dr.HasRows Then
                Do While dr.Read()
                    EMailAddresses = dr("MEmail").ToString
                    GreetName = dr("NickName").ToString
                    SendEmail(GreetName, Attachments, EMailAddresses)
                Loop
            End If
            dr.Close()

            ' send email to show all emails sent

            Me.ESent.Text = Totalcnt.ToString
            SendFinalEmail(Totalcnt, CntMember, "Email Attendees")

            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('All Emails Sent')", True)
        End Using

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
                    cnta += 1
                    Attachments(cnta) = Server.MapPath("~/documents/") & dr("FileName").ToString
                Loop
            End If
            dr.Close()
        End Using

        EMailAddresses = "webmaster@ccfrcville.org;jb_morton@live.com"
        SendEmail("Sample Name", Attachments, EMailAddresses)

        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Sample Email Sent')", True)
    End Sub

    Protected Sub SendEmail(GreetName As String, Attachments() As String, EmailAddresses As String)


        Dim msgBody As String = EmailGreeting(GreetName)

        Dim FromEmail As String = "reservations@ccfrcville.org"


        msgBody += MessageText.Text

        Totalcnt += SendMessage(mSubject.Text, msgBody, EmailAddresses, FromEmail, "", True, Attachments, CntMember)


    End Sub

    Protected Shared Function EmailGreeting(Nam As String) As String

        EmailGreeting = "<p>Dear " & Nam & ",</p><br /><br />"


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
        ' Returns the rightmost characters of a string up  to but not including the rightmost '\'
        ' e.g. 'c:\winnt\win.ini' returns 'win.ini'

        If Right$(strPath, 1) <> "\" And Len(strPath) > 0 Then
            GetFilenameFromPath = GetFilenameFromPath(Left$(strPath, Len(strPath) - 1)) + Right$(strPath, 1)
        Else
            GetFilenameFromPath = Nothing
        End If
    End Function


End Class
