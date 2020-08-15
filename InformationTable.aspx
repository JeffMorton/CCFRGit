<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" validateRequest="false" CodeBehind="InformationTable.aspx.vb" Inherits="CCFRW19.InformationTable" %>
     <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <meta charset="utf-8">
  <meta name="robots" content="noindex, nofollow">
  <title></title>
     <script src="ckeditor/ckeditor.js"></script>

     
   <script type = "text/javascript">
       function alertuser(msg) {
           alert(msg);
       }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="InformationSource" runat="server"
        selectCommand="SELECT [ID]
      ,[FYStartDate]
      ,[FYEndDate]
      ,[NextCheckNumber]
      ,[Dues]
      ,[OrgName]
      ,[OrgAddress]
      ,[OrgPhone]
      ,[OrgEMail]
      ,[Website]
      ,[MealCost]
      ,[NextEventDate]
      ,[Treasurer]
      ,[TreasurerPhone]
      ,[EventID]
      ,[onlineText]
      ,[MenuUpdateDate]
      ,[PreviousMenuUpdateDate]
      ,[HalfDuesDate]
      ,[ZeroDuesDate]
      ,[bylaws]
      ,[MaxLunch]
      ,[MaxDinner]
        ,[LunchCost]
        ,[Administrator]
  FROM [dbo].[Information]"

        updatecommand="UPDATE [dbo].[Information]
   SET [FYStartDate] = @FYStartDate
      ,[FYEndDate] = @FYEndDate
      ,[Dues] = @Dues
      ,[OrgName] = @OrgName
      ,[OrgAddress] = @OrgAddress
      ,[OrgPhone] = @OrgPhone
      ,[OrgEMail] = @OrgEMail
      ,[Website] = @Website
      ,[MealCost] = @MealCost
      ,[Treasurer] = @Treasurer
      ,[TreasurerPhone] = @TreasurerPhone
      ,[onlineText] = @onlineText
      ,[MenuUpdateDate] = @MenuUpdateDate
      ,[PreviousMenuUpdateDate] = @PreviousMenuUpdateDate
      ,[HalfDuesDate] = @HalfDuesDate
      ,[ZeroDuesDate] = @ZeroDuesDate
      ,[MaxLunch] = @MaxLunch
      ,[MaxDinner] = @MaxDinner
      ,[LunchCost] =@LunchCost
        ,[Administrator]=@Administrator
 WHERE ID=1">

            <UpdateParameters>
                 <asp:SessionParameter SessionField="onlineText" Name="onlineText" />
                <asp:SessionParameter SessionField="ByLaws" Name="ByLaws" />
            </UpdateParameters> 

    </asp:SqlDataSource>
    <div class="pPage" style="width:700px;" >      <span class="pHeader">Information Table</span><br/><br/><br/>

    <asp:FormView ID="fvInformationTable" runat="server" DataSourceID="InformationSource">
        <edititemTemplate>
            <asp:Table ID="Table1" runat="server">
               
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label1"  style="text-align:Right" width="170px" runat="server" Text="Fiscal Year Start Date:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="FYStartDate" width="535px" Text='<%# Bind("FYStartDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow>                                   
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label3"  style="text-align:Right" width="170px" runat="server" Text="Fiscal Year End Date:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="FYEndDate" width="535px" Text='<%# Bind("FYEndDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 

                   <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label4"  style="text-align:Right" width="170px" runat="server" Text="Dues:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="Dues" width="535px" Text='<%# Bind("Dues", "{0:c}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                   <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label5"  style="text-align:Right" width="170px" runat="server" Text="Organization Name:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="OrgName" width="535px" Text='<%# Bind("OrgName") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                  <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label6"  style="text-align:Right" width="170px" runat="server" Text="Organization Address:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="OrgAddress" width="535px" Text='<%# Bind("OrgAddress") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                  <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label7"  style="text-align:Right" width="170px" runat="server" Text="Organization Telephone:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="OrgPhone" width="535px" Text='<%# Bind("OrgPhone") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                  <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label8"  style="text-align:Right" width="170px" runat="server" Text="Organization E-Mail:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="OrgEMail" width="535px" Text='<%# Bind("OrgEMail") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                  <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label9"  style="text-align:Right" width="170px" runat="server" Text="Web Site:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="Website" width="535px" Text='<%# Bind("Website") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                  <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label10"  style="text-align:Right" width="170px" runat="server" Text="Meal Cost:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="MealCost" width="535px" Text='<%# Bind("MealCost", "{0:c}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label20"  style="text-align:Right" width="170px" runat="server" Text="Lunch Cost:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="LunchCost" width="535px" Text='<%# Bind("LunchCost", "{0:c}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label11"  style="text-align:Right" width="170px" runat="server" Text="Treasurer:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="Treasurer" width="535px" Text='<%# Bind("Treasurer") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label12"  style="text-align:Right" width="170px" runat="server" Text="Treasurer Phone:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="TreasurerPhone" width="535px" Text='<%# Bind("TreasurerPhone") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                     <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label21"  style="text-align:Right" width="170px" runat="server" Text="Administrator:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="Administrator" width="535px" Text='<%# Bind("Administrator") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow>  
                <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label14"  style="text-align:Right" width="170px" runat="server" Text="Menu Update Date:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="MenuUpdateDate" width="535px" Text='<%# Bind("MenuUpdateDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label15"  style="text-align:Right" width="170px" runat="server" Text="Prior Menu Update date:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="PreviousMenuUpdateDate" width="535px" Text='<%# Bind("PreviousMenuUpdateDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label16"  style="text-align:Right" width="170px" runat="server" Text="Half Dues Date:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="HalfDuesDate" width="535px" Text='<%# Bind("HalfDuesDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label17"  style="text-align:Right" width="170px" runat="server" Text="Zero Dues date:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="ZeroDuesDate" width="535px" Text='<%# Bind("ZeroDuesDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label18"  style="text-align:Right" width="170px" runat="server" Text="Maximum Lunch:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="MaxLunch" width="535px" Text='<%# Bind("MaxLunch") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
                 <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label19"  style="text-align:Right" width="170px" runat="server" Text="Maximum Dinner:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:TextBox ID="MaxDinner" width="535px" Text='<%# Bind("MaxDinner") %>' runat="server"></asp:TextBox>
                        </asp:TableCell> 
                    </asp:TableRow> 
               </asp:Table><br />
        </edititemTemplate>
    </asp:FormView>
        <asp:Button ID="SaveI" runat="server" Text="Save" Height="30px"  Width="120px"/>
        <asp:Table ID="Table1" runat="server">
                      <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label13"  style="text-align:Right" width="100px" runat="server" Text="On Line Text:"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
   <asp:TextBox ID="onlineText" runat="server" TextMode="MultiLine" width="535px"></asp:TextBox>
<script type="text/javascript" lang="javascript">CKEDITOR.replace('<%=onlineText.ClientID%>', { customConfig: '/ccfr_config.js' });</script> 

                        </asp:TableCell> 
                    </asp:TableRow> 
                       <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label2"  style="text-align:Right" width="170px" runat="server" Text="ByLaws:" Visible="false"></asp:Label>
                        </asp:tablecell> <asp:TableCell>
                            <asp:textbox ID="ByLaws" width="535px"  height="300px"  runat="server" Visible="false"></asp:textbox>
                        </asp:TableCell> 
                    </asp:TableRow>
          
            </asp:Table>


    </div>

</asp:Content>
