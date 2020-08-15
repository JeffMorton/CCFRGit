
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports Xceed.Wpf.Toolkit
Public Class EventSignup1
    Inherits PageBase
    Dim conn As SqlConnection
    Dim CurrentMeals As Double
    Dim DuesO As Double
    Dim MealsO As Double
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(True, True))
        conn.Open()
        Session("DateEntered") = Now.ToShortDateString
        SetUpPage()
        If Not IsPostBack Then
            Try
                Dim strSQL = "SELECT ID,Event.EventDate, Event.Cost, Event.Type,Event.speaker FROM Event where ID = @ID"
                Using cmd1 As New SqlCommand(strSQL, conn)
                    cmd1.Parameters.AddWithValue("@ID", Session("EventID"))
                    Dim dr As SqlDataReader
                    dr = cmd1.ExecuteReader
                    dr.Read()
                    Session("EventDate") = dr("EventDate")
                    Me.lbEventDate.Text = String.Format("{0:M/d/yyyy }", dr("EventDate"))
                    Me.lbCost.Text = String.Format("{0:c2}", dr("Cost"))
                    Me.lbSpeaker.Text = dr("Speaker")
                    dr.Close()
                End Using

            Catch ex As Exception
                Response.Redirect("Default.aspx")
            End Try
            Session("IDs") = Session("UserID") & "A" & Session("EventID")
            Session("Quantity") = -1
            Session("first_name") = ""

            Dim sqlQuery As String = "Select Firstname,lastname,address,city,state,zip,telephone,NewMember,MEmail,DuesOwed from member where id = @userid"
            Using cmd As New SqlCommand(sqlQuery, conn)
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
                        Session("email") = Dr("MEmail").ToString
                        Session("NewMember") = Dr("NewMember")
                        Session("DuesOwed") = Dr("DuesOwed")
                    End If

                End While

                Dr.Close()
            End Using

            Me.Panel3.Visible = True

            Me.FormView1.DataSourceID = FVSource.ID
            Me.FormView1.ChangeMode(FormViewMode.Edit)
            Me.MealSource.ConnectionString = conn.ConnectionString
        End If

        Me.MealSource.SelectCommand = "select ID, Meal, Category from  dbo.EventMealCategory(" & Session("EventID") & ") where not meal is null  order by place"
        Me.GuestSource.ConnectionString = conn.ConnectionString
        Me.FVSource.ConnectionString = conn.ConnectionString
        Me.MealSource.ConnectionString = conn.ConnectionString

    End Sub





    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)


        If e.Row.RowType = DataControlRowType.Footer Then
            Dim GuestMeal As DropDownList = DirectCast(e.Row.FindControl("GuestMeal"), DropDownList)  ' Extract the DDL in the Grid Footer 

            Using cmd As New SqlCommand("Select * from dbo.EventMealCategory( @EventID) where not meal is null  order by place")
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


    Protected Function ComputeCost(TotalCost As Decimal) As Decimal


        Dim PayNow As Integer = 0
        Dim i As Integer

        If CType(FormView1.FindControl("MemberAttend"), CheckBox).Checked = True Then PayNow += 1
        If CType(FormView1.FindControl("SpouseAttend"), CheckBox).Checked = True Then PayNow += 1
        Dim ck As CheckBox
        For i = 0 To GuestGridView.Rows.Count - 1
            ck = GuestGridView.Rows(i).FindControl("AddedToMealsOwed")
            If Not ck.Checked Then PayNow += 1
        Next
        PayNow -= CInt(Session("PaidFor"))
        TotalCost = PayNow * CType(lbCost.Text, Decimal)
        If Me.Dues.Checked Then
            TotalCost += DuesO
            Session("DuesOwed") = DuesO
            Session("DuesIncluded") = True
        Else
            Session("DuesOwed") = 0
            Session("DuesIncluded") = False
        End If
        If MealsO > 0 And Me.Meals.Checked Then
            TotalCost += MealsO
        Else
            TotalCost += MealsO
        End If
        Session("MealCost") = lbCost.Text
        ComputeCost = TotalCost
    End Function
    Protected Sub Pay(ByVal sender As Object, ByVal e As System.EventArgs) Handles Review.Click
        Dim TotalCost = 0.00
        If (FormView1.CurrentMode.Equals(FormViewMode.Edit)) Then
            FormView1.UpdateItem(True)
        Else
            FormView1.InsertItem(True)
        End If
        FormView1.DataSourceID = FVSource.ID
        FormView1.ChangeMode(FormViewMode.Edit)
        TotalCost = ComputeCost(TotalCost)
        If TotalCost > 0 Then
            Response.Redirect("EventReview.aspx")
        End If
    End Sub
    Protected Sub SetUpPage()
        Dim PaidFor As Integer
        PaidFor = 0
        Me.Message.Text = ""



        Using cmd0 As SqlCommand = New SqlCommand("CreateTmpSignupRecords", conn)
            cmd0.CommandType = CommandType.StoredProcedure
            cmd0.Parameters.AddWithValue("@EventID", CInt(Session("Eventid")))
            cmd0.Parameters.AddWithValue("@MemberID", CInt(Session("UserID")))
            cmd0.ExecuteNonQuery()
        End Using
        GuestGridView.DataSource = GuestGridDataSource()
        GuestGridView.DataBind()

        Dim strSQL1 = "Select * from membersignup where Memberid= @MemberID And Eventid=@EventID"
        Using cmd1 As New SqlCommand(strSQL1, conn)
            cmd1.Parameters.AddWithValue("@memberID", Session("UserID"))
            cmd1.Parameters.AddWithValue("@EventID", Session("EventID"))
            Dim dr1 As SqlDataReader '
            dr1 = cmd1.ExecuteReader
            dr1.Read()
            If dr1.HasRows Then
                If dr1("MemberAttend") Then PaidFor = 1
                If dr1("SpouseAttend") Then PaidFor += 1
            End If
            dr1.Close()
        End Using

        Session("PaidFor") = PaidFor
        strSQL1 = "Select Mealsowed,duesowed from member where ID= @ID "
        Using cmd1 As New SqlCommand(strSQL1, conn)
            cmd1.Parameters.AddWithValue("@ID", Session("UserID"))
            Dim dr1 As SqlDataReader '
            dr1 = cmd1.ExecuteReader
            dr1.Read()
            MealsO = dr1("MealsOwed")
            DuesO = dr1("DuesOwed")
            If dr1.HasRows Then
                If dr1("DuesOwed") > 0 Then
                    Me.Dues.Visible = True
                    Me.DuesOwed.Visible = True
                    Me.DuesOwed.Text = "Our records indicate that you have not yet paid your   " & String.Format("{0:c2}", dr1("DuesOwed")) & " Dues.  Check here if you wish to have your dues payment included. "
                    Panel3.Visible = True
                End If
                If dr1("MealsOwed") > 0 Then
                    Me.Meals.Visible = True
                    Me.MealsOwed.Text = "Our records indicate that you owe " & String.Format("{0:c2}", dr1("MealsOwed")) & " for prior meals.  Check here if you wish to have your payment included."
                    Me.MealsOwed.Visible = True
                    Panel3.Visible = True
                ElseIf dr1("MealsOwed") < 0 Then
                    Me.MealsOwed.Text = "Our records indicate that you have a credit of " & String.Format("{0:c2}", dr1("MealsOwed")) & " from canceld prior reservations.  This credit will be applied if approprite"
                    Panel3.Visible = True
                End If
            End If
            dr1.Close()
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
            Dim ID As Integer = CType(GuestGridView.Rows(row).FindControl("ID"), TextBox).Text
            Try 'if Webid is null, the record hasn't been added to guestsignup
                Using cmd As New SqlCommand("Select isnull(webID,0) from tmpGuestsignup where id = @ID", conn)
                    cmd.Parameters.AddWithValue("@ID", CInt(ID))
                    Dim webID As Integer = cmd.ExecuteScalar

                    If webID > 0 Then
                        Dim strsql As String = "Delete from GuestSignup where id = @webID"
                        Using cmd1 As New SqlCommand(strsql, conn)
                            cmd1.Parameters.AddWithValue("@webID", webID)
                            cmd1.ExecuteNonQuery()
                        End Using
                    End If
                End Using
                'Place meal charge inas credit in MealsOwed
                Dim Added As Boolean = CType(GuestGridView.Rows(row).FindControl("AddedToMealsOwed"), CheckBox).Checked

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
        Dim credit As Decimal = -CType(lbCost.Text, Decimal)
        Dim strSQL As String = "Update member set MealsOwed=MealsOwed + @MealsOwed where ID = @MemberID"
        Using cmd As New SqlCommand(strSQL, conn)
            cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
            cmd.Parameters.AddWithValue("@MealsOwed", credit)
            cmd.ExecuteNonQuery()
        End Using


    End Sub





End Class
