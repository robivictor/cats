﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Mapping
{
    public class RationDetailMap : EntityTypeConfiguration<RationDetail>
    {
        public RationDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.RationDetailID);

            // Properties
            // Table & Column Mappings
            this.ToTable("RationDetail");
            this.Property(t => t.RationDetailID).HasColumnName("RationDetailID");
            this.Property(t => t.RationID).HasColumnName("RationID");
            this.Property(t => t.CommodityID).HasColumnName("CommodityID");
            this.Property(t => t.Rate).HasColumnName("Rate");
            //this.Property(t => t.rowguid).HasColumnName("rowguid");

            // Relationships
            this.HasRequired(t => t.Ration)
                .WithMany(t => t.RationDetails)
                .HasForeignKey(d => d.RationID);

        }
    }
}
