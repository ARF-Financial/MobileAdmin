Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Configuration
Imports System.Text.RegularExpressions

Public Class DPAppAdd
    Inherits System.Web.UI.Page
    '
    ' DPAppAdd - ASR 03/16/2015
    ' Add loan application that was declined by ARF for potential processing by a Decline Partner.
    '
    '
    Dim BColor As System.Drawing.Color
    Dim Err As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BColor = sc_PartnerNameList.BorderColor
            sc_DeclineDate.MaxDate = Now
            FillPartnerList()
            FillStatusList()
            FillReasonList()
            FillLeadSourceList()
            FillSalesRepList()
            FillISOList()
        End If
    End Sub

    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.Redirect("DPAppList.aspx")
    End Sub

    Protected Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        ValidateData()
        If Not Err Then
            AddData()
        End If
    End Sub

    Protected Sub ValidateData()

        Err = False
        sc_MerchantName.BorderColor = BColor
        sc_Address1.BorderColor = BColor
        sc_City.BorderColor = BColor
        sc_State.BorderColor = BColor
        sc_Zip.BorderColor = BColor
        sc_Phone.BorderColor = BColor
        sc_Mobile.BorderColor = BColor
        sc_Fax.BorderColor = BColor
        sc_Guarantor.BorderColor = BColor
        sc_LeadSource.BorderColor = BColor
        sc_Rep.BorderColor = BColor
        sc_ISO.BorderColor = BColor
        sc_DeclineDate.BorderColor = BColor
        sc_ReqAmt.BorderColor = BColor
        sc_ARFReasonList.BorderColor = BColor
        sc_StatusList.BorderColor = BColor
        sc_PartnerNameList.BorderColor = BColor
        sc_FundAmt1.BorderColor = BColor
        sc_ARFCommDue1.BorderColor = BColor
        sc_RepCommDue1.BorderColor = BColor

        If sc_MerchantName.Text = "" Then
            sc_MerchantName.Focus()
            sc_MerchantName.BorderColor = Drawing.Color.Red
            Err = True
            RadWindowManager1.RadAlert("Merchant Name is required", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_Guarantor.Text = "" Then
            sc_Guarantor.Focus()
            sc_Guarantor.BorderColor = Drawing.Color.Red
            Err = True
            RadWindowManager1.RadAlert("Guarantor Name is required", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_LeadSource.SelectedItem.Text = "*Select One*" Then
            sc_LeadSource.Focus()
            sc_LeadSource.BorderColor = Drawing.Color.Red
            Err = True
            RadWindowManager1.RadAlert("Must select a Lead Source", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_LeadSource.SelectedItem.Text = "ISO" Then
            If sc_Rep.SelectedItem.Text <> "Staff, Admin." Then
                sc_Rep.Focus()
                sc_Rep.BorderColor = Drawing.Color.Red
                Err = True
                RadWindowManager1.RadAlert("Rep must be ""Staff, Admin."" for ISO Lead Source", 350, 130, "Validation Error", "")
                GoTo out
            End If
            If sc_ISO.SelectedItem.Text = "*Select One*" Then
                sc_ISO.Focus()
                sc_ISO.BorderColor = Drawing.Color.Red
                Err = True
                RadWindowManager1.RadAlert("Must select ISO with ISO Lead Source", 350, 130, "Validation Error", "")
                GoTo out
            End If
        End If

        If Not (sc_Rep.SelectedItem.Text = "*Select One*" And sc_ISO.SelectedItem.Text = "*Select One*") Then
            If sc_Rep.SelectedItem.Text = "Staff, Admin." Then
                If sc_ISO.SelectedItem.Text = "*Select One*" Then
                    sc_ISO.Focus()
                    sc_ISO.BorderColor = Drawing.Color.Red
                    Err = True
                    RadWindowManager1.RadAlert("Must select ISO with Rep ""Staff, Admin.""", 350, 130, "Validation Error", "")
                    GoTo out
                End If
            Else
                If sc_ISO.SelectedItem.Text <> "*Select One*" Then
                    sc_ISO.Focus()
                    sc_ISO.BorderColor = Drawing.Color.Red
                    Err = True
                    RadWindowManager1.RadAlert("Cannot select ISO unless Rep is ""Staff, Admin.""", 350, 130, "Validation Error", "")
                    GoTo out
                End If
            End If
        End If

        If Not sc_ReqAmt.Value.HasValue Or sc_ReqAmt.Value = 0 Then
            sc_ReqAmt.Focus()
            sc_ReqAmt.BorderColor = Drawing.Color.Red
            Err = True
            RadWindowManager1.RadAlert("Must enter Amount Requested", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_DeclineDate.IsEmpty Then
            sc_DeclineDate.Focus()
            sc_DeclineDate.BorderColor = Drawing.Color.Red
            Err = True
            RadWindowManager1.RadAlert("Must enter ARF Decline Date", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_ARFReasonList.SelectedItem.Text = "*Select One*" Then
            sc_ARFReasonList.Focus()
            sc_ARFReasonList.BorderColor = Drawing.Color.Red
            Err = True
            RadWindowManager1.RadAlert("Must select an ARF Decline Reason", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_RepCommDue1.Value.HasValue And sc_RepCommDue1.Value > 0 And sc_Rep.SelectedItem.Text = "*Select One*" Then
            sc_RepCommDue1.Focus()
            sc_RepCommDue1.BorderColor = Drawing.Color.Red
            RadWindowManager1.RadAlert("Cannot enter Rep Commission without selecting a Rep", 350, 130, "Validation Error", "")
            Err = True
            GoTo out
        End If

        If sc_StatusList.SelectedItem.Text = "New" Or sc_StatusList.SelectedItem.Text = "Awaiting Rep Approval" Or
           sc_StatusList.SelectedItem.Text = "Removed" Or sc_StatusList.SelectedItem.Text = "Back to ARF" Then
            If sc_PartnerNameList.SelectedIndex <> 0 Then
                sc_PartnerNameList.Focus()
                sc_PartnerNameList.BorderColor = Drawing.Color.Red
                Err = True
                RadWindowManager1.RadAlert("Cannot select Partner with this App Status", 350, 130, "Validation Error", "")
                GoTo out
            End If
        Else
            If sc_PartnerNameList.SelectedItem.Text = "*Select One*" Then
                sc_PartnerNameList.Focus()
                sc_PartnerNameList.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must select a Partner with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        End If

        If sc_StatusList.SelectedItem.Text = "New" Or sc_StatusList.SelectedItem.Text = "Awaiting Rep Approval" Or sc_StatusList.SelectedItem.Text = "Removed" Or
            sc_StatusList.SelectedItem.Text = "In Process" Or sc_StatusList.SelectedItem.Text = "Declined" Then
            If sc_FundAmt1.Value.HasValue And sc_FundAmt1.Value <> 0 Then
                sc_FundAmt1.Focus()
                sc_FundAmt1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Cannot provide Funded Amt with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            ElseIf sc_ARFCommDue1.Value.HasValue And sc_ARFCommDue1.Value <> 0 Then
                sc_ARFCommDue1.Focus()
                sc_ARFCommDue1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Cannot provide ARF Commission Due with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            ElseIf sc_RepCommDue1.Value.HasValue And sc_RepCommDue1.Value <> 0 Then
                sc_RepCommDue1.Focus()
                sc_RepCommDue1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Cannot provide Rep Commission Due with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        ElseIf sc_StatusList.SelectedItem.Text = "Funded - Awaiting Comm" Then
            If Not sc_FundAmt1.Value.HasValue Or sc_FundAmt1.Value = 0 Then
                sc_FundAmt1.Focus()
                sc_FundAmt1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must provide Funded Amt with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        ElseIf sc_StatusList.SelectedItem.Text = "Funded - Received Comm" Then
            If Not sc_FundAmt1.Value.HasValue Or sc_FundAmt1.Value = 0 Then
                sc_FundAmt1.Focus()
                sc_FundAmt1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must provide Funded Amt with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            ElseIf Not sc_ARFCommDue1.Value.HasValue Or sc_ARFCommDue1.Value = 0 Then
                sc_ARFCommDue1.Focus()
                sc_ARFCommDue1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must provide ARF Commission Due with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            ElseIf Not (sc_RepCommDue1.Value.HasValue Or sc_RepCommDue1.Value = 0) Then
                sc_RepCommDue1.Focus()
                sc_RepCommDue1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must provide Rep Commission Due or ""0"" with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        End If

        If sc_StatusList.SelectedItem.Text = "Declined" Then
            If sc_ReasonList.SelectedValue = "*Select One*" Then
                sc_ReasonList.Focus()
                sc_ReasonList.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must select a Partner Decline Reason with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        Else
            If sc_ReasonList.SelectedItem.Text <> "*Select One*" Then
                sc_ReasonList.Focus()
                sc_ReasonList.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Cannot select a Partner Decline Reason with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        End If

out:
    End Sub

    Protected Sub AddData()
        Dim sqlConn As New SqlConnection(Session.Item("Connect_String"))
        Dim sqlCmd1 As New SqlCommand
        Dim rc1 As Object

        'Calculate next available loan number for new record
        sqlCmd1.CommandText = "SELECT max(LoanNumber) from tblDeclinePartnerLoans"
        sqlCmd1.CommandType = CommandType.Text
        sqlCmd1.Connection = sqlConn
        sqlConn.Open()
        rc1 = sqlCmd1.ExecuteScalar

        Dim NextLoanNum As Integer = rc1 + 1

        'Store the new record
        Dim sql As String = "INSERT INTO tblDeclinePartnerLoans (LoanNumber, DeclinedDate, CurrentStatus, PartnerID, PartnerSubmitDate, " & _
                            "PartnerActionDate, CommissionReceivedDate, PartnerFundAmt, PartnerDeclineReason, ARFCommissionDue, RepCommissionDue, OrigRepCommissionDue, " & _
                            "Notes, RepID, ISOid, MerchName, MerchAddress, MerchCity, MerchState, MerchZip, MerchPhone, MerchMobile, MerchFax, GuarName," & _
                            "LeadSource, AmtRequested, ARFDeclineReason, LastUpdateBy, LastUpdateDate, CommissionLock) " & _
                            "VALUES (" & NextLoanNum & ",'" & sc_DeclineDate.DbSelectedDate & "','" & sc_StatusList.SelectedValue & "',"

        If sc_PartnerNameList.SelectedItem.Text = "*Select One*" Then
            sql = sql & "NULL,"
        Else
            sql = sql & sc_PartnerNameList.SelectedValue & ","
        End If

        Select Case sc_StatusList.SelectedValue
            Case "In Process"
                sc_SubmitDate.Text = Now.ToShortDateString
                sql = sql & "getdate(),NULL,NULL,"
            Case "Declined"
                sc_SubmitDate.Text = Now.ToShortDateString
                sc_ActionDate.Text = Now.ToShortDateString
                sql = sql & "getdate(),getdate(),NULL,"
            Case "Funded - Awaiting Comm"
                sc_SubmitDate.Text = Now.ToShortDateString
                sc_ActionDate.Text = Now.ToShortDateString
                sql = sql & "getdate(),getdate(),NULL,"
            Case "Funded - Received Comm"
                sc_SubmitDate.Text = Now.ToShortDateString
                sc_ActionDate.Text = Now.ToShortDateString
                sc_CommRcvDate.Text = Now.ToShortDateString
                sql = sql & "getdate(),getdate(),getdate(),"
            Case Else
                sql = sql & "NULL,NULL,NULL,"
        End Select

        If Not sc_FundAmt1.Value.HasValue Then
            sql = sql & "NULL,"
        Else
            sql = sql & "" & sc_FundAmt1.Value & ","
        End If
        If sc_ReasonList.SelectedItem.Text = "*Select One*" Then
            sql = sql & "NULL,"
        Else
            sql = sql & "'" & sc_ReasonList.SelectedValue & "',"
        End If
        If Not sc_ARFCommDue1.Value.HasValue Then
            sql = sql & "NULL,"
        Else
            sql = sql & "" & sc_ARFCommDue1.Value & ","
        End If
        If Not sc_RepCommDue1.Value.HasValue Then
            sql = sql & "NULL,NULL,"
        Else
            sql = sql & "" & sc_RepCommDue1.Value & "," & sc_RepCommDue1.Value & ","
        End If
        If sc_Notes.Text = "" Then
            sql = sql & "NULL,"
        Else
            sql = sql & "'" & RegexReplace(sc_Notes.Text) & "',"
        End If
        If sc_Rep.SelectedItem.Text = "*Select One*" Then
            sql = sql & "NULL,"
        Else
            sql = sql & sc_Rep.SelectedValue & ","
        End If
        If sc_ISO.SelectedItem.Text = "*Select One*" Then
            sql = sql & "NULL,"
        Else
            sql = sql & sc_ISO.SelectedValue & ","
        End If
        sql = sql & "'" & RegexReplace(sc_MerchantName.Text) & "','" & RegexReplace(sc_Address1.Text) & "','" & RegexReplace(sc_City.Text) & "','" & _
                          sc_State.SelectedValue & "','" & sc_Zip.Text & "','" & sc_Phone.Text & "',"
        If sc_Mobile.Text = "" Then
            sql = sql & "NULL,"
        Else
            sql = sql & "'" & sc_Mobile.Text & "',"
        End If
        If sc_Fax.Text = "" Then
            sql = sql & "NULL,"
        Else
            sql = sql & "'" & sc_Fax.Text & "',"
        End If
        sql = sql & "'" & RegexReplace(sc_Guarantor.Text) & "','" & sc_LeadSource.SelectedValue & "'," & sc_ReqAmt.Value & ",'" & _
                          sc_ARFReasonList.SelectedValue & "',"

        sql = sql & "'" & Session.Item("Username").ToString & "',GetDate(),NULL)"

        sqlCmd1.CommandText = sql
        rc1 = sqlCmd1.ExecuteScalar
        sqlConn.Close()

        Response.Redirect("DPAppList.aspx")

    End Sub

    Protected Sub FillPartnerList()
        ' load Partner dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT PartnerID, PartnerName from tblDeclinePartners where activeflag = 1 order by PartnerName", Conn)
        cmd.Connection.Open()
        Dim Partners As SqlDataReader
        Partners = cmd.ExecuteReader()
        sc_PartnerNameList.DataSource = Partners
        sc_PartnerNameList.DataValueField = "PartnerID"
        sc_PartnerNameList.DataTextField = "PartnerName"
        sc_PartnerNameList.DataBind()
        sc_PartnerNameList.Items.Insert(0, "*Select One*")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Protected Sub FillStatusList()
        sc_StatusList.Items.Insert(0, "New")
        sc_StatusList.Items.Insert(1, "Awaiting Rep Approval")
        sc_StatusList.Items.Insert(2, "In Process")
        sc_StatusList.Items.Insert(3, "Declined")
        sc_StatusList.Items.Insert(4, "Funded - Awaiting Comm")
        sc_StatusList.Items.Insert(5, "Funded - Received Comm")
        sc_StatusList.Items.Insert(6, "Back to ARF")
        sc_StatusList.Items.Insert(7, "Removed")
    End Sub

    Protected Sub FillReasonList()
        ' load both DeclineReason dropdown lists
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim sql As String = "SELECT ReasonID, ReasonDesc from tblDeclineReasons order by ReasonDesc"
        Dim dt As DataTable = New DataTable
        Conn.Open()
        Dim sda As SqlDataAdapter = New SqlDataAdapter(sql, Conn)
        sda.Fill(dt)
        sc_ReasonList.DataSource = dt
        sc_ReasonList.DataValueField = "ReasonID"
        sc_ReasonList.DataTextField = "ReasonDesc"
        sc_ReasonList.DataBind()
        sc_ReasonList.Items.Insert(0, "*Select One*")
        sc_ARFReasonList.DataSource = dt
        sc_ARFReasonList.DataValueField = "ReasonID"
        sc_ARFReasonList.DataTextField = "ReasonDesc"
        sc_ARFReasonList.DataBind()
        sc_ARFReasonList.Items.Insert(0, "*Select One*")
        Conn.Close()
    End Sub

    Protected Sub FillLeadSourceList()
        ' load Lead Source dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT Source, SourceDesc from tblSource order by SourceDesc", Conn)
        cmd.Connection.Open()
        Dim Sources As SqlDataReader
        Sources = cmd.ExecuteReader()
        sc_LeadSource.DataSource = Sources
        sc_LeadSource.DataValueField = "Source"
        sc_LeadSource.DataTextField = "SourceDesc"
        sc_LeadSource.DataBind()
        sc_LeadSource.Items.Insert(0, "*Select One*")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Protected Sub FillSalesRepList()
        ' load SalesRep dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT Rep_ID, LN+', '+FN as RepName FROM tbl_Reps WHERE ActiveFlag = 1 ORDER BY LN+', '+FN", Conn)
        cmd.Connection.Open()
        Dim Reps As SqlDataReader
        Reps = cmd.ExecuteReader()
        sc_Rep.DataSource = Reps
        sc_Rep.DataValueField = "Rep_ID"
        sc_Rep.DataTextField = "RepName"
        sc_Rep.DataBind()
        sc_Rep.Items.Insert(0, "*Select One*")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Protected Sub FillISOList()
        ' load ISO dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT ISOid, ISOName from tblISOs where not ISOName like '%INACTIVE%' order by ISOName", Conn)
        cmd.Connection.Open()
        Dim ISOs As SqlDataReader
        ISOs = cmd.ExecuteReader()
        sc_ISO.DataSource = ISOs
        sc_ISO.DataValueField = "ISOID"
        sc_ISO.DataTextField = "ISOName"
        sc_ISO.DataBind()
        sc_ISO.Items.Insert(0, "*Select One*")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Function RegexReplace(StrIn As String) As String
        Return Regex.Replace(StrIn, "[^\w\. @-]", "")
    End Function

End Class