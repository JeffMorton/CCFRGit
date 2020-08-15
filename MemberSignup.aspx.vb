
Imports System.Data.SqlClient
Public Class MemberSignup
    Inherits CCFRW19.PageBase
    Dim TotalCost As Double
    ReadOnly sqlconnection As New SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sqlconnection.ConnectionString = GetConnectionStringM(False, False)
        sqlconnection.Open()



        'Session("EventType") can have one of three values- "Dinner","Lunch/Discussion" or "Dues Only"
        'Session("loggedin") = True
        'Session("eventtype") = "Dinner"
        'Session("userid") = 83
        'Session("eventid") = Nothing

        'Session("eventdate") = "5/20/2020"
        If Not CBool(Session("LoggedIn")) Then
            Response.Redirect("Default.aspx")
        End If
        SQLDataSource1.ConnectionString = sqlconnection.ConnectionString
        Dim sqlQuery As String
        Select Case Session("EventType").ToString
            Case "Dinner"
                TitleL.Text = "Dinner Meeting"
            Case "Lunch/Discussion"
                TitleL.Text = "Lunch/Discussion"
            Case "Dues Only"
                TitleL.Text = "Dues Payment For " & Session("FullName").ToString
        End Select
        If Not IsPostBack Then
            If Not (Session("EventType").ToString = "Dues Only") Then
                Me.lbCost.Text = Format(CDbl(Session("MealCost")), "c")
                sqlQuery = "Select top 1 [ID],[Cost],[EventDate],Speaker FROM [Event] where [eventdate] >=  @Date And Type =  @type order by eventdate"
                Try
                    Using cmd As New SqlCommand(sqlQuery, sqlconnection)
                        cmd.Parameters.AddWithValue("@Date", Now().ToShortDateString)
                        cmd.Parameters.AddWithValue("type", Session("EventType"))
                        Dim Dr As SqlDataReader
                        Dr = cmd.ExecuteReader()
                        If Dr.HasRows Then
                            While Dr.Read()
                                Session("EventID") = Dr("ID")
                                Session("MealCost") = Dr("cost")
                                lbCost.Text = Format(Dr("cost"), "c")
                                Me.lbEventDate.Text = CDate(Dr("EventDate")).ToString("dddd, MMM d yyyy")
                                Session("EventDate") = Dr("EventDate")
                                If DateDiff(DateInterval.Day, CDate(Now.ToShortDateString), CDate(Me.lbEventDate.Text)) < CLng(3) Then
                                    ' Response.Redirect("ReservationsClosed.aspx")
                                End If
                                If Dr("Speaker") Is System.DBNull.Value Then
                                    Me.lbSpeaker.Text = "To be Announced"
                                Else
                                    Me.lbSpeaker.Text = Dr("Speaker").ToString
                                End If
                                Me.MealSource.ConnectionString = sqlconnection.ConnectionString
                            End While
                        Else
                            If Session("EventType").ToString = "Dinner" Then
                                Response.Redirect("DinnerClosed.aspx?Type=NotAvailable")
                            Else
                                Response.Redirect("LunchClosed?Type=NotAvailable")
                            End If
                        End If
                        Dr.Close()
                    End Using
                Catch ex As Exception

                End Try
            End If
            Session("Quantity") = -1
            Session("first_name") = ""
            If Not (Session("EventType").ToString = "Dues Only") Then
                Using cmd0 As SqlCommand = New SqlCommand("CreateTmpSignupRecords", sqlconnection)
                    cmd0.CommandType = CommandType.StoredProcedure
                    cmd0.Parameters.AddWithValue("@EventID", CInt(Session("Eventid")))
                    cmd0.Parameters.AddWithValue("@MemberID", CInt(Session("UserID")))
                    cmd0.ExecuteNonQuery()
                End Using
                GuestGridView.DataSource = GuestGridDataSource()

                GuestGridView.DataBind()
            End If
            sqlQuery = "Select Firstname,Middlename,lastname,spousefirstname,spousemiddlename,spouselastname,address,city,state,zip,telephone,NewMember,[e-mail],DuesOwed,MealsOwed,mfFullName from member where id = @userid"
            Using cmd As New SqlCommand(sqlQuery, sqlconnection)
                cmd.Parameters.AddWithValue("@userid", Session("userID").ToString)
                Dim Dr As SqlDataReader = cmd.ExecuteReader
                While Dr.Read()
                    If Dr.HasRows Then
                        Session("first_name") = Dr("Firstname").ToString
                        Session("last_name") = Dr("LastName").ToString
                        Session("address1") = Dr("Address").ToString
                        Session("city") = Dr("City").ToString
                        Session("state") = Dr("State").ToString
                        Session("zip") = Dr("Zip").ToString
                        Session("night_phone_a") = Dr("Telephone").ToString
                        Session("email") = Dr("E-Mail").ToString
                        Session("NewMember") = Dr("NewMember")
                        Session("DuesOwed") = Dr("DuesOwed")
                        Session("MealsOwed") = Dr("MealsOwed")
                        Session("FullName") = Dr("mfFullName")
                    End If
                End While
                Dr.Close()
            End Using

        End If
        If Session("EventType").ToString = "Dues Only" And CDbl(Session("DuesOwed")) > 0 Then
            If Session("EventDate") Is Nothing Then Session("EventDate") = Now.ToShortDateString

            Session("DuesIncluded") = True
            Me.lbDuesOwed.Visible = True
            Me.chkDues.Checked = True
            Me.lbDuesOwed.Text = "Our records indicate that you owe   " & String.Format("{0:c2}", Session("DuesOwed")) & " Dues.  Click on Review and Pay to continue. "
            Me.MemberPanel.Visible = False
            Me.GuestPanel.Visible = False
            Me.Cancel.Visible = False
            Panel3.Visible = True
        ElseIf Session("EventType").ToString = "Dues Only" And CDbl(Session("DuesOwed")) = 0 Then
            Me.lbDuesOwed.Visible = True
            Me.lbDuesOwed.Text = "Our records indicate that you do not owe any dues."
            Panel3.Visible = True
            Me.Cancel.Visible = False
            Me.MemberPanel.Visible = False
            Me.GuestPanel.Visible = False
            Me.Review.Enabled = False
        ElseIf Session("EventType").ToString = "Dinner" Or Session("EventType").ToString = "Lunch/Discussion" Then
            Me.GuestPanel.Visible = False
            If CDbl(Session("DuesOwed")) > 0 Then
                Me.chkDues.Visible = True
                Me.lbDuesOwed.Visible = True
                Me.lbDuesOwed.Text = "Our records indicate that you owe   " & String.Format("{0:c2}", Session("DuesOwed")) & " Dues.  Check box if you want to include dues in your payment "
                Panel3.Visible = True
            End If
            If Session("EventType").ToString = "Dinner" Then
                Me.GuestPanel.Visible = True
            End If
        Else
        End If
        If CDbl(Session("MealsOwed")) > 0 Then
            'Me.chkMeals.Checked = False
            Me.chkMeals.Visible = True
            Me.lbMealsOwed.Text = "Our records indicate that you owe " & String.Format("{0:c2}", Session("MealsOwed")) & " for prior meals.  Check here if you wish to have your payment included."
            Me.lbMealsOwed.Visible = True
            Panel3.Visible = True
        ElseIf CDbl(Session("MealsOwed")) < 0 Then
            Me.lbMealsOwed.Text = "Our records indicate that you have a credit of " & String.Format("{0:c2}", Session("MealsOwed")) & " from canceled prior reservations.  This credit will be applied if appropriate."
                Me.lbMealsOwed.Visible = True
                Panel3.Visible = True
            End If
            If Session("EventID") Is Nothing Then
            GetEventID(Session("EventType").ToString)
        End If
        Me.MealSource.SelectCommand = "select ID, Meal, Category from  dbo.EventMealCategory(" & Session("EventID").ToString & ") where not meal is null  order by place"
            Me.FVSource.ConnectionString = sqlconnection.ConnectionString
        Me.FormView1.DataSourceID = FVSource.ID
        Me.FormView1.ChangeMode(FormViewMode.Edit)
        Me.MealSource.ConnectionString = sqlconnection.ConnectionString
    End Sub

    Protected Sub OnConfirm(ByVal sender As Object, ByVal e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            Using cmd As New SqlCommand("CancelReservation", sqlconnection)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@eventID", CInt(Session("EventID")))
                cmd.Parameters.AddWithValue("@UserID", CInt(Session("UserID")))
                cmd.Parameters.Add("@Credit", sqlDbType:=SqlDbType.Money).Direction = ParameterDirection.Output
                cmd.ExecuteNonQuery()
                Session("MealsOwed") = cmd.Parameters("@Credit").Value
            End Using
            SendEMailConfirmation(Me, e)
            RecordError("MemberSignup", "Reservation Canceled", " ", sqlconnection)
            Response.Redirect("Membersignup.aspx")
        Else
            'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Rservation not Canceled)", True)
            Dim myScript As String = "window.alert('Registration not canceled.');"
            ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)
        End If

    End Sub
    Private Function ComputeCost() As Boolean
        Dim cnt As Integer = 0
        If Session("EventType").ToString = "Dues Only" Then
            TotalCost = CDbl(Session("DuesOwed"))
            If chkMeals.Checked = True Or CDbl(Session("MealsOwed")) < 0 Then
                TotalCost += CDbl(Session("MealsOwed"))
            Else
                Session("MealsOwed") = 0
            End If
        Else
            Dim nameerror As Boolean = False
            If CType(FormView1.FindControl("memberAttend"), CheckBox).Checked Then
                cnt = 1
            End If
            If CType(FormView1.FindControl("spouseAttend"), CheckBox).Checked Then
                cnt += 1
            End If
            If cnt = 0 Then
                lbError.Text = "A member must signup to make a reservation"
                ComputeCost = False
                Exit Function
            End If
            Dim strSQL As String = "select count(ID) from tmpGuestsignup where AddedtoMealsOwed='false' and EventID=@EventID and MemberID=@MemberID"
            Using cmd As New SqlCommand(strSQL, sqlconnection)
                cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                cnt += CInt(cmd.ExecuteScalar())
            End Using


            If cnt = 0 And Not chkDues.Checked Then
                If CDbl(Session("Duesowed")) = 0 Then
                    Me.lbError.Text = "Please Select attendees."
                Else
                    Me.lbError.Text = "Please Select attendees Or check box To pay dues"
                End If
                ComputeCost = False
                Exit Function
            End If
            If nameerror Then
                ComputeCost = False
                Me.lbError.Text = "Please provide first And last names For all guests."
                Exit Function
            End If
            Session("Quantity") = cnt
            TotalCost = CDbl(cnt * CDbl(lbCost.Text))
            Session("DuesIncluded") = False
            If Me.chkDues.Checked Then
                TotalCost += CDbl(Session("DuesOwed"))
                Session("DuesIncluded") = True
            Else
                Session("DuesOwed") = 0
                Session("DuesIncluded") = False
            End If
            If CDbl(Session("MealsOwed")) > 0 And Me.chkMeals.Checked Then
                TotalCost += CDbl(Session("MealsOwed"))
            ElseIf CDbl(Session("MealsOwed")) < 0 Then
                TotalCost += CDbl(Session("MealsOwed"))
            Else
                Session("MealsOwed") = 0
            End If
        End If
        ComputeCost = True
    End Function
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim GuestMeal As DropDownList = DirectCast(e.Row.FindControl("GuestMeal"), DropDownList)  ' Extract the DDL in the Grid Footer 

            Using cmd As New SqlCommand("Select * from dbo.EventMealCategory( @EventID) where Not meal Is null  order by place", sqlconnection)
                cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
                GuestMeal.DataSource = GridViewDataS(cmd)
                GuestMeal.DataTextField = "meal"
                GuestMeal.DataValueField = "category"
                GuestMeal.DataBind()
            End Using
        End If
    End Sub
    Protected Function GuestGridDataSource() As DataTable
        Using cmd As New SqlCommand("Select tmpGuestSignUp.GLastName,tmpGuestSignUp.GFirstName, tmpGuestSignUp.ID,tmpGuestSignUp.AddedToMealsOwed,EventMC.Category,EventMC.meal,webID,MemberID,EventID FROM tmpGuestSignUp INNER JOIN EventMC On tmpGuestSignUp.EventID = EventMC.ID And tmpGuestSignUp.GuestMeal = EventMC.category where eventid = @EventID And memberid =@MemberID And Gdeleted ='False'", sqlconnection)
            cmd.Parameters.AddWithValue("@EventID", CInt(Session("Eventid")))
            cmd.Parameters.AddWithValue("@MemberID", CInt(Session("UserID")))

            Using ada As SqlDataAdapter = New SqlDataAdapter(cmd)
                Using dt As DataTable = New DataTable()
                    ada.Fill(dt)
                    If dt.Rows.Count = 0 Then
                        dt.Rows.Add(" ", " ", 0, True, 0, "", DBNull.Value, Session("UserID"), Session("EventID"))
                        'dt.Rows.Add(dt.NewRow)
                    End If
                    GuestGridDataSource = dt
                End Using
            End Using
        End Using
    End Function

    Protected Sub GridCommand(sender As Object, e As GridViewCommandEventArgs)
        FormView1.UpdateItem(True)
        Me.FormView1.ChangeMode(FormViewMode.Edit)
        If e.CommandName = "Insert" AndAlso Page.IsValid Then
            Dim control As Control
            If (Not (GuestGridView.FooterRow) Is Nothing) Then
                control = GuestGridView.FooterRow
            Else
                control = GuestGridView.Controls(0).Controls(0)
            End If

            Dim FirstName As String = CType(control.FindControl("GFirstName"), TextBox).Text
            Dim LastName As String = CType(control.FindControl("GLastName"), TextBox).Text
            Dim GuestMeal As Integer = CInt(CType(control.FindControl("GuestMeal"), DropDownList).Text)
            If FirstName = "" Or LastName = "" Then
                CType(control.FindControl("GFirstName"), TextBox).Focus()
                Exit Sub
            End If

            Dim strSQL As String = "INSERT into [tmpGuestSignUp] (Gfirstname, Glastname, GuestMeal, GuestName, EventID, MemberID, AddedtoMealsOwed,webID) VALUES (@FirstName, @LastName,@GuestMeal, @GuestName,@EventID,@MemberID,'false',0)"
            Using cmd As New SqlCommand(strSQL, sqlconnection)
                cmd.Parameters.AddWithValue("@FirstName", FirstName)
                cmd.Parameters.AddWithValue("@LastName", LastName)
                cmd.Parameters.AddWithValue("@GuestMeal", GuestMeal)
                cmd.Parameters.AddWithValue("@GuestName", FirstName & " " & LastName)
                cmd.Parameters.AddWithValue("@EventID", Session("Eventid"))
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                Dim cnt As Integer = cmd.ExecuteNonQuery()
                Debug.Print("count " & cnt)
            End Using
            GuestGridView.DataSource = GuestGridDataSource()
            GuestGridView.DataBind()
        End If

    End Sub
    Protected Sub DeleteGuest(sender As Object, e As GridViewDeleteEventArgs)
        FormView1.UpdateItem(True)
        Me.FormView1.ChangeMode(FormViewMode.Edit)
        Dim Deletevalue As String = Request.Form("confirm_value")
        Dim strSQL As String

        If Deletevalue = "Yes" Then
            Dim row As Integer = e.RowIndex
            Dim ID As Integer = CInt(CType(GuestGridView.Rows(row).FindControl("ID"), TextBox).Text)
            Try 'if Webid is null, the record hasn't been added to guestsignup
                Dim webID As Integer = CInt(CType(GuestGridView.Rows(row).FindControl("WebID"), TextBox).Text)
                If webID = 0 Then
                    strSQL = "Delete from tmpGuestSignup where id = @ID"
                    Using cmd As New SqlCommand(strSQL, sqlconnection)
                        cmd.Parameters.AddWithValue("@ID", CInt(ID))
                        cmd.ExecuteNonQuery()
                    End Using
                Else
                    strSQL = "Update tmpGuestSignup set GDeleted='True' where webID = @ID"
                    Using cmd As New SqlCommand(strSQL, sqlconnection)
                        cmd.Parameters.AddWithValue("@ID", webID)
                        cmd.ExecuteNonQuery()
                    End Using
                End If
            Catch
            End Try
            Me.GuestGridView.DataSource = GuestGridDataSource()
            Me.GuestGridView.DataBind()
        End If
    End Sub
    Private Sub UpdateTempAccount()
        Dim sqlQuery As String
        Dim cnt As Integer
        sqlQuery = "Delete from tmpAccount where tMemID=@memID"
        Using cmd As New SqlCommand(sqlQuery, sqlconnection)
            cmd.Parameters.AddWithValue("@memID", CInt(Session("UserID")))
            cmd.ExecuteNonQuery()
        End Using
        sqlQuery = "insert into tmpAccount (tCheckNumber, tType,tcheckDate,tcheckAmount,tPayee,tCategory,tDateEntered, tEventDate,tEventID,tMemID, tduespayed) values ('a','D',@dt,@amt,@pay,@Category,@dt,@dat,@evID,@memID,@dues)"
        Using cmd As New SqlCommand(sqlQuery, sqlconnection)
            cmd.Parameters.Add("@pay", SqlDbType.NChar)
            cmd.Parameters("@pay").Value = Session("FullName")
            cmd.Parameters.Add("@amt", SqlDbType.Money)
            cmd.Parameters("@amt").Value = TotalCost
            cmd.Parameters.Add("@dt", SqlDbType.Date)
            cmd.Parameters("@dt").Value = DateTime.Now
            If Not (Session("EventType").ToString = "Dues Only") Then
                cmd.Parameters.AddWithValue("evID", Session("EventID"))
                cmd.Parameters.AddWithValue("Category", "MealsReceipts")
            Else
                cmd.Parameters.AddWithValue("evID", 0)
                cmd.Parameters.AddWithValue("Category", "Dues")
            End If
            cmd.Parameters.AddWithValue("@dat", CDate(Session("EventDate")))
            cmd.Parameters.Add("@MemID", SqlDbType.BigInt)
            cmd.Parameters("@memID").Value = Session("UserID")
            cmd.Parameters.Add("@dues", SqlDbType.Money)
            cmd.Parameters("@dues").Value = CDbl(Session("DuesOwed"))
            cnt = cmd.ExecuteNonQuery()
        End Using
        RecordError("MemberSignup Update TmpAccount", "Records Added ", CStr(cnt), sqlconnection)
    End Sub

    Protected Sub SendEMailConfirmation(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MessageTo As String
        Dim StrMessage As String


        Dim sqlQuery0 As String = "SELECT FirstName, LastName, [E-Mail], ID FROM member WHERE   ID =@memid "
        Using cmd1 As New SqlCommand(sqlQuery0, sqlconnection)
            cmd1.Parameters.Add("@memid", SqlDbType.Int)
            cmd1.Parameters("@memid").Value = Session("UserID")
            Dim dr1 As SqlDataReader = cmd1.ExecuteReader
            dr1.Read()
            StrMessage = "Dear " & dr1("FirstName").ToString & " " & dr1("LastName").ToString & ","

            StrMessage += ("<br /> <br />")
            MessageTo = dr1("E-Mail").ToString
            dr1.Close()
        End Using
        StrMessage += "<br />The cancelation of your reservation for " & CDate(Session("EventDate")).ToString("dddd, MMM d yyyy") & " is confirmed.  <br /><r />A credit of " & Format(CDbl(Session("Mealsowed")), "c") & " has been posted to your account."
        StrMessage += "<br/> <br />"
        StrMessage += "Thank You."

        Dim i As Integer = SendMessageNA("Reservation Canceled", StrMessage, MessageTo, "reservations@ccfrcville.org", "reservations@ccfrcville.org;webmaster@ccfrcville.org", True)
    End Sub
    Private Sub Review_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Review.Click
        ComputeCost()
        If Not (Session("EventType").ToString = "Dues Only") Then FormView1.UpdateItem(True)
        If TotalCost > 0 Then
            UpdateTempAccount()
        End If
        Response.Redirect("EventReview1.aspx")
    End Sub
End Class
