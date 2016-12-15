<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" 
    CodeFile="DPAppAdd.aspx.vb" Inherits="DPAppAdd" title="Decline Partner Processing" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
 
   <script type="text/javascript">
       function OnBlur(sender, args) {
           if (sender.get_value() == "") {
               sender.set_value(null);
           }
       }
  </script> 

          <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="false">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
          <table class="centered-cell" style="width: 100%" >
              <tr>
                <td class="centered-cell" style="padding: 1px; font-family: Arial, Helvetica, sans-serif; font-size: small; border-top-color: #A0A0A0; border-right-color: #C0C0C0; border-bottom-color: #C0C0C0; border-left-color: #A0A0A0; font-weight: bold; ">
                    ARF Decline Processing - ADD APPLICATION
                </td>
             </tr>
              <tr>
                <td>&nbsp;</td>
              </tr>
          </table>
        <table class="tbldetail"  style="font-family:arial; font-size:small;" >
             <tr>
                 <td >Application #:&nbsp;</td><td><asp:Label ID="sc_LoanNum" CssClass="fieldstyle" runat="server" Font-Italic="True">TBD</asp:Label></td><td></td>
                 <td>&nbsp;</td> <td>&nbsp;</td>
             </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>Merchant Name:&nbsp;</td><td><asp:textbox ID="sc_MerchantName" runat="server" Width="190px"></asp:textbox>
               </td>
                <td></td>
                <td>Phone:&nbsp;</td><td ><telerik:radmaskedtextbox ID="sc_Phone"  mask="(###) ###-####" runat="server" DisplayMask="(###) ###-####" LabelWidth="64px" Width="190px"></telerik:radmaskedtextbox></td>
            </tr>
            <tr>
                <td>Address Line:</td>
                <td><asp:textbox ID="sc_Address1"  runat="server" Width="190px"></asp:textbox>&nbsp;</td>
                <td></td>
                <td>Mobile:&nbsp;</td><td><telerik:radmaskedtextbox ID="sc_Mobile"  mask="(###) ###-####" runat="server" DisplayMask="(###) ###-####" Width="190px"></telerik:radmaskedtextbox></td>
            </tr>
            <tr>
                <td>City:</td>
                <td><asp:textbox ID="sc_City"  runat="server" Width="190px"></asp:textbox></td>
                <td></td><td>&nbsp;</td><td>&nbsp;</td>
            </tr>
            <tr>
                <td>State:</td>
                <td><asp:DropDownList ID="sc_State" runat="server" Width="190px">
                    <asp:ListItem Value="  ">*Select One*</asp:ListItem>
	<asp:ListItem Value="AL">Alabama</asp:ListItem>
	<asp:ListItem Value="AK">Alaska</asp:ListItem>
	<asp:ListItem Value="AZ">Arizona</asp:ListItem>
	<asp:ListItem Value="AR">Arkansas</asp:ListItem>
	<asp:ListItem Value="CA">California</asp:ListItem>
	<asp:ListItem Value="CO">Colorado</asp:ListItem>
	<asp:ListItem Value="CT">Connecticut</asp:ListItem>
	<asp:ListItem Value="DC">District of Columbia</asp:ListItem>
	<asp:ListItem Value="DE">Delaware</asp:ListItem>
	<asp:ListItem Value="FL">Florida</asp:ListItem>
	<asp:ListItem Value="GA">Georgia</asp:ListItem>
	<asp:ListItem Value="HI">Hawaii</asp:ListItem>
	<asp:ListItem Value="ID">Idaho</asp:ListItem>
	<asp:ListItem Value="IL">Illinois</asp:ListItem>
	<asp:ListItem Value="IN">Indiana</asp:ListItem>
	<asp:ListItem Value="IA">Iowa</asp:ListItem>
	<asp:ListItem Value="KS">Kansas</asp:ListItem>
	<asp:ListItem Value="KY">Kentucky</asp:ListItem>
	<asp:ListItem Value="LA">Louisiana</asp:ListItem>
	<asp:ListItem Value="ME">Maine</asp:ListItem>
	<asp:ListItem Value="MD">Maryland</asp:ListItem>
	<asp:ListItem Value="MA">Massachusetts</asp:ListItem>
	<asp:ListItem Value="MI">Michigan</asp:ListItem>
	<asp:ListItem Value="MN">Minnesota</asp:ListItem>
	<asp:ListItem Value="MS">Mississippi</asp:ListItem>
	<asp:ListItem Value="MO">Missouri</asp:ListItem>
	<asp:ListItem Value="MT">Montana</asp:ListItem>
	<asp:ListItem Value="NE">Nebraska</asp:ListItem>
	<asp:ListItem Value="NV">Nevada</asp:ListItem>
	<asp:ListItem Value="NH">New Hampshire</asp:ListItem>
	<asp:ListItem Value="NJ">New Jersey</asp:ListItem>
	<asp:ListItem Value="NM">New Mexico</asp:ListItem>
	<asp:ListItem Value="NY">New York</asp:ListItem>
	<asp:ListItem Value="NC">North Carolina</asp:ListItem>
	<asp:ListItem Value="ND">North Dakota</asp:ListItem>
	<asp:ListItem Value="OH">Ohio</asp:ListItem>
	<asp:ListItem Value="OK">Oklahoma</asp:ListItem>
	<asp:ListItem Value="OR">Oregon</asp:ListItem>
	<asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
	<asp:ListItem Value="RI">Rhode Island</asp:ListItem>
	<asp:ListItem Value="SC">South Carolina</asp:ListItem>
	<asp:ListItem Value="SD">South Dakota</asp:ListItem>
	<asp:ListItem Value="TN">Tennessee</asp:ListItem>
	<asp:ListItem Value="TX">Texas</asp:ListItem>
	<asp:ListItem Value="UT">Utah</asp:ListItem>
	<asp:ListItem Value="VT">Vermont</asp:ListItem>
	<asp:ListItem Value="VA">Virginia</asp:ListItem>
	<asp:ListItem Value="WA">Washington</asp:ListItem>
	<asp:ListItem Value="WV">West Virginia</asp:ListItem>
	<asp:ListItem Value="WI">Wisconsin</asp:ListItem>
	<asp:ListItem Value="WY">Wyoming</asp:ListItem>
