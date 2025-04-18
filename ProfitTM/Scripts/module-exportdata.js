﻿/*
 Highcharts JS v9.1.2 (2021-06-16)

 Exporting module

 (c) 2010-2021 Torstein Honsi

 License: www.highcharts.com/license
*/
'use strict'; (function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/export-data", ["highcharts", "highcharts/modules/exporting"], function (k) { a(k); a.Highcharts = k; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function k(a, d, f, p) { a.hasOwnProperty(d) || (a[d] = p.apply(null, f)) } a = a ? a._modules : {}; k(a, "Extensions/DownloadURL.js", [a["Core/Globals.js"]], function (a) {
        var d = a.isSafari,
        f = a.win, p = f.document, l = f.URL || f.webkitURL || f, t = a.dataURLtoBlob = function (a) { if ((a = a.replace(/filename=.*;/, "").match(/data:([^;]*)(;base64)?,([0-9A-Za-z+/]+)/)) && 3 < a.length && f.atob && f.ArrayBuffer && f.Uint8Array && f.Blob && l.createObjectURL) { var d = f.atob(a[3]), g = new f.ArrayBuffer(d.length); g = new f.Uint8Array(g); for (var c = 0; c < g.length; ++c)g[c] = d.charCodeAt(c); a = new f.Blob([g], { type: a[1] }); return l.createObjectURL(a) } }; a = a.downloadURL = function (a, l) {
            var g = f.navigator, c = p.createElement("a"); if ("string" ===
                typeof a || a instanceof String || !g.msSaveOrOpenBlob) { a = "" + a; g = /Edge\/\d+/.test(g.userAgent); if (d && "string" === typeof a && 0 === a.indexOf("data:application/pdf") || g || 2E6 < a.length) if (a = t(a) || "", !a) throw Error("Failed to convert to blob"); if ("undefined" !== typeof c.download) c.href = a, c.download = l, p.body.appendChild(c), c.click(), p.body.removeChild(c); else try { var n = f.open(a, "chart"); if ("undefined" === typeof n || null === n) throw Error("Failed to open window"); } catch (F) { f.location.href = a } } else g.msSaveOrOpenBlob(a,
                    l)
        }; return { dataURLtoBlob: t, downloadURL: a }
    }); k(a, "Extensions/ExportData.js", [a["Core/Axis/Axis.js"], a["Core/Chart/Chart.js"], a["Core/Renderer/HTML/AST.js"], a["Core/Globals.js"], a["Core/DefaultOptions.js"], a["Core/Utilities.js"], a["Extensions/DownloadURL.js"]], function (a, d, f, p, l, t, g) {
        function k(a, B) {
            var b = n.navigator, c = -1 < b.userAgent.indexOf("WebKit") && 0 > b.userAgent.indexOf("Chrome"), f = n.URL || n.webkitURL || n; try {
                if (b.msSaveOrOpenBlob && n.MSBlobBuilder) { var r = new n.MSBlobBuilder; r.append(a); return r.getBlob("image/svg+xml") } if (!c) return f.createObjectURL(new n.Blob(["\ufeff" +
                    a], { type: B }))
            } catch (O) { }
        } var J = p.doc, c = p.seriesTypes, n = p.win; p = l.getOptions; l = l.setOptions; var F = t.addEvent, K = t.defined, G = t.extend, L = t.find, D = t.fireEvent, M = t.isNumber, w = t.pick, H = g.downloadURL; l({
            exporting: { csv: { annotations: { itemDelimiter: "; ", join: !1 }, columnHeaderFormatter: null, dateFormat: "%Y-%m-%d %H:%M:%S", decimalPoint: null, itemDelimiter: null, lineDelimiter: "\n" }, showTable: !1, useMultiLevelHeaders: !0, useRowspanHeaders: !0 }, lang: {
                downloadCSV: "Download CSV", downloadXLS: "Download XLS", exportData: {
                    annotationHeader: "Annotations",
                    categoryHeader: "Category", categoryDatetimeHeader: "DateTime"
                }, viewData: "View data table", hideData: "Hide data table"
            }
        }); F(d, "render", function () { this.options && this.options.exporting && this.options.exporting.showTable && !this.options.chart.forExport && !this.dataTableDiv && this.viewData() }); d.prototype.setUpKeyToAxis = function () { c.arearange && (c.arearange.prototype.keyToAxis = { low: "y", high: "y" }); c.gantt && (c.gantt.prototype.keyToAxis = { start: "x", end: "x" }) }; d.prototype.getDataRows = function (b) {
            var c = this.hasParallelCoordinates,
            m = this.time, f = this.options.exporting && this.options.exporting.csv || {}, d = this.xAxis, r = {}, g = [], p = [], n = [], y; var z = this.options.lang.exportData; var l = z.categoryHeader, N = z.categoryDatetimeHeader, u = function (q, e, m) { if (f.columnHeaderFormatter) { var c = f.columnHeaderFormatter(q, e, m); if (!1 !== c) return c } return q ? q instanceof a ? q.options.title && q.options.title.text || (q.dateTime ? N : l) : b ? { columnTitle: 1 < m ? e : q.name, topLevelColumnTitle: q.name } : q.name + (1 < m ? " (" + e + ")" : "") : l }, I = function (a, b, e) {
                var q = {}, m = {}; b.forEach(function (b) {
                    var c =
                        (a.keyToAxis && a.keyToAxis[b] || b) + "Axis"; c = M(e) ? a.chart[c][e] : a[c]; q[b] = c && c.categories || []; m[b] = c && c.dateTime
                }); return { categoryMap: q, dateTimeValueAxisMap: m }
            }, t = function (a, b) { return a.data.filter(function (a) { return "undefined" !== typeof a.y && a.name }).length && b && !b.categories && !a.keyToAxis ? a.pointArrayMap && a.pointArrayMap.filter(function (a) { return "x" === a }).length ? (a.pointArrayMap.unshift("x"), a.pointArrayMap) : ["x", "y"] : a.pointArrayMap || ["y"] }, h = []; var x = 0; this.setUpKeyToAxis(); this.series.forEach(function (a) {
                var e =
                    a.xAxis, q = a.options.keys || t(a, e), B = q.length, g = !a.requireSorting && {}, k = d.indexOf(e), C = I(a, q), l; if (!1 !== a.options.includeInDataExport && !a.options.isInternal && !1 !== a.visible) {
                        L(h, function (a) { return a[0] === k }) || h.push([k, x]); for (l = 0; l < B;)y = u(a, q[l], q.length), n.push(y.columnTitle || y), b && p.push(y.topLevelColumnTitle || y), l++; var A = { chart: a.chart, autoIncrement: a.autoIncrement, options: a.options, pointArrayMap: a.pointArrayMap }; a.options.data.forEach(function (b, u) {
                            c && (C = I(a, q, u)); var h = { series: A }; a.pointClass.prototype.applyOptions.apply(h,
                                [b]); b = h.x; var d = a.data[u] && a.data[u].name; l = 0; if (!e || "name" === a.exportKey || !c && e && e.hasNames && d) b = d; g && (g[b] && (b += "|" + u), g[b] = !0); r[b] || (r[b] = [], r[b].xValues = []); r[b].x = h.x; r[b].name = d; for (r[b].xValues[k] = h.x; l < B;)u = q[l], d = h[u], r[b][x + l] = w(C.categoryMap[u][d], C.dateTimeValueAxisMap[u] ? m.dateFormat(f.dateFormat, d) : null, d), l++
                        }); x += l
                    }
            }); for (e in r) Object.hasOwnProperty.call(r, e) && g.push(r[e]); var e = b ? [p, n] : [n]; for (x = h.length; x--;) {
                var A = h[x][0]; var E = h[x][1]; var k = d[A]; g.sort(function (a, b) {
                    return a.xValues[A] -
                        b.xValues[A]
                }); z = u(k); e[0].splice(E, 0, z); b && e[1] && e[1].splice(E, 0, z); g.forEach(function (a) { var b = a.name; k && !K(b) && (k.dateTime ? (a.x instanceof Date && (a.x = a.x.getTime()), b = m.dateFormat(f.dateFormat, a.x)) : b = k.categories ? w(k.names[a.x], k.categories[a.x], a.x) : a.x); a.splice(E, 0, b) })
            } e = e.concat(g); D(this, "exportData", { dataRows: e }); return e
        }; d.prototype.getCSV = function (a) {
            var b = "", c = this.getDataRows(), d = this.options.exporting.csv, f = w(d.decimalPoint, "," !== d.itemDelimiter && a ? (1.1).toLocaleString()[1] : "."),
            l = w(d.itemDelimiter, "," === f ? ";" : ","), g = d.lineDelimiter; c.forEach(function (a, d) { for (var m, r = a.length; r--;)m = a[r], "string" === typeof m && (m = '"' + m + '"'), "number" === typeof m && "." !== f && (m = m.toString().replace(".", f)), a[r] = m; b += a.join(l); d < c.length - 1 && (b += g) }); return b
        }; d.prototype.getTable = function (a) {
            var b = function (a) {
                if (!a.tagName || "#text" === a.tagName) return a.textContent || ""; var c = a.attributes, d = "<" + a.tagName; c && Object.keys(c).forEach(function (a) { d += " " + a + '="' + c[a] + '"' }); d += ">"; d += a.textContent || ""; (a.children ||
                    []).forEach(function (a) { d += b(a) }); return d += "</" + a.tagName + ">"
            }; a = this.getTableAST(a); return b(a)
        }; d.prototype.getTableAST = function (a) {
            var b = 0, c = [], d = this.options, f = a ? (1.1).toLocaleString()[1] : ".", l = w(d.exporting.useMultiLevelHeaders, !0); a = this.getDataRows(l); var g = l ? a.shift() : null, k = a.shift(), n = function (a, b, c, d) { var h = w(d, ""); b = "text" + (b ? " " + b : ""); "number" === typeof h ? (h = h.toString(), "," === f && (h = h.replace(".", f)), b = "number") : d || (b = "empty"); c = G({ "class": b }, c); return { tagName: a, attributes: c, textContent: h } };
            !1 !== d.exporting.tableCaption && c.push({ tagName: "caption", attributes: { "class": "highcharts-table-caption" }, textContent: w(d.exporting.tableCaption, d.title.text ? d.title.text : "Chart") }); for (var p = 0, t = a.length; p < t; ++p)a[p].length > b && (b = a[p].length); c.push(function (a, b, c) {
                var f = [], h = 0; c = c || b && b.length; var g = 0, e; if (e = l && a && b) { a: if (e = a.length, b.length === e) { for (; e--;)if (a[e] !== b[e]) { e = !1; break a } e = !0 } else e = !1; e = !e } if (e) {
                    for (e = []; h < c; ++h) {
                        var m = a[h]; var k = a[h + 1]; m === k ? ++g : g ? (e.push(n("th", "highcharts-table-topheading",
                            { scope: "col", colspan: g + 1 }, m)), g = 0) : (m === b[h] ? d.exporting.useRowspanHeaders ? (k = 2, delete b[h]) : (k = 1, b[h] = "") : k = 1, m = n("th", "highcharts-table-topheading", { scope: "col" }, m), 1 < k && m.attributes && (m.attributes.valign = "top", m.attributes.rowspan = k), e.push(m))
                    } f.push({ tagName: "tr", children: e })
                } if (b) { e = []; h = 0; for (c = b.length; h < c; ++h)"undefined" !== typeof b[h] && e.push(n("th", null, { scope: "col" }, b[h])); f.push({ tagName: "tr", children: e }) } return { tagName: "thead", children: f }
            }(g, k, Math.max(b, k.length))); var v = []; a.forEach(function (a) {
                for (var c =
                    [], d = 0; d < b; d++)c.push(n(d ? "td" : "th", null, d ? {} : { scope: "row" }, a[d])); v.push({ tagName: "tr", children: c })
            }); c.push({ tagName: "tbody", children: v }); c = { tree: { tagName: "table", id: "highcharts-data-table-" + this.index, children: c } }; D(this, "aftergetTableAST", c); return c.tree
        }; d.prototype.downloadCSV = function () { var a = this.getCSV(!0); H(k(a, "text/csv") || "data:text/csv,\ufeff" + encodeURIComponent(a), this.getFilename() + ".csv") }; d.prototype.downloadXLS = function () {
            var a = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head>\x3c!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>Ark1</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--\x3e<style>td{border:none;font-family: Calibri, sans-serif;} .number{mso-number-format:"0.00";} .text{ mso-number-format:"@";}</style><meta name=ProgId content=Excel.Sheet><meta charset=UTF-8></head><body>' +
                this.getTable(!0) + "</body></html>"; H(k(a, "application/vnd.ms-excel") || "data:application/vnd.ms-excel;base64," + n.btoa(unescape(encodeURIComponent(a))), this.getFilename() + ".xls")
        }; d.prototype.viewData = function () { this.toggleDataTable(!0) }; d.prototype.hideData = function () { this.toggleDataTable(!1) }; d.prototype.toggleDataTable = function (a) {
            (a = w(a, !this.isDataTableVisible)) && !this.dataTableDiv && (this.dataTableDiv = J.createElement("div"), this.dataTableDiv.className = "highcharts-data-table", this.renderTo.parentNode.insertBefore(this.dataTableDiv,
                this.renderTo.nextSibling)); this.dataTableDiv && (this.dataTableDiv.style.display = a ? "block" : "none", a && (this.dataTableDiv.innerHTML = "", (new f([this.getTableAST()])).addToDOM(this.dataTableDiv), D(this, "afterViewData", this.dataTableDiv))); this.isDataTableVisible = a; a = this.exportDivElements; var b = this.options.exporting; b = b && b.buttons && b.buttons.contextButton.menuItems; var c = this.options.lang; v && v.menuItemDefinitions && c && c.viewData && c.hideData && b && a && a.length && f.setElementHTML(a[b.indexOf("viewData")], this.isDataTableVisible ?
                    c.hideData : c.viewData)
        }; var v = p().exporting; v && (G(v.menuItemDefinitions, { downloadCSV: { textKey: "downloadCSV", onclick: function () { this.downloadCSV() } }, downloadXLS: { textKey: "downloadXLS", onclick: function () { this.downloadXLS() } }, viewData: { textKey: "viewData", onclick: function () { this.toggleDataTable() } } }), v.buttons && v.buttons.contextButton.menuItems.push("separator", "downloadCSV", "downloadXLS", "viewData")); c.map && (c.map.prototype.exportKey = "name"); c.mapbubble && (c.mapbubble.prototype.exportKey = "name"); c.treemap &&
            (c.treemap.prototype.exportKey = "name")
    }); k(a, "masters/modules/export-data.src.js", [], function () { })
});
//# sourceMappingURL=export-data.js.map