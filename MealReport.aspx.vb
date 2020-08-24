Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class MealReport
    Inherits PageBase
    'The meal report is uesed to indicate the meal th meber selected when making the reservation
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strConnectionString As String = GetConnectionString(False, False)
        Session("conn") = strConnectionString
        Dim conn As New SqlConnection(strConnectionString)
        conn.Open()
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "MealReport.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.ShowExportControls = True
        Dim param As ReportParameter() = New ReportParameter(11) {}


        param(0) = New ReportParameter("DayTimePrinted", CStr(Now()))
        param(1) = New ReportParameter("MeetingDate", Session("EventDate").ToString())
        Dim c As Integer = GetScalarInt("Select count(Memberattend) as cnt from membersignup where eventid =  " & Session("EventID").ToString & " And memberattend = 'true'", conn)
        Dim c1 As Integer = GetScalarInt("Select count(Spouseattend) as cnt from membersignup where eventid = " & Session("EventID").ToString & "And spouseattend = 'true'", conn)
        param(2) = New ReportParameter("NumMembers", CStr(c + c1))
        param(3) = New ReportParameter("NumSpouse", CStr(c1))
        Dim c2 As Integer = GetScalarInt("Select count(id) as cnt from Guestsignup where eventid = " & Session("EventID").ToString, conn)
        param(4) = New ReportParameter("Guests", CStr(c2))
        param(5) = New ReportParameter("Total", CStr(c + c1 + c2))
        Dim dr As SqlDataReader
        Using cmd1 As New SqlCommand("Select sum(cnt)As cnt, meal from eventmealcount(@EventID) group by meal", conn)
            cmd1.Parameters.AddWithValue("@EventID", Session("EventID"))

            dr = cmd1.ExecuteReader
            Dim i As Integer = 0

            If dr.Read() Then
                param(6) = New ReportParameter("Meal1", dr("Meal").ToString)
                param(7) = New ReportParameter("Mealcnt1", dr("cnt").ToString)
            Else
                param(6) = New ReportParameter("Meal1", "")
                param(7) = New ReportParameter("Mealcnt1", "0")
            End If
            If dr.Read() Then

                param(8) = New ReportParameter("Meal2", dr("Meal").ToString)
                param(9) = New ReportParameter("Mealcnt2", dr("cnt").ToString)
            Else
                param(8) = New ReportParameter("Meal2", "N/A")
                param(9) = New ReportParameter("Mealcnt2", "0")
            End If

            If dr.Read() Then
                param(10) = New ReportParameter("Meal3", dr("Meal").ToString)
                param(11) = New ReportParameter("Mealcnt3", dr("cnt").ToString)
            Else
                param(10) = New ReportParameter("Meal3", "N/A")
                param(11) = New ReportParameter("Mealcnt3", "0")
            End If

        End Using
        dr.Close()
        Dim strsql As String = "Select Member.mlNickName As mFullName, ' ' AS gst, Member.LastName, Member.NickName, MemberSignup.MemberMeal, EventMeals.Place, MealCategory.MealCategory " _
                & "FROM ((MemberSignup INNER JOIN Member ON MemberSignup.MemberID = Member.ID) INNER JOIN EventMeals ON (MemberSignup.EventID = EventMeals.ID) AND (MemberSignup.MemberMeal = EventMeals.Meal1Category)) INNER JOIN MealCategory ON EventMeals.Meal1Category = MealCategory.ID " _
                & "WHERE (((MemberSignup.EventID)=@EventID) AND ((MemberSignup.MemberAttend)='True')) " _
                & "Union All " _
                & "SELECT Member.slNickName AS mFullName, ' ' AS gst, Member.SpouseLastName, Member.SpouseNickName, MemberSignup.SpouseMeal, EventMeals.Place, MealCategory.MealCategory " _
                & "FROM ((MemberSignup INNER JOIN Member ON MemberSignup.MemberID = Member.ID) INNER JOIN EventMeals ON (MemberSignup.EventID = EventMeals.ID) AND (MemberSignup.SpouseMeal = EventMeals.Meal1Category)) INNER JOIN MealCategory ON EventMeals.Meal1Category = MealCategory.ID " _
                & "WHERE (((MemberSignup.EventID)=@EventID) AND ((MemberSignup.SpouseAttend)='True')) " _
                & "Union All " _
                & "SELECT GuestSignUp.GLastName + ', ' + GFirstName AS mFullName, 'Guest' AS gst, GuestSignUp.GLastName, GuestSignUp.GFirstName , GuestSignUp.GuestMeal, EventMeals.Place, MealCategory.MealCategory " _
                & "FROM GuestSignUp INNER JOIN (EventMeals INNER JOIN MealCategory ON EventMeals.Meal1Category = MealCategory.ID) ON (GuestSignUp.GuestMeal = EventMeals.Meal1Category) AND (GuestSignUp.EventID = EventMeals.ID) " _
                & "WHERE (((GuestSignUp.EventID)=@EventID))order by LastName, nickname "
        Using cmd As New SqlCommand(strsql, conn)
            cmd.Parameters.AddWithValue("@EventID", Session("EventID"))
            ReportViewer1.LocalReport.DataSources.Add(ReportDataSC(cmd, "dsMealReport"))
        End Using
        ReportViewer1.LocalReport.SetParameters(param)
        ReportViewer1.LocalReport.Refresh()
        ReportViewer1.Visible = True


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class
