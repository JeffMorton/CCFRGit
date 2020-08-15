Imports System.Net
Imports System.IO
Imports System.Data.SqlClient
Public Class UpdateRes
    Inherits PageBase
    ReadOnly MealsOwed As Double = CDbl(Session("MealsOwed"))
    ReadOnly evID As Integer = Session("EventID")
    ReadOnly memID As Integer = Session("UserID")
    ReadOnly TotalCost As Double = CDbl(Session("TotalCost"))
    ReadOnly NoAttend As Integer = CInt(Session("NoAttend"))
    ReadOnly DuesPaid As Double = CDbl(Session("DuesPaid"))
    ReadOnly Costper As Double = CDbl(Session("CostPer"))
    Dim sqlConnection As New SqlConnection()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Public Function UpdateTables(ByVal evid As Long, ByVal memid As Long, ByVal trnid As String) As Boolean
        Dim strConnect As String = GetConnectionString(True, True)
        sqlConnection = New SqlConnection(strConnect)
        sqlConnection.Open()




        Dim updateerror As String = ""
        Dim uerr As String = ""
        RecordError("Update Tables Started", " ", " ")
        If MealsOwed > 0 Then
            Using cmd As New SqlCommand("Update Member set MealsOwed = MealsOwed - @MO where ID = @MemID", Sqlconnection)
                cmd.Parameters.AddWithValue("@MemID", memid)
                cmd.Parameters.AddWithValue("@MO", MealsOwed)
                cmd.ExecuteNonQuery()
            End Using
        End If
        'Update membersignup table
        Try
            RecordError("Update Membersignup Table", "started", " ")
            Dim sqlQuery As String = "INSERT INTO MemberSignup ( MemberAttend, SpouseAttend, MemberMeal, SpouseMeal, NewMember, EventID, MemberID ) SELECT tmpMemberSignUp.MemberAttend, tmpMemberSignUp.SpouseAttend, tmpMemberSignUp.MemberMeal, tmpMemberSignUp.SpouseMeal, tmpMemberSignUp.NewMember, tmpMemberSignUp.EventID, tmpMemberSignUp.MemberID  FROM tmpMemberSignUp where tmpMemberSignUp.MemberID = @memID and tmpMemberSignUp.EventId= @evid"
            Dim cmd As New SqlCommand(sqlQuery, Sqlconnection)
            cmd.Parameters.Add("@evID", SqlDbType.Int)
            cmd.Parameters("@evID").Value = evid
            cmd.Parameters.Add("@memid", SqlDbType.Int)
            cmd.Parameters("@memid").Value = memid
            cmd.ExecuteNonQuery()
            RecordError("Update Membersignup Table", "Completed", " ")
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "Membersignup Table Update failed")
        End Try

        If Not ClearTmpMemberSignup(uerr) Then
            RecordError("Membersignup table", "Cleared Failed", " ")
        End If

        RecordError("Update Guestsignup Table", "started", " ")
        'Update GuestSignup
        Try
            Dim sqlQuery As String = "INSERT INTO GuestSignUp ( GuestName, GuestMeal, GLastName, GFirstName, EventID, MemberID, AddedToMealsOwed )SELECT tmpGuestSignUp.GuestName, tmpGuestSignUp.GuestMeal, tmpGuestSignUp.GLastName, tmpGuestSignUp.GFirstName, tmpGuestSignUp.EventID, tmpGuestSignUp.MemberID, tmpGuestSignUp.AddedToMealsOwed FROM tmpGuestSignUp  where MemberID = @memID and EventId= @evid "
            Dim cmd1 As New SqlCommand(sqlQuery, Sqlconnection)
            cmd1.Parameters.Add("@evID", SqlDbType.Int)
            cmd1.Parameters("@evID").Value = evid
            cmd1.Parameters.Add("@memid", SqlDbType.Int)
            cmd1.Parameters("@memid").Value = memid
            cmd1.ExecuteNonQuery()
            RecordError("Update Guestsignup Table", "Completed", " ")
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "Guest signup table error")
        End Try


        If Not ClearTmpGuestSignUp(uerr) Then
            RecordError("Guestsignup table", "Cleared Failed", " ")
        End If
        RecordError("Update Account Table", "started", " ")
        'Update Account
        Try
            Dim sqlQuery As String = "INSERT INTO Account ( tType,tCheckNumber, tCheckAmount, tPayee, tEventDate, tCategory, tcheckdate, tdateentered, tFee ) SELECT tmpAccount.tType, @tchkno, tmpAccount.tCheckAmount, tmpAccount.tPayee, tmpAccount.tEventDate, tmpAccount.tCategory, tmpAccount.tcheckdate, tmpAccount.tdateentered,  @fee FROM tmpAccount where tmpAccount.tMemID = @memID and tmpAccount.tEventId= @evid "
            Dim cmd2 As New SqlCommand(sqlQuery, Sqlconnection)
            cmd2.Parameters.Add("@evID", SqlDbType.Int)
            cmd2.Parameters("@evID").Value = evid
            cmd2.Parameters.Add("@memid", SqlDbType.Int)
            cmd2.Parameters("@memid").Value = memid
            cmd2.Parameters.Add("@tchkno", SqlDbType.NChar)
            cmd2.Parameters("@tchkno").Value = trnid
            cmd2.Parameters.Add("@fee", SqlDbType.Money)
            cmd2.Parameters("@fee").Value = 0
            cmd2.ExecuteNonQuery()
            RecordError("Update Account Table", "Completed", " ")
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "Account table error")
        End Try
        ' check for dues
        RecordError("Process Dues", "Started", " ")
        Try
            Dim sqlQuery As String
            sqlQuery = "Select tDuesPayed from tmpAccount where tEventID = @evid and tMemID= @memID"
            Dim cmd As New SqlCommand(sqlQuery, Sqlconnection)
            cmd.Parameters.Add("@evID", SqlDbType.Int)
            cmd.Parameters("@evID").Value = evid
            cmd.Parameters.Add("@memid", SqlDbType.Int)
            cmd.Parameters("@memid").Value = memid
            Dim amt As Double
            amt = CDbl(cmd.ExecuteScalar)
            If amt > 0 Then
                RecordError("Dues Found", amt, "Amount")
                sqlQuery = "UPDATE Member SET Member.DuesOwed = 0 WHERE (((Member.ID)=@memID));"
                Dim cmddues As New SqlCommand(sqlQuery, Sqlconnection)
                cmddues.Parameters.Add("@memid", SqlDbType.Int)
                cmddues.Parameters("@memid").Value = memid
                cmddues.ExecuteNonQuery()
            Else
                RecordError("No Dues Found", 0, "Amount")
            End If
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "tmpAccount table error")
        End Try

        If Not ClearTmpAccount(uerr) Then
            RecordError("Problem Clearing tmpaccount", uerr, " ")
        End If


        If updateerror = "" Then
            UpdateTables = True
        Else
            UpdateTables = False
            SendMessagetoWebMaster(updateerror)
        End If

    End Function
    Protected Sub SendMessagetoWebMaster(ByVal merror As String)
        'Dim SendResultsTo As String = "jbm@virginia.edu;jb_morton@live.com"
        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr2011"
        Dim MailSubject As String = "Update Error"


        Try
            Dim FromEmail As String '= SendResultsTo
            Dim msgBody As StringBuilder = New StringBuilder()

            FromEmail = "reservations@ccfrcville.org"

            msgBody.AppendLine(merror)


            Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
            myMessage.To.Add("webmaster@ccfrcville.org")
            myMessage.To.Add("Jb_morton@Live.com")
            myMessage.From = New System.Net.Mail.MailAddress(FromEmail)
            myMessage.Subject = MailSubject
            myMessage.Body = msgBody.ToString
            myMessage.IsBodyHtml = False



            Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)
            Dim MailObj As New Mail.SmtpClient(smtpMailServer)
            MailObj.Credentials = basicAuthenticationInfo
            MailObj.Send(myMessage)


        Catch

        End Try
    End Sub

    Protected Sub SendEMailConfirmation(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr2011"
        Dim meals(3, 3) As Object

        'set up meal matrix
        Dim SQLQuery1 As String = "SELECT meal1,meal2,meal3,meal1category,meal2category,meal3category from event where id = @evid"
        Dim command As New SqlCommand(SQLQuery1, sqlConnection)
        command.Parameters.Add("@evid", SqlDbType.Int)
        command.Parameters("@evid").Value = evID
        Dim Dr0 As SqlDataReader
        Dr0 = command.ExecuteReader()

        While Dr0.Read()

            meals(1, 1) = Dr0("meal1")
            meals(2, 1) = Dr0("meal2")
            meals(3, 1) = Dr0("meal3")
            meals(1, 2) = Dr0("Meal1Category")
            meals(2, 2) = Dr0("Meal2Category")
            meals(3, 2) = Dr0("Meal3Category")

        End While
        Dr0.Close()
        Dim msgBody As StringBuilder = New StringBuilder()
        Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        Dim str As String
        Dim sqlQuery0 As String = "SELECT FirstName, LastName, [E-Mail], ID FROM member WHERE   ID =@memid "
        Dim cmd1 As New SqlCommand(sqlQuery0, sqlConnection)
        cmd1.Parameters.Add("@memid", SqlDbType.Int)
        cmd1.Parameters("@memid").Value = MemID
        Dim dr1 As SqlDataReader = cmd1.ExecuteReader
        dr1.Read()
        Dim str0 As String = "Dear " & dr1("FirstName").ToString & " " & dr1("LastName").ToString & ","
        msgBody.AppendLine(str0)
        msgBody.AppendLine(" ")
        myMessage.To.Add(dr1("E-Mail").ToString)
        dr1.Close()


        Try
            Dim sqlQuery3 As String = "select Eventdate from Event where ID = @evid"
            Dim cmd3 As New SqlCommand(sqlQuery3, sqlConnection)
            cmd3.Parameters.Add("@evid", SqlDbType.Int)
            cmd3.Parameters("@evid").Value = evID


            If NoAttend > 0 Then
                Dim strdat As String = cmd3.ExecuteScalar
                str = "Your reservation for the CCFR meeting on " & strdat & " is Confirmed."
                msgBody.AppendLine(str)
                If DuesPaid > 0 Then
                    str = "Your CCFR dues payment of " & Format(DuesPaid, "c") & " is also confirmed"
                    msgBody.AppendLine(str)
                End If
                If DuesPaid > 0 Then
                    str = "Your CCFR dues payment of " & Format(DuesPaid, "c") & " is also confirmed"
                    msgBody.AppendLine(str)
                End If
                If MealsOwed > 0 Then
                    str = "An additional charge or credit of " & Format(MealsOwed, "c") & "is included in this transaction."
                    msgBody.AppendLine(str)
                End If
                If DuesPaid > 0 Then
                    myMessage.Subject = "CCFR Reservation and Dues Payment Confirmation"
                Else
                    myMessage.Subject = "CCFR Reservation Confirmation"
                End If
            Else
                str = "Your CCFR dues payment of " & Format(TotalCost, "c") & " is confirmed"
                msgBody.AppendLine(str)
                msgBody.AppendLine(" ")
                msgBody.AppendLine("Thank You.")
                myMessage.Subject = "CCFR Dues Payment Confirmation"
            End If
        Catch ex As EvaluateException
        End Try
        If NoAttend > 0 Then
            msgBody.AppendLine(" ")
            msgBody.AppendLine("NumberAttending:........" & CStr(NoAttend))
            msgBody.AppendLine("Cost per Person:...." & Format(CostPer, "c"))
            msgBody.AppendLine("Total Cost:.............." & Format(TotalCost, "c"))
            If DuesPaid > 0 Then
                msgBody.AppendLine(" ")
                msgBody.AppendLine("Dues:     ............." & Format(DuesPaid, "c"))
                msgBody.AppendLine("Grand Total:........" & Format(TotalCost + DuesPaid, "c"))
                msgBody.AppendLine(" ")
                msgBody.AppendLine("Thank You.")


            End If
        End If

        'myMessage.Bcc.Add("webmaster@ccfrcville.org")
        'myMessage.Bcc.Add("jb_morton@live.com")
        myMessage.Bcc.Add("reservations@ccfrcville.org")
        myMessage.From = New System.Net.Mail.MailAddress("reservations@ccfrcville.org")
        If NoAttend > 0 Then
            If DuesPaid > 0 Then
                myMessage.Subject = "CCFR Reservation & Dues Payment Confirmation"
            Else
                myMessage.Subject = "CCFR Reservation Confirmation"
            End If
        Else
            myMessage.Subject = "CCFR Dues Payment Confirmation"
        End If
        myMessage.Body = msgBody.ToString
        myMessage.IsBodyHtml = False



        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)
        Dim MailObj As New System.Net.Mail.SmtpClient(smtpMailServer)
        MailObj.Credentials = basicAuthenticationInfo
        MailObj.Send(myMessage)



    End Sub
    Private Sub SendEMailReversed()
        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr2011"
        Dim meals(3, 3) As Object

        Dim msgBody As StringBuilder = New StringBuilder()
        Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()

        Dim sqlQuery0 As String = "SELECT FirstName, LastName, [E-Mail], ID FROM member WHERE   ID =@memid "
        Dim cmd1 As New SqlCommand(sqlQuery0, sqlConnection)
        cmd1.Parameters.Add("@memid", SqlDbType.Int)
        cmd1.Parameters("@memid").Value = MemID
        Dim dr1 As SqlDataReader = cmd1.ExecuteReader
        dr1.Read()
        Dim str0 As String = "Dear " & dr1("FirstName").ToString & " " & dr1("LastName").ToString & ","
        msgBody.AppendLine(str0)
        msgBody.AppendLine(" ")
        myMessage.To.Add(dr1("E-Mail").ToString)
        dr1.Close()

        Try
            Dim sqlQuery3 As String = "select Eventdate from Event where ID = @evid"
            Dim cmd3 As New SqlCommand(sqlQuery3, sqlConnection)
            cmd3.Parameters.Add("@evid", SqlDbType.Int)
            cmd3.Parameters("@evid").Value = evID

            Dim str As String = ""

            Dim strdat As String = cmd3.ExecuteScalar
            str = "PayPal did not accept you payment for the CCFR meeting on " & strdat & " so your reservation cannot be confirmed."

            msgBody.AppendLine(str)

        Catch ex As EvaluateException
        End Try

        msgBody.AppendLine("Please return to CCFRcville.org and try another form of payment. ")

        myMessage.Bcc.Add("webmaster@ccfrcville.org")
        myMessage.Bcc.Add("reservations@ccfrcville.org")
        myMessage.From = New System.Net.Mail.MailAddress("reservations@ccfrcville.org")
        myMessage.Subject = "CCFR Reservation Confirmation"
        myMessage.Body = msgBody.ToString
        myMessage.IsBodyHtml = False



        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)
        Dim MailObj As New Mail.SmtpClient(smtpMailServer)
        MailObj.Credentials = basicAuthenticationInfo
        MailObj.Send(myMessage)


    End Sub
    Private Function ClearTmpGuestSignUp(ByRef uerr As String) As Boolean
        ' clear tmpGuestSignup
        Try
            Dim sqlQuery As String = "Delete from tmpGuestSignup where MemberID=@memid and EventID=@evid"
            Dim cmd As New SqlCommand(sqlQuery, SqlConnection)
            cmd.Parameters.Add("@evID", SqlDbType.Int)
            cmd.Parameters("@evID").Value = evID
            cmd.Parameters.Add("@memid", SqlDbType.Int)
            cmd.Parameters("@memid").Value = MemID
            cmd.ExecuteNonQuery()
            ClearTmpGuestSignUp = True

        Catch ex As exception
            uerr += " Error in clearing  tmpGuestsignup table - " & ex.Message & vbCrLf
            ClearTmpGuestSignUp = False

        End Try

    End Function

    Private Function ClearTmpMemberSignup(ByRef uerr As String) As Boolean
        'clear tmpMemberSignup
        Try
            Dim sqlQuery As String = "Delete from tmpMemberSignup where MemberID=@memid and EventID=@evid"
            Dim cmd As New SqlCommand(sqlQuery, SqlConnection)
            cmd.Parameters.Add("@evID", SqlDbType.Int)
            cmd.Parameters("@evID").Value = evID
            cmd.Parameters.Add("@memid", SqlDbType.Int)
            cmd.Parameters("@memid").Value = MemID
            cmd.ExecuteNonQuery()
            ClearTmpMemberSignup = True
        Catch ex As Exception
            uerr += " Error in clearing  tmpmembersignup table - " & ex.Message & vbCrLf
            ClearTmpMemberSignup = False
        End Try

    End Function
    Private Function ClearTmpAccount(ByRef uerr As String) As Boolean
        ' clear tmpAccount

        Try
            Dim sqlQuery As String = "Delete from tmpAccount where tMemID=@memid and tEventID=@evid "
            Dim cmd As New SqlCommand(sqlQuery, SqlConnection)
            cmd.Parameters.Add("@evID", SqlDbType.Int)
            cmd.Parameters("@evID").Value = evID
            cmd.Parameters.Add("@memid", SqlDbType.Int)
            cmd.Parameters("@memid").Value = MemID
            cmd.Parameters.Add("@Category", SqlDbType.NVarChar)

            cmd.ExecuteNonQuery()
            ClearTmpAccount = True
        Catch ex As Exception
            uerr = " Error in clearing  tmpAccount table - " & ex.Message & vbCrLf
            ClearTmpAccount = False
        End Try

    End Function
    Private Sub RecordError(message As String, errinner As String, loc As String)
        Dim strSQL As String = "insert into ErrorLog(Memberid,EventID,ErrorMessage,Date,Errorinner,loc) values (@memid,@evID,@msg,@dat,@errinner,@loc)"
        Dim cmd As New SqlCommand(strSQL, SqlConnection)
        cmd.Parameters.Add("@memid", SqlDbType.Int)
        cmd.Parameters("@memid").Value = MemID
        cmd.Parameters.Add("@dat", SqlDbType.Date)
        cmd.Parameters("@dat").Value = Now.ToLongDateString
        cmd.Parameters.Add("@evID", SqlDbType.Int)
        cmd.Parameters("@evID").Value = evID
        cmd.Parameters.Add("@msg", SqlDbType.NVarChar)
        cmd.Parameters("@msg").Value = message
        cmd.Parameters.Add("@loc", SqlDbType.NVarChar)
        cmd.Parameters("@loc").Value = loc
        cmd.Parameters.Add("@errinner", SqlDbType.NVarChar)
        cmd.Parameters("@errinner").Value = errinner
        cmd.ExecuteNonQuery()

    End Sub
End Class
