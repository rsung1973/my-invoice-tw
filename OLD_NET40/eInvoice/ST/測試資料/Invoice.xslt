<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:fx="#fx-functions" exclude-result-prefixes="msxsl fx">
  <xsl:output method="xml" indent="yes" omit-xml-declaration="no"/>
  <xsl:template match="/">
    <InvoiceRoot>
      <CompanyBan>70762419</CompanyBan>
   <!--發票可以有多張--> 
        <xsl:for-each select="//Context">
        <xsl:apply-templates select=".">
        </xsl:apply-templates>
      </xsl:for-each>
    </InvoiceRoot>
  </xsl:template>

  <xsl:template match="Context">
    <Invoice>
      <InvoiceNumber>
        <xsl:value-of select="No"/>
      </InvoiceNumber>
      <InvoiceDate>
        <xsl:value-of select="fx:getInvoiceDate(Date)"/>
      </InvoiceDate>
      <InvoiceTime>13:30:30</InvoiceTime>
      <SellerId>70762419</SellerId>
      <BuyerName>
        <xsl:value-of select="BuyerName"/>
      </BuyerName>
      <BuyerId>
        <xsl:value-of select="BuyerReceiptNo"/>
      </BuyerId>
      <InvoiceType>06</InvoiceType>
      <DonateMark>0</DonateMark>
      <CarrierType>AA0001</CarrierType>
      <CarrierId1>N/A</CarrierId1>
      <CarrierId2>3378932131</CarrierId2>
      <PrintMark>N</PrintMark>
      <NPOBAN>N/A</NPOBAN>
      <RandomNumber>1111</RandomNumber>
      <xsl:apply-templates select="Details"/>
      <SalesAmount>
        <xsl:value-of select="CostAmount"/>
      </SalesAmount>
      <FreeTaxSalesAmount>0</FreeTaxSalesAmount>
      <ZeroTaxSalesAmount>0</ZeroTaxSalesAmount>
      <TaxType>1</TaxType>
      <TaxRate>0.05</TaxRate>
      <TaxAmount>
        <xsl:apply-templates select="Tax/TaxAmount"/>
      </TaxAmount>
      <TotalAmount>
        <xsl:value-of select="GrandAmount"/>
      </TotalAmount>
      <DiscountAmount>
        <xsl:value-of select="Details/Discount/Amount"/>
      </DiscountAmount>
    </Invoice>

  </xsl:template>

  <xsl:template match="CalcSubtotal">
    <calcSubtotal>
      <xsl:value-of select="."/>
    </calcSubtotal>
  </xsl:template>
  <xsl:template match="CalcDiscount">
    <calcDiscount>
      <xsl:value-of select="."/>
    </calcDiscount>
  </xsl:template>
  <xsl:template match="AppendantPay">
    <appendantPay>
      <xsl:value-of select="."/>
    </appendantPay>
  </xsl:template>
  <xsl:template match="AppendSubtotalPreTax">
    <appendSubtotalPreTax>
      <xsl:value-of select="."/>
    </appendSubtotalPreTax>
  </xsl:template>
  <xsl:template match="interestRate">
    <interestRate>
      <xsl:value-of select="."/>
    </interestRate>
  </xsl:template>

  <xsl:template match="Dutiable">
    <dutiable>
      <xsl:value-of select="."/>
    </dutiable>
  </xsl:template>
  <xsl:template match="ZeroTaxRate">
    <zeroTaxRate>
      <xsl:value-of select="."/>
    </zeroTaxRate>
  </xsl:template>
  <xsl:template match="DutyFree">
    <dutyFree>
      <xsl:value-of select="."/>
    </dutyFree>
  </xsl:template>
  <xsl:template match="TaxAmount">
    <xsl:value-of select="."/>
    <!--<salesTax>
      <xsl:value-of select="."/>
    </salesTax>-->
  </xsl:template>

  <xsl:template match="Details">
    <xsl:for-each select="Item">
      <InvoiceItem>
        <Description>
          <xsl:value-of select="ProductName"/>
          <xsl:value-of select="Specification"/>
        </Description>
        <Quantity>
          <xsl:value-of select="Piece"/>
        </Quantity>
        <Unit>件</Unit>
        <UnitPrice>
          <xsl:value-of select="UnitCost"/>
        </UnitPrice>
        <Amount>
          <xsl:value-of select="CostAmount"/>
        </Amount>
        <SequenceNumber>
          <xsl:value-of select="position()"/>
        </SequenceNumber>
        <Item>
          <xsl:value-of select="ItemNo"/>
        </Item>
        <Remark>TX</Remark>
        <TaxType>1</TaxType>
      </InvoiceItem>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="OriginalPrice">
    <originalPrice>
      <xsl:value-of select="."/>
    </originalPrice>
  </xsl:template>

  <msxsl:script language="C#" implements-prefix="fx" xmlns:msxls="urn:schemas-microsoft-com:xslt">
    <![CDATA[
		public string getInvoiceDate(XPathNodeIterator node)
		{
      if(node.MoveNext())
      {
        int year = int.Parse(node.Current.SelectSingleNode("Year").Value)+1911;
        int month = int.Parse(node.Current.SelectSingleNode("Month").Value);
        int day = int.Parse(node.Current.SelectSingleNode("Day").Value);
        
        return String.Format("{0:0000}/{1:00}/{2:00}",year,month,day);
      }
      else
        return null;
		}
  ]]>
  </msxsl:script>

</xsl:stylesheet>
