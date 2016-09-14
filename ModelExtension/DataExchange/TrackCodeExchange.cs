using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Model.DataEntity;
using Model.InvoiceManagement;
using Utility;

namespace ModelExtension.DataExchange
{
    public class TrackCodeExchange
    {
        public enum ColumnIndex
        {
            期別 = 1, 
            發票類別, 
            字軌, 
            字軌種類,
            處理狀態
        }

        public XLWorkbook GetSample()
        {
            XLWorkbook xls = new XLWorkbook();

            var workSheet = xls.Worksheets.Add("發票字軌");

            var names = Enum.GetNames(typeof(ColumnIndex));
            for (int i = 0; i < names.Length - 1; i++)
                workSheet.Cell(1, i + 1).Value = names[i];

            var row = workSheet.Row(2);
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<InvoiceTrackCode>().Where(t => t.Year == DateTime.Today.Year - 1)
                    .OrderBy(t => t.PeriodNo).ThenBy(t => t.TrackCode);
                int colIdx;
                foreach (var item in items)
                {
                    colIdx = 1;
                    row.Cell(colIdx++).Value = String.Format("{0}{1:00}", item.Year - 1911, item.PeriodNo * 2);
                    row.Cell(colIdx).Style.NumberFormat.Format = "00";
                    row.Cell(colIdx).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    row.Cell(colIdx++).Value = "07";
                    row.Cell(colIdx++).Value = item.TrackCode;
                    row.Cell(colIdx++).Value = 4;

                    row = row.RowBelow();
                }
            }

            return xls;
        }

        public void ExchangeData(XLWorkbook xlwb)
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
                var table = mgr.GetTable<InvoiceTrackCode>();

                while (!row.Cell((int)ColumnIndex.期別).IsEmpty())
                {
                    var dataRow = row;
                    row = row.RowBelow();
                    try
                    {
                        String period = dataRow.Cell((int)ColumnIndex.期別).GetString();
                        int intVal;
                        if (String.IsNullOrEmpty(period))
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "未設定期別";
                            continue;
                        }
                        if (!int.TryParse(period, out intVal))
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "期別格式錯誤";
                            continue;
                        }

                        int year = (intVal / 100) + 1911;
                        int periodNo = (intVal % 100) / 2;

                        var cell = dataRow.Cell((int)ColumnIndex.字軌);
                        String trackCode;
                        if (cell.IsEmpty())
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "字軌錯誤";
                            continue;
                        }
                        else
                        {
                            trackCode = cell.GetString();
                        }

                        if (trackCode == null || !Regex.IsMatch(trackCode, "^[A-Z]{2}$"))
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "字軌格式錯誤";
                            continue;
                        }

                        var item = table.Where(i => i.TrackCode == trackCode
                            && i.Year == year && i.PeriodNo == periodNo).FirstOrDefault();
                        if (item != null)
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "字軌已存在";
                            continue;
                        }

                        cell = dataRow.Cell((int)ColumnIndex.發票類別);
                        if (!cell.IsEmpty() && cell.GetString() != "7")
                        {
                            dataRow.Cell((int)ColumnIndex.處理狀態).Value = "只接受發票類別07";
                            continue;
                        }

                        table.InsertOnSubmit(new InvoiceTrackCode
                        {
                            PeriodNo = (short)periodNo,
                            TrackCode = trackCode,
                            Year = (short)year
                        });

                        mgr.SubmitChanges();
                        dataRow.Cell((int)ColumnIndex.處理狀態).Value = "字軌已新增";
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
