SELECT 'Web' AS [Data Source], i.TrackCode + i.No AS InvoiceNo, i.InvoiceDate, t.SalesAmount, t.TotalAmount, b.ReceiptNo, b.Address, b.Name, b.ContactName, b.EMail, b.CustomerName, p.OrderNo as [Data Number], b.CustomerID as GoogleID
FROM  CDS_Document AS d INNER JOIN
          InvoiceItem AS i ON d.DocID = i.InvoiceID INNER JOIN
          InvoiceBuyer AS b ON i.InvoiceID = b.InvoiceID INNER JOIN
          InvoiceAmountType AS t ON i.InvoiceID = t.InvoiceID INNER JOIN
              (SELECT InvoiceAmountType_1.SalesAmount, InvoiceAmountType_1.TotalAmount, InvoiceBuyer_1.ReceiptNo, InvoiceBuyer_1.Address, InvoiceBuyer_1.Name, InvoiceBuyer_1.ContactName, InvoiceBuyer_1.EMail, InvoiceBuyer_1.CustomerName
             FROM   CDS_Document AS CDS_Document_1 INNER JOIN
                        InvoiceItem AS InvoiceItem_1 ON CDS_Document_1.DocID = InvoiceItem_1.InvoiceID INNER JOIN
                        InvoiceBuyer AS InvoiceBuyer_1 ON InvoiceItem_1.InvoiceID = InvoiceBuyer_1.InvoiceID INNER JOIN
                        InvoiceAmountType AS InvoiceAmountType_1 ON InvoiceItem_1.InvoiceID = InvoiceAmountType_1.InvoiceID
             WHERE (CDS_Document_1.ChannelID IS NULL) AND (InvoiceItem_1.SellerID = 2359) AND (InvoiceItem_1.InvoiceDate >= '2014/11/1')
             GROUP BY InvoiceAmountType_1.SalesAmount, InvoiceAmountType_1.TotalAmount, InvoiceBuyer_1.ReceiptNo, InvoiceBuyer_1.Address, InvoiceBuyer_1.Name, InvoiceBuyer_1.ContactName, InvoiceBuyer_1.EMail, InvoiceBuyer_1.CustomerName) 
          AS s ON t.SalesAmount = s.SalesAmount AND t.TotalAmount = s.TotalAmount AND b.ReceiptNo = s.ReceiptNo AND b.ContactName = s.ContactName AND b.EMail = s.EMail AND b.CustomerName = s.CustomerName INNER JOIN
          InvoicePurchaseOrder AS p ON i.InvoiceID = p.InvoiceID
WHERE (d.ChannelID IS NOT NULL) AND (i.SellerID = 2359) AND (i.InvoiceDate >= '2014/11/1')
UNION
SELECT 'Ftp' AS [Data Source], i.TrackCode + i.No AS InvoiceNo, i.InvoiceDate, t.SalesAmount, t.TotalAmount, b.ReceiptNo, b.Address, b.Name, b.ContactName, b.EMail, b.CustomerName, p.OrderNo as [Data Number], b.CustomerID as GoogleID
FROM  InvoiceItem AS i INNER JOIN
          CDS_Document AS d ON i.InvoiceID = d.DocID INNER JOIN
          InvoiceBuyer AS b ON i.InvoiceID = b.InvoiceID INNER JOIN
          InvoiceAmountType AS t ON i.InvoiceID = t.InvoiceID INNER JOIN
              (SELECT InvoiceAmountType_1.SalesAmount, InvoiceAmountType_1.TotalAmount, InvoiceBuyer_1.ReceiptNo, InvoiceBuyer_1.Address, InvoiceBuyer_1.Name, InvoiceBuyer_1.ContactName, InvoiceBuyer_1.EMail, InvoiceBuyer_1.CustomerName
             FROM   CDS_Document AS CDS_Document_1 INNER JOIN
                        InvoiceItem AS InvoiceItem_1 ON CDS_Document_1.DocID = InvoiceItem_1.InvoiceID INNER JOIN
                        InvoiceBuyer AS InvoiceBuyer_1 ON InvoiceItem_1.InvoiceID = InvoiceBuyer_1.InvoiceID INNER JOIN
                        InvoiceAmountType AS InvoiceAmountType_1 ON InvoiceItem_1.InvoiceID = InvoiceAmountType_1.InvoiceID
             WHERE (CDS_Document_1.ChannelID IS NOT NULL) AND (InvoiceItem_1.SellerID = 2359) AND (InvoiceItem_1.InvoiceDate >= '2014/11/1')
             GROUP BY InvoiceAmountType_1.SalesAmount, InvoiceAmountType_1.TotalAmount, InvoiceBuyer_1.ReceiptNo, InvoiceBuyer_1.Address, InvoiceBuyer_1.Name, InvoiceBuyer_1.ContactName, InvoiceBuyer_1.EMail, InvoiceBuyer_1.CustomerName) 
          AS s ON t.SalesAmount = s.SalesAmount AND t.TotalAmount = s.TotalAmount AND b.ReceiptNo = s.ReceiptNo AND b.ContactName = s.ContactName AND b.EMail = s.EMail AND b.CustomerName = s.CustomerName INNER JOIN
          InvoicePurchaseOrder AS p ON i.InvoiceID = p.InvoiceID
WHERE (d.ChannelID IS NULL) AND (i.SellerID = 2359) AND (i.InvoiceDate >= '2014/11/1')
ORDER BY b.ContactName, b.CustomerName, b.Address, t.TotalAmount, t.SalesAmount, i.InvoiceDate
go

SELECT InvoiceItem.TrackCode, InvoiceItem.No, InvoiceItem.InvoiceDate, CDS_Document.ChannelID, InvoiceAmountType.SalesAmount, InvoiceAmountType.TotalAmount, InvoiceBuyer.ReceiptNo, InvoiceBuyer.Name, InvoiceBuyer.ContactName, 
          InvoiceBuyer.EMail, InvoiceBuyer.CustomerName, InvoiceBuyer.Address
FROM  CDS_Document INNER JOIN
          InvoiceItem ON CDS_Document.DocID = InvoiceItem.InvoiceID INNER JOIN
          InvoiceBuyer ON InvoiceItem.InvoiceID = InvoiceBuyer.InvoiceID INNER JOIN
          InvoiceAmountType ON InvoiceItem.InvoiceID = InvoiceAmountType.InvoiceID
WHERE (InvoiceItem.SellerID = 2359) AND (InvoiceItem.InvoiceDate >= '2014/11/1')
ORDER BY InvoiceBuyer.CustomerName, InvoiceBuyer.ContactName, InvoiceBuyer.Address, InvoiceBuyer.EMail, InvoiceBuyer.Name, InvoiceItem.InvoiceID
go
SELECT InvoiceItem.InvoiceID, InvoicePurchaseOrder.OrderNo, InvoiceItem.TrackCode, InvoiceItem.No, CDS_Document.DocID, CDS_Document.ChannelID
FROM  CDS_Document INNER JOIN
          InvoiceItem ON CDS_Document.DocID = InvoiceItem.InvoiceID INNER JOIN
          InvoicePurchaseOrder ON InvoiceItem.InvoiceID = InvoicePurchaseOrder.InvoiceID
WHERE (InvoiceItem.SellerID = 2359) AND (InvoiceItem.InvoiceDate >= '2014/11/1') AND (LEN(InvoicePurchaseOrder.OrderNo) < 26)