
Imports System.Data.SqlClient
'This page allows the user to recover their password.
Partial Public Class PassRecover
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("Conn") = GetConnectionStringM(True, False)
    End Sub


    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ValidateName() Then

            Me.SecurityQuestion.Visible = True
            Me.Answer.Visible = True
            Me.lbSecurityQuestion.Visible = True
            Me.SQAnswer.Visible = True
            Me.Button1.Enabled = False
            Me.Button2.Visible = True
        Else
        End If
    End Sub
    Private Function ValidateName() As Boolean

        Dim boolReturnValue As Boolean = False
        Dim strConnection As String = Session("conn").ToString
        Dim sqlConnection As New SqlConnection(strConnection)
        Dim SQLQuery As String = "SELECT UserName, Password, [E-Mail],SecurityQuestion, SecurityAnswer FROM Member where member.[e-mail] =@email"

        Using command As New SqlCommand(SQLQuery, sqlConnection)
            command.Parameters.AddWithValue("@email", TextBox1.Text)
            Dim Dr As SqlDataReader

            sqlConnection.Open()
            Dr = command.ExecuteReader()
            If Not Dr.HasRows Then
                Me.MessageNotification.Text = "E-Mail Address not Found"
                boolReturnValue = False
            Else
                While Dr.Read()
                    If UCase(TextBox1.Text) = UCase(Dr("E-Mail").ToString()) Then
                        Me.SecurityQuestion.Text = Dr("SecurityQuestion").ToString
                        Me.EMailAddress.Text = Dr("E-Mail").ToString
                        Me.Password.Text = Dr("Password").ToString
                        Me.CorrectAnswer.Text = Dr("SecurityAnswer").ToString
                        Me.username.Text = Dr("UserName").ToString
                        boolReturnValue = True
                    Else
                        boolReturnValue = False
                    End If

                End While
            End If
            Dr.Close()
        End Using
        sqlConnection.Dispose()

        Return boolReturnValue
    End Function
    Sub SendEmail()

        If Not Page.IsValid Then Exit Sub

        Dim SendResultsTo As String = Me.EMailAddress.Text
        Dim MailSubject As String = "Information you requested"

        Try
            Dim FromEmail As String = "webmaster@ccfrcville.org"
            Dim msgBody As StringBuilder = New StringBuilder()

            msgBody.Append("Here is the password that you requested" & vbCrLf & vbCrLf)
            msgBody.Append("User Name: " & Me.username.Text & vbCrLf)
            msgBody.Append("Password: " & Me.Password.Text & vbCrLf & vbCrLf)
            msgBody.Append("webmaster@ccfrcville.org")

            msgBody.AppendLine()


            SendMessageNA(MailSubject, msgBody.ToString, Me.EMailAddress.Text, FromEmail, "", False)
            Me.MessageNotification.Text = "Password E-Mail Sent"
        Catch
            Me.MessageNotification.Text = "Password E-Mail Failed.  Please go to contact us page and let us know"
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        If UCase(Me.CorrectAnswer.Text) = UCase(Me.SQAnswer.Text) Then
            SendEmail()
        Else
            Me.MessageNotification.Text = "Security Question Answer does not agree with answer on file!"
        End If
    End Sub
End Class
