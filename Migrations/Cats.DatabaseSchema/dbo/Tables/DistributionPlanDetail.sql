﻿CREATE TABLE [dbo].[DistributionPlanDetail] (
    [DistributionPlanDetailID]    INT             IDENTITY (1, 1) NOT NULL,
    [DistributionPlanID]          INT             NULL,
    [DonorID]                     INT             NULL,
    [ProjectCodeID]               INT             NULL,
    [ShippingInstructionID]       INT             NULL,
    [TransporterID]               INT             NOT NULL,
    [FDPID]                       INT             NULL,
    [Beneficiaries]               INT             NULL,
    [RRDNo]                       NVARCHAR (50)   NULL,
    [RRDStatusID]                 INT             NULL,
    [TransportTypeID]             INT             NULL,
    [HubID]                       INT             NULL,
    [AllocatedDate]               DATETIME        NULL,
    [BidStatus]                   INT             NULL,
    [StoreID]                     INT             NULL,
    [TariffPerQuintal]            DECIMAL (10, 2) NULL,
    [CommodityDistributionPlanID] INT             NULL,
    [DistributedByID]             INT             NULL,
    CONSTRAINT [PK_ReliefRequisitionDetail] PRIMARY KEY CLUSTERED ([DistributionPlanDetailID] ASC),
    CONSTRAINT [FK_DistributionPlanDetail_AllocationType] FOREIGN KEY ([CommodityDistributionPlanID]) REFERENCES [dbo].[CommodityDistributionPlan] ([CommodityDistributionPlanID]),
    CONSTRAINT [FK_DistributionPlanDetail_CommodityDistributionPlan] FOREIGN KEY ([CommodityDistributionPlanID]) REFERENCES [dbo].[CommodityDistributionPlan] ([CommodityDistributionPlanID]),
    CONSTRAINT [FK_DistributionPlanDetail_DistributionBy] FOREIGN KEY ([DistributedByID]) REFERENCES [dbo].[DistributionBy] ([DistributionByID]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_DistributionPlanDetail_DistributionBy1] FOREIGN KEY ([DistributedByID]) REFERENCES [dbo].[DistributionBy] ([DistributionByID]),
    CONSTRAINT [FK_DistributionPlanDetail_DistributionPlan] FOREIGN KEY ([DistributionPlanID]) REFERENCES [dbo].[DistributionPlan] ([DistributionPlanID]),
    CONSTRAINT [FK_DistributionPlanDetail_DistributionPlanDetail] FOREIGN KEY ([RRDStatusID]) REFERENCES [dbo].[RRDStatus] ([RRDStatusID]),
    CONSTRAINT [FK_DistributionPlanDetail_Donor] FOREIGN KEY ([DonorID]) REFERENCES [dbo].[Donor] ([DonorID]),
    CONSTRAINT [FK_DistributionPlanDetail_FDP1] FOREIGN KEY ([FDPID]) REFERENCES [dbo].[FDP] ([FDPID]),
    CONSTRAINT [FK_DistributionPlanDetail_Hub] FOREIGN KEY ([HubID]) REFERENCES [dbo].[Hub] ([HubID]),
    CONSTRAINT [FK_DistributionPlanDetail_ProjectCode] FOREIGN KEY ([ProjectCodeID]) REFERENCES [dbo].[ProjectCode] ([ProjectCodeID]),
    CONSTRAINT [FK_DistributionPlanDetail_ShippingInstruction] FOREIGN KEY ([ShippingInstructionID]) REFERENCES [dbo].[ShippingInstruction] ([ShippingInstructionID]),
    CONSTRAINT [FK_DistributionPlanDetail_Store] FOREIGN KEY ([StoreID]) REFERENCES [dbo].[Store] ([StoreID]),
    CONSTRAINT [FK_DistributionPlanDetail_Transporter] FOREIGN KEY ([TransporterID]) REFERENCES [dbo].[Transporter] ([TransporterID]),
    CONSTRAINT [FK_DistributionPlanDetail_TransportType] FOREIGN KEY ([TransporterID]) REFERENCES [dbo].[TransportType] ([TransportTypeID])
);
