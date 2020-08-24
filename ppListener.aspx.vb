Imports System.Net
Imports System.IO
Imports System.Data.SqlClient
'This page is called by  PayPal to return information about the transation.  This page only handles "Completed" transaction properly.  In the 9 years this has been in use,there has not be any other kind  of transactions.
Public Class PpListener
    Inherits PageBase

    Dim evID As Long = 0
    Dim MemID As Long = 0
    Dim txnid As String = "Not Received"
    Dim NoAttend As Long
    Dim TotalCost As Double = 0
    Dim Fee As Double
    Dim err As String = ""
    Dim DuesPaid As Double
    Dim MealsOwed As Double = 0

    ReadOnly sqlConnection As New SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("conn") = ""
        sqlConnection.ConnectionString = GetConnectionString(True, False)
        sqlConnection.Open()
        Dim uriLive As Uri = New Uri("https://www.paypal.com/cgi-bin/webscr")
        Dim req As HttpWebRequest = CType(WebRequest.Create(uriLive), HttpWebRequest)

        'Set values for the request back
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        Dim Param() As Byte = Request.BinaryRead(HttpContext.Current.Request.ContentLength)
        Dim strRequest As String = Encoding.ASCII.GetString(Param)

        strRequest += "&cmd=_notify-validate"
        req.ContentLength = strRequest.Length

        'Send the request to PayPal and get the response
        Dim streamOut As StreamWriter = New StreamWriter(req.GetRequestStream(), Encoding.ASCII)
        streamOut.Write(strRequest)
        streamOut.Close()
        Dim streamIn As StreamReader = New StreamReader(req.GetResponse().GetResponseStream())
        Dim strResponse As String = streamIn.ReadToEnd()
        streamIn.Close()
        strRequest = Server.UrlDecode(strRequest)
        Dim cmplete As String = ""
        Dim reusedTrn As Boolean = False

        'strResponse = "VERIFIED"
        'strRequest = "mc_gross=104.00&protection_eligibility=Eligible&address_status=confirmed&item_number1=&payer_id=AGVU3HP4HSXXS&address_street=2624 S Bennington Rd&payment_date=10:12:50 Jun 10, 2020 PDT&payment_status=Completed&charset=US-ASCII&address_zip=22901&first_name=Jeffrey&mc_fee=0.52&address_country_code=US&address_name=Jeffrey Morton&notify_version=3.9&custom=83A1215A0ATrueA-43.0000&payer_status=verified&business=ccfr1t@gmail.com&address_country=United States&num_cart_items=1&address_city=Charlottesville&verify_sign=AzmX1OI5lYLqA7iiXllpDssSGlogADQfnQPMi1KIYfESvVUE3oOP7sQY&payer_email=jb_morton@live.com&txn_id=2A975150S8324994A&payment_type=instant&last_name=Morton&item_name1=CCFR Dues&address_state=VA&receiver_email=ccfr1t@gmail.com&payment_fee=0.52&shipping_discount=0.00&quantity1=1&insurance_amount=0.00&receiver_id=LPJCYQKPUSG4L&txn_type=cart&discount=0.00&mc_gross_1=103.00&mc_currency=USD&residence_country=US&shipping_method=Default&transaction_subject=&payment_gross=103.00&ipn_track_id=ae49c8f7c68fd&cmd=_notify-validate"
        If strResponse = "VERIFIED" Then
            Dim ppInfor() As String = Split(strRequest, "&")
            Dim i As Integer
            For i = 0 To ppInfor.Length - 1
                Dim msg() As String = Split(ppInfor(i), "=")
                Select Case msg(0)
                    Case "txn_id"
                        txnid = msg(1)
                        Try
                            Dim sqlQuery1 As String = "select * from ppTranslog where TransNo=@trn and Status = 'Completed'"
                            Using cmd1 As New SqlCommand(sqlQuery1, sqlConnection)
                                cmd1.Parameters.Add("@trn", SqlDbType.NChar)
                                cmd1.Parameters("@trn").Value = txnid.ToCharArray
                                Dim Dr1 As SqlDataReader = cmd1.ExecuteReader
                                If Dr1.HasRows Then
                                    reusedTrn = True
                                    err += "Transaction Number Reused "
                                Else
                                    reusedTrn = False
                                End If
                                Dr1.Close()
                            End Using
                        Catch ex As Exception
                            err += "Error Searching for Transaction Number; "
                        End Try
                    Case "custom"
                        Try
                            MemID = CInt(DecodeID(msg(1)))
                            evID = CInt(DecodeID(msg(1)))
                            NoAttend = CInt(DecodeID(msg(1)))
                            'If NoAtttend is 0 then payment is dues only
                            If NoAttend = 0 Then evID = 0
                            If DecodeID(msg(1)) = "False" Then
                                DuesPaid = 0
                            Else
                                Using cmd As New SqlCommand("select DuesOwed from Member where id =@memID", sqlConnection)
                                    cmd.Parameters.AddWithValue("@memID", MemID)
                                    DuesPaid = CDbl(cmd.ExecuteScalar)
                                End Using
                            End If
                            MealsOwed = CDbl(msg(1))
                            Session("EventID") = evID
                            Session("UserID") = MemID
                        Catch ex As Exception
                            err += "Problem decoding 'custom' " & "-" & msg(1)
                        End Try
                    Case "payment_status"
                        cmplete = msg(1)
                    Case "payment_fee"
                        Fee = CDbl(msg(1))
                    Case "mc_gross_1"
                        TotalCost = CDbl(msg(1))
                    Case "receiver_email"
                        If msg(1) = "ccfr1t@gmail.com" Then
                        Else
                            err += "Receiver Email " & msg(1)
                        End If
                End Select
            Next
            If Not String.IsNullOrEmpty(err) Then
                RecordError("ppListener", "started", err, sqlConnection)
                SendMessagetoWebMaster(err & " IDs " & Session("UserID").ToString & Session("EventID").ToString)
            Else
                RecordError("ppListener", "started", "Decode Complete ", sqlConnection)
            End If

            Try
                Dim strSQL As String = "select tCheckAmount from tmpAccount where tEventID = @evid and tMemID = @memID"
                Using cmd As New SqlCommand(strSQL, sqlConnection)
                    cmd.Parameters.Add("@evid", SqlDbType.BigInt)
                    cmd.Parameters("@evid").Value = evID
                    cmd.Parameters.Add("@memID", SqlDbType.BigInt)
                    cmd.Parameters("@memID").Value = MemID
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        If Not CDbl(dr("tcheckAmount")) = TotalCost Then
                            RecordError("Problem checking payment amount", dr("tcheckAmount").ToString, CStr(TotalCost), sqlConnection)
                        End If
                    End If
                    dr.Close()
                End Using
            Catch ex As Exception
                RecordError(ex.Message, ex.ToString, "Problem checking payment amount", sqlConnection)
            End Try
            If reusedTrn = True Then
                RecordError("Transaction Number Reused", " ", " ", sqlConnection)
            End If
        ElseIf strResponse = "INVALID" Then
            RecordError("Invalid Transaction Posted on CCFRcville.org", " ", " ", sqlConnection)
        Else
            RecordError("strResponse = ", strResponse, " ", sqlConnection)
        End If
        Try
            Dim sqlQuery2 As String = "insert into pptransLog (Date,TransNo,Message,status,Response,AdMessage) values (@dat,@txn,@msg,@st,@res,@ad)"
            Using command As New SqlCommand(sqlQuery2, sqlConnection)
                command.Parameters.Add("@dat", SqlDbType.Date)
                command.Parameters("@dat").Value = Now.ToShortDateString
                command.Parameters.Add("@txn", SqlDbType.NChar)
                command.Parameters("@txn").Value = txnid
                command.Parameters.Add("@msg", SqlDbType.NVarChar)
                command.Parameters("@msg").Value = strRequest
                command.Parameters.Add("@st", SqlDbType.NChar)
                command.Parameters("@st").Value = cmplete
                command.Parameters.Add("@res", SqlDbType.NChar)
                command.Parameters("@res").Value = strResponse
                command.Parameters.Add("@ad", SqlDbType.NVarChar)
                command.Parameters("@ad").Value = err
                command.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "Failed to record pptranslog", sqlConnection)

        End Try
        RecordError("Transaction Status", cmplete, " ", sqlConnection)
        If cmplete = "Completed" Then

            If UpdateTables(evID, MemID, txnid) Then
                SendEMailConfirmation(Me, e)
            End If
        ElseIf cmplete = "Pending" Then
            Dim sqlQueryP As String = "UPDATE tmpAccount SET tmpAccount.tCheckNumber = @tchknum where tEventID = @evid and tMemID = @memID "
            Using cmdp As New SqlCommand(sqlQueryP, sqlConnection)
                cmdp.Parameters.Add("@evID", SqlDbType.Int)
                cmdp.Parameters("@evID").Value = evID
                cmdp.Parameters.Add("@memid", SqlDbType.Int)
                cmdp.Parameters("@memid").Value = MemID
                cmdp.Parameters.Add("@tchknum", SqlDbType.NChar)
                cmdp.Parameters("@tchknum").Value = txnid
                cmdp.ExecuteNonQuery()
            End Using
        ElseIf cmplete = "Reversed" Then
            Using cmd As New SqlCommand("ClearTempTables", sqlConnection)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@memID", MemID)
                cmd.Parameters.AddWithValue("@eviD", evID)
                cmd.ExecuteNonQuery()
            End Using

        End If


    End Sub
    Protected Shared Function DecodeID(ByRef coded As String) As String
        Dim j As Integer
        j = InStr(coded, "A")
        DecodeID = Mid(coded, 1, j - 1)
        Try
            coded = Mid(coded, j + 1)
        Catch
        End Try
    End Function

    Function UpdateTables(ByVal evid As Long, ByVal memid As Long, ByVal trnid As String) As Boolean
        Dim updateerror As String = ""
        RecordError("Update Tables Started", " ", " ", sqlConnection)
        If MealsOwed <> 0 Then
            Using cmd As New SqlCommand("Update Member set MealsOwed = MealsOwed - @MO where ID = @MemID", sqlConnection)
                cmd.Parameters.AddWithValue("@MemID", memid)
                cmd.Parameters.AddWithValue("@MO", MealsOwed)
                cmd.ExecuteNonQuery()
            End Using
        End If
        If evid <> 0 Then
            Using cmd As New SqlCommand("FinishReservation", sqlConnection)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@MemberID", memid)
                cmd.Parameters.AddWithValue("@EventiD", evid)
                cmd.ExecuteNonQuery()
            End Using
        End If

        Dim cnt As Integer

        RecordError("Update Account Table", "started", " ", sqlConnection)
        'Update Account
        Try
            Dim sqlQuery As String = "INSERT INTO Account ( tType,tCheckNumber, tCheckAmount, tPayee, tEventDate, tCategory, tcheckdate, tdateentered ) SELECT tmpAccount.tType, @tchkno, tmpAccount.tCheckAmount, tmpAccount.tPayee, tmpAccount.tEventDate, tmpAccount.tCategory, tmpAccount.tcheckdate, tmpAccount.tdateentered FROM tmpAccount where tmpAccount.tMemID = @memID and tmpAccount.tEventId= @evid "
            Using cmd2 As New SqlCommand(sqlQuery, sqlConnection)
                cmd2.Parameters.Add("@evID", SqlDbType.Int)
                cmd2.Parameters("@evID").Value = evid
                cmd2.Parameters.Add("@memid", SqlDbType.Int)
                cmd2.Parameters("@memid").Value = memid
                cmd2.Parameters.Add("@tchkno", SqlDbType.NChar)
                cmd2.Parameters("@tchkno").Value = trnid
                cmd2.Parameters.Add("@fee", SqlDbType.Money)
                cmd2.Parameters("@fee").Value = Fee
                cnt = cmd2.ExecuteNonQuery()
            End Using
            RecordError("Update Account Table", "Completed", "Records Inserted  " & cnt, sqlConnection)
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "Account table error", sqlConnection)
        End Try
        ' check for dues
        RecordError("Process Dues", "Started", " ", sqlConnection)
        Try
            Dim sqlQuery As String
            sqlQuery = "Select tDuesPayed from tmpAccount where tEventID = @evid and tMemID= @memID"
            Using cmd As New SqlCommand(sqlQuery, sqlConnection)
                cmd.Parameters.Add("@evID", SqlDbType.Int)
                cmd.Parameters("@evID").Value = evid
                cmd.Parameters.Add("@memid", SqlDbType.Int)
                cmd.Parameters("@memid").Value = memid
                Dim amt As Double = CDbl(cmd.ExecuteScalar)
                If amt > 0 Then
                    RecordError("Dues Payment Found", CStr(amt), "Amount", sqlConnection)
                    sqlQuery = "UPDATE Member SET Member.DuesOwed = 0 WHERE (((Member.ID)=@memID));"
                    Dim cmddues As New SqlCommand(sqlQuery, sqlConnection)
                    cmddues.Parameters.Add("@memid", SqlDbType.Int)
                    cmddues.Parameters("@memid").Value = memid
                    cmddues.ExecuteNonQuery()
                Else
                    RecordError("No Dues Payment Found", "0", "Amount", sqlConnection)
                End If
            End Using
        Catch ex As Exception
            RecordError(ex.Message, ex.ToString, "tmpAccount table error", sqlConnection)
            SendMessagetoWebMaster(err)
        End Try
        'clear temp tables
        Using cmd As New SqlCommand("ClearTempTables", sqlConnection)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@memID", memid)
            cmd.Parameters.AddWithValue("@eviD", evid)
            cmd.ExecuteNonQuery()
        End Using

        If updateerror = "" Then
            UpdateTables = True
        Else
            UpdateTables = False
            SendMessagetoWebMaster(updateerror)
        End If

    End Function
    Protected Shared Sub SendMessagetoWebMaster(ByVal merror As String)
        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr@2020"
        Dim MailSubject As String = "Update Error"


        Dim FromEmail As String '= SendResultsTo
        Dim msgBody As StringBuilder = New StringBuilder()

        FromEmail = "reservations@ccfrcville.org"

        msgBody.AppendLine(merror)

        Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        myMessage.To.Add("webmaster@ccfrcville.org")
        myMessage.To.Add("ccfrwm@gmail.com")
        myMessage.From = New System.Net.Mail.MailAddress(FromEmail)
        myMessage.Subject = MailSubject
        myMessage.Body = msgBody.ToString
        myMessage.IsBodyHtml = False

        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)
        Dim MailObj As New Mail.SmtpClient(smtpMailServer) With {
            .Credentials = basicAuthenticationInfo
        }
        MailObj.Send(myMessage)

    End Sub

    Protected Sub SendEMailConfirmation(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim smtpMailServer As String = "mail.ccfrcville.org"
        Dim smtpUsername As String = "webmaster@ccfrcville.org"
        Dim smtpPassword As String = "ccfr@2020"
        Dim StrMessage As String
        Dim CostPer As Double
        Dim EventDate As Date
        Using cmd As New SqlCommand("select Cost,EventDate from Event  where id =@evID", sqlConnection)
            cmd.Parameters.AddWithValue("@evID", evID)
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                CostPer = CDbl(dr("Cost"))
                EventDate = CDate(dr("EventDate"))
            End If
        End Using

        TotalCost = (NoAttend * CostPer + DuesPaid + MealsOwed)

        'myMessage.Bcc.Add("reservations@ccfrcville.org")
        Dim myMessage As New System.Net.Mail.MailMessage With {
            .From = New System.Net.Mail.MailAddress("reservations@ccfrcville.org")
        }

        myMessage.Bcc.Add("ccfrwm@gmail.com")
        myMessage.Bcc.Add("webmaster@ccfrcville.org")
        Dim sqlQuery0 As String = "SELECT FirstName, LastName, [E-Mail], ID FROM member WHERE   ID =@memid "
        Using cmd1 As New SqlCommand(sqlQuery0, sqlConnection)
            cmd1.Parameters.Add("@memid", SqlDbType.Int)
            cmd1.Parameters("@memid").Value = MemID
            Dim dr1 As SqlDataReader = cmd1.ExecuteReader
            dr1.Read()
            StrMessage = "Dear " & dr1("FirstName").ToString & " " & dr1("LastName").ToString & ","

            StrMessage += ("<br /> <br />")
            myMessage.To.Add(dr1("E-Mail").ToString)
            dr1.Close()
        End Using

        Try
            If NoAttend > 0 Then

                myMessage.Subject = "Reservation Confirmation"
                StrMessage += "Your reservation for the CCFR meeting on " & EventDate & " is Confirmed."
                If DuesPaid > 0 Then
                    StrMessage += "<br />Your CCFR dues payment of " & Format(DuesPaid, "c") & " is also confirmed.<br />"
                Else
                    StrMessage += "<br/>"
                End If
            ElseIf NoAttend = 0 And DuesPaid > 0 Then
                StrMessage += "Your CCFR dues payment of " & Format(DuesPaid, "c") & " is confirmed<br />"
                If MealsOwed > 0 Then
                    StrMessage += "This transaction included a charge of " & Format(MealsOwed, "c") & "<br />"

                ElseIf MealsOwed < 0 Then
                    StrMessage += "The credit on your account was reduced by " & Format(-MealsOwed, "c") & "<br />"

                Else
                End If
                StrMessage += "Your total payment of " & Format(TotalCost, "c") & " is confirmed<br />"


                    myMessage.Subject = "CCFR Dues Payment Confirmation"
                ElseIf NoAttend < 0 And DuesPaid = 0 Then
                    myMessage.Subject = "CCFR Reservation Change Confirmation"

                ElseIf DuesPaid > 0 Then
                    StrMessage += "Your CCFR dues payment of " & Format(DuesPaid, "c") & " is also confirmed.<br />"
                myMessage.Subject = "CCFR Reservation Change Confirmation"
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
                        StrMessage += "<tr ><th style='text-align: Left;font-weight:normal'>" & dr("mFullName").ToString & "</th> <th style='text-align: left;width:70px;font-weight:normal'>" & dr("gst").ToString & "</th><th style='text-align: left;width:150px;font-weight:normal'>" & dr("Meal").ToString & "</th></tr>"
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

        'If NoAttend > 0 Then
        '    If DuesPaid > 0 Then
        '        myMessage.Subject = "CCFR Reservation & Dues Payment Confirmation"
        '    Else
        '        myMessage.Subject = "CCFR Reservation Confirmation"
        '    End If
        'Else
        '    myMessage.Subject = "CCFR Dues Payment Confirmation"
        'End If
        myMessage.Body = StrMessage
        myMessage.IsBodyHtml = True



        Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)
        Dim MailObj As New System.Net.Mail.SmtpClient(smtpMailServer) With {
            .Credentials = basicAuthenticationInfo
        }
        MailObj.Send(myMessage)


    End Sub
    'Private Sub SendEMailReversed()
    '    Dim smtpMailServer As String = "mail.ccfrcville.org"
    '    Dim smtpUsername As String = "webmaster@ccfrcville.org"
    '    Dim smtpPassword As String = "ccfr@2020"
    '    Dim meals(3, 3) As Object

    '    Dim msgBody As StringBuilder = New StringBuilder()
    '    Dim myMessage As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()

    '    Dim sqlQuery0 As String = "SELECT FirstName, LastName, [E-Mail], ID FROM member WHERE   ID =@memid "
    '    Dim cmd1 As New SqlCommand(sqlQuery0, sqlConnection)
    '    cmd1.Parameters.Add("@memid", SqlDbType.Int)
    '    cmd1.Parameters("@memid").Value = MemID
    '    Dim dr1 As SqlDataReader = cmd1.ExecuteReader
    '    dr1.Read()
    '    Dim str0 As String = "Dear " & dr1("FirstName").ToString & " " & dr1("LastName").ToString & ","
    '    msgBody.AppendLine(str0)
    '    msgBody.AppendLine(" ")
    '    myMessage.To.Add(dr1("E-Mail").ToString)
    '    dr1.Close()

    '    Try
    '        Dim sqlQuery3 As String = "select Eventdate from Event where ID = @evid"
    '        Dim cmd3 As New SqlCommand(sqlQuery3, sqlConnection)
    '        cmd3.Parameters.Add("@evid", SqlDbType.Int)
    '        cmd3.Parameters("@evid").Value = evID

    '        Dim str As String = ""

    '        Dim strdat As String = cmd3.ExecuteScalar
    '        str = "PayPal did not accept you payment for the CCFR meeting on " & strdat & " so your reservation cannot be confirmed."

    '        msgBody.AppendLine(str)

    '    Catch ex As EvaluateException
    '    End Try

    '    msgBody.AppendLine("Please return to CCFRcville.org and try another form of payment. ")

    '    myMessage.Bcc.Add("webmaster@ccfrcville.org")
    '    myMessage.Bcc.Add("reservations@ccfrcville.org")
    '    myMessage.From = New System.Net.Mail.MailAddress("reservations@ccfrcville.org")
    '    myMessage.Subject = "CCFR Reservation Confirmation"
    '    myMessage.Body = msgBody.ToString
    '    myMessage.IsBodyHtml = False



    '    Dim basicAuthenticationInfo As New System.Net.NetworkCredential(smtpUsername, smtpPassword)
    '    Dim MailObj As New Mail.SmtpClient(smtpMailServer) With {
    '        .Credentials = basicAuthenticationInfo
    '    }
    '    MailObj.Send(myMessage)




    'End Sub

End Class
