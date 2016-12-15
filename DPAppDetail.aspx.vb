Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Configuration
Imports System.Text.RegularExpressions

Public Class DPAppDetail
    Inherits System.Web.UI.Page
    '
    ' DPAppDetail - ASR 07/21/2013
    ' View/Edit loan application that was declined by ARF for potential processing by a Decline Partner.
    '
    '
    Dim BColor As System.Drawing.Color
    Dim Err As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            sc_LoanNum.Text = Session.Item("DPLoanID")
            BColor = sc_PartnerNameList.BorderColor
            FillPartnerList()
            FillStatusList()
            FillReasonList()
            GetRecord()
        End If
    End Sub

    Sub GetRecord()
        'Populate the screen variables with the loan app

        Dim sqlConn As New SqlConnection(Session.Item("Connect_String"))
        Dim sqlCmd As New SqlCommand
        Dim Rdr As SqlDataReader
        Dim SQLtxt As String = "select dpl.LoanNumber, " &
                               "case when dpl.loannumber < 900000 then ds.merchant else 900000 end as MerchantID, " &
                               "case when dpl.loannumber < 900000 then ln.MerchantName else dpl.MerchName end as MerchantName, " &
                               "case when dpl.loannumber < 900000 then ln.MerchAddress1 else dpl.MerchAddress end as MerchAddress1, " &
                               "case when dpl.loannumber < 900000 then ln.MerchCity1 else dpl.MerchCity end as MerchCity1, " &
                               "case when dpl.loannumber < 900000 then ln.MerchState1 else dpl.MerchState end as MerchState1, " &
                               "case when dpl.loannumber < 900000 then ln.MerchZip1 else dpl.MerchZip end as MerchZip1, " &
                               "case when dpl.loannumber < 900000 then ln.MerchPhone1 else dpl.MerchPhone end as MerchPhone1, " &
                               "case when dpl.loannumber < 900000 then ln.MerchCell2 else dpl.MerchMobile end as MerchCell2, " &
                               "case when dpl.loannumber < 900000 then ln.MerchFax1 else dpl.MerchFax end as MerchFax1, " &
                               "case when dpl.loannumber < 900000 then gu.GuarantorName else dpl.GuarName end as GuarantorName, " &
                               "case when dpl.loannumber < 900000 then ln.AmountRequest else dpl.AmtRequested end as AmountRequest, " &
                               "ds.Market, rp.ln + ', ' + rp.FN as RepName, src.SourceDesc, ISO.ISOName, " &
                               "ds.EntryDate, dpl.DeclinedDate, drARF.ReasonDesc as ARFDeclineReason, " &
                               "dpl.partnerID, dpl.PartnerSubmitDate, dpl.CurrentStatus, dpl.PartnerActionDate, dpl.CommissionReceivedDate, " &
                               "coalesce(dpl.undeclined,0) as undeclined, dpl.undeclineddate, " &
                               "dpl.PartnerFundAmt, dpl.ARFCommissionDue, dpl.RepCommissionDue, dpl.OrigRepCommissionDue, dpl.Notes, dpl.LastUpdateBy, dpl.LastUpdateDate, " &
                               "drPTR.ReasonDesc as PartnerDeclineReason, dpl.commissionlock from tblDeclinePartnerLoans dpl " &
                               "left join tblloans ln on ln.LoanNumber = dpl.LoanNumber " &
                               "left join DailyStatus ds on ds.AppNumber = dpl.LoanNumber " &
                               "inner join tblDeclineReasons drARF on drARF.ReasonID = dpl.ARFDeclineReason " &
                               "left join tblDeclineReasons drPTR on drPTR.ReasonID = dpl.PartnerDeclineReason " &
                               "left join tbl_Reps rp on rp.Rep_ID = dpl.RepID " &
                               "left join tblSource src on src.SourceText = dpl.leadsource " &
                               "left join tblLoanGuarantors lg on lg.LoanNumber = ln.LoanNumber " &
                               "left join tblGuarantors gu on gu.GuarantorID = lg.GuarantorID " &
                               "left join tblISOs ISO on ISO.ISOID = dpl.ISOID " &
                               "where dpl.LoanNumber = " & sc_LoanNum.Text

        sqlCmd.CommandType = CommandType.Text
        sqlCmd.CommandText = SQLtxt
        sqlCmd.Connection = sqlConn
        sqlConn.Open()
        Rdr = sqlCmd.ExecuteReader
        Rdr.Read()
        sc_Address1.Text = Rdr("MerchAddress1").ToString
        sc_MerchantName.Text = Rdr("MerchantName").ToString
        If sc_LoanNum.Text > "900000" Then
            sc_MerchantID.Text = "MANUAL ADD"
        Else
            sc_MerchantID.Text = Rdr("MerchantID").ToString
        End If

        sc_City.Text = Rdr("MerchCity1").ToString
        sc_State.Text = Rdr("MerchState1").ToString
        sc_Zip.Text = Rdr("merchzip1").ToString
        sc_Phone.Text = Rdr("MerchPhone1").ToString
        If Len(sc_Phone.Text) = 10 Then
            sc_Phone.Text = "(" & Left(sc_Phone.Text, 3) & ")" & Mid(sc_Phone.Text, 4, 3) & "-" & Right(sc_Phone.Text, 4)
        End If
        sc_Mobile.Text = Rdr("MerchCell2").ToString
        If Len(sc_Mobile.Text) = 10 Then
            sc_Mobile.Text = "(" & Left(sc_Mobile.Text, 3) & ")" & Mid(sc_Mobile.Text, 4, 3) & "-" & Right(sc_Mobile.Text, 4)
        End If
        sc_Fax.Text = Rdr("MerchFax1").ToString
        If Len(sc_Fax.Text) = 10 Then
            sc_Fax.Text = "(" & Left(sc_Fax.Text, 3) & ")" & Mid(sc_Fax.Text, 4, 3) & "-" & Right(sc_Fax.Text, 4)
        End If
        sc_Guarantor.Text = Rdr("GuarantorName").ToString
        sc_Market.Text = Rdr("Market").ToString
        sc_Rep.Text = Rdr("RepName").ToString
        sc_LeadSource.Text = Rdr("SourceDesc").ToString
        sc_ISO.Text = Rdr("ISOName").ToString
        sc_ApplyDate.Text = String.Format("{0:MM/dd/yyyy}", Rdr("EntryDate"))
        sc_ReqAmt.Text = String.Format("{0:c2}", Rdr("AmountRequest"))
        sc_DeclineDate.Text = String.Format("{0:MM/dd/yyyy}", Rdr("DeclinedDate"))
        sc_Reason.Text = Rdr("ARFDeclineReason").ToString
        sc_PartnerNameList.SelectedIndex = sc_PartnerNameList.Items.IndexOf(sc_PartnerNameList.Items.FindByValue(Rdr("partnerID").ToString))
        sc_ReasonList.SelectedIndex = sc_ReasonList.Items.IndexOf(sc_ReasonList.Items.FindByValue(Rdr("PartnerDeclineReason").ToString))
        sc_SubmitDate.Text = String.Format("{0:MM/dd/yyyy hh:mm tt}", Rdr("PartnerSubmitDate"))
        sc_StatusList.SelectedIndex = sc_StatusList.Items.IndexOf(sc_StatusList.Items.FindByValue(Rdr("CurrentStatus").ToString))
        sc_ActionDate.Text = String.Format("{0:MM/dd/yyyy hh:mm tt}", Rdr("PartnerActionDate"))
        sc_CommRcvDate.Text = String.Format("{0:MM/dd/yyyy hh:mm tt}", Rdr("CommissionReceivedDate"))
        sc_FundAmt1.Text = Rdr("PartnerFundAmt").ToString
        sc_ARFCommDue1.Text = Rdr("ARFCommissionDue").ToString
        sc_RepCommDue1.Text = Rdr("RepCommissionDue").ToString
        Session.Item("dpOrigRepCommDue") = Rdr("OrigRepCommissionDue").ToString
        sc_Notes.Text = Rdr("Notes").ToString
        sc_UpDate.Text = Rdr("LastUpdateDate").ToString
        sc_UpBy.Text = Rdr("LastUpdateBy").ToString

        If Rdr("undeclined").ToString = 1 Then
            MsgLine.Text = "APP FUNDED BY ARF ON " & String.Format("{0:MM-dd-yyyy}", Rdr("undeclinedDate"))
        Else
            MsgLine.Text = ""
        End If

        Dim commlock As String = Rdr("CommissionLock").ToString
        If commlock = "True" Then
            sc_RepCommDue1.Enabled = False
            sc_RepCommDue1.ToolTip = "May not modify - already reported to Commissions App"
        End If

        Rdr.Close()
        sqlConn.Close()

        Orig_Partner.Text = sc_PartnerNameList.SelectedValue
        Orig_Status.Text = sc_StatusList.SelectedValue
        Orig_FundAmt.Text = sc_FundAmt1.Text
        Orig_Reason.Text = sc_ReasonList.SelectedValue
        Orig_ARFCommDue.Text = sc_ARFCommDue1.Text
        Orig_RepCommDue.Text = sc_RepCommDue1.Text
        Orig_Notes.Text = sc_Notes.Text

        If Orig_Status.Text = "Declined" Or Left(Orig_Status.Text, 6) = "Funded" Then
            sc_FundAmt1.Enabled = False
            sc_PartnerNameList.Enabled = False
            sc_ReasonList.Enabled = False
        End If

    End Sub

    Protected Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Response.Redirect("DPAppList.aspx")
    End Sub

    Protected Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        Response.Redirect("DPAppDetail.aspx")
    End Sub

    Protected Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        ValidateData()
        If Not Err Then
            UpdateData()
        End If
    End Sub

    Protected Sub ValidateData()

        Err = False
        sc_PartnerNameList.BorderColor = BColor
        sc_FundAmt1.BorderColor = BColor
        sc_ARFCommDue1.BorderColor = BColor
        sc_RepCommDue1.BorderColor = BColor

        Select Case sc_StatusList.SelectedValue
            Case "New"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "Awaiting Rep Approval" Or Orig_Status.Text = "Removed" Or Orig_Status.Text = "Back to ARF") Then
                    Err = True
                End If
            Case "Awaiting Rep Approval"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "Awaiting Rep Approval" Or Orig_Status.Text = "Removed" Or Orig_Status.Text = "Back to ARF") Then
                    Err = True
                End If
            Case "Removed"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "Removed") Then
                    Err = True
                End If
            Case "In Process"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "In Process" Or Orig_Status.Text = "Removed" Or Orig_Status.Text = "Declined") Then
                    Err = True
                End If
            Case "Back to ARF"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "In Process" Or Orig_Status.Text = "Removed" Or Orig_Status.Text = "Declined" Or Orig_Status.Text = "Back to ARF") Then
                    Err = True
                Else
                    sc_PartnerNameList.Enabled = True
                    sc_ReasonList.Enabled = True
                End If
            Case "Declined"
                If Not (Orig_Status.Text = "In Process" Or Orig_Status.Text = "Declined") Then
                    Err = True
                End If
            Case "Funded - Awaiting Comm"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "In Process" Or Orig_Status.Text = "Funded - Awaiting Comm" Or Orig_Status.Text = "Declined") Then
                    Err = True
                End If
            Case "Funded - Received Comm"
                If Not (Orig_Status.Text = "New" Or Orig_Status.Text = "In Process" Or Orig_Status.Text = "Funded - Awaiting Comm" Or Orig_Status.Text = "Funded - Received Comm") Then
                    Err = True
                End If
        End Select
        If Err Then
            sc_StatusList.Focus()
            sc_StatusList.BorderColor = Drawing.Color.Red
            RadWindowManager1.RadAlert("Cannot change App Status from """ & Orig_Status.Text & """ to """ & sc_StatusList.SelectedValue & """", 350, 130, "Validation Error", "")
            GoTo out
        End If

        If sc_StatusList.SelectedValue = "New" Or sc_StatusList.SelectedValue = "Awaiting Rep Approval" Or sc_StatusList.SelectedValue = "Removed" Or sc_StatusList.SelectedValue = "Back to ARF" Then
            If sc_PartnerNameList.SelectedIndex <> 0 Then
                sc_PartnerNameList.Focus()
                sc_PartnerNameList.BorderColor = Drawing.Color.Red
                Err = True
                RadWindowManager1.RadAlert("Cannot select Partner with this App Status", 350, 130, "Validation Error", "")
                GoTo out
            End If
        Else
            If sc_PartnerNameList.SelectedValue = "*Select One*" Then
                sc_PartnerNameList.Focus()
                sc_PartnerNameList.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must select a Partner with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        End If

        If sc_StatusList.SelectedValue = "New" Or sc_StatusList.SelectedValue = "Awaiting Rep Approval" Or sc_StatusList.SelectedValue = "Removed" Or
            sc_StatusList.SelectedValue = "In Process" Or sc_StatusList.SelectedValue = "Declined" Then
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
        ElseIf sc_StatusList.SelectedValue = "Funded - Awaiting Comm" Then
            If Not sc_FundAmt1.Value.HasValue Or sc_FundAmt1.Value = 0 Then
                sc_FundAmt1.Focus()
                sc_FundAmt1.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must provide Funded Amt with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        ElseIf sc_StatusList.SelectedValue = "Funded - Received Comm" Then
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

        If sc_StatusList.SelectedValue = "Declined" Then
            If sc_ReasonList.SelectedValue = "*Select One*" Then
                sc_ReasonList.Focus()
                sc_ReasonList.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Must select a Partner Decline Reason with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        Else
            If sc_ReasonList.SelectedValue <> "*Select One*" Then
                sc_ReasonList.Focus()
                sc_ReasonList.BorderColor = Drawing.Color.Red
                RadWindowManager1.RadAlert("Cannot select a Partner Decline Reason with this App Status", 350, 130, "Validation Error", "")
                Err = True
                GoTo out
            End If
        End If

        If Not IsDBNull(sc_Notes.Text) Then
            sc_Notes.Text = Regex.Replace(sc_Notes.Text, "[^\w\. @-]", "")
        End If
out:
    End Sub

    Protected Sub UpdateData()
        Dim sqlConn As New SqlConnection(Session.Item("Connect_String"))
        Dim sqlCmd As New SqlCommand
        Dim RC As Integer
        Dim SQLtxt As String

        If Orig_Partner.Text = sc_PartnerNameList.SelectedValue And
           Orig_Status.Text = sc_StatusList.SelectedValue And
           Orig_FundAmt.Text = sc_FundAmt1.Text And
           Orig_Reason.Text = sc_ReasonList.SelectedValue And
           Orig_ARFCommDue.Text = sc_ARFCommDue1.Text And
           Orig_RepCommDue.Text = sc_RepCommDue1.Text And
           Orig_Notes.Text = sc_Notes.Text Then
            Response.Redirect("DPAppList.aspx")
        Else
            SQLtxt = "UPDATE tblDeclinePartnerLoans SET lastupdatedate = getdate(), lastupdateby = '" & Session.Item("UserName").ToString & "'"
            If Orig_Partner.Text <> sc_PartnerNameList.SelectedValue Then
                If sc_PartnerNameList.SelectedValue <> "*Select One*" Then
                    SQLtxt = SQLtxt & ", PartnerID = " & sc_PartnerNameList.SelectedValue
                End If
            End If
            If Orig_FundAmt.Text <> sc_FundAmt1.Text Then
                SQLtxt = SQLtxt & ", PartnerFundAmt = " & sc_FundAmt1.DbValue
            End If
            If Orig_Reason.Text <> sc_ReasonList.SelectedValue Then
                If sc_ReasonList.SelectedValue <> "*Select One*" Then
                    SQLtxt = SQLtxt & ", PartnerDeclineReason = '" & sc_ReasonList.SelectedValue & "'"
                End If
            End If
            If Orig_ARFCommDue.Text <> sc_ARFCommDue1.Text Then
                SQLtxt = SQLtxt & ", ARFCommissionDue = " & sc_ARFCommDue1.DbValue
            End If
            If Orig_RepCommDue.Text <> sc_RepCommDue1.Text Then
                SQLtxt = SQLtxt & ", RepCommissionDue = " & sc_RepCommDue1.DbValue
                If Session.Item("dpOrigRepCommDue") = "" Then
                    If Orig_RepCommDue.Text = "" Then
                        SQLtxt = SQLtxt & ", OrigRepCommissionDue = " & sc_RepCommDue1.DbValue
                    Else
                        SQLtxt = SQLtxt & ", OrigRepCommissionDue = " & Orig_RepCommDue.Text
                    End If
                End If
            End If
            If Orig_Notes.Text <> sc_Notes.Text Then
                SQLtxt = SQLtxt & ", Notes = '" & sc_Notes.Text & "'"
            End If
            If Orig_Status.Text <> sc_StatusList.SelectedValue Then
                SQLtxt = SQLtxt & ", CurrentStatus = '" & sc_StatusList.SelectedValue & "'"
                Select Case sc_StatusList.SelectedValue
                    Case "In Process"
                        SQLtxt = SQLtxt & ", PartnerSubmitDate = getdate() "
                    Case "Declined"
                        SQLtxt = SQLtxt & ", PartnerActionDate = getdate(), " & _
                            "PartnerSubmitDate = case when PartnerSubmitDate is null then getdate() else PartnerSubmitDate end "
                    Case "Funded - Awaiting Comm"
                        SQLtxt = SQLtxt & ", PartnerActionDate = getdate(), " & _
                            "PartnerSubmitDate = case when PartnerSubmitDate is null then getdate() else PartnerSubmitDate end "
                    Case "Funded - Received Comm"
                        SQLtxt = SQLtxt & ", CommissionReceivedDate = getdate(), " & _
                            "PartnerSubmitDate = case when PartnerSubmitDate is null then getdate() else PartnerSubmitDate end, " & _
                            "PartnerActionDate = case when PartnerActionDate is null then getdate() else PartnerActionDate end "
                    Case Else
                        SQLtxt = SQLtxt & ", PartnerID = NULL, PartnerSubmitDate = NULL, PartnerActionDate = NULL, PartnerDeclineReason = NULL, CommissionReceivedDate = NULL"
                End Select
            End If
            SQLtxt = SQLtxt & " WHERE LoanNumber = " & sc_LoanNum.Text

            sqlCmd.CommandText = SQLtxt
            sqlCmd.Connection = sqlConn
            sqlConn.Open()
            RC = sqlCmd.ExecuteNonQuery
            sqlConn.Close()
            Response.Redirect("DPAppList.aspx")
        End If
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
        ' load DeclineReason dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT ReasonID, ReasonDesc from tblDeclineReasons order by ReasonDesc", Conn)
        cmd.Connection.Open()
        Dim Reasons As SqlDataReader
        Reasons = cmd.ExecuteReader()
        sc_ReasonList.DataSource = Reasons
        sc_ReasonList.DataValueField = "ReasonID"
        sc_ReasonList.DataTextField = "ReasonDesc"
        sc_ReasonList.DataBind()
        sc_ReasonList.Items.Insert(0, "*Select One*")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub
    
End Class