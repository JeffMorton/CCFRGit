
Imports System.Data.SqlClient

Public Class MemberUpdate
    Inherits PageBase
    Dim conn As SqlConnection
    Dim CntMember As Integer
    '  This page is part of the admin section of the website.  It alllows the administrator to modify the member table.  Note
    'there are fields that are not modifiable in the member table like DateJoined, Username, and Password. 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        If Not IsPostBack Then
            ddlSelectName.Focus()
            pMain.Visible = False
        End If
        Try
            conn.Open()
        Catch
            Response.Redirect("Admin.aspx")
        End Try
        Me.NameSource.ConnectionString = conn.ConnectionString
        Me.ProgramCommitteeSource.ConnectionString = conn.ConnectionString
        Me.LunchCommitteeSource.ConnectionString = conn.ConnectionString
        Me.PositionSource.ConnectionString = conn.ConnectionString

        Me.memSource.ConnectionString = conn.ConnectionString
        Me.MemberForm.DataSourceID = memSource.ID
        Me.MemberForm.ChangeMode(FormViewMode.Edit)

        Me.SSource.ConnectionString = conn.ConnectionString
        Me.SpouseForm.DataSourceID = SSource.ID
        Me.SpouseForm.ChangeMode(FormViewMode.Edit)

        Me.AddressSource.ConnectionString = conn.ConnectionString
        Me.AddressForm.DataSourceID = AddressSource.ID
        Me.AddressForm.ChangeMode(FormViewMode.Edit)
        If Session("Returnfromcommittee") IsNot Nothing Then
            pMain.Visible = True
            lblMessage.Text = ""
            Session("Returnfromcommittee") = System.DBNull.Value
        End If


    End Sub
    Private Sub Up(sender As Object, e As EventArgs) Handles UpdateM.Click
        DevelopNames()
        Me.SpouseForm.UpdateItem(True)
        Me.MemberForm.UpdateItem(True)
        Me.AddressForm.UpdateItem(True)

        Me.MemberForm.ChangeMode(FormViewMode.Edit)
        Me.SpouseForm.ChangeMode(FormViewMode.Edit)
        Me.AddressForm.ChangeMode(FormViewMode.Edit)

        Dim indx As Integer = ddlSelectName.SelectedIndex
        ddlSelectName.SelectedIndex = indx
        SelectName_Index_Changed()
        ddlSelectName.Focus()
    End Sub
    Protected Sub SelectName_Index_Changed()
        Session("UserID") = ddlSelectName.SelectedValue
        pMain.Visible = True
        lblMessage.Text = ""
    End Sub
    Protected Function AddMem(sender As Object, e As EventArgs) As Integer
        Using cmd1 As SqlCommand = New SqlCommand("CreateNewMemberRecord", conn)
            cmd1.Parameters.Add("@MemberID", SqlDbType.Int)
            cmd1.CommandType = CommandType.StoredProcedure
            cmd1.Parameters("@MemberID").Direction = ParameterDirection.Output
            cmd1.ExecuteNonQuery()

            AddMem = CInt(cmd1.Parameters("@MemberID").Value)
            Session("UserID") = AddMem
        End Using
    End Function
    Protected Sub UpdateNames() Handles AddressForm.ItemUpdated
        ddlSelectName.DataBind()
        ddlSelectName.Items.FindByValue(Session("UserID").ToString).Selected = True

    End Sub
    Protected Sub OnConfirm(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            Dim MemberID As Integer = CInt(CType(MemberForm.FindControl("ID"), Label).Text)
            Using cmd As New SqlCommand("Delete Member where ID = @MemberID", conn)
                cmd.Parameters.AddWithValue("@MemberID", MemberID)
                cmd.ExecuteNonQuery()
            End Using
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Delete Canceled)", True)
        End If
    End Sub


    Private Sub DevelopNames()
        Dim lastname As String = CType(MemberForm.FindControl("lastName"), TextBox).Text
        Dim firstname As String = CType(MemberForm.FindControl("firstName"), TextBox).Text
        Dim spouselastname As String = CType(SpouseForm.FindControl("spouselastName"), TextBox).Text
        Dim spousefirstname As String = CType(SpouseForm.FindControl("spousefirstName"), TextBox).Text
        Dim nickname As String = CType(MemberForm.FindControl("NickName"), TextBox).Text
        Dim spousenickname As String = CType(SpouseForm.FindControl("spousenickName"), TextBox).Text

        If (CType(AddressForm.FindControl("EnvelopeName"), TextBox).Text) = "" Then
            If lastname = spouselastname Then
                CType(AddressForm.FindControl("EnvelopeName"), TextBox).Text = firstname & " and " & spousefirstname & " " & lastname
            ElseIf String.IsNullOrEmpty(spouselastname) Then
                CType(AddressForm.FindControl("EnvelopeName"), TextBox).Text = firstname & " " & lastname
            Else
                CType(AddressForm.FindControl("EnvelopeName"), TextBox).Text = firstname & " " & lastname & " and " & spousefirstname & " " & spouselastname
            End If

        End If
        If (CType(AddressForm.FindControl("CombinedNickName"), TextBox).Text) = "" Then
            If LastName = SpouseLastName Then
                CType(AddressForm.FindControl("CombinedNickName"), TextBox).Text = nickname & " and " & spousenickname & " " & lastname
            ElseIf String.IsNullOrEmpty(spouselastname) Then
                CType(AddressForm.FindControl("CombinedNickName"), TextBox).Text = nickname & " " & lastname
            Else
                CType(AddressForm.FindControl("CombinedNickName"), TextBox).Text = Nickname & " " & lastname & " and " & SpouseNickName & " " & spouselastname
            End If
        End If

        If (CType(AddressForm.FindControl("RosterName"), TextBox).Text) = "" Then
            If lastname = spouselastname Then
                CType(AddressForm.FindControl("RosterName"), TextBox).Text = lastname & ", " & firstname & " and " & spousefirstname
            ElseIf String.IsNullOrEmpty(spouselastname) Then
                CType(AddressForm.FindControl("RosterName"), TextBox).Text = lastname & ", " & firstname
            Else
                CType(AddressForm.FindControl("RosterName"), TextBox).Text = lastname & ", " & firstname & " and " & spouselastname & ", " & spousefirstname
            End If
        End If
    End Sub
    Protected Sub MailLoginInfo() Handles MailLogin.Click
        Dim strSQL As String = "select [username],[password],[MEmail] from member where ID = @MemberID"
        Dim uname As String
        Dim passw As String
        Dim memail As String
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            dr.Read()
            uname = dr("username").ToString
            passw = dr("password").ToString
            memail = dr("MEmail").ToString
            dr.Close()
        End Using
        If String.IsNullOrEmpty(uname) Or String.IsNullOrEmpty(passw) Then
            strSQL = "Update Member set username = memail,password = 'Ccfr' + format(id,'0000') where id = @memberID "
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@memberid", Session("UserID"))
                cmd.ExecuteNonQuery()
            End Using
            strSQL = "select [username],[password],[MEmail] from member where ID = @MemberID"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader
                dr.Read()
                uname = dr("username").ToString
                passw = dr("password").ToString
                memail = dr("MEmail").ToString
                dr.Close()
            End Using
        End If
        Dim MailSubject As String = "CCFR Login Information"
        Dim FromEmail As String
        Dim ToEmail As String = memail
        Dim msgBody As String

        Using cmd As New SqlCommand("select onlinetext from information", conn)
            FromEmail = memail
            msgBody = cmd.ExecuteScalar.ToString
        End Using
        msgBody += "<br/>" & "<br />"
        msgBody += "User Name: " & uname
        If passw Is Nothing Then
            passw = "Ccfr" & Format(Session("UserID"), "0000")
        End If


        msgBody += "<br />" & "<br />" & "Password: " & passw & "<br />" & "<br />"
        msgBody += "Note - passwords are case sensitive"
        FromEmail = "reservations@ccfrcville.org"
        Dim Attach(1) As String
        Try
            Dim cnt As Integer = SendMessage(MailSubject, msgBody, ToEmail, FromEmail, "", True, Attach, CntMember)
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Information Sent')", True)

        Catch ex As Exception
            lblMessage.Text = ex.Message & " " & ex.StackTrace.ToString
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Mail Message Failed')", True)
        End Try
    End Sub
    Protected Sub SpouseCommittees()
        Session("Scode") = True
        Session("Returnfromcommittee") = Session("UserID")
        Response.Redirect("committeeasignments.aspx?M=S")
    End Sub
    Protected Sub MemberCommittees()
        Session("Scode") = False
        Session("Returnfromcommittee") = Session("UserID")
        Response.Redirect("committeeasignments.aspx?M=M")

    End Sub


End Class
