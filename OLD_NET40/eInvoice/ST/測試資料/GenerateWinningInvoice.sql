/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
'<InvoiceRoot>
  <Invoice>
    <InvoiceNumber>'+F4+F5+'</InvoiceNumber>
    <InvoiceDate>2010/12/23</InvoiceDate>
    <InvoiceTime>14:24:00</InvoiceTime>
    <SellerId>' + F7 + '</SellerId>
    <BuyerName>N/A</BuyerName>
    <BuyerId />
    <InvoiceType>03</InvoiceType>
    <DonateMark>0</DonateMark>
    <CarrierType>'+F13+'</CarrierType>
    <CarrierId1>'+F15+'</CarrierId1>
    <CarrierId2>'+F16+'</CarrierId2>
    <PrintMark>N</PrintMark>
    <NPOBAN>N/A</NPOBAN>
    <RandomNumber>ONJU</RandomNumber>
    <InvoiceItem>
      <Description>康寶-吉比柔滑花生醬</Description>
      <Quantity>1</Quantity>
      <Unit>N/A</Unit>
      <UnitPrice>110</UnitPrice>
      <Amount>110</Amount>
      <SequenceNumber>1</SequenceNumber>
      <Item>N/A</Item>
      <Remark>N/A</Remark>
      <TaxType>1</TaxType>
    </InvoiceItem>
    <InvoiceItem>
      <Description>冠軍花生麵筋</Description>
      <Quantity>1</Quantity>
      <Unit>N/A</Unit>
      <UnitPrice>25</UnitPrice>
      <Amount>25</Amount>
      <SequenceNumber>2</SequenceNumber>
      <Item>N/A</Item>
      <Remark>N/A</Remark>
      <TaxType>1</TaxType>
    </InvoiceItem>
    <InvoiceItem>
      <Description>小計</Description>
      <Quantity>0</Quantity>
      <Unit>N/A</Unit>
      <UnitPrice>0</UnitPrice>
      <Amount>135</Amount>
      <SequenceNumber>3</SequenceNumber>
      <Item>N/A</Item>
      <Remark>N/A</Remark>
      <TaxType>1</TaxType>
    </InvoiceItem>
    <SalesAmount>129</SalesAmount>
    <FreeTaxSalesAmount>0</FreeTaxSalesAmount>
    <ZeroTaxSalesAmount>0</ZeroTaxSalesAmount>
    <TaxType>1</TaxType>
    <TaxRate>0.05</TaxRate>
    <TaxAmount>6</TaxAmount>
    <TotalAmount>'+F12+'</TotalAmount>
    <DiscountAmount>0</DiscountAmount>
  </Invoice>
</InvoiceRoot>'
  FROM [tempdb].[dbo].[WinningNumber]
  
  
  
  
  insert into eInvoice.dbo.InvoiceUserCarrierType (CarrierType,Description)
  SELECT F13, F14
FROM  WinningNumber
where F13 is not null
GROUP BY F13, F14

insert into eInvoice.dbo.InvoiceUserCarrier (CarrierNo,CarrierNo2,TypeID)
SELECT w.F15,F16,c.TypeID
FROM  WinningNumber AS w INNER JOIN
               eInvoice.dbo.InvoiceUserCarrierType AS c ON w.F13 = c.CarrierType
where not exists (select null 
 from  eInvoice.dbo.InvoiceUserCarrier where CarrierNo=F15 and CarrierNo2=F16)              
 group by w.F15,F16,c.TypeID
 
 insert into eInvoice.dbo.Organization (CompanyName,ReceiptNo,Addr)
 select F6,F7,F9 from winningNumber where F7 is not null group by F6,F7,F9
 
 
 
 

