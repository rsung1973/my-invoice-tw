<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoicePrintView.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.InvoicePrintView" %>
<%@ Register Src="Item/InvoiceProductPrintView.ascx" TagName="InvoiceProductPrintView"
    TagPrefix="uc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Register Src="Item/InvoiceReceiptView.ascx" TagName="InvoiceReceiptView" TagPrefix="uc2" %>
<%@ Register Src="Item/InvoiceBalanceView.ascx" TagName="InvoiceBalanceView" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<div class="fspace">
    <div class="company">
        <span class="title">美商科高國際有限公司 台灣分公司</span><br />
        <p>
            台北市信義區信義路5段7號73樓之1<br />
            <%# _item.Organization.OrganizationStatus.SetToOutsourcingCS == true ? "委外客服電話：0800-010-026" : ""%></p>
    </div>
    <div class="customer">
        <p>
            <%# _buyer.PostCode %><br />
            <%# _buyer.Address %><br />
            <%# _buyer.ContactName %>
            鈞啟</p>
    </div>
</div>
<p style="border-bottom: 1px dotted #000; margin-bottom: 15px">
</p>
<div class="Khaki">
    <uc2:InvoiceReceiptView ID="receiptView" runat="server" Item="<%# _item %>" />
</div>
<p style="border-bottom: 1px dotted #000; margin-top: 13px; margin-bottom: 10px;">
</p>
<div class="or">
    <uc3:InvoiceBalanceView ID="balanceView" runat="server" Item="<%# _item %>" />
