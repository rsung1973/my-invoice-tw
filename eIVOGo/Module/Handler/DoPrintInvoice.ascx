<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register Src="~/Module/Common/DoPrintPOSHandler.ascx" TagPrefix="uc4" TagName="DoPrintPOSHandler" %>
<asp:Button ID="btnShow" runat="server" Text="列印發票" OnClick="btnShow_Click" />&nbsp;&nbsp;
 <uc4:DoPrintPOSHandler runat="server" ID="doPrintPOS" />
            <asp:Button ID="btnCSV" runat="server" Text="CSV下載" OnClick="btnCSV_Click" />
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<script>
    $(function () {
        $("input[value='列印發票']").click(function () {
            $("<div align='center'>列印作業即將進行，請稍後...!!</div>").dialog({ width: 480 });
        });
    });
</script>
<script runat="server">

    Model.Security.MembershipManagement.UserProfileMember _userProfile;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
        doPrintPOS.DoAction = arg =>
        {
            ((ASP.module_common_doprintposhandler_ascx)doPrintPOS).DoPrintInvoicePOS();
        };
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        String keyCodeFile = Path.Combine(Logger.LogPath, "ORCodeKey.txt");
        if (File.Exists(keyCodeFile))
        {
            if (!String.IsNullOrEmpty(File.ReadAllText(keyCodeFile)))
            {
                String[] ar = Request.GetItemSelection();
                if (ar != null && ar.Count() > 0)
                {
                    //_userProfile.EnqueueInvoicePrint(dsEntity.CreateDataManager(), ar.Select(a => int.Parse(a)));
                    _userProfile.EnqueueDocumentPrint(dsEntity.CreateDataManager(), ar.Select(a => int.Parse(a)));
                    //使用XPS列印舊版畫面
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
                    //    String.Format("window.open('{0}?printBack={1}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", VirtualPathUtility.ToAbsolute("~/SAM/PrintInvoicePage.aspx"), Request["printBack"])
                    //    , true);
                    ////使用XPS列印新版畫面
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
                    //    String.Format("window.open('{0}?printBack={1}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", VirtualPathUtility.ToAbsolute("~/SAM/NewPrintInvoicePage.aspx"), Request["printBack"])
                    //    , true);
                    ////使用PDF列印舊版畫面
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
                    //    String.Format("window.open('{0}?printBack={1}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", VirtualPathUtility.ToAbsolute("~/SAM/PrintInvoiceAsPDF.aspx"), Request["printBack"])
                    //    , true);
                    ////使用PDF列印新版畫面
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
                    //    String.Format("window.open('{0}?printBack={1}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", VirtualPathUtility.ToAbsolute("~/SAM/NewPrintInvoiceAsPDF.aspx"), Request["printBack"])
                    //    , true);

                    LiteralControl lc = new LiteralControl(String.Format("<iframe src='{0}?printBack={1}' height='0' width='0'></iframe>", VirtualPathUtility.ToAbsolute("~/SAM/NewPrintInvoiceAsPDF.aspx"), Request["printBack"]));
                    this.Controls.Add(lc);

                    //var modal = btnShow.AttachWaitingMessage("列印作業即將進行，請稍後...!!", false);
                    //modal.Show();
                }
                else
                {
                    this.AjaxAlert("請選擇列印資料!!");
                }
            }
            else
            {
                this.AjaxAlert("QRCode金鑰檔無內容，無法列印!!");
            }
        }
        else
        {
            this.AjaxAlert("無QRCode金鑰檔，無法列印!!");
        }
    }


    protected void btnCSV_Click(object sender, EventArgs e)
    {
        String[] ar = Request.GetItemSelection();
        if (ar != null && ar.Count() > 0)
        {
            string printData = string.Join(",", ar);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
            String.Format("window.open('{0}?printData={1}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", VirtualPathUtility.ToAbsolute("~/SAM/CreateCSV.aspx"), printData)
            , true);
        }
        else
        {
            this.AjaxAlert("請選擇匯出資料!!");
        }
    }   
    
</script>
