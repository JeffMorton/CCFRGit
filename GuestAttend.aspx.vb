Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms
Public Class GuestAttend
    Inherits PageBase

    Protected Sub Page_INIT(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim strConnectionString As String = GetConnectionString(False, False)

        Session("conn") = strConnectionString
        ReportViewer1.Visible = True
        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = "AGuest.rdlc"
        ReportViewer1.LocalReport.DataSources.Clear()

        ReportViewer1.ShowExportControls = True
        Me.PDF.Visible = False


        Dim conn As New SqlConnection(strConnectionString)
        conn.Open()

        Dim strSQL As String = "select * from AttendGuest(" & Session("EventID").ToString & ") order by GLastname,GFirstname"
        ReportViewer1.LocalReport.DataSources.Add(ReportDataS(strSQL, "DSAttendGuest", conn))

        Dim param As ReportParameter() = New ReportParameter(5) {}
        param(0) = New ReportParameter("DayTimePrinted", CStr(Now()))
        Dim c1 As Integer = GetScalarInt("Select count(Spouseattend) as cnt from membersignup where eventid = " & Session("EventID").ToString & "And spouseattend = 'true'", conn)
        param(3) = New ReportParameter("NumSpouse", CStr(c1))
        param(1) = New ReportParameter("MeetingDate", Session("EventDate").ToString())
        Dim c As Integer = GetScalarInt("Select count(Memberattend) as cnt from membersignup where eventid =  " & Session("EventID").ToString & " And memberattend = 'true'", conn)
        param(2) = New ReportParameter("NumMembers", CStr(c + c1))
        Dim c2 As Integer = GetScalarInt("Select count(id) as cnt from Guestsignup where eventid = " & Session("EventID").ToString, conn)
        param(4) = New ReportParameter("Guests", CStr(c2))
        param(5) = New ReportParameter("Total", CStr(c + c1 + c2))
        ReportViewer1.LocalReport.SetParameters(param)

        ReportViewer1.LocalReport.Refresh()
    End Sub


End Class