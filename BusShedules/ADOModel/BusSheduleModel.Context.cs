﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusShedules.ADOModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BusSheduleEntities : DbContext
    {
        public BusSheduleEntities()
            : base("name=BusSheduleEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<BusNumber> BusNumber { get; set; }
        public virtual DbSet<DateRoute> DateRoute { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RouteStop> RouteStop { get; set; }
        public virtual DbSet<Stop> Stop { get; set; }
        public virtual DbSet<StopName> StopName { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
