<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="NewMemberReport.aspx.vb" Inherits="CCFRW19.NewMemberReport" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script>
        function alertuser(msg) {
            alert(msg);
        }

 </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
  <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <div class="pPage" style="width:610px;" >      
     <span class="pHeader">New Member Report</span><br/><br/><br/>
    <asp:Label ID="Label2"  style="text-align:Right" width="100px" runat="server" Text="Joined After:"></asp:Label>
                            <asp:TextBox ID="NewDate" width="90px"  Text='' runat="server" ></asp:TextBox>
            <asp:Button ID="Go" runat="server" Text="Go" />  </div>
    <rsweb:ReportViewer
        ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="715px" Height="595px" ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowParameterPrompts="False" ShowPrintButton="False" ShowBackButton="False" Style="margin-right: 2px" ShowZoomControl="False" ShowRefreshButton="False" DocumentMapCollapsed="True" ShowPromptAreaButton="False" ShowWaitControlCancelLink="False" ShowPageNavigationControls="False">
    </rsweb:ReportViewer>
</asp:Content>