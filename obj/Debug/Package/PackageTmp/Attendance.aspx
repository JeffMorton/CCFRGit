<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/member.Master" CodeBehind="Attendance.aspx.vb" Inherits="CCFRW19.Attendence" %>
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
   
  <div class="bp"> <asp:Button ID="PDF" runat="server" Text="PDF" position="absolute"/></div>
    
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="715px" Height="600px" ShowBackButton="False" ShowParameterPrompts="False" ShowPrintButton="False" style="margin-right: 9px" ShowCredentialPrompts="False" ShowPromptAreaButton="False" ShowRefreshButton="False" ShowWaitControlCancelLink="False" ShowZoomControl="False" ShowPageNavigationControls="False">
        <LocalReport ReportPath="Attendence.rdlc">
            
        </LocalReport>
    </rsweb:ReportViewer>
  
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="CCFRW19.dsattendenceGuestsTableAdapters."></asp:ObjectDataSource>
  
</asp:Content>
