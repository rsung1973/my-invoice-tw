<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %><?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
            <section name="InvoiceClient.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>
    <applicationSettings>
        <InvoiceClient.Properties.Settings>
            <setting name="SignerSubjectName" serializeAs="String">
                <value>UXSigner</value>
            </setting>
            <setting name="SignerCspName" serializeAs="String">
                <value />
            </setting>
            <setting name="SignerKeyPassword" serializeAs="String">
                <value />
            </setting>
            <setting name="UploadInvoiceFolder" serializeAs="String">
                <value>Invoice</value>
            </setting>
            <setting name="UploadInvoiceCancellationFolder" serializeAs="String">
                <value>CancelInvoice</value>
            </setting>
            <setting name="AutoWelfareInterval" serializeAs="String">
                <value>30</value>
            </setting>
            <setting name="WelfareInfoFolder" serializeAs="String">
                <value>SWA</value>
            </setting>
            <setting name="IsAutoWelfare" serializeAs="String">
                <value>False</value>
            </setting>
            <setting name="OutputEncoding" serializeAs="String">
                <value>utf-8</value>
            </setting>
            <setting name="DownloadDataFolder" serializeAs="String">
                <value>傳送大平台資料</value>
            </setting>
            <setting name="IsAutoInvService" serializeAs="String">
                <value>True</value>
            </setting>
            <setting name="AutoInvServiceInterval" serializeAs="String">
                <value>30</value>
            </setting>
            <setting name="UploadAllowanceFolder" serializeAs="String">
                <value>Allowance</value>
            </setting>
            <setting name="UploadAllowanceCancellationFolder" serializeAs="String">
                <value>CancelAllowance</value>
            </setting>
            <setting name="DownloadInvoiceFolder" serializeAs="String">
                <value>傳送大平台資料\發票</value>
            </setting>
            <setting name="DownloadInvoiceCancellationFolder" serializeAs="String">
                <value>傳送大平台資料\作廢發票</value>
            </setting>
            <setting name="DownloadAllowanceFolder" serializeAs="String">
                <value>傳送大平台資料\折讓</value>
            </setting>
            <setting name="DownloadAllowanceCancellationFolder" serializeAs="String">
                <value>傳送大平台資料\作廢折讓</value>
            </setting>
            <setting name="DownloadWinningFolder" serializeAs="String">
                <value>BonusInvoice</value>
            </setting>
            <setting name="OutputEncodingWithoutBOM" serializeAs="String">
                <value>False</value>
            </setting>
            <setting name="DownloadDataInAbsolutePath" serializeAs="String">
                <value>False</value>
            </setting>
            <setting name="AppTitle" serializeAs="String">
                <value>電子發票傳輸服務－店家用戶端</value>
            </setting>
            <setting name="UploadCsvInvoiceFolder" serializeAs="String">
                <value>Invoice_csv</value>
            </setting>
            <setting name="UploadCsvInvoiceCancellationFolder" serializeAs="String">
                <value>CancelInvoice_csv</value>
            </setting>
            <setting name="DownloadInvoiceMapping" serializeAs="String">
                <value>InvoiceMap</value>
            </setting>
            <setting name="CsvEncoding" serializeAs="String">
                <value>Big5</value>
            </setting>
            <setting name="RootCA" serializeAs="String">
                <value>UXB2B Certificate Center.cer</value>
            </setting>
            <setting name="UploadBranchTrackFolder" serializeAs="String">
                <value>InvoiceNoAssignment</value>
            </setting>
            <setting name="UploadSellerInvoiceFolder" serializeAs="String">
                <value>SellerInvoice</value>
            </setting>
            <setting name="UploadPreInvoiceFolder" serializeAs="String">
                <value>PreInvoice</value>
            </setting>
            <setting name="UploadAttachmentFolder" serializeAs="String">
                <value>Attachment</value>
            </setting>
            <setting name="DownloadSaleInvoiceFolder" serializeAs="String">
                <value>InvoicePDF</value>
            </setting>
            <setting name="DownloadInvoiceMailTracking" serializeAs="String">
                <value>InvoiceMailTracking</value>
            </setting>
            <setting name="DownloadInvoiceReturnedMail" serializeAs="String">
                <value>ReturnedMail</value>
            </setting>
            <setting name="SellerReceiptNo" serializeAs="String">
                <value />
            </setting>
            <setting name="InvoiceTxnPath" serializeAs="String">
                <value><%= Request["tx"]!=null ? Request["tx"] : "C:\\UXB2B_EIVO" %></value>
            </setting>
            <setting name="MainTabs" serializeAs="String">
                <value>
                    InvoiceClient.MainContent.SystemConfigTab, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                </value>
            </setting>
            <setting name="UploadC0501InvoiceCancellationFolder" serializeAs="String">
                <value>CancelInvoice_C0501</value>
            </setting>
            <setting name="UploadC0401SellerInvoiceFolder" serializeAs="String">
                <value>SellerInvoice_C0401</value>
            </setting>
            <setting name="UploadInvoiceEnterprise" serializeAs="String">
                <value>Enterprise</value>
            </setting>
            <setting name="TrackCodeFolder" serializeAs="String">
                <value>InvoiceTrackCode</value>
            </setting>
            <setting name="VacantInvoiceNoFolder" serializeAs="String">
                <value>VacantInvoiceNo</value>
            </setting>
            <setting name="ServiceName" serializeAs="String">
                <value>EIVO03ClientService(<%= _settings[_svcType][0] %>)</value>
            </setting>
            <setting name="ResponseUpload" serializeAs="String">
                <value>True</value>
            </setting>
            <setting name="UploadAttachment" serializeAs="String">
                <value><%= _settings[_svcType][1] %>Published/UploadAttachmentForGoogle.ashx</value>
            </setting>
            <setting name="ActivationKey" serializeAs="String">
                <value><%= _item.OrganizationToken!=null ? _item.OrganizationToken.KeyID : null %></value>
            </setting>
            <setting name="InvoiceClient_WS_Invoice_eInvoiceService" serializeAs="String">
                <value><%= _settings[_svcType][1] %>Published/eInvoiceService.asmx</value>
            </setting>
            <setting name="TransferManager" serializeAs="String">
                <value>
                    <% if(_item.InvoiceInsurerAgent.Any()) { %>
                    InvoiceClient.TransferManagement.InvoiceTransferManagerForAgent, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    InvoiceClient.TransferManagement.BranchTrackTransferManager, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    InvoiceClient.TransferManagement.MIGInvoiceTransferManager, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    <% } else { 
                           if(Request["ut"]=="XML") {%>
                    InvoiceClient.TransferManagement.InvoiceTransferManagerV2, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                           <% }
                           else if (Request["ut"] == "CSV")
                           {  %>
                    InvoiceClient.TransferManagement.CsvInvoiceTransferManagerV2, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                           <% }
                           else if (Request["ut"] == "MIG")
                           {  %>
                    InvoiceClient.TransferManagement.MIGInvoiceTransferManager, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                            <% } else { %>
                    InvoiceClient.TransferManagement.InvoiceAllowanceTransferManager, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                            <% } %>
                    <% } %>
                </value>
            </setting>
            <setting name="ServerInspector" serializeAs="String">
                <value>
                    <% if(_item.OrganizationStatus.DownloadDataNumber == true) { %>
                    InvoiceClient.Agent.InvoiceMappingInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    <% }
                       if(_item.OrganizationStatus.UploadBranchTrackBlank == true) { %>
                    InvoiceClient.Agent.VacantInvoiceNoInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    <% }
                       if(_item.OrganizationStatus.DownloadDataNumber == true) { %>
                    InvoiceClient.Agent.InvoiceMappingInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    <% }
                       if(_item.InvoiceInsurerAgent.Any()) { %>
                    InvoiceClient.Agent.InvoiceTrackCodeInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                    <% } %>
                    InvoiceClient.Agent.InvoiceServerInspector, InvoiceClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;
                </value>
            </setting>
            <setting name="PGPWaitingForExitInMilliSeconds" serializeAs="String">
                <value>30000</value>
            </setting>
            <setting name="ClientID" serializeAs="String">
                <value></value>
            </setting>
        </InvoiceClient.Properties.Settings>
    </applicationSettings>
    <runtime>
        <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
            <!--<dependentAssembly>
        <assemblyIdentity name="Microsoft.WindowsAzure.Storage" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.3.0.0" newVersion="4.3.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Data.Edm" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-5.6.3.0" newVersion="5.6.3.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Data.Services.Client" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-5.6.3.0" newVersion="5.6.3.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Data.OData" publicKeyToken="31bf3856ad364e35" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-5.6.3.0" newVersion="5.6.3.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="HtmlAgilityPack" publicKeyToken="bd319b19eaf3b43a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-1.4.9.0" newVersion="1.4.9.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="AjaxMin" publicKeyToken="21ef50ce11b5d80f" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-5.14.5506.26196" newVersion="5.14.5506.26196" />
      </dependentAssembly>-->
        </assemblyBinding>
    </runtime>
</configuration>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<script runat="server">

    Organization _item;
    int _svcType;
    String[][] _settings = new String[][] {
        new String[]{"測試","http://eivo4all.uxifs.com/NewEivo03/"},
        new String[]{"正式","https://eceivo.uxifs.com/"}};

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //Response.ContentType = "text/xml";

        int companyID;
        if (Request.GetRequestValue("id", out companyID))
        {
            _item = dsEntity.CreateDataManager().EntityList.Where(o => o.CompanyID == companyID).FirstOrDefault();
        }

        if (!Request.GetRequestValue("st", out _svcType) || (_svcType > 1 && _svcType < 0))
        {
            _svcType = 0;
        }

        if (_item == null)
        {
            this.Visible = false;
        }
        else
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", "attachment;filename=InvoiceClient.exe.config");
        }
    }
</script>
