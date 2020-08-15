Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports Xceed.Wpf.Toolkit
Public Class EventSignUp
    Inherits PageBase
    Dim conn As SqlConnection
    Dim CurrentMeals As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        'Session("loggedin") = True
        'Session("eventtype") = "Dinner"
        'Session("userid") = 83
        'Session("eventid") = 1208
        'Session("eventdate") = "5/20/2020"

        Session("DateEntered") = Now.ToShortDateString
        If Not IsPostBack Then
            Session("ConfirmCheckAmount") = False
            Try
                Dim strSQL As String = "SELECT ID,Event.EventDate, Event.Cost, Event.Type FROM Event where ID = @ID"
                Using cmd1 As New SqlCommand(strSQL, conn)
                    cmd1.Parameters.AddWithValue("@ID", Session("EventID"))
                    Dim dr As SqlDataReader
                    dr = cmd1.ExecuteReader
                    dr.Read()
                    Session("EventDate") = dr("EventDate")
                    Me.EventDate.Text = String.Format("{0:M/d/yyyy }", dr("EventDate"))
                    Me.Cost.Text = String.Format("{0:c2}", dr("Cost"))
                    Me.Type.Text = dr("type").ToString
                    Me.hEventID.Text = dr("ID").ToString

                    dr.Close()
                End Using

            Catch ex As Exception
                Response.Redirect("admin.aspx")
            End Try

            Me.Panel3.Visible = True
            Me.MainPanel.Visible = False

            Me.MealSource.ConnectionString = conn.ConnectionString
        End If
        Me.MemNameDLL.Focus()
        Me.MealSource.SelectCommand = "select ID, Meal, Category from  dbo.EventMealCategory(" & Session("EventID").ToString & ") where not meal is null  order by place"
        Me.GuestSource.ConnectionString = conn.ConnectionString
        Me.FVSource.ConnectionString = conn.ConnectionString
        Me.NameSource.ConnectionString = conn.ConnectionString
        Me.AccSource.ConnectionString = conn.ConnectionString
        Me.MealSource.ConnectionString = conn.ConnectionString

    End Sub
    Protected Sub CancelRes(ByVal sender As Object, ByVal e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            Dim cnt As Integer = 0
            Dim strSQl As String = "select memberAttend,spouseAttend from Membersignup where memberid =@memID and EventID=@evID"
            Using cmd As New SqlCommand(strSQl, conn)
                cmd.Parameters.AddWithValue("@MemID", Session("UserID"))
                cmd.Parameters.AddWithValue("@evID", Session("EventID"))
                Dim DR As SqlDataReader
                DR = cmd.ExecuteReader
                If DR.HasRows Then
                    DR.Read()
                    If CBool(DR("memberAttend")) Then cnt += 1
                    If CBool(DR("SpouseAttend")) Then cnt += 1
                End If
                DR.Close()
            End Using
            strSQl = "Delete Membersignup where memberid =@memID and EventID=@evID"
            Using cmd As New SqlCommand(strSQl, conn)
                cmd.Parameters.AddWithValue("@MemID", Session("UserID"))
                cmd.Parameters.AddWithValue("@evID", Session("EventID"))
                cmd.ExecuteNonQuery()
            End Using
            strSQl = "delete guestsignup where memberid =@memID and EventID=@evID "
            Using cmd As New SqlCommand(strSQl, conn)
                cmd.Parameters.AddWithValue("@MemID", Session("UserID"))
                cmd.Parameters.AddWithValue("@evID", Session("EventID"))
                cnt += cmd.ExecuteNonQuery()
            End Using
            Dim Credit As Double = cnt * CDbl(Session("MealCost"))
            strSQl = "update Member set MealsOwed = MealsOwed - @Credit  where id =@memID  "
            Using cmd As New SqlCommand(strSQl, conn)
                cmd.Parameters.AddWithValue("@MemID", Session("UserID"))
                cmd.Parameters.AddWithValue("@Credit", Credit)
                cmd.ExecuteNonQuery()
            End Using
            Session("MealsOwed") = Credit
            Dim indx As Integer

            indx = MemNameDLL.SelectedIndex
            Debug.Print(CStr(indx))
            MemNameDLL_Index_Changed("", e)
            RecordError("MemberSignup", "Reservation Canceled", " ", conn)
        Else
            Dim myScript As String = "window.alert('Registration not canceled.');"
            ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)
        End If
    End Sub
    Protected Sub Update_form(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim TotalDue As Double
        Dim CheckAmount As Double
        Session("Category") = "Meal Receipts"
        Dim NoRefund As Boolean = CType(AccForm.FindControl("NoRefund"), CheckBox).Checked
        If Not NoRefund Then
            'if user changes MealsOwed, save the change.
            Dim MealsOwed As Double = CDbl(CType(AccForm.FindControl("MealsOwed"), TextBox).Text)
            Using Cmd As New SqlCommand("Update Member set MealsOwed = @MealsOwed where id =@MemberID", conn)
                Cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                Cmd.Parameters.AddWithValue("@MealsOwed", MealsOwed)
                Cmd.ExecuteNonQuery()
            End Using
            ComputeCost()
            TotalDue = CDbl(CType(AccForm.FindControl("TotalCost"), Label).Text)
            'is there a check associated with transaction?
            If IsNumeric(CType(AccForm.FindControl("CheckAmount"), TextBox).Text) And CType(AccForm.FindControl("CheckNumber"), TextBox).Text IsNot Nothing Then
                'check included in transaction
                CheckAmount = CDbl(CType(AccForm.FindControl("CheckAmount"), TextBox).Text)
                If CheckAmount > TotalDue And CBool(Session("ConfirmCheckAmount")) = False Then
                    ' request confirmation if check larger that amount owed.   
                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Check Amount Greater that Total Due. Click Save to Confirm or fix and click  Save')", True)
                    Session("ConfirmCheckAmount") = True
                    Exit Sub
                End If
                Session("ConfirmCheckAmount") = False
                'Are Dues being paid in the transaction
                If CType(AccForm.FindControl("PayDues"), CheckBox).Checked Then
                    CheckAmount -= CDbl(CType(AccForm.FindControl("DuesOwed"), Label).Text)
                    If CheckAmount < 0 Then
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Check Amount not enough to pay dues.  Save canceled')", True)
                        Exit Sub
                    Else
                        If CheckAmount = 0 Then
                            Session("Category") = "Dues"
                        End If
                        Dim strSQL As String = "Update member set DuesOwed=0 where id = @MemberID"
                        Using Cmd As New SqlCommand(strSQL, conn)
                            Cmd.Parameters.AddWithValue("MemberID", Session("UserID"))
                            Cmd.ExecuteNonQuery()
                        End Using
                    End If
                End If
                CheckAmount -= CurrentMeals
                'Is usemealsowed checked?
                If CType(AccForm.FindControl("UseMealsOwed"), CheckBox).Checked Then
                    CheckAmount -= CDbl(CType(AccForm.FindControl("MealsOwed"), TextBox).Text)
                    Using Cmd As New SqlCommand("Update Member set mealsowed =  @mealsowed where ID = @MemberID", conn)
                        Cmd.Parameters.AddWithValue("MemberID", Session("UserID"))
                        Cmd.Parameters.AddWithValue("@mealsowed", CheckAmount)
                        Cmd.ExecuteNonQuery()
                    End Using
                Else
                    Using Cmd As New SqlCommand("Update Member set mealsowed =mealsowed +  @mealsowed where ID = @memberID", conn)
                        Cmd.Parameters.AddWithValue("MemberID", Session("UserID"))
                        Cmd.Parameters.AddWithValue("@mealsowed", CheckAmount)
                        Cmd.ExecuteNonQuery()
                    End Using
                End If
                AccForm.InsertItem(True)
                AccForm.ChangeMode(FormViewMode.Insert)
            Else
                'no check in transaction
                If CType(AccForm.FindControl("PayDues"), CheckBox).Checked Then
                    Dim DuesOwed As Double = CDbl(CType(AccForm.FindControl("DuesOwed"), Label).Text)
                    Dim mo As Double = CDbl(CType(AccForm.FindControl("DuesOwed"), Label).Text)

                    If CType(AccForm.FindControl("UseMealsOwed"), CheckBox).Checked And mo < DuesOwed Then
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Not Enought Meals Oweed to Pay Dues. Save canceled')", True)
                        Exit Sub
                    Else
                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alertuser('Cannot pay dues without a check. Save canceled')", True)
                        Exit Sub
                    End If
                Else
                    'check if meals owed used.
                    If CType(AccForm.FindControl("UseMealsOwed"), CheckBox).Checked Then
                        'if meals owed used then replace meals owed with totaldue
                        Using Cmd As New SqlCommand("Update Member set MealsOwed = @MealsOwed where id =@MemberID", conn)
                            Cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                            Cmd.Parameters.AddWithValue("@MealsOwed", TotalDue)
                            Cmd.ExecuteNonQuery()
                        End Using
                    Else
                        Using Cmd As New SqlCommand("Update Member set MealsOwed =MealsOwed + @MealsOwed where id =@MemberID", conn)
                            Cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                            Cmd.Parameters.AddWithValue("@MealsOwed", TotalDue)
                            Cmd.ExecuteNonQuery()
                        End Using
                    End If
                End If
            End If
        End If

        'Guests are marked as paid for in FinishReservation.  This is done by setting AddedToMealsOwed to true
        If (FormView1.CurrentMode.Equals(FormViewMode.Edit)) Then
            FormView1.UpdateItem(True)
        Else
            FormView1.InsertItem(True)
        End If
        Using Cmd1 As New SqlCommand
            Cmd1.CommandType = CommandType.StoredProcedure
            Cmd1.Connection = conn
            Cmd1.CommandText = "FinishReservation"
            Cmd1.Parameters.AddWithValue("@MemberID", Session("UserID"))
            Cmd1.Parameters.AddWithValue("@EventID", Session("EventID"))
            Cmd1.ExecuteNonQuery()
        End Using
        FormView1.DataSourceID = FVSource.ID
        FormView1.ChangeMode(FormViewMode.Edit)
        Dim indx As Integer = MemNameDLL.SelectedIndex
        'RefreshPaymentArea(CInt(MemNameDLL.SelectedItem.Value))
        MemNameDLL.SelectedIndex = indx
        MemNameDLL_Index_Changed("", e)

    End Sub
    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Footer Then
            Dim GuestMeal As DropDownList = DirectCast(e.Row.FindControl("GuestMeal"), DropDownList)  ' Extract the DDL in the Grid Footer 

            Using cmd As New SqlCommand("Select * from dbo.EventMealCategory( @EventID) where not meal is null  order by place", conn)
                cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
                GuestMeal.DataSource = GridViewDataS(cmd)
                GuestMeal.DataTextField = "meal"
                GuestMeal.DataValueField = "category"
                GuestMeal.DataBind()
            End Using

        End If
    End Sub
    Protected Sub UpdateMealsOwed(MemberID As Integer, Change As Double)
        Dim strsql As String = "Update member Set MealsOwed = MealsOwed + @MealsOwed where Id = @ID"
        Using cmd As SqlCommand = New SqlCommand(strsql, conn)
            cmd.Parameters.AddWithValue("@ID", MemberID)
            cmd.Parameters.AddWithValue("@MealsOwed", Change)
            cmd.ExecuteNonQuery()
        End Using
    End Sub
    Protected Sub ComputeCost()
        Dim TotalCost As Double = 0.00
        Dim PayNow As Integer = 0
        Dim i As Integer

        If CType(FormView1.FindControl("MemberAttend"), CheckBox).Checked = True Then PayNow += 1
        If CType(FormView1.FindControl("SpouseAttend"), CheckBox).Checked = True Then PayNow += 1
        Dim ck As New CheckBox
        For i = 0 To GuestGridView.Rows.Count - 1
            ck.Checked = CType(GuestGridView.Rows(i).FindControl("AddedToMealsOwed"), CheckBox).Checked
            If Not ck.Checked Then PayNow += 1
        Next
        PayNow -= CInt(Session("PaidFor"))

        CurrentMeals = PayNow * CType(Cost.Text, Double)
        If CType(AccForm.FindControl("usemealsowed"), CheckBox).Checked Then
            TotalCost += CDbl(CType(AccForm.FindControl("mealsowed"), TextBox).Text)
        End If
        If CType(AccForm.FindControl("paydues"), CheckBox).Checked Then
            TotalCost += CDbl(CType(AccForm.FindControl("duesowed"), Label).Text)
        End If
        TotalCost += CurrentMeals
        Try
            Dim Total As Label = CType(AccForm.FindControl("TotalCost"), Label)
            Total.Text = Format(TotalCost, "c")
        Finally
        End Try
    End Sub
    Protected Sub MemNameDLL_Index_Changed(sender As Object, e As EventArgs)
        Dim PaidFor As Integer
        PaidFor = 0
        Me.Message.Text = ""
        ClearCostFields()
        Session("UserID") = MemNameDLL.SelectedItem.Value

        Using cmd0 As SqlCommand = New SqlCommand("CreateTmpSignupRecords", conn)
            cmd0.CommandType = CommandType.StoredProcedure
            cmd0.Parameters.AddWithValue("@EventID", CInt(Session("Eventid")))
            cmd0.Parameters.AddWithValue("@MemberID", CInt(Session("UserID")))
            cmd0.ExecuteNonQuery()
        End Using
        GuestGridView.DataSource = GuestGridDataSource()
        GuestGridView.DataBind()

        Dim strSQL1 As String = "Select * from membersignup where Memberid= @MemberID And Eventid=@EventID"
        Using cmd1 As New SqlCommand(strSQL1, conn)
            cmd1.Parameters.AddWithValue("@memberID", MemNameDLL.SelectedItem.Value)
            cmd1.Parameters.AddWithValue("@EventID", Session("EventID"))
            Dim dr1 As SqlDataReader '
            dr1 = cmd1.ExecuteReader
            dr1.Read()
            If dr1.HasRows Then
                If CBool(dr1("MemberAttend")) Then PaidFor = 1
                If CBool(dr1("SpouseAttend")) Then PaidFor += 1
            End If
            dr1.Close()
        End Using

        Session("PaidFor") = PaidFor

        RefreshPaymentArea(CInt(MemNameDLL.SelectedItem.Value))
        Dim strSQL As String = "Select  Member.DuesOwed, member.MealsOwed FROM Member where  member.id = " & MemNameDLL.SelectedItem.Value
        AccSource.SelectCommand = strSQL
        Using cmd0 As New SqlCommand("Select top 6 tID, tcheckNumber, tcheckdate, tcheckamount, tPayee  from account where tPayee Like '%' + @LastName + '%' and tPayee Like '%' + @FirstName + '%' order by tID desc", conn)
            cmd0.Parameters.AddWithValue("@FirstName", Session("FirstName"))
            cmd0.Parameters.AddWithValue("@LastName", Session("LastName"))
            PmtGridview.DataSource = GridViewDataS(cmd0)
        End Using
        PmtGridview.DataBind()
        Me.FormView1.DataSourceID = FVSource.ID
        Me.FormView1.ChangeMode(FormViewMode.Edit)
        Me.MainPanel.Visible = True
    End Sub
    Protected Sub RefreshPaymentArea(memberID As Integer)
        Dim strSQL As String = "Select ID, isnull(MealsOwed, 0) As MealsOwed, isnull(DuesOwed, 0) As DuesOwed, mfFullName, firstname, lastname  from member where id = @MemberID "
        Dim dr As SqlDataReader
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@memberID", memberID)
            dr = cmd.ExecuteReader
            While dr.Read()
                If dr.HasRows Then
                    Me.hMemberID.Text = dr("ID").ToString
                    Session("UserID") = dr("ID")
                    Session("MealsOwed") = FormatCurrency(dr("MealsOwed"))
                    Session("DuesOwed") = FormatCurrency(dr("DuesOwed"))
                    Session("FirstName") = dr("FirstName")
                    Session("LastName") = dr("LastName")
                    Session("FullName") = dr("mfFullName")
                End If
            End While
            AccForm.ChangeMode(FormViewMode.Insert)
            dr.Close()
        End Using
    End Sub
    Protected Function GuestGridDataSource() As DataTable
        Using cmd As New SqlCommand("select tmpGuestSignUp.GLastName,tmpGuestSignUp.GFirstName, tmpGuestSignUp.ID,tmpGuestSignUp.AddedToMealsOwed,EventMC.Meal,webID,MemberID,EventID FROM tmpGuestSignUp INNER JOIN EventMC ON tmpGuestSignUp.EventID = EventMC.ID And tmpGuestSignUp.GuestMeal = EventMC.category where eventid = @EventID And memberid =@MemberID", conn)
            cmd.Parameters.AddWithValue("@EventID", CInt(Session("Eventid")))
            cmd.Parameters.AddWithValue("@MemberID", CInt(Session("UserID")))

            Using ada As SqlDataAdapter = New SqlDataAdapter(cmd)
                Using dt As DataTable = New DataTable()
                    ada.Fill(dt)
                    If dt.Rows.Count = 0 Then
                        dt.Rows.Add(" ", " ", 0, True, "", DBNull.Value, Session("EventID"), Session("UserID"))
                    End If
                    GuestGridDataSource = dt
                End Using
            End Using
        End Using
    End Function

    Protected Sub GridCommand(sender As Object, e As GridViewCommandEventArgs)
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

            Dim strSQL As String = "INSERT into [tmpGuestSignUp] (Gfirstname, Glastname, GuestMeal, GuestName, EventID, MemberID, AddedtoMealsOwed) VALUES (@FirstName, @LastName,@GuestMeal, @GuestName,@EventID,@MemberID,'false')"
            Using cmd As New SqlCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@FirstName", FirstName)
                cmd.Parameters.AddWithValue("@LastName", LastName)
                cmd.Parameters.AddWithValue("@GuestMeal", GuestMeal)
                cmd.Parameters.AddWithValue("@GuestName", FirstName & " " & LastName)
                cmd.Parameters.AddWithValue("@EventID", Session("Eventid"))
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                cmd.ExecuteNonQuery()
            End Using
            GuestGridView.DataSource = GuestGridDataSource()
            GuestGridView.DataBind()
        End If

    End Sub
    Protected Sub DeleteGuest(sender As Object, e As GridViewDeleteEventArgs)
        Dim Deletevalue As String = Request.Form("confirm_value")

        If Deletevalue = "Yes" Then
            Dim row As Integer = e.RowIndex
            Dim ID As Integer = CInt(CType(GuestGridView.Rows(row).FindControl("ID"), TextBox).Text)
            Try 'if Webid is null, the record hasn't been added to guestsignup
                Using cmd As New SqlCommand("Select isnull(webID,0) from tmpGuestsignup where id = @ID", conn)
                    cmd.Parameters.AddWithValue("@ID", CInt(ID))
                    Dim webID As Integer = CInt(cmd.ExecuteScalar)

                    If webID > 0 Then
                        Dim strsql As String = "Delete from GuestSignup where id = @webID"
                        Using cmd1 As New SqlCommand(strsql, conn)
                            cmd1.Parameters.AddWithValue("@webID", webID)
                            cmd1.ExecuteNonQuery()
                        End Using
                    End If
                End Using
                'Place meal charge inas credit in MealsOwed
                Dim NoRefund As Boolean = CType(AccForm.FindControl("NoRefund"), CheckBox).Checked
                Dim Added As Boolean = CType(GuestGridView.Rows(row).FindControl("AddedToMealsOwed"), CheckBox).Checked
                If Added = True And NoRefund = False Then
                    RemoveMeal()

                End If
            Catch
            End Try
            Using cmd As New SqlCommand("Delete from tmpGuestsignup where id = @ID", conn)
                cmd.Parameters.AddWithValue("@ID", CInt(ID))
                cmd.ExecuteNonQuery()
                GuestGridView.DataSource = GuestGridDataSource()
                GuestGridView.DataBind()
            End Using
        End If
    End Sub
    Protected Sub RemoveMeal()
        Dim credit As Decimal = -CType(Cost.Text, Decimal)
        Dim strSQL As String = "Update member set MealsOwed=MealsOwed + @MealsOwed where ID = @MemberID"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
            cmd.Parameters.AddWithValue("@MealsOwed", credit)
            cmd.ExecuteNonQuery()
        End Using

        CType(AccForm.FindControl("MealsOwed"), TextBox).Text = CStr(CDbl(CType(AccForm.FindControl("MealsOwed"), TextBox).Text) + credit)
    End Sub
    Protected Sub ClearCostFields()

        Session("ConfirmCheckAmount") = False
        'CType(AccForm.FindControl("DuesOwed"), Label).Text = 0
        ' CType(AccForm.FindControl("MealsOwed"), TextBox).Text = 0
        ' CType(AccForm.FindControl("TotalCost"), Label).Text = 0


    End Sub
End Class
