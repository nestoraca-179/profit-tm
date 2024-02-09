using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

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
    private XRTableCell tableCell8;
    private XRTableCell tableCell9;
    private XRTableCell tableCell10;
    private DevExpress.XtraReports.Parameters.Parameter parameter1;
    private DevExpress.XtraReports.Parameters.Parameter parameter2;
    private DevExpress.XtraReports.Parameters.Parameter parameter3;
    private DevExpress.XtraReports.Parameters.Parameter parameter4;
    private DevExpress.XtraReports.Parameters.Parameter parameter5;
    private DevExpress.XtraReports.Parameters.Parameter parameter6;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepMayorAnalitico2KDoce));
            this.DemoCont = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.Title = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailCaption1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.DetailData3_Odd = new DevExpress.XtraReports.UI.XRControlStyle();
            this.PageInfo = new DevExpress.XtraReports.UI.XRControlStyle();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.pageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.pageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.parameter1 = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter2 = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter3 = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter4 = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter5 = new DevExpress.XtraReports.Parameters.Parameter();
            this.parameter6 = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DemoCont
            // 
            this.DemoCont.ConnectionName = "DemoCont";
            this.DemoCont.Name = "DemoCont";
            storedProcQuery1.Name = "RepMayorAnalitico2KDocePW";
            queryParameter1.Name = "@fecEmisD";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?parameter1", typeof(System.DateTime));
            queryParameter2.Name = "@fecEmisH";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?parameter2", typeof(System.DateTime));
            queryParameter3.Name = "@coCenD";
            queryParameter3.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter3.Value = new DevExpress.DataAccess.Expression("?parameter3", typeof(string));
            queryParameter4.Name = "@coCenH";
            queryParameter4.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter4.Value = new DevExpress.DataAccess.Expression("?parameter4", typeof(string));
            queryParameter5.Name = "@coGasD";
            queryParameter5.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter5.Value = new DevExpress.DataAccess.Expression("?parameter5", typeof(string));
            queryParameter6.Name = "@coGasH";
            queryParameter6.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter6.Value = new DevExpress.DataAccess.Expression("?parameter6", typeof(string));
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.StoredProcName = "RepMayorAnalitico2KDocePW";
            this.DemoCont.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
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
            this.BottomMargin.Name = "BottomMargin";
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
            this.Detail.HeightF = 32.08335F;
            this.Detail.Name = "Detail";
            // 
            // pageInfo1
            // 
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(450.5F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            // 
            // pageInfo2
            // 
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(450.5F, 9.999974F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(399.4999F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 210.5417F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(850F, 28F);
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
            // tableCell2
            // 
            this.tableCell2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StyleName = "DetailCaption1";
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.Text = "Saldo Inicial";
            this.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell2.Weight = 0.11764706024677296D;
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
            this.tableCell3.Weight = 0.11764706737474136D;
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
            this.tableCell4.Weight = 0.11764705999214202D;
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
            this.tableCell5.Weight = 0.35294119907040589D;
            // 
            // table2
            // 
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4F);
            this.table2.Name = "table2";
            this.table2.OddStyleName = "DetailData3_Odd";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(700F, 25F);
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell6,
            this.tableCell10,
            this.tableCell7,
            this.tableCell8,
            this.tableCell9});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 11.5D;
            // 
            // tableCell6
            // 
            this.tableCell6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[co_cue]")});
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StyleName = "DetailData1";
            this.tableCell6.StylePriority.UseBorders = false;
            this.tableCell6.Weight = 0.15384615925488326D;
            // 
            // tableCell7
            // 
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SaldoInicial]")});
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StyleName = "DetailData1";
            this.tableCell7.StylePriority.UseTextAlignment = false;
            this.tableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell7.TextFormatString = "{0:C2}";
            this.tableCell7.Weight = 0.15384616633228732D;
            // 
            // tableCell8
            // 
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MontoD]")});
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StyleName = "DetailData1";
            this.tableCell8.StylePriority.UseTextAlignment = false;
            this.tableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell8.TextFormatString = "{0:C2}";
            this.tableCell8.Weight = 0.1538461519348408D;
            // 
            // tableCell9
            // 
            this.tableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[MontoH]")});
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.StyleName = "DetailData1";
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell9.TextFormatString = "{0:C2}";
            this.tableCell9.Weight = 0.15384616367237128D;
            // 
            // tableCell10
            // 
            this.tableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[des_cue]")});
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.StyleName = "DetailData1";
            this.tableCell10.Weight = 0.46153848629036215D;
            // 
            // parameter1
            // 
            this.parameter1.Name = "parameter1";
            this.parameter1.Type = typeof(System.DateTime);
            this.parameter1.ValueInfo = "1753-01-01";
            // 
            // parameter2
            // 
            this.parameter2.Name = "parameter2";
            this.parameter2.Type = typeof(System.DateTime);
            this.parameter2.ValueInfo = "1753-01-01";
            // 
            // parameter3
            // 
            this.parameter3.Name = "parameter3";
            // 
            // parameter4
            // 
            this.parameter4.Name = "parameter4";
            // 
            // parameter5
            // 
            this.parameter5.Name = "parameter5";
            // 
            // parameter6
            // 
            this.parameter6.Name = "parameter6";
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
            // ReportFooter
            // 
            this.ReportFooter.Name = "ReportFooter";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(149.5F, 148.889F);
            this.label1.Name = "label1";
            this.label1.SizeF = new System.Drawing.SizeF(550.5F, 26.27766F);
            this.label1.StyleName = "Title";
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "Mayor Analítico";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
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
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(638.4008F, 33.83329F);
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
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(638.4008F, 10.83331F);
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
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(808.2499F, 10.83331F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(41.75F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
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
            this.xrTableCell1.Weight = 0.17647057283327566D;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(402F, 0.9166665F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(448F, 2.083333F);
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
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DemoCont});
            this.DataMember = "RepMayorAnalitico2KDocePW";
            this.DataSource = this.DemoCont;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 70, 100);
            this.PageWidth = 950;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.parameter1,
            this.parameter2,
            this.parameter3,
            this.parameter4,
            this.parameter5,
            this.parameter6});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailData3_Odd,
            this.PageInfo});
            this.Version = "18.2";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
