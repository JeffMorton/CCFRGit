Imports System.Data.SqlClient
Partial Public Class ChangePassword
    Inherits PageBase
    'This page hanges changing the username and password.  It is only available on the members ownly section of the website
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Me.CuserName.Text = Session("UserName").ToString
                Me.NUserName.Text = Session("UserName").ToString
            Catch
                Response.Redirect("login.aspx")
            End Try
        End If
    End Sub


    Private Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim strConnection As String = Session("conn").ToString
        Dim sqlConnection As New SqlConnection(strConnection)
        sqlConnection.Open()

        ' check current user name and current password
        Dim sqlQuery0 As String = "Select username,password from member where id = @memID"
        Using cmd0 As New SqlCommand(sqlQuery0, sqlConnection)
            cmd0.Parameters.Add("@memID", SqlDbType.Int)
            cmd0.Parameters("@memID").Value = Session("UserID")
            Dim Dr0 As SqlDataReader
            Dr0 = cmd0.ExecuteReader
            Dr0.Read()
            If (String.Compare(Me.CuserName.Text, Dr0("UserName").ToString, True) = 0) And (String.Compare(Me.CurrentPassword.Text, Dr0("PassWord").ToString, False) = 0) Then
                Dr0.Close()
                If Len(Me.NewPassword.Text) < 6 Then
                    msg.Text = "Password must be at least 6 characters long"
                    Exit Sub
                End If
                If Me.NewPassword.Text = Me.Confirm.Text Then
                    Dim sqlQuery As String = "UPDATE Member SET Member.UserName = @un, Member.[Password] = @pw WHERE (((Member.ID)=@memid));"
                    Using cmd1 As New SqlCommand(sqlQuery, sqlConnection)
                        cmd1.Parameters.Add("@un", SqlDbType.NChar)
                        cmd1.Parameters("@un").Value = Me.NUserName.Text
                        cmd1.Parameters.Add("@pw", SqlDbType.NChar)
                        cmd1.Parameters("@pw").Value = Me.NewPassword.Text
                        cmd1.Parameters.Add("@memID", SqlDbType.Int)
                        cmd1.Parameters("@memID").Value = Session("UserID")
                        Dim rw As Integer = cmd1.ExecuteNonQuery()
                        If rw = 1 Then
                            msg.Text = "User Name and Password updated"
                        End If
                    End Using
                Else
                    msg.Text = "New Password doesn't match Conformation"
                    Exit Sub
                End If
            Else
                msg.Text = "Current Username and/or Current Passord don't match record"
                Exit Sub
            End If
        End Using
    End Sub
End Class
