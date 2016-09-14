<%@ Page Language="c#" Inherits="OpenSite.CAUsageNote" StylesheetTheme="Visitor" CodeBehind="CAUsageNote.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head runat="server">
    <title>簽章元件安裝說明</title>
</head>
<body bgcolor="#cce28b" leftmargin="0" topmargin="0">
    <form id="Form1" method="post" runat="server">
    <table height="100%" cellspacing="0" cellpadding="0" width="800" align="center" bgcolor="#cce28b"
        border="0">
        <tbody>
            <tr>
                <td align="center">
                    數位簽章元件安裝注意事項
                </td>
            </tr>
            <tr>
                <td>
                    <ol>
                        <!--<li>用戶端必需先安裝.Net Framework 執行環境 2.0 以上的版本，請參考<a href="http://www.microsoft.com/downloads/details.aspx?displaylang=zh-tw&amp;FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5">這裡</a>。</li>-->
                        <li>用戶端必需先下載安裝簽章元件，<a href="UxnetCAObj.exe">請下載</a>。</li>
                        <li>請將網址(<asp:Literal ID="litWebSite" runat="server"></asp:Literal>)加入到Internet Explorer瀏覽器信任的網站。</li>
                        <!--<li>開啟命令提示字元，執行下列指命：<br />
                            <asp:LinkButton ID="lbCommand" runat="server" onclick="lbCommand_Click"></asp:LinkButton>
                        </li>-->
                    </ol>
                </td>
            </tr>
            <tr>
                <td>
                    <table align="center" border="0">
                        <tbody>
                            <tr>
                                <td class="copyright" colspan="2">
                                    服務電話： 0800-010-626 電子信箱：<a href="mailto:cds_service@uxb2b.com">cds_service@uxb2b.com</a>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="copyright" align="center">
                                        本網站由網際優勢股份有限公司建置 最佳瀏覽範圍1024*768 版權所有，轉載必究 Copyright (c) 2001,<a href="http://www.uxb2b.com/"
                                            target="_blank">http://www.uxb2b.com/</a> . All rights reserved.
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(published_causagenote_aspx_Load);
    }

    void published_causagenote_aspx_Load(object sender, EventArgs e)
    {
        litWebSite.Text = eIVOGo.Properties.Settings.Default.mailLinkAddress;
        lbCommand.Text = String.Format("%WINDIR%\\Microsoft.NET\\Framework\\v2.0.50727\\CasPol.exe -m -ag All_Code -site {0} FullTrust -name TrustedSite", Request.Url.Host);
    }
</script>