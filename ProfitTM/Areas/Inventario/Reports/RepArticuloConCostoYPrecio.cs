﻿using DevExpress.XtraReports.UI;
using System;

/// <summary>
/// Summary description for RepArticuloConCostoYPrecio
/// </summary>
public class RepArticuloConCostoYPrecio : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.DataAccess.Sql.SqlDataSource DemoAdmin;
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
    private DetailBand Detail;
    private XRTable table2;
    private XRTableRow tableRow2;
    private XRTableCell tableCell6;
    private XRTableCell tableCell7;
    private XRTableCell tableCell8;
    private XRTableCell tableCell9;
    private XRTableCell tableCell10;
    private DevExpress.XtraReports.Parameters.Parameter artDesde;
    private DevExpress.XtraReports.Parameters.Parameter artHasta;
    private DevExpress.XtraReports.Parameters.Parameter codPrecio;
    private DevExpress.XtraReports.Parameters.Parameter codAlmacen;
    private DevExpress.XtraReports.Parameters.Parameter codCosto;
    private XRTableCell tableCell5;
    private XRTableCell tableCell4;
    private XRTableCell tableCell3;
    private XRTableCell tableCell2;
    private XRTableCell tableCell1;
    private XRTableRow tableRow1;
    private XRTable table1;
    private XRLabel xrLabel3;
    private XRPageInfo xrPageInfo2;
    private XRPageInfo xrPageInfo1;
    private XRLabel label1;
    private PageHeaderBand PageHeader;
    public XRPictureBox PB_Logo;
    public XRLabel LBL_RIF;
    public XRLabel LBL_Telf;
    public XRLabel LBL_Direc;
    public XRLabel LBL_DescEmpresa;
    private XRLabel xrLabel7;
    private XRLabel xrLabel8;
    private XRLabel xrLabel6;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RepArticuloConCostoYPrecio()
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
            DevExpress.DataAccess.Sql.QueryParameter queryParameter9 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter10 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter11 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter12 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter13 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter14 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter15 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter16 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter17 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter18 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter19 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter20 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter21 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter22 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.QueryParameter queryParameter23 = new DevExpress.DataAccess.Sql.QueryParameter();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery1 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery2 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            DevExpress.DataAccess.Sql.CustomSqlQuery customSqlQuery3 = new DevExpress.DataAccess.Sql.CustomSqlQuery();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepArticuloConCostoYPrecio));
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings1 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings2 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings3 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.DynamicListLookUpSettings dynamicListLookUpSettings4 = new DevExpress.XtraReports.Parameters.DynamicListLookUpSettings();
            DevExpress.XtraReports.Parameters.StaticListLookUpSettings staticListLookUpSettings1 = new DevExpress.XtraReports.Parameters.StaticListLookUpSettings();
            this.DemoAdmin = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
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
            this.table2 = new DevExpress.XtraReports.UI.XRTable();
            this.tableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.artDesde = new DevExpress.XtraReports.Parameters.Parameter();
            this.artHasta = new DevExpress.XtraReports.Parameters.Parameter();
            this.codPrecio = new DevExpress.XtraReports.Parameters.Parameter();
            this.codAlmacen = new DevExpress.XtraReports.Parameters.Parameter();
            this.codCosto = new DevExpress.XtraReports.Parameters.Parameter();
            this.tableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.table1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.label1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.LBL_RIF = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_Telf = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_Direc = new DevExpress.XtraReports.UI.XRLabel();
            this.LBL_DescEmpresa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.PB_Logo = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // DemoAdmin
            // 
            this.DemoAdmin.ConnectionName = "DemoAdmin";
            this.DemoAdmin.Name = "DemoAdmin";
            storedProcQuery1.Name = "RepArticuloConCostoYPrecio";
            queryParameter1.Name = "@sCo_Art_d";
            queryParameter1.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter1.Value = new DevExpress.DataAccess.Expression("?artDesde", typeof(string));
            queryParameter2.Name = "@sCo_Art_h";
            queryParameter2.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter2.Value = new DevExpress.DataAccess.Expression("?artHasta", typeof(string));
            queryParameter3.Name = "@sCo_Linea_d";
            queryParameter3.Type = typeof(string);
            queryParameter4.Name = "@sCo_Linea_h";
            queryParameter4.Type = typeof(string);
            queryParameter5.Name = "@sCo_Categoria_d";
            queryParameter5.Type = typeof(string);
            queryParameter6.Name = "@sCo_Categoria_h";
            queryParameter6.Type = typeof(string);
            queryParameter7.Name = "@sCo_SubLinea_d";
            queryParameter7.Type = typeof(string);
            queryParameter8.Name = "@sCo_SubLinea_h";
            queryParameter8.Type = typeof(string);
            queryParameter9.Name = "@sCo_Color_d";
            queryParameter9.Type = typeof(string);
            queryParameter10.Name = "@sCo_Color_h";
            queryParameter10.Type = typeof(string);
            queryParameter11.Name = "@sCo_Almacen";
            queryParameter11.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter11.Value = new DevExpress.DataAccess.Expression("?codAlmacen", typeof(string));
            queryParameter12.Name = "@dCo_FechaHasta";
            queryParameter12.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter12.Value = new DevExpress.DataAccess.Expression("Now()", typeof(System.DateTime));
            queryParameter13.Name = "@sCo_Precio01";
            queryParameter13.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter13.Value = new DevExpress.DataAccess.Expression("?codPrecio", typeof(string));
            queryParameter14.Name = "@sCo_Precio02";
            queryParameter14.Type = typeof(string);
            queryParameter15.Name = "@sCo_Precio03";
            queryParameter15.Type = typeof(string);
            queryParameter16.Name = "@sCo_Precio04";
            queryParameter16.Type = typeof(string);
            queryParameter17.Name = "@sCo_Precio05";
            queryParameter17.Type = typeof(string);
            queryParameter18.Name = "@sCo_TipoCosto";
            queryParameter18.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter18.Value = new DevExpress.DataAccess.Expression("?codCosto", typeof(char));
            queryParameter19.Name = "@sCo_Excluir";
            queryParameter19.Type = typeof(DevExpress.DataAccess.Expression);
            queryParameter19.Value = new DevExpress.DataAccess.Expression("\' \'", typeof(char));
            queryParameter20.Name = "@sCo_Sucursal";
            queryParameter20.Type = typeof(string);
            queryParameter21.Name = "@sCampOrderBy";
            queryParameter21.Type = typeof(string);
            queryParameter22.Name = "@sDir";
            queryParameter22.Type = typeof(string);
            queryParameter23.Name = "@bHeaderRep";
            queryParameter23.Type = typeof(bool);
            queryParameter23.ValueInfo = "False";
            storedProcQuery1.Parameters.Add(queryParameter1);
            storedProcQuery1.Parameters.Add(queryParameter2);
            storedProcQuery1.Parameters.Add(queryParameter3);
            storedProcQuery1.Parameters.Add(queryParameter4);
            storedProcQuery1.Parameters.Add(queryParameter5);
            storedProcQuery1.Parameters.Add(queryParameter6);
            storedProcQuery1.Parameters.Add(queryParameter7);
            storedProcQuery1.Parameters.Add(queryParameter8);
            storedProcQuery1.Parameters.Add(queryParameter9);
            storedProcQuery1.Parameters.Add(queryParameter10);
            storedProcQuery1.Parameters.Add(queryParameter11);
            storedProcQuery1.Parameters.Add(queryParameter12);
            storedProcQuery1.Parameters.Add(queryParameter13);
            storedProcQuery1.Parameters.Add(queryParameter14);
            storedProcQuery1.Parameters.Add(queryParameter15);
            storedProcQuery1.Parameters.Add(queryParameter16);
            storedProcQuery1.Parameters.Add(queryParameter17);
            storedProcQuery1.Parameters.Add(queryParameter18);
            storedProcQuery1.Parameters.Add(queryParameter19);
            storedProcQuery1.Parameters.Add(queryParameter20);
            storedProcQuery1.Parameters.Add(queryParameter21);
            storedProcQuery1.Parameters.Add(queryParameter22);
            storedProcQuery1.Parameters.Add(queryParameter23);
            storedProcQuery1.StoredProcName = "RepArticuloConCostoYPrecio";
            customSqlQuery1.Name = "Articulos";
            customSqlQuery1.Sql = "select * from saArticulo where tipo <> \'S\'";
            customSqlQuery2.Name = "Precios";
            customSqlQuery2.Sql = "select co_precio, des_precio from saTipoPrecio order by co_precio";
            customSqlQuery3.Name = "Almacenes";
            customSqlQuery3.Sql = "select * from saAlmacen";
            this.DemoAdmin.Queries.AddRange(new DevExpress.DataAccess.Sql.SqlQuery[] {
            storedProcQuery1,
            customSqlQuery1,
            customSqlQuery2,
            customSqlQuery3});
            this.DemoAdmin.ResultSchemaSerializable = resources.GetString("DemoAdmin.ResultSchemaSerializable");
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
            // pageInfo1
            // 
            this.pageInfo1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.pageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 6.00001F);
            this.pageInfo1.Name = "pageInfo1";
            this.pageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.pageInfo1.SizeF = new System.Drawing.SizeF(374.1247F, 23F);
            this.pageInfo1.StyleName = "PageInfo";
            this.pageInfo1.StylePriority.UseFont = false;
            // 
            // pageInfo2
            // 
            this.pageInfo2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.pageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(374.1247F, 6.00001F);
            this.pageInfo2.Name = "pageInfo2";
            this.pageInfo2.SizeF = new System.Drawing.SizeF(375.8753F, 23F);
            this.pageInfo2.StyleName = "PageInfo";
            this.pageInfo2.StylePriority.UseFont = false;
            this.pageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.pageInfo2.TextFormatString = "Página {0} de {1}";
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 1.458343F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.table2});
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            // 
            // table2
            // 
            this.table2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.table2.Name = "table2";
            this.table2.OddStyleName = "DetailData3_Odd";
            this.table2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow2});
            this.table2.SizeF = new System.Drawing.SizeF(750F, 25F);
            // 
            // tableRow2
            // 
            this.tableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell6,
            this.tableCell7,
            this.tableCell8,
            this.tableCell9,
            this.tableCell10});
            this.tableRow2.Name = "tableRow2";
            this.tableRow2.Weight = 11.5D;
            // 
            // tableCell6
            // 
            this.tableCell6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[co_art]")});
            this.tableCell6.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell6.Name = "tableCell6";
            this.tableCell6.StyleName = "DetailData1";
            this.tableCell6.StylePriority.UseBorders = false;
            this.tableCell6.StylePriority.UseFont = false;
            this.tableCell6.Weight = 0.18256080040564904D;
            // 
            // tableCell7
            // 
            this.tableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[art_des]")});
            this.tableCell7.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell7.Name = "tableCell7";
            this.tableCell7.StyleName = "DetailData1";
            this.tableCell7.StylePriority.UseFont = false;
            this.tableCell7.Weight = 0.53998131774586411D;
            // 
            // tableCell8
            // 
            this.tableCell8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[co_uni]")});
            this.tableCell8.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell8.Name = "tableCell8";
            this.tableCell8.StyleName = "DetailData1";
            this.tableCell8.StylePriority.UseFont = false;
            this.tableCell8.Weight = 0.12226511945856056D;
            // 
            // tableCell9
            // 
            this.tableCell9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[precio1]")});
            this.tableCell9.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell9.Name = "tableCell9";
            this.tableCell9.StyleName = "DetailData1";
            this.tableCell9.StylePriority.UseFont = false;
            this.tableCell9.StylePriority.UseTextAlignment = false;
            this.tableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell9.TextFormatString = "{0:n}";
            this.tableCell9.Weight = 0.15377638517752257D;
            // 
            // tableCell10
            // 
            this.tableCell10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ultimo]")});
            this.tableCell10.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.tableCell10.Name = "tableCell10";
            this.tableCell10.StyleName = "DetailData1";
            this.tableCell10.StylePriority.UseFont = false;
            this.tableCell10.StylePriority.UseTextAlignment = false;
            this.tableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tableCell10.TextFormatString = "{0:n}";
            this.tableCell10.Weight = 0.15526254460186142D;
            // 
            // artDesde
            // 
            this.artDesde.AllowNull = true;
            this.artDesde.Description = "Articulo Desde";
            dynamicListLookUpSettings1.DataMember = "Articulos";
            dynamicListLookUpSettings1.DataSource = this.DemoAdmin;
            dynamicListLookUpSettings1.DisplayMember = "art_des";
            dynamicListLookUpSettings1.SortMember = "co_art";
            dynamicListLookUpSettings1.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings1.ValueMember = "co_art";
            this.artDesde.LookUpSettings = dynamicListLookUpSettings1;
            this.artDesde.Name = "artDesde";
            // 
            // artHasta
            // 
            this.artHasta.AllowNull = true;
            this.artHasta.Description = "Articulo Hasta";
            dynamicListLookUpSettings2.DataMember = "Articulos";
            dynamicListLookUpSettings2.DataSource = this.DemoAdmin;
            dynamicListLookUpSettings2.DisplayMember = "art_des";
            dynamicListLookUpSettings2.SortMember = "co_art";
            dynamicListLookUpSettings2.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings2.ValueMember = "co_art";
            this.artHasta.LookUpSettings = dynamicListLookUpSettings2;
            this.artHasta.Name = "artHasta";
            // 
            // codPrecio
            // 
            this.codPrecio.Description = "Precio (*)";
            dynamicListLookUpSettings3.DataMember = "Precios";
            dynamicListLookUpSettings3.DataSource = this.DemoAdmin;
            dynamicListLookUpSettings3.DisplayMember = "des_precio";
            dynamicListLookUpSettings3.SortMember = "co_precio";
            dynamicListLookUpSettings3.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings3.ValueMember = "co_precio";
            this.codPrecio.LookUpSettings = dynamicListLookUpSettings3;
            this.codPrecio.Name = "codPrecio";
            // 
            // codAlmacen
            // 
            this.codAlmacen.Description = "Almacen (*)";
            dynamicListLookUpSettings4.DataMember = "Almacenes";
            dynamicListLookUpSettings4.DataSource = this.DemoAdmin;
            dynamicListLookUpSettings4.DisplayMember = "des_alma";
            dynamicListLookUpSettings4.SortMember = "co_alma";
            dynamicListLookUpSettings4.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            dynamicListLookUpSettings4.ValueMember = "co_alma";
            this.codAlmacen.LookUpSettings = dynamicListLookUpSettings4;
            this.codAlmacen.Name = "codAlmacen";
            this.codAlmacen.ValueInfo = " ";
            // 
            // codCosto
            // 
            this.codCosto.Description = "Costo (*)";
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue('1', "Ultimo Costo"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue('2', "Costo Promedio"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue('3', "Último Costo OM"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue('4', "Costo Promedio OM"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue('5', "Reposición"));
            staticListLookUpSettings1.LookUpValues.Add(new DevExpress.XtraReports.Parameters.LookUpValue('6', "Proveedor"));
            this.codCosto.LookUpSettings = staticListLookUpSettings1;
            this.codCosto.Name = "codCosto";
            this.codCosto.Type = typeof(char);
            this.codCosto.ValueInfo = " ";
            // 
            // tableCell5
            // 
            this.tableCell5.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell5.Name = "tableCell5";
            this.tableCell5.StyleName = "DetailCaption1";
            this.tableCell5.StylePriority.UseFont = false;
            this.tableCell5.StylePriority.UseTextAlignment = false;
            this.tableCell5.Text = "Costo";
            this.tableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell5.Weight = 0.15526255586913748D;
            // 
            // tableCell4
            // 
            this.tableCell4.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell4.Name = "tableCell4";
            this.tableCell4.StyleName = "DetailCaption1";
            this.tableCell4.StylePriority.UseFont = false;
            this.tableCell4.StylePriority.UseTextAlignment = false;
            this.tableCell4.Text = "Precio";
            this.tableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell4.Weight = 0.15377638438749708D;
            // 
            // tableCell3
            // 
            this.tableCell3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell3.Name = "tableCell3";
            this.tableCell3.StyleName = "DetailCaption1";
            this.tableCell3.StylePriority.UseFont = false;
            this.tableCell3.StylePriority.UseTextAlignment = false;
            this.tableCell3.Text = "Unidad";
            this.tableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell3.Weight = 0.1222650951558526D;
            // 
            // tableCell2
            // 
            this.tableCell2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.tableCell2.Name = "tableCell2";
            this.tableCell2.StyleName = "DetailCaption1";
            this.tableCell2.StylePriority.UseFont = false;
            this.tableCell2.StylePriority.UseTextAlignment = false;
            this.tableCell2.Text = "Artículo";
            this.tableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell2.Weight = 0.53998134511462559D;
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
            this.tableCell1.Text = "Código";
            this.tableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tableCell1.Weight = 0.18256080040564904D;
            // 
            // tableRow1
            // 
            this.tableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tableCell1,
            this.tableCell2,
            this.tableCell3,
            this.tableCell4,
            this.tableCell5});
            this.tableRow1.Name = "tableRow1";
            this.tableRow1.Weight = 1D;
            // 
            // table1
            // 
            this.table1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 173.0416F);
            this.table1.Name = "table1";
            this.table1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.tableRow1});
            this.table1.SizeF = new System.Drawing.SizeF(750F, 28F);
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(548.3333F, 10.00001F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(153.1251F, 22.99998F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Página";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(548.3333F, 32.99999F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(201.6667F, 23F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(701.4583F, 10.00001F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(48.54169F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei", 14.25F, System.Drawing.FontStyle.Bold);
            this.label1.LocationFloat = new DevExpress.Utils.PointFloat(149.4583F, 135.8473F);
            this.label1.Name = "label1";
            this.label1.SizeF = new System.Drawing.SizeF(451.0417F, 24.19433F);
            this.label1.StyleName = "Title";
            this.label1.StylePriority.UseFont = false;
            this.label1.StylePriority.UseTextAlignment = false;
            this.label1.Text = "Articulos con Costo y Precio";
            this.label1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LBL_RIF,
            this.LBL_Telf,
            this.LBL_Direc,
            this.LBL_DescEmpresa,
            this.xrLabel7,
            this.xrLabel8,
            this.xrLabel6,
            this.PB_Logo,
            this.label1,
            this.xrPageInfo1,
            this.xrPageInfo2,
            this.xrLabel3,
            this.table1});
            this.PageHeader.HeightF = 201.0416F;
            this.PageHeader.Name = "PageHeader";
            // 
            // LBL_RIF
            // 
            this.LBL_RIF.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_RIF.LocationFloat = new DevExpress.Utils.PointFloat(213F, 33.00001F);
            this.LBL_RIF.Multiline = true;
            this.LBL_RIF.Name = "LBL_RIF";
            this.LBL_RIF.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_RIF.SizeF = new System.Drawing.SizeF(327.9606F, 17.99997F);
            this.LBL_RIF.StylePriority.UseFont = false;
            // 
            // LBL_Telf
            // 
            this.LBL_Telf.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_Telf.LocationFloat = new DevExpress.Utils.PointFloat(212.9983F, 50.99997F);
            this.LBL_Telf.Multiline = true;
            this.LBL_Telf.Name = "LBL_Telf";
            this.LBL_Telf.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_Telf.SizeF = new System.Drawing.SizeF(327.9623F, 18F);
            this.LBL_Telf.StylePriority.UseFont = false;
            // 
            // LBL_Direc
            // 
            this.LBL_Direc.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.LBL_Direc.LocationFloat = new DevExpress.Utils.PointFloat(212.9983F, 68.99994F);
            this.LBL_Direc.Multiline = true;
            this.LBL_Direc.Name = "LBL_Direc";
            this.LBL_Direc.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_Direc.SizeF = new System.Drawing.SizeF(327.9623F, 43.61108F);
            this.LBL_Direc.StylePriority.UseFont = false;
            // 
            // LBL_DescEmpresa
            // 
            this.LBL_DescEmpresa.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold);
            this.LBL_DescEmpresa.LocationFloat = new DevExpress.Utils.PointFloat(149.4583F, 10.00003F);
            this.LBL_DescEmpresa.Multiline = true;
            this.LBL_DescEmpresa.Name = "LBL_DescEmpresa";
            this.LBL_DescEmpresa.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LBL_DescEmpresa.SizeF = new System.Drawing.SizeF(391.5023F, 22.99998F);
            this.LBL_DescEmpresa.StylePriority.UseFont = false;
            this.LBL_DescEmpresa.Text = "LBL_DescEmpresa";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(149.4583F, 50.99997F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(63.54F, 18F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Teléfonos:";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(149.4583F, 33.00001F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(63.54167F, 17.99997F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "RIF:";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Microsoft JhengHei", 8.25F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(149.4583F, 68.99994F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(63.53999F, 43.61108F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Dirección:";
            // 
            // PB_Logo
            // 
            this.PB_Logo.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 9.79166F);
            this.PB_Logo.Name = "PB_Logo";
            this.PB_Logo.SizeF = new System.Drawing.SizeF(128.5417F, 102.611F);
            this.PB_Logo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // RepArticuloConCostoYPrecio
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.Detail,
            this.PageHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.DemoAdmin});
            this.DataMember = "RepArticuloConCostoYPrecio";
            this.DataSource = this.DemoAdmin;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 70, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.artDesde,
            this.artHasta,
            this.codPrecio,
            this.codAlmacen,
            this.codCosto});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.Title,
            this.DetailCaption1,
            this.DetailData1,
            this.DetailData3_Odd,
            this.PageInfo});
            this.Version = "18.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.RepArticuloConCostoYPrecio_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void RepArticuloConCostoYPrecio_DataSourceDemanded(object sender, EventArgs e)
    {
        XtraReport report = (XtraReport)sender;

        var parameters = report.Parameters;

        foreach (var param in parameters)
        {
            if (param.Value.ToString() == "")
            {
                param.Value = DBNull.Value;
            }
        }
    }
}
