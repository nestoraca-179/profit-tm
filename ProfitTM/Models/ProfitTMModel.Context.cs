﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProfitTM.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ProfitTMEntities : DbContext
    {
        public ProfitTMEntities() : base("name=ProfitTMEntities")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Connections> Connections { get; set; }
        public virtual DbSet<GroupReports> GroupReports { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<Options> Options { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }
        public virtual DbSet<TreeReports> TreeReports { get; set; }
        public virtual DbSet<UserModules> UserModules { get; set; }
        public virtual DbSet<UserOptions> UserOptions { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Incidents> Incidents { get; set; }
        public virtual DbSet<BoxMoves> BoxMoves { get; set; }
        public virtual DbSet<Transfers> Transfers { get; set; }
        public virtual DbSet<Boxes> Boxes { get; set; }
        public virtual DbSet<LogsFactOnline> LogsFactOnline { get; set; }
    }
}
