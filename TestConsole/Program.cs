using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DataEntity;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ModelSource<InvoiceItem> models = new ModelSource<InvoiceItem>())
            {
                int idx = 0;
                var items = models.Items.Select((i, x) => new
                {
                    RowIndex = x,
                    InvoiceNo = i.TrackCode + i.No
                }).Take(100).ToList();

            }
        }
    }
}
