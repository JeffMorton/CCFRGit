Imports System.Data.SqlClient

Public Class EventReview1
    Inherits CCFRW19.PageBase
    Dim memID As Integer
    Dim evid As Integer
    Dim ecnt As Integer
    ReadOnly dt As New DataTable()
    Dim naCaption As String = "Number Attending"
    Dim TotalCost As Double = 0
    Dim DuesPaid As Double = 0
    Dim MealsOwed As Double = 0
    Dim NoAttend As Long
    Dim CostPer As Double
    ReadOnly sqlconnection As New SqlConnection
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sqlconnection.ConnectionString = GetConnectionStringM(False, False)
        Dim cnt As Integer = 0
        memID = CInt(Session("Userid"))
        evid = CInt(Session("Eventid"))
        dt.Columns.AddRange(New DataColumn() {New DataColumn("Item", GetType(String)),
                                               New DataColumn("Amount", GetType(String))})

        sqlconnection.Open()
        If Session("UserID") Is Nothing Then
            Response.Redirect("login.aspx")
        Else
            Using cmd As New SqlCommand("Select mfFullName from member where ID=@MemberId", sqlConnection)
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                If Session("EventType").ToString = "Dues Only" Then
                    Reserve.Text = "Dues Payment For: " & cmd.ExecuteScalar.ToString
                Else
                    Reserve.Text = "Reservations For: " & cmd.ExecuteScalar.ToString
                End If
            End Using
        End If
        Dim x As Integer = CInt(Session("NothingMuch"))
        Dim PmemAttend, PspAttend, memAttend, spAttend As Boolean
        If Not IsPostBack Then
            If Not (Session("EventType").ToString = "Dues Only") Then
                Dim sqlQuery As String = "select * from membersignup where Memberid = @memID and EventID = @evID"
                Using cmd0 As SqlCommand = New SqlCommand(sqlQuery, sqlconnection)
                    cmd0.Parameters.Add("@memID", SqlDbType.Int)
                    cmd0.Parameters("@memID").Value = CInt(Session("Userid"))
                    cmd0.Parameters.Add("@evID", SqlDbType.Int)
                    cmd0.Parameters("@evID").Value = CInt(Session("Eventid"))
                    Dim Dr As SqlDataReader
                    Dr = cmd0.ExecuteReader
                    Dr.Read()
                    If Dr.HasRows Then
                        PmemAttend = CBool(Dr("memberAttend"))
                        PspAttend = CBool(Dr("SpouseAttend"))
                    End If
                End Using

                sqlQuery = "select * from tmpmembersignup where Memberid = @memID and EventID = @evID"
                Using cmd0 As SqlCommand = New SqlCommand(sqlQuery, sqlconnection)
                    cmd0.Parameters.Add("@memID", SqlDbType.Int)
                    cmd0.Parameters("@memID").Value = CInt(Session("Userid"))
                    cmd0.Parameters.Add("@evID", SqlDbType.Int)
                    cmd0.Parameters("@evID").Value = CInt(Session("Eventid"))
                    Dim Dr As SqlDataReader
                    Dr = cmd0.ExecuteReader
                    If Dr.HasRows Then
                        Dr.Read()
                        memAttend = CBool(Dr("memberAttend"))
                        spAttend = CBool(Dr("SpouseAttend"))
                    Else
                        If Not CBool(Session("DuesIncluded")) Then
                            Me.Msg.Text = "No Reservation Created. Please indicate who the reservation is for"
                            'Dim e1 As System.EventArgs
                            'Cancel_Click("No Reservations", e1)
                        Else
                            Me.Reserve.Text = "Pay Dues For"
                            'Me.CP.Visible = False
                            'Me.NA.Visible = False
                            'Me.SignedUp.Visible = False
                            'Me.NumAttend.Visible = False
                            'Me.CostPer.Visible = False
                            'Me.Reserve.Visible = False


                        End If
                    End If
                    Dr.Close()
                End Using

                sqlQuery = "select FirstName,LastName, SpouseFirstName,SpouseLastName from Member where ID = @memID"
                Using cmd1 As SqlCommand = New SqlCommand(sqlQuery, sqlconnection)
                    cmd1.Parameters.Add("@memID", SqlDbType.Int)
                    cmd1.Parameters("@memID").Value = CInt(Session("Userid"))
                    Dim Dr1 As SqlDataReader
                    Dr1 = cmd1.ExecuteReader
                    If Dr1.HasRows Then
                        Dr1.Read()
                        If memAttend Then
                            If PmemAttend Then
                                Me.SignedUp.Items.Add(Dr1("FirstName").ToString & " " & Dr1("LastName").ToString & " " & " - Previously Paid for")
                                naCaption = "Additional Attending"
                            Else
                                cnt += 1
                                Me.SignedUp.Items.Add(Dr1("FirstName").ToString & " " & Dr1("LastName").ToString)

                            End If
                        ElseIf PmemAttend Then
                            Me.SignedUp.Items.Add(Dr1("FirstName").ToString & " " & Dr1("LastName").ToString & " " & "Removed")
                            cnt -= 1
                        End If

                        If spAttend Then
                            If PspAttend Then
                                Me.SignedUp.Items.Add(Dr1("SpouseFirstName").ToString & " " & Dr1("SpouseLastName").ToString & " " & " - Previously Paid for")
                                naCaption = "Additional Attending"
                            Else
                                cnt += 1
                                Me.SignedUp.Items.Add(Dr1("SpouseFirstName").ToString & " " & Dr1("SpouseLastName").ToString)

                            End If
                        ElseIf PspAttend Then
                            Me.SignedUp.Items.Add(Dr1("SpouseFirstName").ToString & " " & Dr1("SpouseLastName").ToString & " " & "Removed")
                            cnt -= 1
                        End If
                    End If
                    Dr1.Close()
                End Using


                Dim sqlQuery1 As String = "Select GuestName,addedtomealsowed, GDeleted  from tmpGuestSignup where MemberID=@memID and EventID  = @evid"
                Using cmd2 As SqlCommand = New SqlCommand(sqlQuery1, sqlconnection)
                    cmd2.Parameters.Add("@memID", SqlDbType.Int)
                    cmd2.Parameters("@memID").Value = CInt(Session("Userid"))
                    cmd2.Parameters.Add("@evID", SqlDbType.Int)
                    cmd2.Parameters("@evID").Value = CInt(Session("Eventid"))
                    Dim Dr2 As SqlDataReader
                    Dr2 = cmd2.ExecuteReader
                    Try
                        While Dr2.HasRows
                            Dr2.Read()
                            If Not (Dr2("GuestName") Is Nothing) Then
                                If CBool(Dr2("GDeleted")) = True And CBool(Dr2("addedtoMealsOwed")) = True Then
                                    Me.SignedUp.Items.Add(Dr2("GuestName").ToString & " " & " - Removed")
                                    cnt -= 1
                                ElseIf CBool(Dr2("AddedtoMealsOwed")) Then
                                    Me.SignedUp.Items.Add(Dr2("GuestName").ToString & " " & " - Previously Paid for")
                                Else
                                    cnt += 1
                                    Me.SignedUp.Items.Add(Dr2("GuestName").ToString)
                                End If
                            End If
                        End While
                    Catch ex As Exception
                    End Try
                End Using
                ecnt = CheckCount(evid)
                RecordError("ER Event Count", CStr(ecnt + cnt), " ", sqlconnection)
                If ecnt + cnt >= 200 And cnt > 0 Then
                    Response.Redirect("Dinnerclosed.aspx")
                End If
                If cnt > 0 Or (cnt < 0 And CBool(Session("DuesIncluded")) = True) Then
                    Me.Panel3.Visible = True
                    dt.Rows.Add(naCaption, CStr(cnt))
                    dt.Rows.Add("Cost of Meals at" & " " & Format(CDbl(Session("mealCost")), "c") & ":", CStr(Format(CDbl(cnt * CDbl(Session("MealCost"))), "c"))) '
                    Session("NoAttend") = cnt
                Else
                    Me.Panel3.Visible = False
                    Msg.Text = "You have removed " & -cnt & " " & CStr(IIf(cnt = -1, "person", "people")) & " from your reservation.  A credit of " & CStr(Format(CDbl(-cnt * CDbl(Session("MealCost"))), "c")) & " will be added to your account. "
                    Msg.Visible = True
                    Session("NoAttend") = cnt
                End If
            Else
                If Session("MealCost") Is Nothing Then Session("MealCost") = 0
                Panel1.Visible = True
                Panel3.Visible = True
                cnt = 0
                Me.SignedUp.Visible = False
                TotalCost = CDbl(Session("DuesOwed"))
            End If
            If CBool(Session("DuesIncluded")) Then
                dt.Rows.Add("Dues Owed: ", Format(CDbl(Session("DuesOwed")), "c"))
                DuesPaid = CDbl(Session("DuesOwed"))
            Else
                DuesPaid = 0
            End If

            TotalCost = CDbl(Format(cnt * CDbl(Session("MealCost")) + CDbl(Session("DuesOwed")) + CDbl(Session("MealsOwed")), "c"))
            If TotalCost <= 0 Then
                Session("MealsOwedAfter") = CDbl(TotalCost)
                Session("MealsOwed") = CDbl(Session("MealsOwed")) - CDbl(Session("MealsOwedAfter"))
                TotalCost = CDbl(Format(cnt * CDbl(Session("MealCost")) + CDbl(Session("DuesOwed")) + CDbl(Session("MealsOwed")), "c"))
                Me.Msg1.Text = "No payment required for this reservation.  Please click on Complete Reservation to save your reservation. "
                Me.Msg1.Visible = True
            Else
                Session("MealsOwedAfter") = 0
                If CDbl(Session("MealsOwed")) > 0 Then
                    dt.Rows.Add("Prior Charges:", Format(CDbl(Session("MealsOwed")), "c"))
                End If
            End If
            'MealsOwed = Session("MealsOwed")

            If CDbl(Session("MealsOwed")) < 0 Then
                dt.Rows.Add("Prior Credits:   ", Format(CDbl(Session("MealsOwed")), "c"))

                If CDbl(Session("MealsOwedAfter")) < 0 Then
                    Me.Msg.Text = "You have a remaining credit of " & Format(-CDbl(Session("MealsOwedAfter")), "c") & ". "
                    Me.Msg.Visible = True
                    'Me.MealsLabel.Text = ""
                    'Me.Meals.Text = ""
                End If
            ElseIf CDbl(Session("MealsOweed")) > 0 Then
                dt.Rows.Add("Prior Charges:", Format(Session("MealsOwed"), "c"))
            End If
            dt.Rows.Add(" ", " ")
            dt.Rows.Add("Total cost:", Format(TotalCost, "c"))
            If TotalCost <= 0 Then
                Complete.Visible = True
                Panel1.Visible = False
            Else
                Panel1.Visible = True
            End If
            Session("IDs") = Session("UserID").ToString & "A" & Session("EventID").ToString
            Session("IDs") = Session("IDs").ToString & "A" & cnt & "A" & Session("DuesIncluded").ToString & "A" & Session("MealsOwed").ToString
            Debug.Print(Session("IDs").ToString)
            If cnt = 0 Then
                If CBool(Session("DuesIncluded")) Then
                    Session("Item1Name") = "CCFR Dues"
                    Session("Item1cost") = TotalCost
                    Session("Item1quantity") = 1
                Else
                    Me.Msg.Text = "Nothing to Pay for."
                    'Response.Redirect("EventRegistration")
                End If

            Else
                Session("Item1Name") = ""
                If cnt > 0 Then
                    Session("Item1Name") = "CCFR Reservations"
                End If
                If CBool(Session("DuesIncluded")) Then
                    Session("Item1Name") = Session("Item1Name").ToString & " and Dues"
                End If
                If CDbl(Session("Mealsowed")) > 0 Then
                    Session("Item!Name") = Session("Item1Name").ToString & " with Prior Charge"
                End If
                If CDbl(Session("MealsOwed")) < 0 Then
                    Session("Item1Name") = Session("Item1Name").ToString & " with Prior Credit"
                End If
                Session("Item1cost") = TotalCost
                Session("Item1quantity") = 1

            End If
        End If

        GridView1.DataSource = dt
        GridView1.DataBind()
    End Sub

    Protected Sub Complete_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Complete.Click
        RecordError("EventReview1 Started", "Complete Button", " ", sqlconnection)
        Dim strSQL As String = "Update Member set MealsOwed= @MealsOwed where id = @memberID"

        'update mealsowed
        If CDbl(Session("MealsOwedAfter")) <> 0 Then
            Using cmd As New SqlCommand(strSQL, sqlconnection)
                cmd.Parameters.AddWithValue("@MealsOwed", Session("MealsOwedAfter"))
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                cmd.ExecuteNonQuery()
            End Using
        Else
            Using cmd As New SqlCommand(strSQL, sqlconnection)
                cmd.Parameters.AddWithValue("@MealsOwed", 0)
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                cmd.ExecuteNonQuery()
            End Using
        End If

        'update dues
        If CBool(Session("DuesIncluded")) = True Then
            strSQL = "Update Member set DuesOwed = 0 where id = @memberID"
            Using cmd As New SqlCommand(strSQL, sqlconnection)
                cmd.Parameters.AddWithValue("@MemberID", Session("UserID"))
                cmd.ExecuteNonQuery()
            End Using
        End If

        Using Cmd1 As New SqlCommand
            Cmd1.CommandType = CommandType.StoredProcedure
            Cmd1.Connection = sqlconnection
            Cmd1.CommandText = "FinishReservation"
            Cmd1.Parameters.AddWithValue("@MemberID", Session("UserID"))
            Cmd1.Parameters.AddWithValue("@EventID", Session("EventID"))
            Cmd1.ExecuteNonQuery()
        End Using

        SendEMailConfirmation(Me, e)
        Dim myScript As String = "window.confirm('Reservation is complete.');"
        ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)

        Response.Redirect("Default.aspx")

    End Sub
    'Private Sub Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.Click, Cancel1.Click
    '    Dim strSQL As String = "execute dbo.cleartemptables @memID,@evid"
    '    Using cmdClear As New SqlCommand(strSQL, sqlconnection)
    '        cmdClear.Parameters.Add("@evID", SqlDbType.Int)
    '        cmdClear.Parameters("@evID").Value = evid
    '        cmdClear.Parameters.Add("@memid", SqlDbType.Int)
    '        cmdClear.Parameters("@memid").Value = memID
    '        cmdClear.ExecuteNonQuery()
    '    End Using
    '    sqlconnection.Dispose()
    '    Response.Redirect("Default.aspx")
    'End Sub

    Private Function CheckCount(ByVal evid As Integer) As Integer
        If Not IsNumeric(evid) Then
            CheckCount = 0
        Else
            Using cmd As New SqlCommand()
                With cmd
                    .CommandText = "select dbo.memcount(@eventid)"
                    .CommandType = CommandType.Text
                    .Connection = SqlConnection
                End With
                cmd.Parameters.AddWithValue("@eventid", evid)
                CheckCount = CInt(cmd.ExecuteScalar)

            End Using
        End If
    End Function
    Protected Sub SendEMailConfirmation(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim MessageTo As String
        Dim mSubject As String = ""
        Dim meals(3, 3) As Object
        Dim StrMessage As String
        '       Dim sTotalcost As String
        DuesPaid = CDbl(Session("DuesOwed"))
        MealsOwed = CDbl(Session("MealsOwed"))
        NoAttend = CLng(Session("NoAttend"))
        CostPer = CDbl(Session("MealCost"))
        '        sTotalCost = Format(NoAttend * CDbl(Session("MealCost")) + CDbl(Session("DuesOwed")) + CDbl(Session("MealsOwed")), "c")




        Dim sqlQuery0 As String = "SELECT FirstName, LastName, [E-Mail], ID FROM member WHERE   ID =@memid "
        Using cmd1 As New SqlCommand(sqlQuery0, sqlConnection)
            cmd1.Parameters.Add("@memid", SqlDbType.Int)
            cmd1.Parameters("@memid").Value = memID
            Dim dr1 As SqlDataReader = cmd1.ExecuteReader
            dr1.Read()
            StrMessage = "Dear " & dr1("FirstName").ToString & " " & dr1("LastName").ToString & ","

            StrMessage += ("<br /> <br />")
            MessageTo = dr1("E-Mail").ToString
            dr1.Close()
        End Using


        Try


            If NoAttend > 0 Then
                Dim strdat As String
                Dim sqlQuery3 As String = "select Eventdate from Event where ID = @evid"
                Using cmd3 As New SqlCommand(sqlQuery3, sqlConnection)
                    cmd3.Parameters.Add("@evid", SqlDbType.Int)
                    cmd3.Parameters("@evid").Value = evid
                    strdat = CDate(cmd3.ExecuteScalar).ToString("dddd, MMM d yyyy")
                End Using
                StrMessage += "Your reservation for the CCFR meeting on " & strdat & " is Confirmed."
                If DuesPaid > 0 Then
                    StrMessage += "<br />Your CCFR dues payment of " & Format(DuesPaid, "c") & " is also confirmed.<br />"
                Else
                    StrMessage += "<br/>"
                End If
            ElseIf noattend = 0 And DuesPaid > 0 Then
                StrMessage += "Your CCFR dues  of " & Format(DuesPaid, "c") & " is Paid<br />"
                StrMessage += "The credit on your account was reduced by " & Format(DuesPaid, "c") & "<br />"
                StrMessage += "Your total payment of " & Format(TotalCost, "c") & " is confirmed<br />"

                StrMessage += "<br />Thank You."
                mSubject = "CCFR Dues Payment Confirmation"
            ElseIf NoAttend < 0 And DuesPaid = 0 Then
                mSubject = "CCFR Reservation Change Confirmation"

            ElseIf NoAttend < 0 And DuesPaid > 0 Then
                StrMessage += "Your CCFR dues payment of " & Format(DuesPaid, "c") & " is also confirmed.<br />"
                mSubject = "CCFR Reservation Change Confirmation"
            End If

        Catch ex As EvaluateException
        End Try
        StrMessage += "<br />"
        Dim cnt As Integer
        Using cmd As New SqlCommand("select count(mFullName) from AttendDetail(@evID,@memID)", sqlConnection)
            cmd.Parameters.Add("@evid", SqlDbType.Int)
            cmd.Parameters("@evID").Value = Session("EventID")
            cmd.Parameters.Add("@memID", SqlDbType.Int)
            cmd.Parameters("@memID").Value = Session("UserID")
            cnt = CInt(cmd.ExecuteScalar)
        End Using

        If NoAttend <> 0 And cnt > 0 Then
            StrMessage += "<br/>Reservation For:"
            StrMessage += "<br /><Table>"
            Using cmd As New SqlCommand("select * from AttendDetail(@evID,@memID)", sqlConnection)
                cmd.Parameters.Add("@evid", SqlDbType.Int)
                cmd.Parameters("@evID").Value = Session("EventID")
                cmd.Parameters.Add("@memID", SqlDbType.Int)
                cmd.Parameters("@memID").Value = Session("UserID")
                Dim dr As SqlDataReader
                dr = cmd.ExecuteReader
                If dr.HasRows Then
                    StrMessage += "<tr ><th style='text-align: Left;width:300px'>Name</th> <th style='text-align: left;width:70px'>Guest</th><th style='text-align: left;width:150px'>Meal</th></tr>"
                    While dr.Read
                        StrMessage += "<tr ><th style='text-align: Left;font-weight:normal'>" & dr("mFullName").ToString & "</th> <th style='text-align: left;width:70px;font-weight:normal'>" & dr("gst").ToString & "</th><th style='text-align: left;width:250px;font-weight:normal'>" & dr("Meal").ToString & "</th></tr>"
                    End While
                End If
            End Using
            StrMessage += "</table><br />"
        End If
        If NoAttend > 0 Then
            StrMessage += "<br /><Table>"
            StrMessage += "<tr ><th style='text-align: Left;font-weight:normal;width:125px'>Meals Paid For:</th><th style='text-align: right;width:175px;font-weight:normal'>" & CStr(NoAttend) & "</th></tr>"
            StrMessage += "<tr > <th style='text-align: Left;font-weight:normal'>Cost per Person:</th><th style='text-align: right;font-weight:normal'>" & Format(CostPer, "c") & "</th></tr>"
            StrMessage += "</table><br /><br /><table> "
            StrMessage += "<tr ><th style='text-align: Left;font-weight:normal;width:100px'>Cost for Meals:</th><th style='text-align: right;width:200px;font-weight:normal'>" & Format(CostPer * NoAttend, "c") & "</th></tr>"

            If MealsOwed <> 0 Then
                Dim Cap As String
                If MealsOwed > 0 Then
                    Cap = "Prior Charges: "
                Else
                    Cap = "Prior Credits: "
                End If
                StrMessage += "<tr><th style='text-align: Left;font-weight:normal'>" & Cap & " </th><th style='text-align: right;font-weight:normal'>" & Format(MealsOwed, "c") & "</th><tr>"
            End If
            If DuesPaid > 0 Then
                StrMessage += "<tr><th style='text-align: Left;font-weight:normal'>Dues: </th><th style='text-align: right;font-weight:normal'>" & Format(DuesPaid, "c") & "</th></Tr>"
            End If
            If MealsOwed <> 0 Or DuesPaid > 0 Then
                StrMessage += "<tr><th style='text-align: Left;font-weight:normal'></th><th style='text-align: right'>-----------</th></Tr>"
                StrMessage += "<tr><th style='text-align: Left'>Total Cost:</th><th style='text-align: right;;font-weight:normal'> " & Format(CostPer * NoAttend + DuesPaid + MealsOwed, "c") & "</th></tr>"
            End If
            StrMessage += "</table> "


        ElseIf NoAttend < 0 Then
            Dim mcap As String
            If NoAttend = -1 Then
                mcap = "person"
            Else
                mcap = "people"
            End If
            StrMessage += "You have removed " & CStr(-NoAttend) & " " & mcap & " from your reservation.  A credit of " & Format(CDbl(-NoAttend * CostPer), "c") & " as been added to you account."
        Else
            StrMessage += "No additional meals purchased"
        End If
        StrMessage += "<br/> <br />"
        StrMessage += "Thank You."




        SendMessageNA(mSubject, StrMessage, MessageTo, "reservations@ccfrcville.org", "webmaster@ccfrcville.org;reservations@ccfrcville.org", True)
    End Sub
End Class