</div>
<p style="page-break-after: always">
</p>
<div class="br" id="backPage" runat="server" visible="<%# printBack %>" style="page-break-after: always">
    <div style="border-bottom: #000 1px dotted; margin-bottom: 15px" class="bspace" id="b2cAnnouncement"
        runat="server" visible="<%# _buyer.IsB2C() %>">
        <h3>
            Google電子發票使用說明</h3>
        <p class="note">
            Google為響應台灣政府環保政策，於2011年推動「電子發票」，特此敬告長期支持Google的貴賓們，我們將採循序漸進的方式，<br />
            不躁進的逐步轉換，若您有任何寶貴意見，請不吝賜教。</p>
        <ul>
            <li>問：何謂電子發票?<br />
                答：賣方將消費者的交易明細與發票號碼透過e-mail通知消費者，並將發票即時上傳財政部電子發票整合服務平台，<br />
                消費者可於該平台查詢所有消費紀錄，網址：http://www.einvoice.nat.gov.tw全民稽核發票資料查詢系統。</li>
            <li>問：如何對領獎?<br />
                答：Google已取得財政部北市國稅局信義分局核准(財北國稅信義營業字第1000213452號函)，針對接收電子發票的消費者，提供自動對獎服務，<br />
                若Google開給您的 二聯式電子發票中獎，我們將會以e-mail通知消費者，採掛號方式，將中獎紙本發票送達消費者手中，<br />
                消費者持中獎紙本發票，至郵局領獎。</li>
            <li>問：Google何時開始全面實施電子發票，不再寄發紙本發票?<br />
                答：Google本於服務客戶精神，會於大多數消費者都表態支持時，全面實施，全面實施前會向消費者公告。 </li>
        </ul>
    </div>
    <div class="bspace" style="margin-bottom: 15px; border-bottom-color: #000; border-bottom-width: 1px;
        border-bottom-style: dotted;" id="b2bAnnouncement" runat="server" visible="<%# !_buyer.IsB2C() %>">
        <h3>
            Google電子發票使用說明
        </h3>
        <p class="note">
            Google為響應台灣政府環保政策，於2011年推動「電子發票」，特此敬告長期支持Google的貴賓們，我們將採循序漸進的方式，<br />
            逐步轉換，在未公告實施前，仍採紙本發票寄送，若您有任何寶貴意見，請不吝賜教。
        </p>
        <ul>
            <li>問：何謂電子發票?<br />
                答：賣方將買方的發票電子檔透過e-mail提供給買方，買方視需求自行存檔或列印。 </li>
            <li>問：買方自行列印的黑白電子發票，是否可作為報稅憑證?<br />
                答：Google已取得財政部北市國稅局信義分局核准(財北國稅信義營業字第1000213452號函) ，此為合法之憑證。 </li>
            <li>問：Google何時開始全面實施電子發票，不再寄發紙本發票?<br />
                答：Google本於服務客戶精神，會於大多數客戶都表態支持時，全面實施，全面實施前會向客戶公告。 </li>
        </ul>
    </div>
    <table width="95%" border="0" align="center" cellpadding="3" cellspacing="0">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td height="30" colspan="2" align="center" class="title">
                            統一發票領獎注意事項
                        </td>
                    </tr>
                    <tr>
                        <td width="50%" valign="top">
                            <div class="notice" style="float: right;">
                                <p>
                                    一、統一發票之給獎，依統一發票給獎辦法之規定辦理。</p>
                                <p>
                                    二、各期統一發票開獎日期及領獎期間如下，領獎末日如遇假日順延至次一上班日：</p>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table-br" style="margin-bottom: 5px;">
                                    <tr>
                                        <td align="center">
                                            期 別
                                        </td>
                                        <td align="center">
                                            開獎日期
                                        </td>
                                        <td align="center">
                                            領獎期間
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" nowrap="nowrap">
                                            1～2月
                                        </td>
                                        <td align="center">
                                            3/25
                                        </td>
                                        <td align="center">
                                            4/6～7/5
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            3～4月
                                        </td>
                                        <td align="center">
                                            5/25
                                        </td>
                                        <td align="center">
                                            6/6～9/5
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            5～6月
                                        </td>
                                        <td align="center">
                                            7/25
                                        </td>
                                        <td align="center">
                                            8/6～11/5
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            7～8月
                                        </td>
                                        <td align="center">
                                            9/25
                                        </td>
                                        <td align="center">
                                            10/6～1/5
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            9～10月
                                        </td>
                                        <td align="center">
                                            11/25
                                        </td>
                                        <td align="center">
                                            12/6～3/5
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            11～12月
                                        </td>
                                        <td align="center">
                                            1/25
                                        </td>
                                        <td align="center">
                                            2/6～5/5
                                        </td>
                                    </tr>
                                </table>
                                <p>
                                    三、中獎人請於領獎期間內攜帶國民身分證(非本國國籍人士得以護照、居留證等文件替代)及中獎統一發票收執聯，向下列郵局兌領，並應於公告之兌獎領獎時間內為之，逾期則不得領獎。</p>
                                <p class="sublist">
                                    (一)特別獎、特獎、頭獎為各直轄市及各縣、市之指定郵局。</p>
                                <p class="sublist">
                                    (二)二獎、三獎、四獎、五獎及六獎為各地郵局。</p>
                                <p>
                                    四、對獎若有疑義，請洽郵局服務專線<br />
                                    電話：(02)2396-1651。</p>
                            </div>
                        </td>
                        <td align="left" valign="top">
                            <div class="notice">
                                <table style="margin-bottom: 5px;" class="table-br" border="0" cellspacing="0" cellpadding="0"
                                    width="100%">
                                    <tbody>
                                        <tr>
                                            <td colspan="11" align="center">
                                                領 獎 收 據
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11" align="center">
                                                貼 用 印 花
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="35%" nowrap="nowrap" align="center">
                                                金 額
                                            </td>
                                            <td colspan="10" align="right">
                                                新台幣<br />
                                                元 整
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="35%" nowrap="nowrap" align="center">
                                                中 獎 人
                                            </td>
                                            <td style="padding: 0px;" colspan="10" align="center">
                                                <div style="margin: 0px auto; width: 30px; padding-top: 2px; border-right-color: rgb(51, 51, 121);
                                                    border-left-color: rgb(51, 51, 121); border-right-width: 1px; border-left-width: 1px;
                                                    border-right-style: solid; border-left-style: solid;">
                                                    簽名<br />
                                                    或<br />
                                                    蓋章</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="35%" nowrap="nowrap" align="center">
                                                國民身分證<br />
                                                統 一 編 號
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="35%" nowrap="nowrap" align="center">
                                                戶 籍<br />
                                                地 址
                                            </td>
                                            <td colspan="10" align="center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="35%" nowrap="nowrap" align="center">
                                                電 話
                                            </td>
                                            <td colspan="10" align="center">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div class="tel">
                                    檢舉不法逃漏稅，請寫真實姓名地址，寄<br />
                                    營業人所在地稽徵機關。<br />
                                    國稅局全國免費服務專線：<br />
                                    0800-000-321<br />
                                    檢舉貪瀆不法信箱：台北郵政5-75號信箱</div>
                                <div class="urllink">
                                    查詢中獎號碼網址：<br />
                                    http://www.dot.gov.tw</div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<script runat="server">
    bool printBack = false;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (!String.IsNullOrEmpty(Request["printBack"]))
        {
            bool.TryParse(Request["printBack"], out printBack);
        }
    }
</script>
