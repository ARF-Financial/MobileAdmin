Imports Telerik.Web.UI
Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Data

Public Class DLRequestStatus
    Inherits System.Web.UI.Page
    '
    ' DLRequestStatus - ASR 02/16/2016
    ' List DecisionLogic Bank Statement Requests
    '
    '
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session.Item("Connect_String") = ConfigurationManager.ConnectionStrings("ARF_Prod_ConnStr").ConnectionString
            Session.Item("Username") = "arobins" 'Membership.GetUser.UserName
            RepList_Load() ' load salesrep dropdown
            If Request.QueryString("RepToShow") Is Nothing Then
                RepList.SelectedIndex = 0
                RepToShow.Text = RepList.SelectedItem.Text
            Else
                RepToShow.Text = Request.QueryString("RepToShow")
                RepList.SelectedValue = RepList.Items.FindByText(RepToShow.Text).Value
            End If
            If Request.QueryString("ShowCompleted") = "1" Then
                ShowCompleted.Checked = True
            Else
                ShowCompleted.Checked = False
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
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim Cmd As New SqlCommand("sp_ARFMobile_DLRequestStatus")
        Dim DT As New DataTable
        Cmd.Parameters.AddWithValue("@Rep", RepToShow.Text)
        Dim showCompl As Integer
        If ShowCompleted.Checked Then
            showCompl = 1
        Else
            showCompl = 0
        End If
        Cmd.Parameters.AddWithValue("@ShowCompleted", showCompl)
        Cmd.Connection = Conn
        Cmd.CommandType = CommandType.StoredProcedure
        Dim DA As New SqlDataAdapter(Cmd)
        DA.SelectCommand.CommandTimeout = 300
        DA.Fill(DT)

        Dim row As DataRow
        If DT.Rows.Count = 0 Then 'check for records
            row = DT.NewRow()
            row("RepName") = "No records found"
            DT.Rows.Add(row)
        End If

        Conn.Close()
        Conn.Dispose()

        Return DT

    End Function

    Protected Sub RepList_Load()
        ' load Rep dropdown list
        Dim Conn As New SqlConnection(Session.Item("Connect_String"))
        Dim cmd As SqlCommand
        Dim SQL As String = "select CreatedBy as RepName from tblDecisionLogicRequest where (LastSentDate > dateadd(day,-90,getdate()) or DLStatusDate > dateadd(day,-90,getdate())) " &
                            "and canceldate is null group by CreatedBy order by CreatedBy"
        cmd = New SqlCommand(SQL, Conn)
        cmd.Connection.Open()
        Dim reps As SqlDataReader
        reps = cmd.ExecuteReader()
        RepList.DataSource = reps
        RepList.DataValueField = "RepName"
        RepList.DataTextField = "RepName"
        RepList.DataBind()
        RepList.Items.Insert(0, "*All Reps")
        cmd.Connection.Close()
        cmd.Connection.Dispose()
    End Sub

    Sub RadGrid1_RowCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs)
        Dim item As GridDataItem = e.Item
        If e.CommandName = "Resend" Then
        ElseIf e.CommandName = "Cancel" Then
        ElseIf e.CommandName = "Analyze" Then
        End If

    End Sub

    Sub RadGrid1_RowDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles RadGrid1.ItemDataBound
        ' configure the datagrid as it is being loaded with data
        Dim img As ImageButton
        If Not e.Item Is Nothing Then
            If e.Item.ItemType = GridItemType.Item Or e.Item.ItemType = GridItemType.AlternatingItem Then
                Dim item As GridDataItem = e.Item
                item("Resend").Enabled = False
                item("Cancel").Enabled = False
                item("Analyze").Enabled = False
                img = item("Resend").Controls(0)
                img.ImageUrl = "image/trans25.png"
                img = item("Cancel").Controls(0)
                img.ImageUrl = "image/trans25.png"
                img = item("Analyze").Controls(0)
                img.ImageUrl = "image/trans25.png"
                'color statuses
                If item("dlstatustext").Text = "Login, Verified" Then
                    item("dlstatustext").ForeColor = System.Drawing.Color.DarkGreen
                ElseIf item("dlstatustext").Text = "Stmts Matched" Then
                    item("dlstatustext").ForeColor = System.Drawing.Color.Blue
                    item("Analyze").Enabled = True
                    img = item("Analyze").Controls(0)
                    img.ImageUrl = "image/dlanalysis.png"
                Else
                    item("Cancel").Enabled = True
                    img = item("Cancel").Controls(0)
                    img.ImageUrl = "image/dlcancel.png"
                    'can resend only once per 24 hours
                    Dim LastDate As DateTime = Convert.ToDateTime(item("lastsentdate").Text)
                    If LastDate < DateAdd(DateInterval.Minute, -1440, Now()) Then
                        item("Resend").Enabled = True
                        img = item("resend").Controls(0)
                        img.ImageUrl = "image/dlsend.png"
                    End If
                    If item("dlstatustext").Text = "Bank Error" Then
                        item("dlstatustext").ForeColor = System.Drawing.Color.Red
                    ElseIf item("dlstatustext").Text = "Account Error" Then
                        item("dlstatustext").ForeColor = System.Drawing.Color.Goldenrod
                    ElseIf item("dlstatustext").Text = "Not Started" Then
                        item("dlstatustext").ForeColor = System.Drawing.Color.LightGray
                    ElseIf item("dlstatustext").Text = "Started, Not Completed" Then
                        item("dlstatustext").ForeColor = System.Drawing.Color.DarkGray
                    ElseIf item("dlstatustext").Text = "Login, Not Verified" Then
                        item("dlstatustext").ForeColor = System.Drawing.Color.LightGreen
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub RepList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RepList.SelectedIndexChanged
        RepToShow.Text = RepList.SelectedItem.Text
        BindData()
    End Sub

    Protected Sub ShowCompleted_Changed(sender As Object, e As EventArgs) Handles ShowCompleted.CheckedChanged
        BindData()
    End Sub
End Class