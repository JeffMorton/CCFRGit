Imports System.Data.SqlClient
Imports System.Web
Partial Public Class Login
    Inherits PageBase
    Dim etype As String
    'This page provides a way for members to login.  For new memebrs, it requires that they select and answer a security question.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session("conn") Is Nothing) Then
            Session("conn") = GetConnectionStringM(True, False).ToString
        End If
        Dim conn As New SqlConnection(Session("Conn").ToString())
        conn.Open()
        Session("EventID") = 1215
        'Session("UserID") = 83
        RecordError("Login Started", " ", " ", conn)
        Me.Login1.PasswordRecoveryUrl = "PassRecover.aspx"
        Me.Login1.PasswordRecoveryText = "Forget Password?"
        etype = Request.QueryString("type")
        Session("eType") = etype
        If etype Is Nothing Then
            etype = "Not Supplied"
        End If
        RecordError("Login type", etype, " ", conn)

        If CBool(Session("LoggedIn")) Then
            ReDir(etype)
        End If
        Me.Form.DefaultFocus = Login1.FindControl("Username").ClientID

    End Sub
    Private Sub ReDir(etype As String)
        Select Case etype
            Case "Dinner"
                Session("EventType") = "Dinner"
                Response.Redirect("MemberSignup.aspx?type=Dinner")
            Case "Lunch"
                Session("EventType") = "Lunch/Discussion"
                Response.Redirect("MemberSignup.aspx?type=Lunch/Discussion")
            Case "Roster"
                Response.Redirect("RosterRP.aspx")
            Case "Dues"
                Session("EventType") = "Dues Only"
                Response.Redirect("MemberSignup.aspx?type=Dues Only")
            Case "Profile"
                Response.Redirect("Profile.aspx")
            Case "AddGuests"
                Response.Redirect("AddGuests.aspx")
            Case "Attendance"
                Response.Redirect("Attendance.aspx?Type=Dinner", True)
            Case "AttendanceL"
                Response.Redirect("Attendance.aspx?Type=Lunch", True)
            Case Else
                Response.Redirect("Default.aspx")
        End Select
    End Sub
    Private Function YourValidationFunction(ByVal UserName As String, ByVal Password As String) As Boolean
        Dim sqlConnection As SqlConnection

        Dim boolReturnValue As Boolean = False
        If Not (Session("conn") Is Nothing) Then

            sqlConnection = New SqlConnection(Session("conn").ToString)

        Else
            Session("conn") = False
            sqlConnection = New SqlConnection(Session("conn").ToString)
        End If

        sqlConnection.Open()
        Session("UName") = UserName

        Dim command As New SqlCommand("SELECT ID, FirstName,Lastname, UserName,  Password, SecurityQuestion FROM Member where member.username =@logname", sqlConnection)
        command.Parameters.AddWithValue("@logname", Login1.UserName)
        Dim Dr As SqlDataReader


        Dr = command.ExecuteReader()
        While Dr.Read()
            If (String.Compare(UserName, Dr("UserName").ToString, True) = 0) And (String.Compare(Password, Dr("PassWord").ToString, False) = 0) Then
                Session("userID") = Dr("ID").ToString
                Session("UserName") = UserName
                Session("FullName") = Dr("FirstName").ToString & " " & Dr("LastName").ToString
                If Not (Dr("SecurityQuestion") Is System.DBNull.Value) Then
                    Me.txtQuestion.Text = CStr(Dr("SecurityQuestion"))
                Else
                    Me.txtQuestion.Text = ""
                End If

                boolReturnValue = True
            End If
            Dr.Close()
            Return boolReturnValue
        End While
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

            ReDir(etype)

        Else
            e.Authenticated = False
        End If

    End Sub

    Private Sub Login1_LoginError(ByVal sender As Object, ByVal e As System.EventArgs) Handles Login1.LoginError
        If ViewState("LoginErrors") Is Nothing Then
            ViewState("LoginErrors") = 0


        End If
        Dim ErrorCount As Integer = CInt(ViewState("LoginErrors")) + 1

        ViewState("LoginErrors") = ErrorCount
        If (ErrorCount > 3) AndAlso (Login1.PasswordRecoveryUrl <> "PassRecover.aspx") Then
            Response.Redirect(Login1.PasswordRecoveryUrl)
        End If
    End Sub

    Protected Sub SQButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SQButton.Click
        Dim strSQL As String
        Dim strConnection As String = Session("conn").ToString
        Dim sqlConnection As New SqlConnection(strConnection)
        Dim respnse As Integer

        sqlConnection.Open()

        strSQL = "UPDATE Member SET Member.SecurityQuestion = @question, Member.SecurityAnswer = @answer where Member.ID = " & Session("UserID").ToString
        Using cmd As New SqlCommand(strSQL, sqlConnection)
            cmd.Parameters.AddWithValue("@question", ddQuestion.Text)
            cmd.Parameters.AddWithValue("@answer", txtAnswer.Text)
            respnse = cmd.ExecuteNonQuery()
        End Using
        Session("LoggedIn") = True
        ReDir(etype)
    End Sub

End Class
