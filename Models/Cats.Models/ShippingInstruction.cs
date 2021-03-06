﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models
{
    public class ShippingInstruction
    {
        public ShippingInstruction()
        {
            //this.DispatchAllocations = new List<DispatchAllocation>();
            //this.OtherDispatchAllocations = new List<OtherDispatchAllocation>();
            this.ProjectCodeAllocations = new List<ProjectCodeAllocation>();
            this.Transactions = new List<Transaction>();
            this.DonationPlanHeaders = new List<DonationPlanHeader>();
            this.LocalPurchases=new List<LocalPurchase>();
            this.LoanReciptPlans=new List<LoanReciptPlan>();
            this.Transfers=new List<Transfer>();
        }

        public int ShippingInstructionID { get; set; }
        public string Value { get; set; }
        //public virtual ICollection<DispatchAllocation> DispatchAllocations { get; set; }
        //public virtual ICollection<OtherDispatchAllocation> OtherDispatchAllocations { get; set; }
        public virtual ICollection<ProjectCodeAllocation> ProjectCodeAllocations { get; set; }
        public virtual ICollection<GiftCertificate> GiftCertificates { get; set; } 
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<OtherDispatchAllocation> OtherDispatchAllocations { get; set; }
        public virtual ICollection<DonationPlanHeader> DonationPlanHeaders { get; set; }
        public virtual ICollection<LocalPurchase> LocalPurchases { get; set; }
        public virtual ICollection<LoanReciptPlan> LoanReciptPlans { get; set; }
        public virtual ICollection<Transfer> Transfers  { get; set; }
        public List<ShippingInstruction> GetSIList()
        {
            List<ShippingInstruction> shippingInstructions = new List<ShippingInstruction>();

            shippingInstructions.Add(new Cats.Models.ShippingInstruction { ShippingInstructionID = 104, Value = "00013753" });

            shippingInstructions.Add(new Cats.Models.ShippingInstruction { ShippingInstructionID = 102, Value = "00014110" });

            return shippingInstructions;

        }
    }
            
}

