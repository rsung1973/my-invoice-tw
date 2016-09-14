<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="menuItem">
    <menuItem>
      <xsl:attribute name="value">
        <xsl:value-of select="@value" />
      </xsl:attribute>
      <xsl:attribute name="url">
        <xsl:value-of select="@url" />
      </xsl:attribute>
      <xsl:for-each select="menuItem">
        <xsl:apply-templates select="."></xsl:apply-templates>
      </xsl:for-each>
    </menuItem>
  </xsl:template>
</xsl:stylesheet>
