<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" validateRequest="false" CodeBehind="EventTable.aspx.vb" Inherits="CCFRW19.EventTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
     <script src="ckeditor/ckeditor.js"></script>
<script type = "text/javascript">
    function Confirm() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Do you want to Delete Event?")) {
            confirm_value.value = "Yes";
        } else {
            confirm_value.value = "No";
        }
        document.forms[0].appendChild(confirm_value);
    }
     </script>
    <script type="text/javascript">
    function alertuser(msg) {
        alert(msg);
    }

    </script>
  
 
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="pPage" style="width:715px;" >      <span class="pHeader">Event Table</span><br/>
        <div style="margin-left:150px"  >   <strong>Select Event Date:</strong>    <asp:DropDownList ID="EventDateDDL" autopostback="true" runat="server" Height="27px" Width="110px" DataSourceID="DateDDL" OnSelectedIndexChanged="EventDLL_Index_Changed"
                                                                                       DataTextField="EDate"
                                                                                       DataValueField="ID">
            </asp:DropDownList></div> <br />
        <asp:SqlDataSource ID="DateDDL" runat="server" selectcommand="select CONVERT(VARCHAR(10), eventdate, 101) as EDate, Id from Event order by EventDate desc ">
        </asp:SqlDataSource>
             <asp:Panel id = MainPanel runat="server">
       <asp:FormView ID="fvEventTable" runat="server" DataSourceID="dsEvent">
            <edititemTemplate>
                <asp:Table ID="Table1" runat="server">

                    <asp:TableRow>
                        <asp:TableCell>

                            <asp:Label ID="Label2"  style="text-align:Right" width="100px" runat="server" Text="Event Date:"></asp:Label>
                            <asp:TextBox ID="EventDate" width="90px" Text='<%# Bind("EventDate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell>
                       <asp:TableCell>
                            <asp:Label ID="Label13"  style="text-align:Right" width="150px" runat="server" Text="Postponed To Date:"></asp:Label>
                            <asp:TextBox ID="AltDate" width="90px" Text='<%#Bind("altdate", "{0:d}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label3" style="text-align:Right" width="100px" runat="server" Text="Cost:"></asp:Label>
                            <asp:TextBox ID="Cost" width="90px" Text='<%# Bind("Cost", "{0:N2}") %>' runat="server"></asp:TextBox>
                        </asp:TableCell>
                         <asp:TableCell>
                            <asp:Label ID="Label12"  style="text-align:Right" width="150px" runat="server" Text="EventID:"></asp:Label>
                            <asp:TextBox ID="EventId" width="90px" Text='<%# Eval("ID") %>' runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label4" style="text-align:Right" width="100px" runat="server" Text="Type:"></asp:Label>
                            <asp:Dropdownlist ID="Type" AppendDataBoundItems="true" width="94px"  Text='<%# Bind("Type") %>' runat="server">
                                 <asp:listitem value="Dinner" text="Dinner"></asp:listitem>
                                <asp:listitem value="Lunch/Discussion" text="Lunch/Discussion"></asp:listitem>
                            </asp:Dropdownlist>
                        </asp:TableCell>
                    </asp:TableRow>                

                </asp:Table><br />
                <asp:Table ID="Table2" runat="server">
                    <asp:TableHeaderRow id="Table1HeaderRow" 
            
            runat="server">
            <asp:TableHeaderCell 
                Scope="Column" 
                Text="Meal" />
            <asp:TableHeaderCell  
                Scope="Column" 
                Text="Category" />
            <asp:TableHeaderCell 
                Scope="Column" 
                Text="Signed Up" />
        </asp:TableHeaderRow>   
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label5" style="text-align:Right" width="100px" runat="server" Text="Meal 1:"></asp:Label>
                            <asp:Textbox ID="Meal1" width="200px" Text='<%# Bind("Meal1") %>' runat="server"></asp:Textbox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Dropdownlist ID="Meal1Category" runat="server" Text='<%# Bind("Meal1Category") %>' 
                                    DataKeyNames="ID"
                                    DataSourceID="MealCat"
                                    DataValueField="ID"
                                AppendDataBoundItems="True"
                                    DataTextField="MealCategory">
                            </asp:Dropdownlist>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Textbox ID="Meal1SignedUp" width="50px" Text='<%# Session("Meal1") %>'  runat="server"></asp:Textbox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label7" style="text-align:Right" width="100px" runat="server" Text="Meal 2:"></asp:Label>
                            <asp:Textbox ID="Meal2" width="200px" Text='<%# Bind("Meal2") %>'  runat="server"></asp:Textbox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Dropdownlist ID="Meal2Category" runat="server" Text='<%# Bind("Meal2Category") %>'
                                    DataKeyNames="ID"
                                    DataSourceID="MealCat"
                                    DataValueField="ID"
                                    DataTextField="MealCategory"></asp:Dropdownlist>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Textbox ID="Meal2Signup" width="50px" Text='<%# Session("Meal2") %>' runat="server"></asp:Textbox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label6" style="text-align:Right" width="100px" runat="server" Text="Meal 3:"></asp:Label>
                            <asp:Textbox ID="Meal3" width="200px" Text='<%# Bind("Meal3") %>'  runat="server"></asp:Textbox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Dropdownlist ID="Meal3Category" runat="server" Text='<%# Bind("Meal3Category") %>'
                                    DataKeyNames="ID"
                                    DataSourceID="MealCat"
                                    DataValueField="ID"
                                    DataTextField="MealCategory"></asp:Dropdownlist>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Textbox ID="Meal3signup" width="50px" Text='<%# Session("Meal3") %>' runat="server"></asp:Textbox>
                        </asp:TableCell>
                    </asp:TableRow>
                    </asp:Table><br /><br />
                <asp:Table ID="Table3" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label8" style="text-align:Right" width="100px" runat="server" Text="Speaker:"></asp:Label>
                            <asp:Textbox ID="Speaker" Text='<%# Bind("Speaker") %>' textmode="MultiLine" Rows="2" width="600px"  runat="server"></asp:Textbox>
                        </asp:TableCell>
                    </asp:TableRow>
                   <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label9" style="text-align:Right" width="100px" runat="server" Text="Speech Title:"></asp:Label>
                            <asp:TextBox ID="SpeechTitle" Text='<%# Bind("SpeechTitle") %>' textmode="MultiLine" Rows="2" width="600px"  runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label10" style="text-align:Right" width="100px"  runat="server" Text="Short Bio:"></asp:Label>
                            <asp:TextBox ID="ShortSpeakerBio" Text='<%# Bind("ShortSpeakerBio") %>' textmode="MultiLine" Rows="2"  width="600px"  runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label11" style="text-align:Right" width="100px" runat="server" Text="Preface:"></asp:Label>
                            <asp:textbox ID="Preface" Text='<%# Bind("Preface") %>' textmode="MultiLine" Rows="2"  width="600px" runat="server"></asp:textbox>
                        </asp:TableCell>
                    </asp:TableRow>                
                      <asp:TableRow>
                        <asp:TableCell><br />
                          
                        </asp:TableCell>
                    </asp:TableRow>    
                </asp:Table>
   
