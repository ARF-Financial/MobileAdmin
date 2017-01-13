<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" 
    CodeFile="DPAppDetail.aspx.vb" Inherits="DPAppDetail" title="Decline Partner Processing" %>
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
           <table class="centered-cell" style="width: 100%">
              <tr>
                <td class="centered-cell" style="padding: 1px; font-family: Arial, Helvetica, sans-serif; font-size: small; border-top-color: #A0A0A0; border-right-color: #C0C0C0; border-bottom-color: #C0C0C0; border-left-color: #A0A0A0; font-weight: bold; ">
                    ARF Decline Processing - MODIFY APPLICATION
                </td>
             </tr>
              <tr>
                <td>
                    <asp:Label ID="MsgLine" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small" ForeColor="Red"></asp:Label>
                  </td>
              </tr>
          </table>
        <table class="tbldetail"  style="font-family:arial; font-size:small;" >
             <tr>
                 <td >Application #:&nbsp;</td><td><asp:Label ID="sc_LoanNum" CssClass="fieldstyle" runat="server"></asp:Label></td><td></td>
                 <td>Contract #:&nbsp;</td> <td><asp:Label ID="sc_MerchantID" CssClass="fieldstyle" runat="server"></asp:Label></td>
             </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>Merchant:&nbsp;</td><td><asp:Label ID="sc_MerchantName" CssClass="fieldstyle" runat="server"></asp:Label>
               </td>
                <td></td>
                <td>Phone:&nbsp;</td><td ><asp:Label ID="sc_Phone" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Label ID="sc_Address1" CssClass="fieldstyle" runat="server"></asp:Label>&nbsp;</td>
                <td></td>
                <td>Mobile:&nbsp;</td><td><asp:Label ID="sc_Mobile" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Label ID="sc_City" CssClass="fieldstyle" runat="server"></asp:Label>&nbsp;<asp:Label ID="sc_State" CssClass="fieldstyle" runat="server"></asp:Label>&nbsp;
                    <asp:Label ID="sc_Zip" CssClass="fieldstyle" runat="server"></asp:Label>
                </td>
                <td></td>
                <td>Fax:&nbsp;</td><td><asp:Label ID="sc_Fax" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td>Guarantor:&nbsp;</td><td><asp:Label ID="sc_Guarantor" CssClass="fieldstyle" runat="server"></asp:Label></td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>Market:&nbsp;</td><td><asp:Label ID="sc_Market" CssClass="fieldstyle" runat="server"></asp:Label></td><td></td>
                <td>Sales Rep:&nbsp;</td><td><asp:Label ID="sc_Rep" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Lead Source:&nbsp;</td><td><asp:Label ID="sc_LeadSource" CssClass="fieldstyle" runat="server"></asp:Label></td><td></td>
                <td>ISO:&nbsp;</td><td><asp:Label ID="sc_ISO" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>Apply Date:&nbsp;</td><td><asp:Label ID="sc_ApplyDate" CssClass="fieldstyle" runat="server"></asp:Label></td><td></td>
                <td>Amt Requested:&nbsp;</td><td><asp:Label ID="sc_ReqAmt" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>Declined Date:&nbsp;</td><td><asp:Label ID="sc_DeclineDate" CssClass="fieldstyle" runat="server"></asp:Label></td><td></td>
                <td>ARF Decline Rsn:&nbsp;</td><td><asp:Label ID="sc_Reason" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr><td>
                <asp:Label ID="Orig_Partner" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="Orig_Status" runat="server" Visible="False"></asp:Label>
                </td><td>&nbsp;</td></tr>
            <tr>
                <td>App Status:&nbsp;</td><td><asp:dropdownlist ID="sc_StatusList" runat="server" Width="190px"></asp:dropdownlist></td><td></td>
                <td>Partner Submit Date:&nbsp;</td><td><asp:Label ID="sc_SubmitDate" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr>
               <td>Partner Name:&nbsp;</td><td><asp:dropdownlist ID="sc_PartnerNameList" runat="server" Width="190px"></asp:dropdownlist></td><td></td>
               <td>Partner Action Date:&nbsp;</td><td><asp:Label ID="sc_ActionDate" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
             <tr>
                <td>
                    <asp:Label ID="Orig_FundAmt" runat="server" Visible="False"></asp:Label>
                 </td><td>
                   <asp:Label ID="Orig_RepCommDue" runat="server" Visible="False"></asp:Label>
                 </td><td>
                 <td>
                <asp:Label ID="Orig_ARFCommDue" runat="server" Visible="False"></asp:Label>
                 </td>
            </tr>
            <tr><td>
                <asp:TextBox ID="Orig_Notes" runat="server" Height="16px" TextMode="MultiLine" Visible="False"></asp:TextBox>&nbsp;
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
                <asp:Label ID="Orig_Reason" runat="server" Visible="False"></asp:Label>
                </td>
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
                <td>Notes:&nbsp;</td><td><asp:textbox ID="sc_Notes" runat="server" Height="84px" TextMode="MultiLine" Width="629px"></asp:textbox></td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td></td><td class="centered-cell">Last Updated:&nbsp;<asp:Label ID="sc_UpDate" CssClass="fieldstyle" runat="server"></asp:Label>&nbsp;
                By:&nbsp;<asp:Label ID="sc_UpBy" CssClass="fieldstyle" runat="server"></asp:Label></td>
            </tr>
            <tr><td></td></tr>
        </table>
           <p class="centered-cell">
            <asp:Button ID="UpdateButton" runat="server" Text="Update" Height="29px" Width="77px" />&nbsp;
            <asp:Button ID="ResetButton" runat="server" Text="Reset" Height="29px" Width="77px" />&nbsp;
            <asp:Button ID="CancelButton" runat="server" Text="Cancel" Height="29px" Width="77px" />
           </p>

</asp:Content>