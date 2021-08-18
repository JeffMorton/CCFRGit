Imports System.Data.SqlClient
Imports Microsoft.Ajax.Utilities

Public Class Admin1
    Inherits PageBase
    'This is the login page for the admin section.  To reach this page, the user needs to enter ccfrcville.org/admin.aspx.  Note - this admin section has its own masterpage admin.master.  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Login1.PasswordRecoveryUrl = "PassRecover.aspx"
        Me.Login1.PasswordRecoveryText = "Forget Password?"
        If Request.IsSecureConnection = False Then
            Response.Redirect("https://ccfrcville.org/Admin.aspx")
        End If
        Session("Debug") = False
        'Session("Admin") = True
        Session("conn") = GetConnectionString(True, False)
        If CBool(Session("Loggedin")) = True Then Response.Redirect("AdminStart.aspx")
        Me.Form.DefaultFocus = Login1.FindControl("Username").ClientID
    End Sub
    Private Function YourValidationFunction(ByVal UserName As String, ByVal Password As String) As Boolean
        Dim sqlConnection As SqlConnection = Nothing

        Dim boolReturnValue As Boolean = False
        If Not (Session("conn") Is Nothing) Then
            sqlConnection = New SqlConnection(Session("conn").ToString)
        Else
            Response.Redirect("Default.aspx")
        End If

        sqlConnection.Open()
        Session("UName") = UserName

        Using command As New SqlCommand("SELECT ID, FirstName,Lastname, UserName,  Password, SecurityQuestion, Admin FROM Member where member.username =@logname", sqlConnection)
            command.Parameters.AddWithValue("@logname", Login1.UserName)
            Dim Dr As SqlDataReader
            Dr = command.ExecuteReader()
            While CBool(Dr.Read())
                If CBool(Dr("Admin")) = False Then
                    Dr.Close()
                    Response.Redirect("Default.aspx")
                Else
                    If (String.Compare(UserName, Dr("UserName").ToString, True) = 0) And (String.Compare(Password, Dr("PassWord").ToString, False) = 0) Then
                        Session("userID") = Dr("ID").ToString
                        Session("UserName") = UserName
                        Session("Name") = Dr("FirstName").ToString & " " & Dr("LastName").ToString
                        If Not String.IsNullOrEmpty(Dr("SecurityQuestion").ToString()) Then
                            Me.txtQuestion.Text = Dr("SecurityQuestion").ToString
                        Else
                            Me.txtQuestion.Text = ""
                        End If

                        boolReturnValue = True
                    End If
                End If
                Return boolReturnValue
            End While
        End Using
        Return boolReturnValue
    End Function
    Private Sub Login1_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        If YourValidationFunction(Login1.UserName, Login1.Password) Then
            ' e.Authenticated = true; 
            Login1.Visible = False
            MessageLabel.Text = "Successfully Logged In"

            If String.IsNullOrEmpty(txtQuestion.Text) Then
                SQTable.Visible = True
                SQLabel.Visible = True
                SQButton.Visible = True
                Exit Sub
            End If
            Session("LoggedIn") = True
            Response.Redirect("AdminStart.aspx")

        End If

    End Sub

    Private Sub Login1_LoginError(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoginError
        If ViewState("LoginErrors") Is Nothing Then
            ViewState("LoginErrors") = 0
        End If
        Dim ErrorCount As Integer = CInt(ViewState("LoginErrors")) + 1

        ViewState("LoginErrors") = ErrorCount
        If (ErrorCount > 3) AndAlso ((Not Login1.PasswordRecoveryUrl = "PassRecover.aspx")) Then
            Response.Redirect(Login1.PasswordRecoveryUrl)
        End If
    End Sub

    Protected Sub SQButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SQButton.Click
        Dim strSQL As String
        Dim strConnection As String = Session("conn").ToString
        Using sqlConnection As New SqlConnection(strConnection)
            sqlConnection.Open()
            If Not Len(txtAnswer.Text) > 1 Then
                Dim respnse As Integer = MsgBox("Please enter the answer to the security question", MsgBoxStyle.Information)
                Exit Sub
            End If
            strSQL = "UPDATE Member SET Member.SecurityQuestion = @question, Member.SecurityAnswer = @answer where Member.ID = @MemberID "
            Using cmd As New SqlCommand(strSQL, sqlConnection)
                cmd.Parameters.AddWithValue("@question", ddQuestion.Text)
                cmd.Parameters.AddWithValue("@answer", txtAnswer.Text)
                cmd.Parameters.AddWithValue("@EventID", Session("UserID"))
                cmd.ExecuteNonQuery()
            End Using
            Session("LoggedIn") = True
            Response.Redirect("AdminStart.aspx")
        End Using

    End Sub

End Class
