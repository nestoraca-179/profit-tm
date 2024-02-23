using DevExpress.XtraReports.UI;
using System;

/// <summary>
/// Summary description for RepMayorAnalitico2KDoce
/// </summary>
public class RepMayorAnalitico2KDoce : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.DataAccess.Sql.SqlDataSource DemoCont;
    private XRControlStyle Title;
    private XRControlStyle DetailCaption1;
    private XRControlStyle DetailData1;
    private XRControlStyle DetailData3_Odd;
    private XRControlStyle PageInfo;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private XRPageInfo pageInfo1;
    private XRPageInfo pageInfo2;
    private ReportHeaderBand ReportHeader;
    private XRTable table1;
    private XRTableRow tableRow1;
    private XRTableCell tableCell1;
    private XRTableCell tableCell2;
    private XRTableCell tableCell3;
    private XRTableCell tableCell4;
    private XRTableCell tableCell5;
    private DetailBand Detail;
    private XRTable table2;
    private XRTableRow tableRow2;
    private XRTableCell tableCell6;
    private XRTableCell tableCell7;
    private XRTableCell tableCell10;
    private DevExpress.XtraReports.Parameters.Parameter fecDesde;
    private DevExpress.XtraReports.Parameters.Parameter fecHasta;
    private DevExpress.XtraReports.Parameters.Parameter cenDesde;
    private DevExpress.XtraReports.Parameters.Parameter cenHasta;
    private DevExpress.XtraReports.Parameters.Parameter cueDesde;
    private DevExpress.XtraReports.Parameters.Parameter cueHasta;
    private PageHeaderBand PageHeader;
    private XRLabel label1;
    private ReportFooterBand ReportFooter;
    private XRTableCell xrTableCell1;
    public XRLabel LBL_DescEmpresa;
    private XRLabel xrLabel1;
    private XRLabel xrLabel3;
    private XRLabel xrLabel4;
    public XRLabel LBL_Direc;
    public XRLabel LBL_Telf;
    public XRLabel LBL_RIF;
    public XRPictureBox PB_Logo;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel9;
    private XRPageInfo xrPageInfo1;
    private XRLine xrLine1;
    private XRLabel xrLabel15;
    private CalculatedField totalSaldoInicial;
    private XRLabel xrLabel2;
    private XRLabel xrLabel24;
    private XRLabel xrLabel22;
    private XRLabel xrLabel20;
    private XRLine xrLine2;
    private XRLine xrLine3;
    private CalculatedField activo;
    private XRLabel xrLabel5;
    private XRLabel xrLabel6;
    private CalculatedField pasivoCapital;
    private CalculatedField ingEgr;
    private XRLabel xrLabel7;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRLabel xrLabel8;
    private CalculatedField totalSaldoActual;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private CalculatedField totalDebe;
    private CalculatedField totalHaber;
    private DevExpress.XtraReports.Parameters.Parameter contDesde;
    private DevExpress.XtraReports.Parameters.Parameter contHasta;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RepMayorAnalitico2KDoce()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter6 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter7 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter8 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery2 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery3 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepMayorAnalitico2KDoce));
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings4 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings5 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings6 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            this.DemoCont = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fecDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.fecHasta = new DevExpress.XtraReports.Parameters.Parameter();
            this.cenDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.cenHasta = new DevExpress.XtraReports.Parameters.Parameter();
            this.cueDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.cueHasta = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.LBL_DescEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_Direc = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_Telf = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_RIF = new DevExpress.XtraReports.UI.XRLabel();
            this.PB_Logo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.totalSaldoInicial = new DevExpress.XtraReports.UI.CalculatedField();
            this.activo = new DevExpress.XtraReports.UI.CalculatedField();
            this.pasivoCapital = new DevExpress.XtraReports.UI.CalculatedField();
            this.ingEgr = new DevExpress.XtraReports.UI.CalculatedField();
            this.totalSaldoActual = new DevExpress.XtraReports.UI.CalculatedField();
            this.totalDebe = new DevExpress.XtraReports.UI.CalculatedField();
            this.totalHaber = new DevExpress.XtraReports.UI.CalculatedField();
            this.contDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.contHasta = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DemoCont
            // 
            this.DemoCont.ConnectionName = "DemoCont";
            this.DemoCont.ConnectionOptions.CommandTimeout = 90;
            this.DemoCont.ConnectionOptions.DbCommandTimeout = 90;
            this.DemoCont.Name = "DemoCont";
            storedProcQuery1.Name = "RepMayorAnalitico2KDocePW";
            queryParameter1.Name = "@fecEmisD";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?fecDesde", typeof(System.DateTime));
            queryParameter2.Name = "@fecEmisH";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?fecHasta", typeof(System.DateTime));
            queryParameter3.Name = "@cueContD";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("?contDesde", typeof(string));
            queryParameter4.Name = "@cueContH";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("?contHasta", typeof(string));
            queryParameter5.Name = "@coCenD";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("?cenDesde", typeof(string));
            queryParameter6.Name = "@coCenH";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("?cenHasta", typeof(string));
            queryParameter7.Name = "@coGasD";
            queryParameter7.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter7.Value = new DevExpress.DataAccess.Expression("?cueDesde", typeof(string));
            queryParameter8.Name = "@coGasH";
            queryParameter8.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter8.Value = new DevExpress.DataAccess.Expression("?cueHasta", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.Parameters.Add(queryParameter7);
            storedProcQuery1.Parameters.Add(queryParameter8);
            storedProcQuery1.StoredProcName = "RepMayorAnalitico2KDocePW";
            customSqlQuery1.Name = "Centros";
            customSqlQuery1.Sql = "select co_cen, des_cen from sccentro";
            customSqlQuery2.Name = "Cuentas";
            customSqlQuery2.Sql = "select co_gas, des_gas from scgastos";
            customSqlQuery3.Name = "Contables";
            customSqlQuery3.Sql = "select co_cue, des_cue from sccuenta";
            this.DemoCont.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1,
            customSqlQuery1,
            customSqlQuery2,
            customSqlQuery3});
            this.DemoCont.ResultSchemaSerializable = resources.GetString("DemoCont.ResultSchemaSerializable");
            // 
            // Title
            // 
            this.Title.BackColor = System.Drawing.Color.Transparent;
            this.Title.BorderColor = System.Drawing.Color.Black;
            this.Title.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Title.BorderWidth = 1F;
            this.Title.Font = new System.Drawing.Font("Arial", 14.25F);
            this.Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.Title.Name = "Title";
            // 
            // DetailCaption1
            // 
            this.DetailCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(104)))), ((int)(((byte)(196)))));
            this.DetailCaption1.BorderColor = System.Drawing.Color.White;
            this.DetailCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailCaption1.BorderWidth = 2F;
            this.DetailCaption1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.DetailCaption1.ForeColor = System.Drawing.Color.White;
            this.DetailCaption1.Name = "DetailCaption1";
            this.DetailCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData1
            // 
            this.DetailData1.BackColor = System.Drawing.Color.Transparent;
            this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData1.BorderWidth = 2F;
            this.DetailData1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData1.ForeColor = System.Drawing.Color.Black;
            this.DetailData1.Name = "DetailData1";
            this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData3_Odd.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.DetailData3_Odd.BorderWidth = 1F;
            this.DetailData3_Odd.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData3_Odd.ForeColor = System.Drawing.Color.Black;
            this.DetailData3_Odd.Name = "DetailData3_Odd";
            this.DetailData3_Odd.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData3_Odd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageInfo
            // 
            this.PageInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.PageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(70)))), ((int)(((byte)(80)))));
            this.PageInfo.Name = "PageInfo";
            this.PageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 70F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.pageInfo1,
            this.pageInfo2});
            this.BottomMargin.Name = "BottomMargin";
            // 
            // pageInfo1
            // 
            this.pageInfo1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(473.9583F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            this.pageInfo1.StylePriority.UseFont = false;
            // 
            // pageInfo2
            // 
            this.pageInfo2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(473.9583F, 9.999974F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(456.0417F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.StylePriority.UseFont = false;
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 0F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.table2});
            this.Detail.HeightF = 29F;
            this.Detail.Name = "Detail";
            // 
            // xrLine1
            // 
            this.xrLine1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[is_total]")});
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(473.9583F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(456.0417F, 3.000005F);
            // 
            // table2
            // 
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4F);
            this.table2.Name = "table2";
            this.table2.OddStyleName = "DetailData3_Odd";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(930.0001F, 25F);
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell6,
            this.tableCell10,
            this.tableCell7,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell4});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 11.5D;
            // 
            // tableCell6
            // 
            this.tableCell6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Size", "Iif(IsNull([comp_num]), 8.25, 7)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Padding.Left", "Iif(IsNull([comp_num]), 0, 20)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Padding.Top", "Iif(IsNull([comp_num]), 0, 2.5)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Padding.Bottom", "Iif(IsNull([comp_num]), 0, 2.5)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "WidthF", "Iif(IsNull([comp_num]), 100, 150)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", resources.GetString("tableCell6.ExpressionBindings")),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]")});
            this.tableCell6.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.tableCell6.StyleName = "DetailData1";
            this.tableCell6.StylePriority.UseBorders = false;
            this.tableCell6.StylePriority.UseFont = false;
            this.tableCell6.StylePriority.UsePadding = false;
            this.tableCell6.Weight = 0.15384615925488326D;
            // 
            // tableCell10
            // 
            this.tableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(IsNull([comp_num]), [des_cue], [descri])"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Size", "Iif(IsNull([comp_num]), 8.25, 7)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "WidthF", "Iif(IsNull([comp_num]), 375, 325)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]")});
            this.tableCell10.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.StyleName = "DetailData1";
            this.tableCell10.StylePriority.UseFont = false;
            this.tableCell10.Weight = 0.5769231080719921D;
            // 
            // tableCell7
            // 
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SaldoInicial]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[is_total] Or Iif(IsNull([detalle]), false, [detalle])")});
            this.tableCell7.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StyleName = "DetailData1";
            this.tableCell7.StylePriority.UseFont = false;
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell7.TextFormatString = "{0:n}";
            this.tableCell7.Weight = 0.19070515640640295D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[is_total] Or !IsNull([comp_num])"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([is_total] Or IsNull([detalle]), [MontoD], 0)")});
            this.xrTableCell2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StyleName = "DetailData1";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.TextFormatString = "{0:n}";
            this.xrTableCell2.Weight = 0.15384635116519291D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[is_total] Or !IsNull([comp_num])"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([is_total] Or IsNull([detalle]), [MontoH], 0)")});
            this.xrTableCell3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StyleName = "DetailData1";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.TextFormatString = "{0:n}";
            this.xrTableCell3.Weight = 0.15413085667720394D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[is_total] Or !IsNull([comp_num])"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([is_total] Or IsNull([des_cue]), [SaldoInicial] + ([MontoD] - [MontoH]), 0)")});
            this.xrTableCell4.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StyleName = "DetailData1";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell4.TextFormatString = "{0:n}";
            this.xrTableCell4.Weight = 0.20131777241678145D;
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 210.5417F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(930.0001F, 28F);
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell5,
            this.tableCell2,
            this.tableCell3,
            this.tableCell4,
            this.xrTableCell1});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // tableCell1
            // 
            this.tableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableCell1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell1.Name = "tableCell1";
            this.tableCell1.StyleName = "DetailCaption1";
            this.tableCell1.StylePriority.UseBorders = false;
            this.tableCell1.StylePriority.UseFont = false;
            this.tableCell1.StylePriority.UseTextAlignment = false;
            this.tableCell1.Text = "Codigo";
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell1.Weight = 0.1176470639577222D;
            // 
            // tableCell5
            // 
            this.tableCell5.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StyleName = "DetailCaption1";
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            this.tableCell5.Text = "Cuenta";
            this.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell5.Weight = 0.4411764952593818D;
            // 
            // tableCell2
            // 
            this.tableCell2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StyleName = "DetailCaption1";
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.Text = "Saldo Inicial";
            this.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell2.Weight = 0.14583331148289541D;
            // 
            // tableCell3
            // 
            this.tableCell3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StyleName = "DetailCaption1";
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "Debe";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell3.Weight = 0.11764706737474134D;
            // 
            // tableCell4
            // 
            this.tableCell4.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StyleName = "DetailCaption1";
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "Haber";
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell4.Weight = 0.11786477598989995D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StyleName = "DetailCaption1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Saldo a la fecha";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.15394903048472822D;
            // 
            // fecDesde
            // 
            this.fecDesde.AllowNull = true;
            this.fecDesde.Description = "Fecha Desde";
            this.fecDesde.Name = "fecDesde";
            this.fecDesde.Type = typeof(System.DateTime);
            this.fecDesde.ValueInfo = "2007-01-01";
            // 
            // fecHasta
            // 
            this.fecHasta.AllowNull = true;
            this.fecHasta.Description = "Fecha Hasta";
            this.fecHasta.Name = "fecHasta";
            this.fecHasta.Type = typeof(System.DateTime);
            this.fecHasta.ValueInfo = "2007-12-31";
            // 
            // cenDesde
            // 
            this.cenDesde.AllowNull = true;
            this.cenDesde.Description = "Centro de Costos Desde";
            dynamicListLookUpSettings1.DataMember = "Centros";
            dynamicListLookUpSettings1.DataSource = this.DemoCont;
            dynamicListLookUpSettings1.DisplayMember = "des_cen";
            dynamicListLookUpSettings1.SortMember = "co_cen";
            dynamicListLookUpSettings1.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings1.ValueMember = "co_cen";
            this.cenDesde.LookUpSettings = dynamicListLookUpSettings1;
            this.cenDesde.Name = "cenDesde";
            // 
            // cenHasta
            // 
            this.cenHasta.AllowNull = true;
            this.cenHasta.Description = "Centro de Costos Hasta";
            dynamicListLookUpSettings2.DataMember = "Centros";
            dynamicListLookUpSettings2.DataSource = this.DemoCont;
            dynamicListLookUpSettings2.DisplayMember = "des_cen";
            dynamicListLookUpSettings2.SortMember = "co_cen";
            dynamicListLookUpSettings2.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings2.ValueMember = "co_cen";
            this.cenHasta.LookUpSettings = dynamicListLookUpSettings2;
            this.cenHasta.Name = "cenHasta";
            // 
            // cueDesde
            // 
            this.cueDesde.AllowNull = true;
            this.cueDesde.Description = "Cuenta de Gastos Desde";
            dynamicListLookUpSettings3.DataMember = "Cuentas";
            dynamicListLookUpSettings3.DataSource = this.DemoCont;
            dynamicListLookUpSettings3.DisplayMember = "des_gas";
            dynamicListLookUpSettings3.SortMember = "co_gas";
            dynamicListLookUpSettings3.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings3.ValueMember = "co_gas";
            this.cueDesde.LookUpSettings = dynamicListLookUpSettings3;
            this.cueDesde.Name = "cueDesde";
            // 
            // cueHasta
            // 
            this.cueHasta.AllowNull = true;
            this.cueHasta.Description = "Cuenta de Gastos Hasta";
            dynamicListLookUpSettings4.DataMember = "Cuentas";
            dynamicListLookUpSettings4.DataSource = this.DemoCont;
            dynamicListLookUpSettings4.DisplayMember = "des_gas";
            dynamicListLookUpSettings4.SortMember = "co_gas";
            dynamicListLookUpSettings4.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings4.ValueMember = "co_gas";
            this.cueHasta.LookUpSettings = dynamicListLookUpSettings4;
            this.cueHasta.Name = "cueHasta";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LBL_DescEmpresa,
            this.xrLabel1,
            this.xrLabel3,
            this.xrLabel4,
            this.LBL_Direc,
            this.LBL_Telf,
            this.LBL_RIF,
            this.PB_Logo,
            this.xrPageInfo2,
            this.xrLabel9,
            this.xrPageInfo1,
            this.label1,
            this.table1});
            this.PageHeader.HeightF = 238.5417F;
            this.PageHeader.Name = "PageHeader";
            // 
            // LBL_DescEmpresa
            // 
            this.LBL_DescEmpresa.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.LBL_DescEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(149.5F, 10F);
            this.LBL_DescEmpresa.Multiline = true;
            this.LBL_DescEmpresa.Name = "LBL_DescEmpresa";
            this.LBL_DescEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_DescEmpresa.SizeF = new System.Drawing.SizeF(476.9189F, 22.99998F);
            this.LBL_DescEmpresa.StylePriority.UseFont = false;
            this.LBL_DescEmpresa.Text = "LBL_DescEmpresa";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(149.5F, 68.99992F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(63.53999F, 44.4444F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Dirección:";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(149.5F, 50.99995F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(63.54F, 18F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Teléfonos:";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(149.5F, 32.99999F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(63.54167F, 17.99997F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "RIF:";
            // 
            // LBL_Direc
            // 
            this.LBL_Direc.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_Direc.LocationFloat = new DevExpress.Utils.PointFloat(213.04F, 68.99992F);
            this.LBL_Direc.Multiline = true;
            this.LBL_Direc.Name = "LBL_Direc";
            this.LBL_Direc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_Direc.SizeF = new System.Drawing.SizeF(413.379F, 44.4444F);
            this.LBL_Direc.StylePriority.UseFont = false;
            // 
            // LBL_Telf
            // 
            this.LBL_Telf.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_Telf.LocationFloat = new DevExpress.Utils.PointFloat(213.04F, 50.99996F);
            this.LBL_Telf.Multiline = true;
            this.LBL_Telf.Name = "LBL_Telf";
            this.LBL_Telf.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_Telf.SizeF = new System.Drawing.SizeF(413.379F, 18F);
            this.LBL_Telf.StylePriority.UseFont = false;
            // 
            // LBL_RIF
            // 
            this.LBL_RIF.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_RIF.LocationFloat = new DevExpress.Utils.PointFloat(213.0417F, 32.99999F);
            this.LBL_RIF.Multiline = true;
            this.LBL_RIF.Name = "LBL_RIF";
            this.LBL_RIF.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_RIF.SizeF = new System.Drawing.SizeF(413.3773F, 17.99997F);
            this.LBL_RIF.StylePriority.UseFont = false;
            // 
            // PB_Logo
            // 
            this.PB_Logo.LocationFloat = new DevExpress.Utils.PointFloat(10F, 10.83332F);
            this.PB_Logo.Name = "PB_Logo";
            this.PB_Logo.SizeF = new System.Drawing.SizeF(128.5417F, 102.611F);
            this.PB_Logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(699.4008F, 33.83329F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(211.5992F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(699.4008F, 10.83331F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(169.8493F, 22.99998F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Página";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(869.2499F, 10.83331F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(41.75F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(149.5F, 148.889F);
            this.label1.Name = "label1";
            this.label1.SizeF = new System.Drawing.SizeF(625.5F, 26.27766F);
            this.label1.StyleName = "Title";
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "Mayor Analítico";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLine3,
            this.xrLabel24,
            this.xrLabel22,
            this.xrLabel20,
            this.xrLine2,
            this.xrLabel2,
            this.xrLabel15});
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel11
            // 
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([totalHaber], 0)")});
            this.xrLabel11.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(698.9584F, 9.083302F);
            this.xrLabel11.Multiline = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(100.1852F, 29.25002F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "xrLabel8";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel11.TextFormatString = "{0:n}";
            // 
            // xrLabel10
            // 
            this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([totalDebe], 0)")});
            this.xrLabel10.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(600.2004F, 9.083302F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(98.75806F, 29.25002F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel8";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel10.TextFormatString = "{0:n}";
            // 
            // xrLabel8
            // 
            this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([totalSaldoActual], 0)")});
            this.xrLabel8.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(799.1436F, 9.083302F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(130.8564F, 29.25002F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "xrLabel8";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel8.TextFormatString = "{0:n}";
            // 
            // xrLabel7
            // 
            this.xrLabel7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([ingEgr], 0)")});
            this.xrLabel7.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(710.2733F, 63.99997F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(124.87F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "xrLabel7";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel7.TextFormatString = "{0:n}";
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([pasivoCapital], 0)")});
            this.xrLabel6.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(417.9878F, 63.99997F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(156.25F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "xrLabel6";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel6.TextFormatString = "{0:n}";
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([activo], 0)")});
            this.xrLabel5.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(147.3964F, 63.99997F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(127.0834F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel5.TextFormatString = "{0:n}";
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(930.0001F, 2.083333F);
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(600.2003F, 64F);
            this.xrLabel24.Multiline = true;
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(110.0729F, 22.99997F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UsePadding = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Ingreso - Egreso:";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(298.1668F, 64.00003F);
            this.xrLabel22.Multiline = true;
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(119.8209F, 22.99998F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UsePadding = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "Pasivo + Capital:";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(86.25017F, 64F);
            this.xrLabel20.Multiline = true;
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(61.14623F, 22.99996F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UsePadding = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Activo:";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 48.20846F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(930.0001F, 2.083336F);
            // 
            // xrLabel2
            // 
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([totalSaldoInicial], 0)")});
            this.xrLabel2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(475F, 9.083302F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(123.9583F, 29.25002F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UsePadding = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel2.TextFormatString = "{0:n}";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(100F, 9.083366F);
            this.xrLabel15.Multiline = true;
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(375F, 29.25002F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UsePadding = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "TOTAL DE MAYOR ANALITICO";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // totalSaldoInicial
            // 
            this.totalSaldoInicial.DataMember = "RepMayorAnalitico2KDocePW";
            this.totalSaldoInicial.Expression = "[][IsNull([co_cuepadre])].Sum([SaldoInicial])";
            this.totalSaldoInicial.Name = "totalSaldoInicial";
            // 
            // activo
            // 
            this.activo.DataMember = "RepMayorAnalitico2KDocePW";
            this.activo.Expression = "[][[EsActivo] And IsNull([co_cuepadre])].Sum([SaldoInicial] + ([MontoD] - [MontoH" +
    "]))";
            this.activo.Name = "activo";
            // 
            // pasivoCapital
            // 
            this.pasivoCapital.DataMember = "RepMayorAnalitico2KDocePW";
            this.pasivoCapital.Expression = "[][([EsPasivo] Or [EsCapital] Or [EsAdicional]) And IsNull([co_cuepadre])].Sum([S" +
    "aldoInicial] + ([MontoD] - [MontoH]))";
            this.pasivoCapital.Name = "pasivoCapital";
            // 
            // ingEgr
            // 
            this.ingEgr.DataMember = "RepMayorAnalitico2KDocePW";
            this.ingEgr.Expression = "[][[EsIngEgr] And IsNull([co_cuepadre])].Sum([SaldoInicial] + ([MontoD] - [MontoH" +
    "]))";
            this.ingEgr.Name = "ingEgr";
            // 
            // totalSaldoActual
            // 
            this.totalSaldoActual.DataMember = "RepMayorAnalitico2KDocePW";
            this.totalSaldoActual.Expression = "Sum(Iif(![detalle] And IsNull([co_cuepadre]), [SaldoInicial] + ([MontoD] - [Monto" +
    "H]), 0))";
            this.totalSaldoActual.Name = "totalSaldoActual";
            // 
            // totalDebe
            // 
            this.totalDebe.DataMember = "RepMayorAnalitico2KDocePW";
            this.totalDebe.Expression = "[][![is_total] And IsNull([co_cuepadre])].Sum([MontoD])";
            this.totalDebe.Name = "totalDebe";
            // 
            // totalHaber
            // 
            this.totalHaber.DataMember = "RepMayorAnalitico2KDocePW";
            this.totalHaber.Expression = "[][![is_total] And IsNull([co_cuepadre])].Sum([MontoH])";
            this.totalHaber.Name = "totalHaber";
            // 
            // contDesde
            // 
            this.contDesde.AllowNull = true;
            this.contDesde.Description = "Cuenta Contable Desde";
            dynamicListLookUpSettings5.DataMember = "Contables";
            dynamicListLookUpSettings5.DataSource = this.DemoCont;
            dynamicListLookUpSettings5.DisplayMember = "des_cue";
            dynamicListLookUpSettings5.SortMember = "co_cue";
            dynamicListLookUpSettings5.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings5.ValueMember = "co_cue";
            this.contDesde.LookUpSettings = dynamicListLookUpSettings5;
            this.contDesde.Name = "contDesde";
            // 
            // contHasta
            // 
            this.contHasta.AllowNull = true;
            this.contHasta.Description = "Cuenta Contable Hasta";
            dynamicListLookUpSettings6.DataMember = "Contables";
            dynamicListLookUpSettings6.DataSource = this.DemoCont;
            dynamicListLookUpSettings6.DisplayMember = "des_cue";
            dynamicListLookUpSettings6.SortMember = "co_cue";
            dynamicListLookUpSettings6.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings6.ValueMember = "co_cue";
            this.contHasta.LookUpSettings = dynamicListLookUpSettings6;
            this.contHasta.Name = "contHasta";
            // 
            // RepMayorAnalitico2KDoce
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.Detail,
            this.PageHeader,
            this.ReportFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.totalSaldoInicial,
            this.activo,
            this.pasivoCapital,
            this.ingEgr,
            this.totalSaldoActual,
            this.totalDebe,
            this.totalHaber});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DemoCont});
            this.DataMember = "RepMayorAnalitico2KDocePW";
            this.DataSource = this.DemoCont;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(9, 10, 70, 100);
            this.PageWidth = 950;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.fecDesde,
            this.fecHasta,
            this.contDesde,
            this.contHasta,
            this.cenDesde,
            this.cenHasta,
            this.cueDesde,
            this.cueHasta});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailData3_Odd,
            this.PageInfo});
            this.Version = "18.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.RepMayorAnalitico2KDoce_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void RepMayorAnalitico2KDoce_DataSourceDemanded(object sender, EventArgs e)
    {
        XtraReport report = sender as XtraReport;
        var parameters = report.Parameters;

        foreach (var param in parameters)
        {
            if (param.Value != null)
            {
                if (param.Value.ToString() == "")
                {
                    param.Value = DBNull.Value;
                }
            }
        }
    }
}
