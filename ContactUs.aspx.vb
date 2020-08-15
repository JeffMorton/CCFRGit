Public Class ContactUS
    Inherits PageBase
    'This is a fairly standard contact us page.  It is part of the public section.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub SubmitForm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Page.IsValid Then Exit Sub

        Dim SendResultsTo As String = "ccfr@pobox.com"
        Dim MailSubject As String = "Form Results"
        Dim Bcc As String
        Try

            Dim FromEmail As String = Email.Text
            Dim msgBody As StringBuilder = New StringBuilder()
            Dim messagesubject As String = Subject.Text

            msgBody.Append("Contact Us Message From: " & First_Name.Text & " " & Last_Name.Text & "<br /><br />")
            msgBody.Append("Phone: " & Phone.Text & "<br /><br />")
            msgBody.Append("Subject: " & Subject.Text & "<br/><br/>")
            msgBody.Append("Messsage:" & "<br />" & Message.Text & "<br /><br />")



            msgBody.Append("Server Date & Time: " & DateTime.Now & "<br /><br />")


            If CheckBoxCC.Checked = True Then Bcc = FromEmail



            SendMessageNA(MailSubject, msgBody.ToString, SendResultsTo, FromEmail, FromEmail, True)
            Me.YourForm.ActiveViewIndex = 1
        Catch
            Me.YourForm.ActiveViewIndex = 2
        End Try

    End Sub

End Class
