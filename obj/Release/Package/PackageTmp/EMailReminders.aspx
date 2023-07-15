   <%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="EMailReminders.aspx.vb" validateRequest="false" Inherits="CCFRW19.EMailReminders" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script src="https://cdn.ckeditor.com/4.14.1/standard/ckeditor.js"></script>
     <script>
         CKEDITOR.config.customConfig = 'ccfr_config.js';

   </script> 
     <script type = "text/javascript">
       function alertuser(msg) {
           alert(msg);
       }
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pPage"><div class="pHeader">Event Reminder Emails</div><br />
        <asp:Table ID="Table1" runat="server">
            <asp:TableRow>
                <asp:TableCell >
                    <asp:Label ID="Label1" runat="server" Text="Event ID:" width="100px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell >  <asp:TextBox ID="EventID" text= "" runat="server" />
                </asp:TableCell> </asp:TableRow>
           <asp:TableRow>
               <asp:TableCell>
                    <asp:Label ID="Label2" runat="server" Text="Event Date:" width="100px" cssclass="pStrongTextRight" />
               </asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="EventDate" runat="server"  width="250px"  />
                </asp:TableCell> </asp:TableRow>
 <asp:TableRow>
                <asp:TableCell  >
                    <asp:Label ID="Label3" runat="server" Text="Attachment Files:" width="100px" height="150px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  >
                    <asp:ListBox ID="ListAttachments" runat="server" Width="600px" Height =" 200px"></asp:ListBox> <asp:FileUpload ID="FileUpload1" allowMultiple="true" Width="440px" height="23px" runat="server" Font-Names="Verdana" Font-Size="11px" />           
                    <asp:Button ID="btnUpload" Text="Upload" runat="server" Height="23px" OnClick ="UploadMultipleFiles"  />
                     <asp:Button ID="Clear" Text="Clear" runat="server" Height="23px" OnClick ="ClearAttachments"  />
                </asp:TableCell>
     </asp:TableRow>
      <asp:TableRow>
                <asp:TableCell >
                    <asp:Label ID="Label4" runat="server"  Text="Additional Text:" width="100px" height="150px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  >  <asp:TextBox ID="AddText" height="150px" width="500px" textmode="MultiLine"  rows=10 runat ="server" />
                    <script type="text/javascript" lang="javascript">CKEDITOR.replace('<%=AddText.ClientID%>', { customConfig: '/ccfr_config.js' });</script> 
                </asp:TableCell> </asp:TableRow>      
       </asp:Table>
        <asp:Button ID="Button1" runat="server" Text="Send Reminders" OnClick="SendEmailReminders" />
        &nbsp;&nbsp;&nbsp;<asp:Label ID="Label5" runat="server" Text="EMails Sent: "></asp:Label><asp:TextBox ID="ESent" runat="server"></asp:TextBox>
    </div>
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <rsweb:ReportViewer ID="ReportViewer1" runat="server"  BackColor ="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226" Width="725px" Height="600px" ShowBackButton="False" ShowParameterPrompts="False" ShowPrintButton="False" style="margin-right: 9px" ShowCredentialPrompts="False" ShowPromptAreaButton="False" ShowRefreshButton="False" ShowWaitControlCancelLink="False" ShowZoomControl="False" ShowPageNavigationControls="False">
 <LocalReport ReportPath="Attendence.rdlc">
            
        </LocalReport>
    </rsweb:ReportViewer >
   
       
  
  
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="CCFRW19.dsattendenceGuestsTableAdapters."></asp:ObjectDataSource>
  
</asp:Content>
