using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.PlanInformation;
using SG2.CORE.MODAL.ViewModals.Backend.PlanInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SG2.CORE.MODAL;

namespace SG2.CORE.DAL.Repositories
{
   public class PlanInformationRepository
    {

        public IList<PlanInformationListingViewModel> GetPlanInformationData(string SearchCriteria, int PageNumber, string PageSize, int? StatusId)
        {
            try

            {
                using (var _db = new SocialGrowth2Connection())
                {
                    
                    var planInformationdata = _db.SG2_usp_PlanInformation_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (planInformationdata != null)
                    {
                        List<PlanInformationListingViewModel> planInformationListingViewModalsList = new List<PlanInformationListingViewModel>();

                        foreach (var item in planInformationdata)
                        {
                            PlanInformationListingViewModel planInformationListingViewModel = new PlanInformationListingViewModel();
                            planInformationListingViewModel.PlanId = item.PlanId;
                            planInformationListingViewModel.NoOfFollow = item.NoOfFollow;
                            planInformationListingViewModel.NoOfStoryView = item.NoOfStoryView;
                            planInformationListingViewModel.NoOfComments = item.NoOfComments;
                            planInformationListingViewModel.PlanName = item.PlanName;
                            planInformationListingViewModel.TotatRecord = item.TotalRecord;
                            planInformationListingViewModel.IsBrokerPlan = Convert.ToString(item.IsBrokerPlan); //item.PlanType;
                            planInformationListingViewModel.PlanPrice = item.PlanPrice;
                            planInformationListingViewModel.DisplayPrice = item.DisplayPrice;
                            planInformationListingViewModel.StripPlanId = item.StripePlanId;
                            planInformationListingViewModalsList.Add(planInformationListingViewModel);
                        }
                        return planInformationListingViewModalsList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public PlanInformationDTO SavePlan(PlanInformationDTO entity)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var plan = _db.PaymentPlans.Where(g => g.PaymentPlanId == entity.PlanId).SingleOrDefault();
                    if ( plan != null)
                    {
                        plan.PlanName = entity.PlanName;
                        plan.PlanShortDescription = entity.PlanDescription;
                        plan.IsBrokerPlan = entity.IsBrokerPlan;
                        plan.NoOfFollow = entity.NoOfFollow;
                        plan.NoOfStoryView = entity.NoOfStoryView;
                        plan.NoOfComments = entity.NoOfComments;
                        plan.DisplayPrice = entity.DisplayPrice;
                        plan.NoOfLikesDuration = entity.NoOfLikesDuration;
                        plan.StatusId = entity.StatusId;
                        plan.SortOrder = entity.SortOrder;
                        plan.IsDefault = entity.IsDefault;
                        plan.StripePlanId = entity.StripePlanId;
                        plan.StripePlanPrice = entity.PlanPrice;
                        plan.SocialPlatform = entity.SocialPlatform;

                        _db.SaveChanges();
                    }
                    else
                    {
                        plan = new PaymentPlan();
                        plan.PlanName = entity.PlanName;
                        plan.PlanShortDescription = entity.PlanDescription;
                        plan.IsBrokerPlan = entity.IsBrokerPlan;
                        plan.NoOfFollow = entity.NoOfFollow;
                        plan.NoOfStoryView = entity.NoOfStoryView;
                        plan.NoOfComments = entity.NoOfComments;
                        plan.DisplayPrice = entity.DisplayPrice;
                        plan.NoOfLikesDuration = entity.NoOfLikesDuration;
                        plan.StatusId = entity.StatusId;
                        plan.SortOrder = entity.SortOrder;
                        plan.IsDefault = entity.IsDefault;
                        plan.StripePlanId = entity.StripePlanId;
                        plan.StripePlanPrice = entity.PlanPrice;
                        plan.SocialPlatform = entity.SocialPlatform;
                        _db.PaymentPlans.Add(plan);

                        _db.SaveChanges();
                        entity.PlanId = plan.PaymentPlanId;
                    }
                    
                    return entity;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public IList<PlanInformationDTO> GetallIntagramPlans(bool IsBroker)
        {
            try
            {
                using (var _db=new SocialGrowth2Connection())
                {
                    List<PaymentPlan> pros = null;
                    
                    if ( IsBroker == true)
                        pros = _db.PaymentPlans.Where(g => g.SocialPlatform == 30 && (g.IsBrokerPlan == IsBroker && g.IsDefault != true) || g.PaymentPlanId == 1).ToList();
                    else
                        pros = _db.PaymentPlans.Where(g => g.SocialPlatform == 30 && g.IsBrokerPlan == IsBroker ).ToList();

                    if (pros != null)
                    {
                        List<PlanInformationDTO> planInformationDTOs = new List<PlanInformationDTO>();
                        foreach (var pro in pros)
                        {
                            PlanInformationDTO plnInfo = new PlanInformationDTO();
                            plnInfo.PlanId = pro.PaymentPlanId;
                            plnInfo.PlanDescription = pro.PlanShortDescription;
                            plnInfo.PlanName = pro.PlanName;
                            plnInfo.IsBrokerPlan = pro.IsBrokerPlan.Value;
                            plnInfo.NoOfFollow = pro.NoOfFollow;
                            plnInfo.NoOfStoryView = pro.NoOfStoryView;
                            plnInfo.NoOfComments = pro.NoOfComments;
                            plnInfo.PlanPrice = (pro.StripePlanPrice);
                            plnInfo.DisplayPrice = pro.DisplayPrice;
                            plnInfo.StripePlanId = pro.StripePlanId;
                            plnInfo.NoOfLikesDuration = pro.NoOfLikesDuration.Value;
                            plnInfo.StatusId = pro.StatusId??0;
                            plnInfo.SortOrder = pro.SortOrder;
                            plnInfo.IsDefault = pro.IsDefault??false;
                            //plnInfo.PlantypeName = pro.PlanType;
                            plnInfo.StatusName = pro.StatusId.Value == 19 ? "Active": "Inactive";
                            plnInfo.SocialPlatform = pro.SocialPlatform;
                            planInformationDTOs.Add(plnInfo);
                            }
                        return planInformationDTOs;

                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public PlanInformationDTO GetPlanInformationById(int PlanInformationId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    var pro = _db.PaymentPlans.Where (g=> g.PaymentPlanId ==  PlanInformationId).FirstOrDefault();
                    if (pro != null)
                    {
                        PlanInformationDTO plnInfo = new PlanInformationDTO()
                        {
                            PlanId = pro.PaymentPlanId,
                            PlanDescription = pro.PlanShortDescription,
                            PlanName = pro.PlanName,
                            IsBrokerPlan = pro.IsBrokerPlan.Value,
                            NoOfFollow = pro.NoOfFollow,
                            NoOfComments = pro.NoOfComments,
                            NoOfStoryView = pro.NoOfStoryView,
                            PlanPrice = pro.StripePlanPrice,
                            DisplayPrice=pro.DisplayPrice,
                            StripePlanId=pro.StripePlanId,
                            NoOfLikesDuration=pro.NoOfLikesDuration.Value,
                            StatusId=pro.StatusId,
                            SortOrder=pro.SortOrder,
                            IsDefault=pro.IsDefault,
                            SocialPlatform=pro.SocialPlatform

                        };
                        return plnInfo;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeletePlanInformation(int PlanInformationId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    //var PLinfo = _db.SG2_usp_PlanInformation_Delete(PlanInformationId);
                    //if (PLinfo == 1)
                    //    return true;
                    //else
                    //    return false;

                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPlanNameExists(string PlanName, int PlanId = 0)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    if (PlanId == 0) return !_db.PaymentPlans.Any(u => u.PlanName.Equals(PlanName));
                    var user = _db.PaymentPlans.Find(PlanId);
                    if (user.PlanName.Equals(PlanName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.PaymentPlans.Any(r => r.PlanName.Equals(PlanName) && r.PaymentPlanId != PlanId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

    }
}
