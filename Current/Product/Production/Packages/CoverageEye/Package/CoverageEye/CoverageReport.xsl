<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html"/>

  <xsl:variable name="AssemblyCount" select="count(/cruisecontrol/build/root/Assembly)" />
  <xsl:variable name="TotalLines" select="sum(/cruisecontrol/build/root/Assembly/@InstructionCount)" />
  <xsl:variable name="TotalCoveredLines" select="sum(/cruisecontrol/build/root/Assembly/@CoveredCount)" />
  <xsl:variable name="TotalPercentCoverage" select="round($TotalCoveredLines div $TotalLines * 100)" />
	
  <xsl:template match="/">
		<HTML>
			<HEAD>
				<TITLE/>
        <STYLE>H4 { height:10px; font: 9pt Courier New;  }</STYLE>
			  </HEAD>
			  <BODY>
          <TABLE width="800px" align="left" border="0">
            <TR>
              <td>
                <h2>Summary</h2>
              </td>
            </TR>
            <TR>
              <td>
                <H4>
                  Assembly Count :<xsl:value-of select="$AssemblyCount"/>
                </H4>
              </td>
            </TR>
            <TR>
              <td>
                <h3>Total Percent Coverage</h3>
              </td>
            </TR>
            <TR>
              <TD style="width:300px;" valign="bottom">
                <table>
                  <td>
                    <TABLE style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                      <TR>
                        <xsl:call-template name="CreateBar">
                          <xsl:with-param name="width" select="$TotalPercentCoverage"></xsl:with-param>
                          <xsl:with-param name="colour">Green</xsl:with-param>
                        </xsl:call-template>
                        <xsl:call-template name="CreateBar">
                          <xsl:with-param name="width" select="100 - ($TotalPercentCoverage)"></xsl:with-param>
                          <xsl:with-param name="colour">Red</xsl:with-param>
                        </xsl:call-template>
                      </TR>
                    </TABLE>
                  </td>
                  <td>
                    <xsl:value-of select="$TotalCoveredLines"/>/<xsl:value-of select="$TotalLines"/>&#160;<xsl:value-of select="$TotalPercentCoverage"/>%
                  </td>
                </table>
              </TD>
            </TR>
            <TR>
              <td>
                <hr/>
                <hr/>
                <h2>Assemblies</h2>
                <hr/>
              </td>
            </TR>
            <TR>
              <td>
                <xsl:call-template name="AssemblyTable"/>
              </td>
            </TR>
            <TR>
              <td>
                <xsl:call-template name="AssemblyDetails"/>
              </td>
            </TR>
          </TABLE>
        </BODY>
      </HTML>
  </xsl:template>

  <xsl:template name="AssemblyTable">
    <table style="width:100%;" border="1" cellpadding="0" cellspacing="0">
      <tr>
        <td>Assembly</td>
        <td>Percent Coverage</td>
        <td>Lines Covered</td>
        <td>Total Lines</td>
      </tr>
      <xsl:for-each select="cruisecontrol/build/root/Assembly">
        <tr>
          <td>
            <a>
              <xsl:attribute name="href">#<xsl:value-of select="@AssemblyName"/>
            </xsl:attribute>
              <xsl:value-of select="@AssemblyName"/>
            </a>
          </td>
          <td>
            <xsl:value-of select="@PercentageCovered"/>%
          </td>
          <td>
            <xsl:value-of select="@CoveredCount"/>
          </td>
          <td>
            <xsl:value-of select="@InstructionCount"/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template name="AssemblyDetails">
    <table>
      <xsl:for-each select="cruisecontrol/build/root/Assembly">
        <tr>
          <td>
            <xsl:call-template name="AssemblyDetail"/>
          </td>
        </tr>
        <tr>
          <td border="1">
            <hr/>
            <hr/>
          </td>
        </tr>
      </xsl:for-each>
    </table>
	</xsl:template>
	
  <xsl:template name="AssemblyDetail">

		<TABLE width="800px" align="left" border="0">
			<TR>
				<td colspan="3">
					<H3>
            <a>
              <xsl:attribute name="name">
                <xsl:value-of select="@AssemblyName"/>
              </xsl:attribute>
            </a>
						Assembly Path :<xsl:value-of select="@AssemblyName"/>
					</H3>					
				</td>				
			</TR>
			<TR>
				<td>
					<H4>
						Total Coverage
					</H4>					
				</td>
				<td colspan="2">
					<H4>
						Total IL Instructions
					</H4>					
				</td>				
			</TR>
			<TR>				
				<TD style="width:300px;" valign="bottom">	
					<TABLE style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					<TR>				
						<xsl:call-template name="CreateBar">
							<xsl:with-param name="width" select="@PercentageCovered"></xsl:with-param>
							<xsl:with-param name="colour">Green</xsl:with-param>
						</xsl:call-template>
						<xsl:call-template name="CreateBar">
							<xsl:with-param name="width" select="100 - @PercentageCovered"></xsl:with-param>
							<xsl:with-param name="colour">Red</xsl:with-param>
						</xsl:call-template>	
					</TR>
					</TABLE>		
				</TD>	
				<TD colspan="2" style="height:10px; font: 9pt Courier New;">
          <xsl:value-of select="@CoveredCount"/>/<xsl:value-of select="@InstructionCount"/>&#160;<xsl:value-of select="@PercentageCovered"/>%
        </TD>			
			</TR>
			<tr>
				<td colspan="3"><hr/></td>
			</tr>
			<TR>
				<TD  width="*">
					<H4>Coverage</H4>
				</TD>

				<TD width="10%">
					<H4>IL Count</H4>
				</TD>

				<TD width="40%">
					<H4>Function</H4>
				</TD>
			</TR>
      <xsl:for-each select="Function">
        <xsl:call-template name="Function" />
      </xsl:for-each>
		</TABLE>
	</xsl:template>
	
  <xsl:template name="Function">
		<TR style="height:1px">

			<TD style="width:300px;height:10px" valign="bottom">
				<TABLE style="width:100%;" border="0" cellpadding="0" cellspacing="0">
					<TR height='10px'>
						<xsl:call-template name="CreateBar">
							<xsl:with-param name="width" select="@PercentageCovered"></xsl:with-param>
							<xsl:with-param name="colour">Green</xsl:with-param>
						</xsl:call-template>
						<xsl:call-template name="CreateBar">
							<xsl:with-param name="width" select="100 - @PercentageCovered"></xsl:with-param>
							<xsl:with-param name="colour">Red</xsl:with-param>
						</xsl:call-template>
					</TR>
				</TABLE>
			</TD>			
			<TD style="height:10px; font: 9pt Courier New;" valign="bottom">

        <xsl:value-of select="@CoveredCount"/>/<xsl:value-of select="@InstructionCount"/>&#160;<xsl:value-of select="@PercentageCovered"/>%

      </TD>
			<TD style="height:10px; font: 9pt Courier New;" valign="bottom">
				
					<xsl:value-of select="@FunctionName"/>
				
			</TD>
		</TR>
		<xsl:apply-templates/>
	</xsl:template>
	
  <xsl:template name="CreateBar">
		<xsl:param name="width"></xsl:param>
		<xsl:param name="colour"></xsl:param>
		<xsl:element name="TD">
		<xsl:attribute name="bgcolor"><xsl:value-of select="$colour"/></xsl:attribute>
		<xsl:attribute name="height">10px</xsl:attribute>
		<xsl:attribute name="width"><xsl:value-of select="$width*3"/></xsl:attribute>
		<xsl:attribute name="onmouseover">window.event.srcElement.title="<xsl:value-of select="$width"/>%"</xsl:attribute>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
