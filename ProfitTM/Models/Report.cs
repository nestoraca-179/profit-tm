using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Report
    {
        public static List<TreeReports> GetTreeReportsByProdMod(string prod, string mod)
        {
            ProfitTMEntities db = new ProfitTMEntities();
            List<TreeReports> treeReports = new List<TreeReports>();

            treeReports = db.TreeReports.AsNoTracking().Where(t => t.Product == prod && t.Module == mod).ToList();
            foreach (TreeReports tree in treeReports)
            {
                tree.GroupReports = db.GroupReports.AsNoTracking().Where(g => g.TreeID == tree.ID).ToList();
                foreach (GroupReports group in tree.GroupReports)
                {
                    group.Reports = db.Reports.AsNoTracking().Where(r => r.GroupID == group.ID).ToList();
                    foreach (Reports report in group.Reports)
                    {
                        report.GroupReports = null;
                    }

                    group.TreeReports = null;
                }
            }

            return treeReports;
        }
    }
}