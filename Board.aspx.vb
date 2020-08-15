Public Class Leadership
    Inherits PageBase
    ' This page is part of the public section of the website.  This page is run when the Leadership tab on the public member is selected.
    'The information on this pageis mostly drawn from the memebr table.  The officers and Board are controlled by the member field,  Program and lucheon committeed are driven by specific fields.
    ' Membership and financial commiittee changes require chenges to this page and the rosterpd.aspx page.
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.SqlDataSource1.ConnectionString = GetConnectionStringM(True, False)
        Me.ProgramDataSource.connectionstring = SqlDataSource1.ConnectionString
        Me.LunchDataSource.ConnectionString = SqlDataSource1.ConnectionString
        Me.BoarddataSource.ConnectionString = SqlDataSource1.ConnectionString
        Me.Address.Text = "P.O.Box 4304 <br /> Charlottesville, Va 22901 <br /> Voice Mail:(434) 923-9185"
        Me.Administrator.Text = CStr(Info("Administrator"))
    End Sub

End Class
