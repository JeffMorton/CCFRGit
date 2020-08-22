Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class EventCard
    Inherits PageBase
    'Event cards are used to tell the server at CCFR dinners, what the member or guest ordered. This cards are printed out a day or two before the dinner.
    'The member places this card on the their place on their table for the server to see what meal they selected.
    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim strConnectionString As String = GetConnectionString(False, False)
        Session("conn") = strConnectionString
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "EventCard.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()

        ReportViewer1.ShowExportControls = True


        Dim conn As New SqlConnection(strConnectionString)
        conn.Open()
        Dim strsql As String = "SELECT Member.mlNickName AS mFullName, ' ' AS gst, Member.LastName, Member.NickName, MemberSignup.MemberMeal, EventMeals.Place, MealCategory.MealCategory " _
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
            ReportViewer1.LocalReport.DataSources.Add(ReportDataSC(cmd, "dsEventCard"))
        End Using

        ReportViewer1.LocalReport.Refresh()
    End Sub

End Class
