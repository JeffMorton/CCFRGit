Imports System.Data.SqlClient
Partial Public Class CurReservation
    Inherits PageBase
    ReadOnly sqlConnection As New SqlConnection

    ReadOnly meals(3, 3) As Object
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        sqlConnection.ConnectionString = GetConnectionString(False, False)
        sqlConnection.Open()
        SetUpMeals()
        Dim cnt As Integer = 0
        Dim sqlQuery As String
        If Session("Status") = "Confirmed" Then
            sqlQuery = "SELECT Member.FirstName, Member.LastName, Member.SpouseFirstName, Member.SpouseLastName, MemberSignup.MemberAttend, MemberSignup.SpouseAttend, MemberSignup.MemberMeal, MemberSignup.SpouseMeal, GuestSignUp.GuestName, GuestSignUp.GuestMeal, MemberSignup.MemberMeal, MemberSignup.SpouseMeal, MemberSignup.EventID, MemberSignup.MemberID FROM (Member INNER JOIN MemberSignup ON Member.ID = MemberSignup.MemberID) LEFT JOIN GuestSignUp ON (MemberSignup.EventID = GuestSignUp.EventID) AND (MemberSignup.MemberID = GuestSignUp.MemberID) WHERE (((MemberSignup.EventID)=@evid) AND ((MemberSignup.MemberID)=@memID))"
        Else
            sqlQuery = "SELECT Member.FirstName, Member.LastName, Member.SpouseFirstName, Member.SpouseLastName, tmpMemberSignup.MemberAttend, tmpMemberSignup.SpouseAttend, tmpMemberSignup.MemberMeal, tmpMemberSignup.SpouseMeal, tmpGuestSignUp.GuestName, tmpGuestSignUp.GuestMeal, tmpMemberSignup.MemberMeal, tmpMemberSignup.SpouseMeal, tmpMemberSignup.EventID, tmpMemberSignup.MemberID FROM (Member INNER JOIN tmpMemberSignup ON Member.ID = tmpMemberSignup.MemberID) LEFT JOIN tmpGuestSignUp ON (tmpMemberSignup.EventID = tmpGuestSignUp.EventID) AND (tmpMemberSignup.MemberID = tmpGuestSignUp.MemberID)WHERE (((tmpMemberSignup.EventID)=@evid) AND ((tmpMemberSignup.MemberID)=@memID))"
        End If
        Using cmd0 As SqlCommand = New SqlCommand(sqlQuery, sqlConnection)
            cmd0.Parameters.Add("@memID", SqlDbType.Int)
            cmd0.Parameters("@memID").Value = CInt(Session("Userid"))
            cmd0.Parameters.Add("@evID", SqlDbType.Int)
            cmd0.Parameters("@evID").Value = CInt(Session("Eventid"))
            Dim DR As SqlDataReader = cmd0.ExecuteReader
            If DR.HasRows Then
                While DR.Read
                    cnt += 1
                    Select Case cnt
                        Case 1
                            If CBool(DR("MemberAttend")) Then
                                Mem1.Text = DR("FirstName").ToString & " " & DR("LastName").ToString
                                Mem1Meal.Text = DecodeMeal(CInt(DR("MemberMeal")))
                                lbMemberName.Text = Mem1.Text
                            End If
                            If CBool(DR("SpouseAttend")) Then
                                mem2.Text = DR("Spousefirstname").ToString & " " & DR("SpouseLastName").ToString
                                Mem2Meal.Text = DecodeMeal(CInt(DR("SpouseMeal"))).ToString
                            End If
                            If Not CBool((DR("GuestName")) Is System.DBNull.Value) Then
                                G1.Text = DR("GuestName").ToString
                                G1Meal.Text = DecodeMeal(CInt(DR("GuestMeal")))
                            End If
                        Case 2
                            If Not (DR("GuestName") Is System.DBNull.Value) Then
                                G2.Text = DR("GuestName").ToString
                                G2Meal.Text = DecodeMeal(CInt(DR("GuestMeal")))

                            End If
                        Case 3
                            If Not CBool((DR("GuestName")) Is System.DBNull.Value) Then
                                G3.Text = DR("GuestName").ToString
                                G3Meal.Text = DecodeMeal(CInt(DR("GuestMeal")))
                            End If
                        Case 4
                            If Not CBool((DR("GuestName")) Is System.DBNull.Value) Then
                                G4.Text = DR("GuestName").ToString
                                G4Meal.Text = DecodeMeal(CInt(DR("GuestMeal")))
                            End If
                        Case 5
                            If Not (DR("GuestName") Is System.DBNull.Value) Then
                                G5.Text = DR("GuestName").ToString
                                G5Meal.Text = DecodeMeal(CInt(DR("GuestMeal")))
                            End If
                        Case 6
                            If Not (DR("GuestName") Is System.DBNull.Value) Then
                                G6.Text = DR("GuestName").ToString
                                G6Meal.Text = DecodeMeal(CInt(DR("GuestMeal")))
                            End If


                    End Select

                End While

            Else
                'no reservation

            End If
            DR.Close()
        End Using
        Me.Tres.Text = Info("Treasurer")
        Me.tresphone.Text = Format(CLng(Info("TreasurerPhone")), "(###) ###-####")
        Me.lbStatus.Text = Session("Status")
        If Session("status") = "Pending*" Then
            Dim sqlQueryD As String = "Select tDateEntered from tmpAccount where tEventID =@evid and tMemID = @memID"
            Using cmdD As New SqlCommand(sqlQueryD, sqlConnection)
                cmdD.Parameters.Add("@memID", SqlDbType.Int)
                cmdD.Parameters("@memID").Value = CInt(Session("Userid"))
                cmdD.Parameters.Add("@evID", SqlDbType.Int)
                cmdD.Parameters("@evID").Value = CInt(Session("Eventid"))
                Dim dt As Date = cmdD.ExecuteScalar
                Dim hr As Integer = DateDiff(DateInterval.Hour, dt, Date.Now)
                If hr < 6 Then
                    Me.lbPending.Text = "Normally, reservations are confirmed within hours.  This reservation was created " & CStr(hr) & " hours ago.  If you don't get an e-mail confirmation soon, please contact our Treasurer"
                Else
                    Me.lbPending.Text = "Normally, reservations are confirmed within hours.  This reservation was created " & CStr(hr) & " hours ago.  This appears to be excessive -- please contact our Treasurer"

                End If
            End Using
        End If
    End Sub
    Private Function DecodeMeal(ByVal cd As Integer) As String
        Dim i As Integer
        DecodeMeal = ""
        For i = 1 To 3
            If meals(i, 2) = cd Then
                DecodeMeal = meals(i, 1)
                Exit For
            End If
        Next

    End Function
    Private Sub SetUpMeals()
        Dim SQLQuery As String = ""
        Try
            SQLQuery = "SELECT EventDate,Speaker,meal1,meal2,meal3,meal1category,meal2category,meal3category from event where id = " & Session("EventID").ToString
        Catch ex As Exception
            Response.Redirect("default.aspx")
        End Try
        Using command As New SqlCommand(SQLQuery, sqlConnection)
            Dim Dr As SqlDataReader
            Dr = command.ExecuteReader()
            While Dr.Read()
                lbEventDate.Text = Dr("EventDate") & " " & Session("EventType")
                meals(1, 1) = Dr("meal1")
                meals(2, 1) = Dr("meal2")
                meals(3, 1) = Dr("meal3")
                meals(1, 2) = Dr("Meal1Category")
                meals(2, 2) = Dr("Meal2Category")
                meals(3, 2) = Dr("Meal3Category")
            End While
            Dr.Close()
        End Using
    End Sub

    Private Sub Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx")
    End Sub

End Class
