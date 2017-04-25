Imports Telerik.Web.UI
Imports xi = Telerik.Web.UI.ExportInfrastructure
Imports Telerik.Web.UI.GridExcelBuilder
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Data

Public Class DPAppList
    Inherits System.Web.UI.Page
    '
    ' DPAppList - ASR 07/21/2013
    ' List loan applications that were declined by ARF and are now in various stages of processing by Decline Partners.
    ' 4/6/17 ASR - Finance cant Add or Delete, Sales cant see commissions.
    '
    Dim GotRecs As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session.Item("Connect_String") = WebConfigurationManager.ConnectionStrings("ARF_Production").ConnectionString
        Session.Item("Username") = Membership.GetUser.UserName
        If Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPSales") Or
           Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPFinance") Or
           Roles.IsUserInRole(Session.Item("Username"), "SystemControl") Then
            Dim x As Integer = 1
        Else
            If Request.QueryString("from") = "Finance" Then
                Response.Redirect("~/Secure_Financial/Fin_QueueHome.aspx")
            Else
                Response.Redirect("MobileAdminDefault.aspx?parm=noAuth")
            End If
        End If

        'Dont show Add button to Finance or commissions to Sales
        If Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPFinance") Then
            AddButton.Visible = False
        ElseIf Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPSales") Then
            RadGrid1.MasterTableView.GetColumn("ARFComm").Display = False
        End If

        If Not IsPostBack Then

            ' replace default filter values if returning from detail screen

            StatusList_Load() ' load status list
            If Session.Item("DPStatusIndex") Is Nothing Then
                StatusList.SelectedIndex = 0
            Else
                StatusList.SelectedIndex = Session.Item("DPStatusIndex")
            End If

            If Session.Item("DPStartDate") Is Nothing Then
                StartDate1.SelectedDate = DateAdd(DateInterval.Month, -3, Today()) ' default to 3 months ago
            Else
                StartDate1.SelectedDate = Session.Item("DPStartDate")
            End If

            PartnerList_Load() ' load partner dropdown
            If Session.Item("DPPartnerIndex") Is Nothing Then
                PartnerList.SelectedIndex = 0
            Else
                PartnerList.SelectedIndex = Session.Item("DPPartnerIndex")
            End If

            RepList_Load() ' load salesrep dropdown
            If Session.Item("DPRepIndex") Is Nothing Then
                RepList.SelectedIndex = 0
            Else
                RepList.SelectedIndex = Session.Item("DPRepIndex")
            End If

            ReasonList_Load() ' load decline reason dropdown
            If Session.Item("DPReasonIndex") Is Nothing Then
                ReasonList.SelectedIndex = 0
            Else
                ReasonList.SelectedIndex = Session.Item("DPReasonIndex")
            End If

        End If

    End Sub

    Private Sub BindData()
        RadGrid1.Rebind()
    End Sub

    Protected Sub RadGrid1_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs)
        TryCast(sender, RadGrid).DataSource = GetData()
    End Sub

    Private Function GetData() As System.Data.DataTable
        'Populate the datatable

        If StatusList.SelectedItem.Value = "New" Or StatusList.SelectedItem.Value = "Awaiting Rep Approval" Then 'Newly declined apps do not have a partner assigned yet
            PartnerList.SelectedIndex = 0
            PartnerList.Enabled = False
        Else
            PartnerList.Enabled = True
        End If

        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim DT As New DataTable

        Dim statusline As String = ""
        If StatusList.SelectedItem.Value = "ALL" Then
            statusline = "AND dpl.CurrentStatus <> 'Removed' " ' dont show Removed if ALL selected
        Else
            statusline = "AND dpl.CurrentStatus = '" & StatusList.SelectedItem.Value & "' "
        End If
        Dim partnerline As String = ""
        If PartnerList.SelectedItem.Value <> "ALL" Then
            partnerline = "AND replace(dp.partnername,'''','') = '" & Replace(PartnerList.SelectedItem.Value, "'", "") & "' "
        End If
        Dim repline As String = ""
        If RepList.SelectedItem.Value <> "ALL" Then
            repline = "AND dpl.repid = " & RepList.SelectedItem.Value & " "
        End If
        Dim reasonline As String = ""
        If ReasonList.SelectedItem.Value <> "ALL" Then
            reasonline = "AND Replace(dr.reasondesc,'''','') = '" & Replace(ReasonList.SelectedItem.Value, "'", "") & "' "
        End If
        Dim merchline As String = ""
        If MerchSearch.Text <> "" Then
            merchline = "AND (Replace(ln.merchantname,'''','') like '%" & Replace(MerchSearch.Text, "'", "") & "%' or " & _
                        "dpl.MerchName like '%" & Replace(MerchSearch.Text, "'", "") & "%')"
        End If

        Dim SQL As String = "select dpl.loannumber, case when dpl.loannumber < 900000 then left(ln.MerchantName,40) else left(dpl.MerchName,40) end as MerchantName, " &
                            "rep.ln + ', ' + rep.fn as SalesRep, dpl.DeclinedDate, " &
                            "dpl.CurrentStatus, dp.PartnerName, " &
                            "dpl.PartnerSubmitDate, dpl.PartnerActionDate, dpl.PartnerFundAmt, dpl.ARFCommissionDue, " &
                            "dpl.RepCommissionDue, dpl.CommissionReceivedDate, dr.reasondesc, dpl.commissionlock, coalesce(dpl.undeclined,0) as undeclined,  " &
                            "dpl.undeclineddate from tbldeclinepartnerloans dpl " &
                            "left join tbldeclinepartners dp on dp.partnerid = dpl.PartnerID " &
                            "left join tblLoans ln on ln.LoanNumber = dpl.LoanNumber " &
                            "left join dailystatus ds on ds.appnumber = dpl.loannumber " &
                            "inner join tblDeclineReasons dr on dr.reasonid = dpl.ARFdeclinereason " &
                            "left join tbl_reps rep on rep.rep_id = dpl.repid " &
                            "where dpl.currentstatus <> 'System' and dpl.declineddate >= '" & StartDate1.SelectedDate & "' " & statusline & partnerline & repline & reasonline & merchline &
                            "order by dpl.declineddate"

        Conn.Open()
        Dim DA As New SqlDataAdapter(SQL, Conn)
        DA.Fill(DT)

        Dim row As DataRow
        If DT.Rows.Count = 0 Then 'check for records
            row = DT.NewRow()
            row("MerchantName") = "No records found for above criteria"
            DT.Rows.Add(row)
            RadGrid1.Columns.Item(1).Visible = False ' hide edit & delete buttons
            RadGrid1.Columns.Item(2).Visible = False
            GotRecs = False
        Else
            RadGrid1.Columns.Item(1).Visible = True
            RadGrid1.Columns.Item(2).Visible = True
            GotRecs = True
        End If

        Conn.Close()
        Conn.Dispose()

        ' save filter values for return trip
        Session.Item("DPStatusIndex") = StatusList.SelectedIndex
        Session.Item("DPStartDate") = StartDate1.SelectedDate
        Session.Item("DPPartnerIndex") = PartnerList.SelectedIndex
        Session.Item("DPRepIndex") = RepList.SelectedIndex
        Session.Item("DPReasonIndex") = ReasonList.SelectedIndex

        Return DT

    End Function

    Protected Sub RemoveRecFromList(ByVal LoanID As String)
        ' Update a record so that it is removed from the list and not processed further

        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim UpdateBy As String = Session.Item("UserName").ToString
        Dim SQL As String = "UPDATE tbldeclinepartnerloans SET CurrentStatus = 'Removed', LastUpdateBy = '" & UpdateBy & "', " & _
                            "LastUpdateDate = '" & Now() & "' WHERE LoanNumber = " & LoanID

        Dim cmd As New SqlCommand(SQL, Conn)
        Conn.Open()
        Dim RowCt As Integer = cmd.ExecuteNonQuery()
        Conn.Close()
        Conn.Dispose()

        BindData()

    End Sub

    Protected Sub StatusList_Load()
        'load status list
        StatusList.Items.Add("ALL")
        StatusList.Items.Add("New")
        StatusList.Items.Add("Awaiting Rep Approval")
        StatusList.Items.Add("In Process")
        StatusList.Items.Add("Declined")
        StatusList.Items.Add("Funded - Awaiting Comm")
        StatusList.Items.Add("Funded - Received Comm")
        StatusList.Items.Add("Back to ARF")
        StatusList.Items.Add("Removed")
        StatusList.Items.Item(0).Selected = True
    End Sub

    Protected Sub PartnerList_Load()
        ' load Partner dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT PartnerName from tblDeclinePartners order by PartnerName", Conn)
        cmd.Connection.Open()
        Dim Partners As SqlDataReader
        Partners = cmd.ExecuteReader()
        PartnerList.DataSource = Partners
        PartnerList.DataValueField = "PartnerName"
        PartnerList.DataTextField = "PartnerName"
        PartnerList.DataBind()
        PartnerList.Items.Insert(0, "ALL")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Protected Sub RepList_Load()
        ' load Partner dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT Rep_ID, LN+', '+FN as RepName FROM tbl_Reps WHERE ActiveFlag = 1 ORDER BY LN+', '+FN", Conn)
        cmd.Connection.Open()
        Dim Reps As SqlDataReader
        Reps = cmd.ExecuteReader()
        RepList.DataSource = Reps
        RepList.DataValueField = "Rep_ID"
        RepList.DataTextField = "RepName"
        RepList.DataBind()
        RepList.Items.Insert(0, "ALL")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Protected Sub ReasonList_Load()
        ' load DeclineReason dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As New SqlCommand("SELECT ReasonDesc from tblDeclineReasons order by ReasonDesc", Conn)
        cmd.Connection.Open()
        Dim Reasons As SqlDataReader
        Reasons = cmd.ExecuteReader()
        ReasonList.DataSource = Reasons
        ReasonList.DataValueField = "ReasonDesc"
        ReasonList.DataTextField = "ReasonDesc"
        ReasonList.DataBind()
        ReasonList.Items.Insert(0, "ALL")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Sub RadGrid1_RowCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        'process row selection when either Edit or Delete button is clicked
        If e.CommandName = "Mod" Or e.CommandName = "Del" Then
            Dim item As GridDataItem = e.Item
            Dim loanid As String = item("LoanNumber").Text
            Session.Item("DPLoanID") = 0
            If e.CommandName = "Mod" Then
                'goto detail screen
                Session.Item("DPLoanID") = loanid
                Response.Redirect("DPAppDetail.aspx")
            ElseIf e.CommandName = "Del" Then
                'remove record from further processing
                RemoveRecFromList(loanid)
            End If
        End If
    End Sub

    Sub RadGrid1_RowDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles RadGrid1.ItemDataBound
        ' configure the datagrid as it is being loaded with data
        If Not e.Item Is Nothing Then
            If e.Item.ItemType = GridItemType.Item Or e.Item.ItemType = GridItemType.AlternatingItem Then
                Dim item As GridDataItem = e.Item
                If GotRecs Then
                    ' If rep commission has been reported to the commissions app then record may not be updated 
                    If item("CommissionLock").Text = "True" Then
                        item("Del").Enabled = False
                        item("Del").Text = ""
                        item("Mod").Enabled = False
                        item("Mod").Text = ""
                    End If
                    ' If Finance user and status not funded then record may not be updated
                    If Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPFinance") And Not Left(item("CurrentStatus").Text, 6) = "Funded" Then
                        item("Mod").Enabled = False
                        item("Mod").Text = ""
                    End If
                    ' If sales user and commission received then record may not be updated
                    If Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPSales") And item("CurrentStatus").Text = "Funded - Received Comm" Then
                        item("Mod").Enabled = False
                        item("Mod").Text = ""
                    End If
                    ' Only enable the Delete button if the status is New or Awaiting Rep Approval.  Once status is changed, can no longer delete.
                    ' Finance cannot delete ASR 4/6/17
                    If (item("CurrentStatus").Text = "New" Or item("CurrentStatus").Text = "Awaiting Rep Approval") And
                        Not Roles.IsUserInRole(Session.Item("Username"), "MobileAdmin_DPFinance") Then
                        item("Del").Enabled = True
                    Else
                        item("Del").Enabled = False
                        item("Del").Text = ""
                    End If
                    'hilight if loan has been "undeclined"
                    If item("undeclined").Text = 1 Then
                        item("merchantname").ForeColor = System.Drawing.Color.Red
                        Dim funddate As DateTime = item("undeclineddate").Text
                        item("merchantname").ToolTip = "App funded by ARF on " & funddate.ToString("MM-dd-yyyy")
                    End If
                    'hilight the recently edited loan if any
                    If IsNumeric(item("LoanNumber").Text) Then
                        If item("LoanNumber").Text = Session.Item("DPLoanID") Then
                            item("Mod").BackColor = System.Drawing.Color.LightGreen
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub ExcelButton_Click(sender As Object, e As ImageClickEventArgs) Handles ExcelButton.Click
        RadGrid1.MasterTableView.ExportToExcel()
    End Sub

    Protected Sub StatusList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StatusList.SelectedIndexChanged
        BindData()
    End Sub

    Protected Sub StartDate1_SelectedDateChanged(sender As Object, e As Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs) Handles StartDate1.SelectedDateChanged
        BindData()
    End Sub

    Protected Sub ReasonList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ReasonList.SelectedIndexChanged
        BindData()
    End Sub

    Protected Sub PartnerList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PartnerList.SelectedIndexChanged
        BindData()
    End Sub

    Protected Sub RepList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RepList.SelectedIndexChanged
        BindData()
    End Sub

    Protected Sub MerchSearch_TextChanged(sender As Object, e As EventArgs) Handles MerchSearch.TextChanged
        BindData()
    End Sub

    Protected Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        StatusList.SelectedIndex = 0
        StartDate1.SelectedDate = DateAdd(DateInterval.Month, -3, Today())
        PartnerList.SelectedIndex = 0
        RepList.SelectedIndex = 0
        ReasonList.SelectedIndex = 0
        MerchSearch.Text = ""
        BindData()
    End Sub

    Protected Sub AddButton_Click(sender As Object, e As EventArgs) Handles AddButton.Click
        Session.Remove("DPLoanID")
        Response.Redirect("DPAppAdd.aspx")
    End Sub

End Class