</asp:DropDownList>

                </td>
                <td></td><td>Fax:&nbsp;</td><td><telerik:radmaskedtextbox ID="sc_Fax"  mask="(###) ###-####" runat="server" DisplayMask="(###) ###-####" Width="190px"></telerik:radmaskedtextbox></td>
            </tr>
            <tr>
                <td>Zip:</td>
                <td><telerik:radmaskedtextbox ID="sc_Zip" mask="#####-####" runat="server" DisplayMask="#####-####" Width="190px"></telerik:radmaskedtextbox></td>
                <td></td><td>&nbsp;</td><td>&nbsp;</td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>Guarantor:&nbsp;</td><td><asp:textbox ID="sc_Guarantor"  runat="server" Width="190px"></asp:textbox></td></tr>
            <tr><td>&nbsp;</td></tr>
            
            <tr>
                <td>Lead Source:&nbsp;</td><td><asp:DropDownList ID="sc_LeadSource" runat="server" Width="190px"></asp:DropDownList></td><td></td>
                <td>Sales Rep:</td><td><asp:DropDownList ID="sc_Rep" runat="server" Width="190px"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>&nbsp;</td><td>&nbsp;</td><td></td>
                <td>ISO:</td><td><asp:DropDownList ID="sc_ISO" runat="server" Width="190px"></asp:DropDownList></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>&nbsp;</td><td>&nbsp;</td><td></td>
                <td>Amt Requested:&nbsp;</td><td><telerik:RadNumericTextBox ID="sc_ReqAmt" Runat="server" Culture="en-US" Type="Currency" style="text-align:right" Width="190px">
                   <NumberFormat ZeroPattern="$n"></NumberFormat><ClientEvents OnBlur="OnBlur" /></telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td>Declined Date:&nbsp;</td><td><telerik:raddateinput ID="sc_DeclineDate"  runat="server" dateformat="d" emptymessage="mm/dd/yyyy" culture="en-US" Width="190px" MinDate="01/01/2013"></telerik:raddateinput></td><td></td>
                <td>ARF Decline Rsn:&nbsp;</td><td><asp:DropDownList ID="sc_ARFReasonList" runat="server" AutoPostBack="True" Width="190px">
                  </asp:DropDownList></td>
            </tr>
            <tr><td>
                &nbsp;</td><td>&nbsp;</td></tr>
            <tr>
                <td>App Status:&nbsp;</td><td><asp:dropdownlist ID="sc_StatusList" runat="server" Width="190px"></asp:dropdownlist></td><td></td>
                <td>Partner Submit Date:&nbsp;</td><td><asp:Label ID="sc_SubmitDate" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr>
               <td>Partner Name:&nbsp;</td><td><asp:dropdownlist ID="sc_PartnerNameList" runat="server" Width="190px"></asp:dropdownlist></td><td></td>
               <td>Partner Action Date:&nbsp;</td><td><asp:Label ID="sc_ActionDate" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
             <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr><td>
                &nbsp;
                </td></tr>
            <tr>
               <td>Funded Amt:&nbsp;</td><td>
                   <telerik:RadNumericTextBox ID="sc_FundAmt1" Runat="server" Culture="en-US" Type="Currency" style="text-align:right" Width="190px">
                   <NumberFormat ZeroPattern="$n"></NumberFormat><ClientEvents OnBlur="OnBlur" /></telerik:RadNumericTextBox>
               </td><td></td>
               <td>Partner Decline Rsn:</td><td>
                  <asp:DropDownList ID="sc_ReasonList" runat="server" AutoPostBack="True" Width="190px">
                  </asp:DropDownList>
                </td>
            </tr>
            <tr>
               <td>
                   ARF Commission Due:</td><td>
                   <telerik:RadNumericTextBox ID="sc_ARFCommDue1" Runat="server" Culture="en-US" Type="Currency" style="text-align:right" Width="190px">
                   <NumberFormat ZeroPattern="$n"></NumberFormat><ClientEvents OnBlur="OnBlur" /></telerik:RadNumericTextBox>
                </td><td></td><td>
                &nbsp;</td>
               <td>&nbsp;</td>
            </tr>
            <tr>
               <td>
                   Rep Commission Due:</td><td>
                    <telerik:RadNumericTextBox ID="sc_RepCommDue1" Runat="server" Culture="en-US" Type="Currency" style="text-align:right" Width="190px">
                   <NumberFormat ZeroPattern="$n"></NumberFormat><ClientEvents OnBlur="OnBlur" /></telerik:RadNumericTextBox>
                </td><td></td><td>Comm Rcvd Date:</td>
               <td><asp:Label ID="sc_CommRcvDate" CssClass="fieldstyle" runat="server"></asp:Label>
               </td>
            </tr>
            <tr><td>
                &nbsp;</td></tr>
            </table>
        <table class="tbldetail" style="font-family:arial; font-size:small;">
            <tr>
                <td>Notes:&nbsp;</td><td><asp:textbox ID="sc_Notes" runat="server" Height="84px" TextMode="MultiLine" Width="700px"></asp:textbox></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td></td><td class="centered-cell">Last Updated:&nbsp;<asp:Label ID="sc_UpDate" CssClass="fieldstyle" runat="server"></asp:Label>&nbsp;
                By:&nbsp;<asp:Label ID="sc_UpBy" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr><td></td></tr>
        </table>
           <p class="centered-cell">
            <asp:Button ID="AddButton" runat="server" Text="Add" Height="29px" Width="77px" />&nbsp;
            &nbsp;
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" Height="29px" Width="77px" />
           </p>

</asp:Content>