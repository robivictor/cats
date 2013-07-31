using System.Data.Entity;
using Cats.Models;
using Cats.Models.Mapping;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Cats.Data
{
    public partial class CatsContext : DbContext
    {
        static CatsContext()
        {
            Database.SetInitializer<CatsContext>(null);
        }

        public CatsContext() : base("Name=CatsContext") { }

        // TODO: Add properties to access set of Poco classes
        public DbSet<RegionalRequest> RegionalRequests { get; set; }
        public DbSet<RegionalRequestDetail> RegionalRequestDetails { get; set; }
        public DbSet<ReliefRequisition> ReliefRequisitions { get; set; }
        public DbSet<ReliefRequisitionDetail> ReliefRequisitionDetails { get; set; }
        public DbSet<AdminUnit> AdminUnits { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<CommodityType> CommodityTypes { get; set; }
        public DbSet<FDP> Fdps { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<AdminUnitType> AdminUnitTypes { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<DispatchAllocation> DispatchAllocations { get; set; }
        public DbSet<DispatchAllocationDetail> DispatchDetail { get; set; }
        public DbSet<Bid> Bids { get; set; } 
        public DbSet<BidDetail> BidDetails { get; set; }
        public DbSet<Status> Statuses { get; set; } 

        public DbSet<TransportBidPlan> TransportBidPlans { get; set; }
        public DbSet<TransportBidPlanDetail> TransportBidPlanDetails { get; set; }
        //public DbSet<HubAllocation> HubAllocations { get; set; }
        public DbSet<ProjectCodeAllocation> ProjectCodeAllocation { get; set; }

        public DbSet<TransportRequisition> TransportRequisition { get; set; }
        public DbSet<HubAllocation> HubAllocation { get; set; }
        public DbSet<ProjectCode> ProjectCode { get; set; }
        public DbSet<ShippingInstruction> ShippingInstruction { get; set; }

       
        public DbSet<BidWinner> BidWinners { get; set; }
        //public DbSet<HubAllocation> HubAllocation { get; set; } 

        public DbSet<TransportOrder> TransportOrders { get; set; }
        public DbSet<TransportOrderDetail> TransportOrderDetails { get; set; }
        public DbSet<vwTransportOrder> vwTransportOrders { get; set; }
        //public DbSet<TransportRequisition> TransportRequisitions { get; set; }
        public DbSet<TransportRequisitionDetail> TransportRequisitionDetails { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<ReceiptAllocation> ReceiptAllocation { get; set; } 


        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
        public DbSet<TransportBidQuotation> TransportBidQuotations { get; set; }
        public DbSet<ApplicationSetting> ApplicationSetting { get; set; }
        public DbSet<Ration> Rations { get; set; }
        public DbSet<NeedAssessment> NeedAssessment { get; set; }
        //public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO: Add mapping information for each Poco model.
            modelBuilder.Configurations.Add(new RegionalRequestMap());
            modelBuilder.Configurations.Add(new RegionalRequestDetailMap());
            modelBuilder.Configurations.Add(new ReliefRequisitionMap());
            modelBuilder.Configurations.Add(new ReliefRequisitionDetailMap());
            modelBuilder.Configurations.Add(new AdminUnitMap());
            modelBuilder.Configurations.Add(new CommodityMap());
            modelBuilder.Configurations.Add(new CommodityTypeMap());
            modelBuilder.Configurations.Add(new FDPMap());
            modelBuilder.Configurations.Add(new ProgramMap());
            modelBuilder.Configurations.Add(new AdminUnitTypeMap());
            modelBuilder.Configurations.Add(new BidDetailMap());
            modelBuilder.Configurations.Add(new BidMap());
            modelBuilder.Configurations.Add(new StatusMap());
            //modelBuilder.Configurations.Add(new OrderDeatilMap());
            modelBuilder.Configurations.Add(new TransporterMap());
            modelBuilder.Configurations.Add(new TransportBidPlanMap());
            modelBuilder.Configurations.Add(new TransportBidPlanDetailMap());

            modelBuilder.Configurations.Add(new ProjectCodeAllocationMap());

            modelBuilder.Configurations.Add(new HubAllocationMap());
            modelBuilder.Configurations.Add(new ProjectCodeMap());
            modelBuilder.Configurations.Add(new ShippingInstructionMap());

            modelBuilder.Configurations.Add(new BidWinnerMap());
            //modelBuilder.Configurations.Add(new HubAllocationMap());
            modelBuilder.Configurations.Add(new TransportOrderMap());
            modelBuilder.Configurations.Add(new TransportOrderDetailMap());
            modelBuilder.Configurations.Add(new vwTransportOrderMap());
            modelBuilder.Configurations.Add(new TransportRequisitionMap());
            modelBuilder.Configurations.Add(new TransportRequisitionDetailMap());
            modelBuilder.Configurations.Add(new TransactionMap());
            modelBuilder.Configurations.Add(new ReceiptAllocationMap());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
           
             modelBuilder.Configurations.Add(new WorkflowMap());
            modelBuilder.Configurations.Add(new WorkflowStatusMap());
            modelBuilder.Configurations.Add(new TransportBidQuotationMap());
            modelBuilder.Configurations.Add(new ApplicationSettingMap());
            modelBuilder.Configurations.Add(new RationMap());
            modelBuilder.Configurations.Add(new NeedAssessmentMap());
        }

    }
}
