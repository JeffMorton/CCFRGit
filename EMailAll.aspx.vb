Imports System.Data.SqlClient
Public Class EamilAll
    Inherits PageBase
    Dim conn As SqlConnection
    Dim Totalcnt As Integer = 0
    Dim CntMember As Integer = 0
    'This pages provides a way for the administrator to send an email to all members.  
    'This page includes CKEditor rich text editor and emails are html enabled.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()

        If Not IsPostBack Then
            Using cmd As New SqlCommand("delete attachments where eventid = @eventid", conn)
                cmd.Parameters.AddWithValue("@EventID", 1)
                cmd.ExecuteNonQuery()
            End Using
        End If

    End Sub
    Protected Sub SendAllEMail(sender As Object, e As EventArgs)
        Dim Attachments(20) As String
        Dim cnta As Integer = 0
        Dim EmailAddresses As String = ""
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
        Dim orgEmail As String
        Dim strSQL As String = "Select orgEMail from information"
        Using cmd As New SqlCommand(strSQL, conn)
            orgEmail = cmd.ExecuteScalar.ToString
        End Using
        Dim cnt As Integer = 0
        strsql = "select MEmail from member where MEMail like '%@%'" _
            & " union all select SpouseEmail from member where SpouseEMail like '%@%'"
        Using cmd As New SqlCommand(strSQL, conn)
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.HasRows Then
                Do While dr.Read()
                    cnt += 1
                    If EmailAddresses = "" Then
                        EmailAddresses = dr("MEMail").ToString
                    Else
                        EmailAddresses = EmailAddresses & ";" & dr("MEMail").ToString
                    End If
                    If cnt > 98 Then
                        SendEmail(Attachments, EmailAddresses, orgEmail)
                        cnt = 0
                        EmailAddresses = ""
                    End If
                Loop
            End If
            dr.Close()
        End Using
        If cnt > 0 Then

            SendEmail(Attachments, EmailAddresses, orgEmail)
        End If

        Me.ESent.Text = Totalcnt.ToString
        SendFinalEmail(Totalcnt, CntMember, "Email All")
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
                    cnta += 1
                    Attachments(cnta) = Server.MapPath("~/documents/") & dr("FileName").ToString
                Loop
            End If
            dr.Close()
        End Using
        Dim orgEmail As String
        Dim strSQL As String = "Select orgEMail from information"
        Using cmd As New SqlCommand(strSQL, conn)
            orgEmail = cmd.ExecuteScalar.ToString
            ' Send sampleemail
            EMailAddresses = "webmaster@ccfrcville.org"
            SendEmail(Attachments, EMailAddresses, orgEmail)
        End Using
        ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Sample Email Sent')", True)

    End Sub
    Protected Sub SendEmail(attachments() As String, emailaddresses As String, orgEmail As String)



        Dim msgBody As String
        Dim msgSubject As String

        If Len(MessageText.Text) < 10 Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a message longer that 10 characters')", True)
            Exit Sub
        Else
            msgBody = MessageText.Text
        End If
        If Len(Subject.Text) < 2 Then
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Must have a subject  longer that 2 characters')", True)
            Exit Sub
        Else
            msgSubject = Subject.Text

        End If


        Dim FromEmail As String = orgEmail

        Totalcnt += SendMessage(msgSubject, msgBody, "reservations@ccfrcville.org", FromEmail, emailaddresses, True, attachments, CntMember)








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