</edititemTemplate>
        </asp:FormView>     

    <asp:Label ID="Label1" style="text-align:Right" width="100px" runat="server" Text="Speaker Bio:"></asp:Label>
 <asp:TextBox ID="SpeakerBio" runat="server" TextMode="MultiLine"></asp:TextBox>
<script type="text/javascript" lang="javascript">CKEDITOR.replace('<%=SpeakerBio.ClientID%>', { customConfig: '/ccfr_config.js' });</script> 
                 
        <asp:Button ID="Save" runat="server" Width="120px" Text="Save" />
          <asp:Button ID="AddEvent" runat="server" Width="120px" Text="Add Event" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <asp:Button ID="DeleteEvent"  runat="server" OnClick = "OnConfirm"  Width="120px" Text = "Delete Event" OnClientClick = "Confirm()"/>
          <asp:Button ID="WebPreview"  runat="server"  Width="120px" Text = "Web Preview" OnClick = "redirect"/>

</asp:Panel>
        <asp:Panel ID="AddEventPanel" runat="server">
            <asp:Label ID="l1" Text="Enter New Event Date" runat="server" style="text-align:Right" />
            <asp:TextBox ID="NewEventDate" runat="server"></asp:TextBox>
            <asp:Button ID="CreateNewEvent" runat="server" Text="Continue" /> 

        </asp:Panel>

     <asp:SqlDataSource ID="Mealcount" runat="server" 
         selectcommand = "sum(cnt) as cnt,meal from EventmealCount(@ID) group by meal">
          <SelectParameters>
            <asp:sessionparameter  SessionField="EID" Name=EID />
        </SelectParameters> 
     </asp:SqlDataSource><br />
      
    <asp:SqlDataSource ID="MealCat" runat="server"> </asp:SqlDataSource><br />
        <asp:SqlDataSource ID="dsEvent" runat="server"
            selectcommand="SELECT [ID],[EventDate]
      ,[Cost]
      ,[Type]
      ,[Meal1]
      ,[Meal2]
      ,[Meal3]
      ,[Meal3Category]
      ,[Meal1Category]
      ,[Meal2Category]
      ,[Speaker]
      ,[SpeakerBio]
      ,[SpeechTitle]
      ,[upsize_ts]
      ,[SaveTheDate]
      ,[ShortSpeakerBio]
      ,[Preface]
      ,[Eventdate] as altdate
  FROM [dbo].[Event] where id = @EID"
            Updatecommand="UPDATE [dbo].[Event]
   SET [EventDate] = @EventDate
      ,[Cost] = @cost
      ,[Type] =@Type
      ,[Meal1] =@Meal1
      ,[Meal2] =@Meal2
      ,[Meal3] = @Meal3
      ,[Meal3Category] = @Meal3Category
      ,[Meal1Category] =  @Meal1Category
      ,[Meal2Category] =  @Meal2Category
      ,[Speaker] = @Speaker
      ,[SpeakerBio] = @SpeakerBio
      ,[SpeechTitle] = @SpeechTitle
      ,[ShortSpeakerBio] = @ShortSpeakerBio
      ,[Preface] = @Preface
      ,[altdate] = @altdate
 WHERE  id = @EID">
             <SelectParameters>
                 <asp:SessionParameter SessionField="EID" Name="EID" />
             </SelectParameters> 
            <UpdateParameters>
                <asp:SessionParameter SessionField="EID" Name="EID" />
                 <asp:SessionParameter SessionField="SpeakerBioText" Name="SpeakerBio" />
            </UpdateParameters> 

        </asp:SqlDataSource>















    </div>
</asp:Content>
