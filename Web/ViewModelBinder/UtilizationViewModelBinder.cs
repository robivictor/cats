﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Areas.Logistics.Models;
using Cats.Models;
using Cats.Models.ViewModels;
namespace Cats.ViewModelBinder
{
    public class UtilizationViewModelBinder
    {
        public static IEnumerable<UtilizationDetailViewModel> GetUtilizationDetailViewModel(List<ReliefRequisitionDetail> reliefRequisitionDetails )
        {
            return reliefRequisitionDetails.Select(u => new UtilizationDetailViewModel()
                                                          {
                                                              FdpId = u.FDPID,
                                                              FDP = u.FDP.Name,
                                                              NumberOfBeneficiaries = u.BenficiaryNo,
                                                              AllocatedAmount = Helpers.DistributionHelper.GetAllocated(u.RequisitionDetailID,u.FDPID),
                                                              DispatchedToFDPAmount = Helpers.DistributionHelper.GetDispatched(u.RequisitionID,u.FDPID),
                                                              ReceivedAtFDPAmount = Helpers.DistributionHelper.GetReceived(u.ReliefRequisition.RequisitionNo, u.FDPID),
                                                              RequestedAmount =  u.Amount,
                                                              RegionId = (int) u.ReliefRequisition.RegionID,
                                                              Region = u.ReliefRequisition.AdminUnit.Name,
                                                              RequisitionId = u.RequisitionID,
                                                              RequistionNo = u.ReliefRequisition.RequisitionNo,
                                                              Program = u.ReliefRequisition.Program.Name,
                                                              Programid = (int)u.ReliefRequisition.ProgramID,
                                                              PlanId = u.ReliefRequisition.RegionalRequest.PlanID
                                                          });
        }

        public static IEnumerable<DistributionByAgeDetailViewModel> GetDistributionByAgeDetail(List<ReliefRequisitionDetail> reliefRequisitionDetails)
        {
            return reliefRequisitionDetails.Select(u => new DistributionByAgeDetailViewModel()
            {
                FdpId = u.FDPID,
                FDP = u.FDP.Name,
                NumberOfBeneficiaries = u.BenficiaryNo,
                AllocatedAmount = Helpers.DistributionHelper.GetAllocated(u.RequisitionDetailID, u.FDPID),
                DispatchedToFDPAmount = Helpers.DistributionHelper.GetDispatched(u.RequisitionID, u.FDPID),
                ReceivedAtFDPAmount = Helpers.DistributionHelper.GetReceived(u.ReliefRequisition.RequisitionNo, u.FDPID),
                RequestedAmount = u.Amount,
                RegionId = (int)u.ReliefRequisition.RegionID,
                Region = u.ReliefRequisition.AdminUnit.Name,
                RequisitionId = u.RequisitionID,
                RequistionNo = u.ReliefRequisition.RequisitionNo,
                Program = u.ReliefRequisition.Program.Name,
                Programid = (int)u.ReliefRequisition.ProgramID
                
            });
        }

        public static IEnumerable<UtilizationViewModel> GetUtilizationViewModel(List<ReliefRequisition> reliefRequisitionDetails)
        {
            return reliefRequisitionDetails.Select(u => new UtilizationViewModel()
            {
             
                RegionId = (int)u.RegionID,
                Region = u.AdminUnit.Name,
                RequisitionId = u.RequisitionID,
                RequisitionNo = u.RequisitionNo,
                Program = u.Program.Name,
                Programid = (int) u.ProgramID
            });
        }

    }
}