Imports System.Data.SqlClient

Public Class EventTable
    Inherits PageBase
    Dim conn As New SqlConnection
    'This page allows the administrator to create or edit events.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(False, False))
        conn.Open()
        DateDDL.ConnectionString = conn.ConnectionString
        dsEvent.ConnectionString = conn.ConnectionString
        Me.MealCat.ConnectionString = conn.ConnectionString
        Me.MealCat.SelectCommand = "select ID, MealCategory from  MealCategory"
        Me.Mealcount.ConnectionString = conn.ConnectionString
        Me.MainPanel.Visible = False
        Me.AddEventPanel.Visible = False
        fvEventTable.ChangeMode(FormViewMode.Edit)
        If Not IsPostBack Then
            EventDateDDL.DataBind()
            Dim strSQl As String = "select ID,eventdate from Event where EventDate >= @Now order by EventDate "
            Dim id As Integer

            Using cmd As New SqlCommand(strSQl, conn)
                cmd.Parameters.AddWithValue("@Now", Now().ToShortDateString)
                Dim dr As SqlDataReader = cmd.ExecuteReader
                If dr.HasRows Then
                    dr.Read()
                    id = CInt(dr("ID"))
                    Session("EventDate") = dr("Eventdate")
                    Session("EventD") = dr("ID")
                Else
                    dr.Close()
                    strSQl = "select top 1  id,eventdate from event order by eventdate desc"
                    cmd.CommandText = strSQl
                    dr = cmd.ExecuteReader
                    If dr.HasRows Then
                        dr.Read()
                        id = CInt(dr("ID"))
                        Session("EventDate") = dr("Eventdate")
                        Session("EventD") = dr("ID")
                    End If
                End If
                dr.Close()
            End Using
            EventDateDDL.Items.FindByValue(CStr(id)).Selected = True
            EventDLL_Index_Changed("", e)


            MainPanel.Visible = True

        End If
    End Sub
    Protected Sub EventDLL_Index_Changed(sender As Object, e As EventArgs)

        Dim dr As SqlDataReader
        Dim meals(3) As String
        Session("EID") = EventDateDDL.SelectedValue
        Session("EDate") = EventDateDDL.SelectedItem.Text
        Using cmd As New SqlCommand("select meal1,meal2,meal3,speakerbio from event where ID=@EID ", conn)
            cmd.Parameters.AddWithValue("@EID", Session("EID"))
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                dr.Read()
                meals(1) = IIf(IsDBNull(dr("Meal1")), " ", dr("Meal1")).ToString
                meals(2) = IIf(IsDBNull(dr("Meal2")), " ", dr("Meal2")).ToString
                meals(3) = IIf(IsDBNull(dr("Meal3")), " ", dr("Meal3")).ToString
                If dr("SpeakerBio") Is System.DBNull.Value Then
                    Me.SpeakerBio.Text = ""
                Else
                    Me.SpeakerBio.Text = dr("SpeakerBio").ToString
                End If
            End If
            dr.Close()
        End Using
        Session("Meal1") = 0
        Session("Meal2") = 0
        Session("Meal3") = 0
        Using cmd As New SqlCommand("Select sum(cnt) as cnt,meal from EventmealCount(@EID) group by meal", conn)
            cmd.Parameters.AddWithValue("@EID", Session("EID"))
            dr = cmd.ExecuteReader
            If dr.HasRows Then
                While dr.Read()
                    Select Case dr("Meal").ToString
                        Case meals(1)
                            Session("Meal1") = dr("cnt")
                        Case meals(2)
                            Session("Meal2") = dr("cnt")
                        Case meals(3)
                            Session("Meal3") = dr("cnt")
                    End Select
                End While
            End If
            dr.Close()
        End Using
        fvEventTable.DataBind()
        MainPanel.Visible = True
    End Sub

    Protected Sub SaveEvent(sender As Object, e As EventArgs) Handles Save.Click
        Dim meal1cat As Integer = CInt(CType(Me.fvEventTable.FindControl("Meal1Category"), DropDownList).Text)
        Dim meal2cat As Integer = CInt(CType(Me.fvEventTable.FindControl("Meal2Category"), DropDownList).Text)
        Dim meal3cat As Integer = CInt(CType(Me.fvEventTable.FindControl("Meal3Category"), DropDownList).Text)
        If Not ((meal1cat = meal2cat And meal1cat >= 11) Or (meal1cat = meal3cat And meal1cat >= 11) Or (meal2cat = meal3cat And meal2cat >= 11)) Then
            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('All meal categories must be different or TBA or No Meal')", True)
            MainPanel.Visible = True
            Exit Sub
        End If

        Dim SBString As String = Me.SpeakerBio.Text
        fvEventTable.UpdateItem(True)
        Using cmd As New SqlCommand("update Event set SpeakerBio = @SpeakerBio where id = @EventID", conn)
            cmd.Parameters.Add("@SpeakerBio", SqlDbType.NText)
            cmd.Parameters("@SpeakerBio").Value = SBString
            cmd.Parameters.AddWithValue("@EventID", Session("EID"))
            cmd.ExecuteNonQuery()
        End Using
        fvEventTable.ChangeMode(FormViewMode.Edit)
        MainPanel.Visible = True
        AddEventPanel.Visible = False
    End Sub
    Protected Sub Add(sender As Object, e As EventArgs) Handles AddEvent.Click
        Me.MainPanel.Visible = False
        Me.AddEventPanel.Visible = True
    End Sub
    Protected Sub CreateEvent(sender As Object, e As EventArgs) Handles CreateNewEvent.Click
        Dim ID As Integer
        Using cmd As New SqlCommand("CreateNewEvent", conn)
            cmd.Parameters.AddWithValue("@Eventdate", NewEventDate.Text)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@id", SqlDbType.Int)
            cmd.Parameters("@id").Direction = ParameterDirection.Output
            cmd.ExecuteNonQuery()
            ID = CInt(cmd.Parameters("@id").Value)
        End Using

        EventDateDDL.DataBind()
        EventDateDDL.Items.FindByValue(CStr(ID)).Selected = True
        EventDLL_Index_Changed("", e)
        Me.AddEventPanel.Visible = False
        Me.MainPanel.Visible = True

    End Sub
    Protected Sub OnConfirm(sender As Object, e As EventArgs)
        Dim confirmValue As String = Request.Form("confirm_value")
        If confirmValue = "Yes" Then
            Using cmd As New SqlCommand("Delete Event where ID = @EventID", conn)
                cmd.Parameters.AddWithValue("@EventID", CInt(Session("EID")))
                cmd.ExecuteNonQuery()
            End Using
            EventDateDDL.DataBind()
            Me.AddEventPanel.Visible = False
            Me.MainPanel.Visible = False
        Else
            ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('Delete Canceled)", True)
            'Me.AddEventPanel.Visible = False
            Me.MainPanel.Visible = True
        End If
    End Sub
    Protected Sub Redirect()
        Response.Redirect("https:\\ccfrcville.org?dt=" + Session("EDate").ToString)
    End Sub
    Protected Sub UpdateWebMenu(sender As Object, e As EventArgs) Handles NewEventDate.TextChanged
        Try
            Dim EDate As Date = CDate(Server.HtmlEncode(NewEventDate.Text))
            Dim MenuUpdateDate As Date
            Using cmd As New SqlCommand("Select MenuUpdateDate from information", conn)
                MenuUpdateDate = CDate(cmd.ExecuteScalar)
            End Using
            If Year(MenuUpdateDate) = Year(EDate) Then
                Exit Sub
            End If
            Dim mn As Integer = Month(EDate)
            If mn = 9 Then
                Using cmd As New SqlCommand("UpdateWebMenu", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.ExecuteNonQuery()
                End Using
            End If
        Catch
            Exit Sub
        End Try
    End Sub

End Class
