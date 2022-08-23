Imports System.Data.SqlClient

Public Class Leadership
    Inherits PageBase
    ' This page is part of the public section of the website.  This page is run when the Leadership tab on the public member is selected.
    'The information on this page is mostly drawn from the memebr table.  The officers and Board are controlled by the member field,  Program and lucheon committeed are driven by specific fields.
    ' Membership and financial commiittee changes require chenges to this page and the RosterRP.aspx page.
    Dim conn As New SqlConnection()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        conn = New SqlConnection(GetConnectionString(True, False))
        conn.Open()


        Dim col1 As String = " <h5>Officers</h5>"
        col1 += CreateList("Officers", conn)

        col1 += "<br /><br /><h5>Membership Committee</h5>" & CreateList("Membership Committee", conn)
        col1 += "<br /><br /><h5>Financial Review Committee</h5>" & CreateList("Financial Review Committee", conn)
        col1 += "<br /><br /><h5>Administrator</h5>" & CStr(Info("Administrator"))
        FillListBox(col1, tb1)

        Dim col2 As String = "<h5>Directors</h5>" & CreateList("Directors", conn)
        FillListBox(col2, tb2)
        Dim col3 As String = "<h5>Program Committee</h5>" & CreateList("Program Committee", conn)
        col3 += "<br/><h5>Luncheon Committee</h5>" & CreateList("Lunch Committee", conn)
        FillListBox(col3, tb3)
        Me.Address.Text = "P.O.Box 4304 <br/> Charlottesville, Va 22901 <br/> Voice Mail:(434) 923-9185"
        'Me.Administrator.Text = CStr(Info("Administrator"))
    End Sub

    Protected Sub FillListBox(lst As String, lstb As Label)
        'Dim itm = lst.Replace("<br/>", vbCrLf)
        lstb.Text = lst
        'Dim itms() As String = Itm.Split(CChar(";"))


    End Sub

End Class
