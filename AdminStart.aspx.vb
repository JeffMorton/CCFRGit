Imports System.Data.SqlClient
Public Class AdminStart
    Inherits PageBase
    ReadOnly conn As New SqlConnection
    'When you log into the Admin section (see Admin.aspx), you are brought to this page.  
    'This page contains some summary information and allows the user to select which event they want to work on.
    'All other admin section pagee, which involve events, process the event select on this page.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn.ConnectionString = GetConnectionString(False, False)
        conn.Open()
        Dim id As Integer
        Me.DateDDL.ConnectionString = conn.ConnectionString
        Me.SqlDataSource1.ConnectionString = conn.ConnectionString

        If Not IsPostBack Then
            Using cmd As New SqlCommand("FillPPTranslog", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.ExecuteNonQuery()
            End Using
            EventDateDDL.DataBind()
            Dim EvDate As Date = CDate("1/1/2019")
            Dim EVID As Integer = 1
            If Session("EventID") Is Nothing Then
                Dim strSQl As String = "select ID,eventdate from Event where EventDate >= @Now order by EventDate "
                Using cmd As New SqlCommand(strSQl, conn)
                    cmd.Parameters.AddWithValue("@Now", Now().ToShortDateString)
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        id = CInt(dr("ID"))
                        EvDate = CDate(dr("Eventdate"))
                        EVID = id
                    Else
                        dr.Close()
                        cmd.CommandText = "Select top 1 ID,eventdate from event order by eventdate desc"
                        dr = cmd.ExecuteReader
                        dr.Read()
                        id = CInt(dr("ID"))
                        EvDate = CDate(dr("Eventdate"))
                        EVID = id
                    End If
                    dr.Close()
                End Using
                Session("EventDate") = EvDate
                Session("EventID") = EVID
                EventDateDDL.Items.FindByValue(id.ToString()).Selected = True
            Else

                id = CInt(Session("EventID"))
                EventDateDDL.Items.FindByValue(id.ToString()).Selected = True
            End If

            Using cmd As New SqlCommand("select count(ID) from member where DontIncludeonReports='False'", conn)
                Me.TotalMembers.Text = cmd.ExecuteScalar.ToString
            End Using

        End If
        FillBadRegGrid()
        Session("Admin") = "Admin"
        EventDLL_Index_Changed("", e)
    End Sub
    Protected Sub UpdateProblemRegistrations()
        Using Cmd1 As New SqlCommand
            Cmd1.CommandType = CommandType.StoredProcedure
            Cmd1.Connection = conn
            Cmd1.CommandText = "CheckForMissingReservations"
            Cmd1.Parameters.AddWithValue("@EventID", Session("EventID"))
            Cmd1.ExecuteNonQuery()
        End Using
    End Sub
    Protected Sub FillBadRegGrid()
        UpdateProblemRegistrations()
        Using cmd0 As New SqlCommand("select mlfullname,Member.ID from member inner join droppedreservation on member.id=DroppedReservation.memberid where droppedreservation.Eventid=@Eventid order by lastname,firstname", conn)
            cmd0.Parameters.AddWithValue("@EventID", Session("EventID"))
            ERRGridview.DataSource = GridViewDataS(cmd0)
        End Using
        ERRGridview.DataBind()
    End Sub
    Protected Sub ComputePercentOnline()
        Dim Online As Single
        Dim MailIn As Single
        Using cmd As New SqlCommand("select count(ID) from ppTransLog where eventid = @EventID and Transtype='CCFR Reservations'", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            Online = CSng(cmd.ExecuteScalar)
        End Using
        Using cmd As New SqlCommand("select count(ID) from membersignup where eventid =@EventID and (memberattend = 'true'or spouseattend = 'true')", conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            MailIn = CSng(cmd.ExecuteScalar) - Online
        End Using
        If MailIn + Online > 0 Then
            Dim temp As Single = (Online / (MailIn + Online))
            POnLine.Text = temp.ToString("0%")
            temp = (MailIn / (MailIn + Online))
            PMailedIn.Text = temp.ToString("0%")
        Else
            PMailedIn.Text = ""
            POnLine.Text = ""
        End If
    End Sub
    Public Sub EventDLL_Index_Changed(sender As Object, e As EventArgs)
        ComputePercentOnline()
        Me.SqlDataSource1.SelectCommand = "SELECT [ID],[Cost],[EventDate], [Meal1], [Meal2], [Meal3], [Speaker], [SpeakerBio], [SpeechTitle],[type] FROM [Event] where [eventdate] >= '" & EventDateDDL.SelectedItem.Text & "' order by eventdate"

        Dim strsql As String = "Select COUNT(*) As [cnt] from MemberSignup where MemberAttend ='True' and EventID =@Eventid " _
             & "union all SELECT COUNT(*) as [cnt] from MemberSignup where SpouseAttend ='True' and EventID =@EventID " _
             & "union all SELECT Count(*) AS cnt FROM Event INNER JOIN GuestSignUp ON Event.ID = GuestSignUp.EventID " _
                & "WHERE Event.ID=@EventID"

        Dim v(2) As Integer
        Dim i As Integer = 0
        Using cmd As New SqlCommand(strsql, conn)
            cmd.Parameters.Add("@Eventid", SqlDbType.Int)
            cmd.Parameters("@Eventid").Value = EventDateDDL.SelectedValue
            Dim RDR As SqlDataReader = cmd.ExecuteReader()
            If RDR.HasRows Then
                Do While RDR.Read()
                    v(i) = CInt(RDR("cnt"))
                    i += 1
                Loop
            End If
            RDR.Close()
        End Using
        Me.Members.Text = CStr(v(0) + v(1))
        Me.Guests.Text = CStr(v(2))
        Me.ETotal.Text = CStr(v(0) + v(1) + v(2))
        FormView1.DataBind()
        Session("Eventdate") = CType(Me.FormView1.FindControl("EventDateLabel1"), Label).Text
        Session("EventID") = CType(Me.FormView1.FindControl("EventID"), Label).Text
        Session("MealCost") = CType(Me.FormView1.FindControl("MealCost"), Label).Text
        Session("EventType") = CType(Me.FormView1.FindControl("EType"), Label).Text
        FillBadRegGrid()
        FormView1.Visible = True
        Panel1.Visible = True
    End Sub
End Class
