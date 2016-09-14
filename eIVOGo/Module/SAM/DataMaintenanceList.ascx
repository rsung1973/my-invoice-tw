<%@ Control Language="c#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<%@ Register Src="../Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="¸ê®ÆºûÅ@" />
<table class="content" cellspacing="0" cellpadding="0" border="0">
    <tbody>
        <tr>
            <!-- InstanceBeginEditable name="contant" -->
            <td valign="top" align="center">
                <table borderder="0" cellspacing="0" cellpadding="0" class="full-content">
                    <tr>
                        <!-- InstanceBeginEditable name="contant" -->
                        <td valign="top" align="left">
                            SQL:<asp:TextBox ID="keyWord" runat="server" 
                                Columns="80" Rows="10" TextMode="MultiLine"></asp:TextBox>
                            <asp:CheckBox ID="cbExcel" runat="server" Text="Excel¦sÀÉ" />
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn-blue" Text="·j´M" 
                                CausesValidation="False" onclick="btnSearch_Click">
                            </asp:Button>
                        </td>
                        <!-- InstanceEndEditable -->
                    </tr>
                    <tr>
                    <td align="left" valign="top">
                        <asp:TextBox ID="objType" runat="server" Columns="80"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="True" AllowPaging="False"
                    Width="98%" ClientIDMode="Static" GridLines="None" CellPadding="2" EnableViewState="False"
                    ShowFooter="False" CellSpacing="1" ShowHeader="True">
                    <Columns>
                    </Columns>
                    <PagerStyle HorizontalAlign="Center" />
                    <PagerTemplate>
                    </PagerTemplate>
                </asp:GridView>
                <uc3:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                <br />
                <asp:TextBox ID="msgBox" runat="server" EnableViewState="false" Visible="false" 
                    Columns="80" ReadOnly="True" Rows="5" TextMode="MultiLine"></asp:TextBox>
                <br />
            </td>
            <!-- InstanceEndEditable -->
        </tr>
    </tbody>
</table>
<br />
<uc2:ActionHandler ID="doSelect" runat="server" />
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>


<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
        this.PreRender += new EventHandler(module_sam_maintainbeneficiarymain_ascx_PreRender);

        doSelect.DoAction = arg =>
        {
            
        };
    }


    void module_sam_maintainbeneficiarymain_ascx_PreRender(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(btnSearch.CommandArgument))
        {
            try
            {
                if (cbExcel.Checked)
                {
                    saveAsExcel();
                }
                else
                {
                    buildQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                msgBox.Visible = true;
                msgBox.Text = ex.Message;
            }
        }
    }

    void buildQuery()
    {
        if (!String.IsNullOrEmpty(keyWord.Text))
        {
            Type itemType = String.IsNullOrEmpty(objType.Text) ? typeof(String) : Type.GetType(objType.Text);
            var mgr = dsEntity.CreateDataManager();
            
            SqlConnection conn = (SqlConnection)mgr.GetTable<Organization>().Context.Connection;
            SqlCommand sqlCmd = new SqlCommand(keyWord.Text, conn);
            conn.Open();
            SqlDataReader reader = sqlCmd.ExecuteReader();
            gvEntity.DataSource = reader;
            
            //if (gvEntity.PageIndex >= 0)
            //{
            //    gvEntity.DataSource = mgr.ExecuteQuery<dynamic>(keyWord.Text).Skip(gvEntity.PageSize * gvEntity.PageIndex).Take(gvEntity.PageSize);
            //}
            //else
            //{
            //    gvEntity.DataSource = mgr.ExecuteQuery<dynamic>(keyWord.Text).Take(gvEntity.PageSize);
            //}
            gvEntity.DataBind();
//            pagingList.RecordCount = mgr.ExecuteQuery<dynamic>(keyWord.Text).Count();
        }
        else
        {
            btnSearch.CommandArgument = null;
        }
    }

    void gvEntity_DataBound(object sender, EventArgs e)
    {
        
    }

    protected void PageIndexChanged(object source, Uxnet.Web.Module.Common.PageChangedEventArgs e)
    {
        gvEntity.PageIndex = e.NewPageIndex;
    }
    

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        btnSearch.CommandArgument = "Query";
    }
    
    void saveAsExcel()
    {
        System.Threading.ThreadPool.QueueUserWorkItem(stateInfo =>
        {
            try
            {
                String resultFile = System.IO.Path.Combine(Logger.LogDailyPath, Guid.NewGuid().ToString() + ".xlsx");

                using (Model.InvoiceManagement.InvoiceManager mgr = new Model.InvoiceManagement.InvoiceManager())
                {
                    SqlConnection conn = (SqlConnection)mgr.GetTable<Organization>().Context.Connection;
                    SqlCommand sqlCmd = new SqlCommand(keyWord.Text, conn);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adapter.Fill(ds);
                            using (ClosedXML.Excel.XLWorkbook xls = new ClosedXML.Excel.XLWorkbook())
                            {
                                xls.Worksheets.Add(ds);
                                xls.SaveAs(resultFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        });
    }

</script>
