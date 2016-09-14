<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/Common/PrintingButton2.ascx" TagPrefix="uc4" TagName="PrintingButton2" %>
<%@ Register Src="~/Module/Common/SaveAsExcelButton.ascx" TagPrefix="uc4" TagName="SaveAsExcelButton" %>
<uc4:PrintingButton2 ID="PrintingButton21" runat="server" />&nbsp;&nbsp;
<uc4:SaveAsExcelButton ID="SaveAsExcelButton1" runat="server" />

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        this.PrintingButton21.BeforeClick += new EventHandler(PrintingButton21_BeforeClick);
        this.SaveAsExcelButton1.BeforeClick += new EventHandler(SaveAsExcelButton1_BeforeClick);
        this.PrintingButton21.btnPrint.Text = "資料列印";
    }

    public Action BeforeSaving
    { get; set; }

    public Action BeforePrinting
    { get; set; }

    [System.ComponentModel.Bindable(true)]
    public String OutputFileName
    {
        get
        {
            return SaveAsExcelButton1.OutputFileName;
        }
        set
        {
            SaveAsExcelButton1.OutputFileName = value;
        }
    }

    public Control QueryResultList
    {
        set
        {
            this.PrintingButton21.PrintControls.Add(value);
            this.SaveAsExcelButton1.DownloadControls.Add(value);
        }
    }    

    void SaveAsExcelButton1_BeforeClick(object sender, EventArgs e)
    {
        if(BeforeSaving!=null)
            BeforeSaving();
    }

    void PrintingButton21_BeforeClick(object sender, EventArgs e)
    {
        if (BeforePrinting != null)
            BeforePrinting();
    }    
        
</script>