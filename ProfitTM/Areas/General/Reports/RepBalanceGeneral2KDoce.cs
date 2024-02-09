using DevExpress.XtraReports.UI;
using System;

/// <summary>
/// Summary description for RepBalanceGeneral2KDoce
/// </summary>
public class RepBalanceGeneral2KDoce : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.DataAccess.Sql.SqlDataSource DemoCont;
    private XRControlStyle Title;
    private XRControlStyle GroupCaption1;
    private XRControlStyle GroupData1;
    private XRControlStyle DetailCaption1;
    private XRControlStyle DetailData1;
    private XRControlStyle GroupFooterBackground3;
    private XRControlStyle DetailData3_Odd;
    private XRControlStyle PageInfo;
    private TopMarginBand TopMargin;
    private BottomMarginBand BottomMargin;
    private ReportHeaderBand ReportHeader;
    private DetailBand Detail;
    private PageHeaderBand PageHeader;
    private DevExpress.XtraReports.Parameters.Parameter fecha;
    private XRPageInfo pageInfo1;
    private XRPageInfo pageInfo2;
    private XRLabel label1;
    private XRTable table2;
    private XRTableRow tableRow2;
    private XRTableCell xrTableCell1;
    private XRTableCell tableCell3;
    private XRTableCell xrTableCell2;
    public XRPictureBox PB_Logo;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel3;
    private XRPageInfo xrPageInfo1;
    private DevExpress.XtraReports.Parameters.Parameter cenDesde;
    private DevExpress.XtraReports.Parameters.Parameter cenHasta;
    private DevExpress.XtraReports.Parameters.Parameter cueDesde;
    private DevExpress.XtraReports.Parameters.Parameter cueHasta;
    private XRLabel xrLabel9;
    private XRLabel xrLabel10;
    private XRLine xrLine1;
    private CalculatedField saldoActual;
    private XRLabel xrLabel11;
    private ReportFooterBand ReportFooter;
    private XRLabel xrLabel15;
    private XRLine xrLine2;
    private XRLabel xrLabel20;
    private XRLabel xrLabel21;
    private XRLabel xrLabel22;
    private XRLabel xrLabel23;
    private XRLabel xrLabel24;
    private XRLabel xrLabel25;
    private XRLine xrLine3;
    private CalculatedField totalSaldoA;
    private XRLabel xrLabel12;
    private XRLabel xrLabel1;
    public XRLabel LBL_Telf;
    public XRLabel LBL_Direc;
    private XRLabel xrLabel4;
    private XRLabel xrLabel2;
    public XRLabel LBL_RIF;
    public XRLabel LBL_DescEmpresa;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RepBalanceGeneral2KDoce()
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
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery2 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.StoredProcQuery storedProcQuery1 = new DevExpress.DataAccess.Sql.StoredProcQuery();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter1 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter2 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter3 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter4 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter5 = new DevExpress.DataAccess.Sql.QueryParameter();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepBalanceGeneral2KDoce));
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings4 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            this.DemoCont = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupFooterBackground3 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_Telf = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_Direc = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_RIF = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_DescEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.PB_Logo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fecha = new DevExpress.XtraReports.Parameters.Parameter();
            this.cenDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.cenHasta = new DevExpress.XtraReports.Parameters.Parameter();
            this.cueDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.cueHasta = new DevExpress.XtraReports.Parameters.Parameter();
            this.saldoActual = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.totalSaldoA = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DemoCont
            // 
            this.DemoCont.ConnectionName = "DemoCont";
            this.DemoCont.Name = "DemoCont";
            customSqlQuery1.Name = "Centros";
            customSqlQuery1.Sql = "select co_cen, des_cen from sccentro";
            customSqlQuery2.Name = "Cuentas";
            customSqlQuery2.Sql = "select co_gas, des_gas from scgastos";
            storedProcQuery1.Name = "RepBalanceGeneral2KDoce";
            queryParameter1.Name = "@fecEmis";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?fecha", typeof(System.DateTime));
            queryParameter2.Name = "@coCenD";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?cenDesde", typeof(string));
            queryParameter3.Name = "@coCenH";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("?cenHasta", typeof(string));
            queryParameter4.Name = "@coGasD";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("?cueDesde", typeof(string));
            queryParameter5.Name = "@coGasH";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("?cueHasta", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.StoredProcName = "RepBalanceGeneral2KDocePW";
            this.DemoCont.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            customSqlQuery1,
            customSqlQuery2,
            storedProcQuery1});
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
            // GroupCaption1
            // 
            this.GroupCaption1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(104)))), ((int)(((byte)(196)))));
            this.GroupCaption1.BorderColor = System.Drawing.Color.White;
            this.GroupCaption1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.GroupCaption1.BorderWidth = 2F;
            this.GroupCaption1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.GroupCaption1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
            this.GroupCaption1.Name = "GroupCaption1";
            this.GroupCaption1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 2, 0, 0, 100F);
            this.GroupCaption1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupData1
            // 
            this.GroupData1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(104)))), ((int)(((byte)(196)))));
            this.GroupData1.BorderColor = System.Drawing.Color.White;
            this.GroupData1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.GroupData1.BorderWidth = 2F;
            this.GroupData1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.GroupData1.ForeColor = System.Drawing.Color.White;
            this.GroupData1.Name = "GroupData1";
            this.GroupData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 2, 0, 0, 100F);
            this.GroupData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            this.DetailData1.BorderColor = System.Drawing.Color.Transparent;
            this.DetailData1.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.DetailData1.BorderWidth = 2F;
            this.DetailData1.Font = new System.Drawing.Font("Arial", 8.25F);
            this.DetailData1.ForeColor = System.Drawing.Color.Black;
            this.DetailData1.Name = "DetailData1";
            this.DetailData1.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 0, 0, 100F);
            this.DetailData1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupFooterBackground3
            // 
            this.GroupFooterBackground3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(117)))), ((int)(((byte)(129)))));
            this.GroupFooterBackground3.BorderColor = System.Drawing.Color.White;
            this.GroupFooterBackground3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.GroupFooterBackground3.BorderWidth = 2F;
            this.GroupFooterBackground3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.GroupFooterBackground3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
            this.GroupFooterBackground3.Name = "GroupFooterBackground3";
            this.GroupFooterBackground3.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 2, 0, 0, 100F);
            this.GroupFooterBackground3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // DetailData3_Odd
            // 
            this.DetailData3_Odd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
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
            this.BottomMargin.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[EsIngEgr] = false")});
            this.BottomMargin.Name = "BottomMargin";
            // 
            // pageInfo1
            // 
            this.pageInfo1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.00001F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(376.8333F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            this.pageInfo1.StylePriority.UseFont = false;
            // 
            // pageInfo2
            // 
            this.pageInfo2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(376.8333F, 6.00001F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(373.1667F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.StylePriority.UseFont = false;
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 0F;
            this.ReportHeader.KeepTogether = true;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(150F, 144.5973F);
            this.label1.Name = "label1";
            this.label1.SizeF = new System.Drawing.SizeF(451.0417F, 24.19434F);
            this.label1.StyleName = "Title";
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "Balance General";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrLine1,
            this.xrLabel9,
            this.xrLabel10});
            this.Detail.HeightF = 28.70849F;
            this.Detail.Name = "Detail";
            // 
            // xrLabel11
            // 
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[saldoActual]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[detalle]")});
            this.xrLabel11.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(538.5417F, 3.999996F);
            this.xrLabel11.Multiline = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(211.4583F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel11.TextFormatString = "{0:n}";
            // 
            // xrLine1
            // 
            this.xrLine1.BorderColor = System.Drawing.Color.Black;
            this.xrLine1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[is_total]")});
            this.xrLine1.ForeColor = System.Drawing.Color.DimGray;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(538.5417F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(210.4584F, 3.041681F);
            this.xrLine1.StylePriority.UseBorderColor = false;
            this.xrLine1.StylePriority.UseForeColor = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[des_cue]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "![EsIngEgr]")});
            this.xrLabel9.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(122.9167F, 4F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(415.625F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UsePadding = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "xrLabel9";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[co_cue]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Font.Bold", "[is_total]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "![EsIngEgr]")});
            this.xrLabel10.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(122.9167F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UsePadding = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel10";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.LBL_Telf,
            this.LBL_Direc,
            this.xrLabel4,
            this.xrLabel2,
            this.LBL_RIF,
            this.LBL_DescEmpresa,
            this.PB_Logo,
            this.xrPageInfo2,
            this.xrLabel3,
            this.xrPageInfo1,
            this.label1,
            this.table2});
            this.PageHeader.HeightF = 207.7916F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(150F, 69.20827F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(63.53999F, 44.4444F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Dirección:";
            // 
            // LBL_Telf
            // 
            this.LBL_Telf.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_Telf.LocationFloat = new DevExpress.Utils.PointFloat(213.54F, 51.20832F);
            this.LBL_Telf.Multiline = true;
            this.LBL_Telf.Name = "LBL_Telf";
            this.LBL_Telf.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_Telf.SizeF = new System.Drawing.SizeF(325.0017F, 18F);
            this.LBL_Telf.StylePriority.UseFont = false;
            // 
            // LBL_Direc
            // 
            this.LBL_Direc.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_Direc.LocationFloat = new DevExpress.Utils.PointFloat(213.54F, 69.20826F);
            this.LBL_Direc.Multiline = true;
            this.LBL_Direc.Name = "LBL_Direc";
            this.LBL_Direc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_Direc.SizeF = new System.Drawing.SizeF(325.0017F, 44.4444F);
            this.LBL_Direc.StylePriority.UseFont = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(150F, 33.20834F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(63.54167F, 17.99997F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "RIF:";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(150F, 51.2083F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(63.54F, 18F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Teléfonos:";
            // 
            // LBL_RIF
            // 
            this.LBL_RIF.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_RIF.LocationFloat = new DevExpress.Utils.PointFloat(213.5417F, 33.20835F);
            this.LBL_RIF.Multiline = true;
            this.LBL_RIF.Name = "LBL_RIF";
            this.LBL_RIF.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_RIF.SizeF = new System.Drawing.SizeF(325F, 17.99997F);
            this.LBL_RIF.StylePriority.UseFont = false;
            // 
            // LBL_DescEmpresa
            // 
            this.LBL_DescEmpresa.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.LBL_DescEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(150F, 10.20837F);
            this.LBL_DescEmpresa.Multiline = true;
            this.LBL_DescEmpresa.Name = "LBL_DescEmpresa";
            this.LBL_DescEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_DescEmpresa.SizeF = new System.Drawing.SizeF(388.5417F, 22.99998F);
            this.LBL_DescEmpresa.StylePriority.UseFont = false;
            this.LBL_DescEmpresa.Text = "LBL_DescEmpresa";
            // 
            // PB_Logo
            // 
            this.PB_Logo.LocationFloat = new DevExpress.Utils.PointFloat(13.54167F, 10.20835F);
            this.PB_Logo.Name = "PB_Logo";
            this.PB_Logo.SizeF = new System.Drawing.SizeF(128.5417F, 102.611F);
            this.PB_Logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(550.2083F, 33.20832F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(199.7917F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(550.2083F, 10.20834F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(153.1251F, 22.99998F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Página";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(703.3333F, 10.20834F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(46.66675F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // table2
            // 
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 179.7916F);
            this.table2.Name = "table2";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(750F, 28F);
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.tableCell3,
            this.xrTableCell2});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StyleName = "DetailCaption1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Código";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.18910256019005409D;
            // 
            // tableCell3
            // 
            this.tableCell3.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.tableCell3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StyleName = "DetailCaption1";
            this.tableCell3.StylePriority.UseBorders = false;
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "Cuenta";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell3.Weight = 0.639423112135667D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StyleName = "DetailCaption1";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Saldo a la fecha";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.32532048152043275D;
            // 
            // fecha
            // 
            this.fecha.AllowNull = true;
            this.fecha.Description = "Fecha";
            this.fecha.Name = "fecha";
            this.fecha.Type = typeof(System.DateTime);
            // 
            // cenDesde
            // 
            this.cenDesde.AllowNull = true;
            this.cenDesde.Description = "Centro de Costo Desde";
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
            this.cenHasta.Description = "Centro de Costo Hasta";
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
            // saldoActual
            // 
            this.saldoActual.DataMember = "RepBalanceGeneral2KDoce";
            this.saldoActual.Expression = "[MontoD] - [MontoH]";
            this.saldoActual.Name = "saldoActual";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.xrLabel15,
            this.xrLine2,
            this.xrLabel20,
            this.xrLabel21,
            this.xrLabel22,
            this.xrLabel23,
            this.xrLabel24,
            this.xrLabel25,
            this.xrLine3});
            this.ReportFooter.HeightF = 115.9583F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel12
            // 
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[totalSaldoA]")});
            this.xrLabel12.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(538.5417F, 10.00001F);
            this.xrLabel12.Multiline = true;
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(211.4583F, 27.16668F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UsePadding = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "xrLabel12";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel12.TextFormatString = "{0:n}";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(122.9167F, 10.00001F);
            this.xrLabel15.Multiline = true;
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(415.625F, 27.16668F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UsePadding = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "TOTAL BALANCE DE COMPROBACIÓN";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(1.041667F, 48.20846F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(748.9583F, 2.083336F);
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(0F, 63.00002F);
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
            // xrLabel21
            // 
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([][[EsActivo] && IsNull([co_cuepadre])].Sum([saldoActual]), 0)")});
            this.xrLabel21.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(61.14623F, 63.00001F);
            this.xrLabel21.Multiline = true;
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(126.7686F, 23F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UsePadding = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "xrLabel21";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel21.TextFormatString = "{0:n}";
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(220.0417F, 63.00005F);
            this.xrLabel22.Multiline = true;
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(116.6959F, 22.99997F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UsePadding = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "Pasivo + Capital:";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel23
            // 
            this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([][([EsPasivo] || [EsCapital] || [EsAdicional]) && IsNull([co_cuepadre])]." +
                    "Sum([saldoActual]), 0)")});
            this.xrLabel23.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(336.7376F, 63.00001F);
            this.xrLabel23.Multiline = true;
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(153.8789F, 23F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UsePadding = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "xrLabel23";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel23.TextFormatString = "{0:n}";
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(514.95F, 63.00001F);
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
            // xrLabel25
            // 
            this.xrLabel25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "IsNull([][[EsIngEgr] && IsNull([co_cuepadre])].Sum([saldoActual]), 0)")});
            this.xrLabel25.Font = new System.Drawing.Font("Microsoft JhengHei", 8.75F);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(625.0229F, 62.99995F);
            this.xrLabel25.Multiline = true;
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(6, 6, 3, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(123.9772F, 23F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UsePadding = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "xrLabel25";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrLabel25.TextFormatString = "{0:n}";
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(1.041698F, 0.9166555F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(748.9583F, 2.083333F);
            // 
            // totalSaldoA
            // 
            this.totalSaldoA.DataMember = "RepBalanceGeneral2KDoce";
            this.totalSaldoA.Expression = "[][IsNull([co_cuepadre]) && ![EsIngEgr]].Sum([saldoActual])";
            this.totalSaldoA.Name = "totalSaldoA";
            // 
            // RepBalanceGeneral2KDoce
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.Detail,
            this.PageHeader,
            this.ReportFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.saldoActual,
            this.totalSaldoA});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DemoCont});
            this.DataMember = "RepBalanceGeneral2KDoce";
            this.DataSource = this.DemoCont;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 70, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.fecha,
            this.cenDesde,
            this.cenHasta,
            this.cueDesde,
            this.cueHasta});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.GroupCaption1,
            this.GroupData1,
            this.DetailCaption1,
            this.DetailData1,
            this.GroupFooterBackground3,
            this.DetailData3_Odd,
            this.PageInfo});
            this.Version = "18.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.RepBalanceGeneral2KDoce_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void RepBalanceGeneral2KDoce_DataSourceDemanded(object sender, EventArgs e)
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