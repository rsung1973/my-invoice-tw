using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Model.DataEntity;
using Model.InvoiceManagement;
using Utility;

namespace ModelExtension.DataExchange
{
    public class InvoiceBuyerExchange
    {
        public enum ColumnIndex
        {
            發票號碼 = 1,
            營業人名稱,
            收件人姓名,
            地址,
            電話,
            EMail,
            處理狀態
        }

        public XLWorkbook GetSample()
        {
            XLWorkbook xls = new XLWorkbook();

            var workSheet = xls.Worksheets.Add("買受人");
            workSheet.Cells("A1").Value = "發票號碼";
            workSheet.Cells("B1").Value = "營業人名稱";
            workSheet.Cells("C1").Value = "收件人姓名";
            workSheet.Cells("D1").Value = "地址";
            workSheet.Cells("E1").Value = "電話";
            workSheet.Cells("F1").Value = "EMail";

            return xls;
        }

        public void ExchangeData(XLWorkbook xlwb,Func<InvoiceItem,bool> checkItem = null)
        {
            if (xlwb.Worksheets.Count < 1)
                return;
            var ws = xlwb.Worksheet(1);
            var firstRow = ws.FirstRowUsed();
            if (firstRow == null)
                return;
            var row = firstRow.RowUsed();
            if (row == null)
                return;

            ///title row
            ///
            row.Cell((int)ColumnIndex.處理狀態).Value = ColumnIndex.處理狀態.ToString();

            ///data row
            row = row.RowBelow();
            using (InvoiceManager mgr = new InvoiceManager())
            {
                while (!row.Cell((int)ColumnIndex.發票號碼).IsEmpty())
                {
                    var dataRow = row;
                    row = row.RowBelow();
                    try
                    {
                        String invoiceNo = dataRow.Cell((int)ColumnIndex.發票號碼).GetString();
                        if (String.IsNullOrEmpty(invoiceNo))
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "未設定發票號碼";
                            continue;
                        }
                        if (invoiceNo.Length != 10)
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "發票號碼格式錯誤";
                            continue;
                        }
                        var item = mgr.EntityList.Where(i => i.TrackCode == invoiceNo.Substring(0, 2) && i.No == invoiceNo.Substring(2)).FirstOrDefault();
                        if (item == null || (checkItem!=null && !checkItem(item)))
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "發票不存在";
                            continue;
                        }
                        var buyer = item.InvoiceBuyer;
                        if (buyer == null)
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "買受人資料不存在";
                            continue;
                        }

                        var cell = dataRow.Cell((int)ColumnIndex.EMail);
                        if (cell.IsEmpty())
                        {
                            cell.Value = buyer.EMail;
                        }
                        else
                        {
                            buyer.EMail = cell.GetString();
                        }
                        cell = dataRow.Cell((int)ColumnIndex.地址);
                        if (cell.IsEmpty())
                        {
                            cell.Value = buyer.Address;
                        }
                        else
                        {
                            buyer.Address = cell.GetString();
                        }
                        cell = dataRow.Cell((int)ColumnIndex.收件人姓名);
                        if (cell.IsEmpty())
                        {
                            cell.Value = buyer.ContactName;
                        }
                        else
                        {
                            buyer.ContactName = cell.GetString();
                        }
                        cell = dataRow.Cell((int)ColumnIndex.營業人名稱);
                        if (cell.IsEmpty())
                        {
                            cell.Value = buyer.CustomerName;
                        }
                        else
                        {
                            buyer.CustomerName = cell.GetString();
                        }
                        cell = dataRow.Cell((int)ColumnIndex.電話);
                        if (cell.IsEmpty())
                        {
                            cell.Value = buyer.Phone;
                        }
                        else
                        {
                            buyer.Phone = cell.GetString();
                        }

                        mgr.SubmitChanges();
                        dataRow.Cell((int)ColumnIndex.處理狀態).Value = "已更新";
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        dataRow.Cell((int)ColumnIndex.處理狀態).Value = ex.Message;
                    }
                }
            }
        }
    }
}
