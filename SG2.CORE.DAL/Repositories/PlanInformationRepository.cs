using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.PlanInformation;
using SG2.CORE.MODAL.ViewModals.Backend.PlanInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.DAL.Repositories
{
   public class PlanInformationRepository
    {

        public IList<PlanInformationListingViewModel> GetPlanInformationData(string SearchCriteria, int PageNumber, string PageSize, int? StatusId)
        {
            try

            {
                using (var _db = new SocialGrowth2Entities())
                {
                    
                    var planInformationdata = _db.SG2_usp_PlanInformation_GetAll(SearchCriteria, PageNumber, PageSize, StatusId).ToList();
                    if (planInformationdata != null)
                    {
                        List<PlanInformationListingViewModel> planInformationListingViewModalsList = new List<PlanInformationListingViewModel>();

                        foreach (var item in planInformationdata)
                        {
                            PlanInformationListingViewModel planInformationListingViewModel = new PlanInformationListingViewModel();
                            planInformationListingViewModel.PlanId = item.PlanId;
                            planInformationListingViewModel.Likes = item.Likes;
                            planInformationListingViewModel.PlanName = item.PlanName;
                            planInformationListingViewModel.TotatRecord = item.TotalRecord;
                            planInformationListingViewModel.PlanType = Convert.ToString(item.PlanType); //item.PlanType;
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
                using (var _db = new SocialGrowth2Entities())
                {
                    _db.SG2_usp_PlanInformation_Save(entity.PlanId, entity.PlanName, entity.PlanDescription, entity.PlanType, entity.Likes, entity.DisplayPrice,entity.NoOfLikesDuration,entity.StatusId,entity.SortOrder,entity.IsDefault,entity.StripePlanId,entity.PlanPrice,entity.SocialPlanTypeId);
                    return entity;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public IList<PlanInformationDTO> GetAllSocialGrowthPlans()
        {
            try
            {
                using (var _db=new SocialGrowth2Entities())
                {
                    var pros = _db.SG2_usp_Get_SocialProfile_PaymentPlan().ToList();
                    if (pros != null)
                    {
                        List<PlanInformationDTO> planInformationDTOs = new List<PlanInformationDTO>();
                        foreach (var pro in pros)
                        {
                            PlanInformationDTO plnInfo = new PlanInformationDTO();
                            plnInfo.PlanId = pro.PlanId;
                            plnInfo.PlanDescription = pro.PlanDescription;
                            plnInfo.PlanName = pro.PlanName;
                            plnInfo.PlanType = Convert.ToString(pro.PlanTypeId);
                            plnInfo.Likes = pro.Likes;
                            plnInfo.PlanPrice = (pro.PlanPrice);
                            plnInfo.DisplayPrice = pro.DisplayPrice;
                            plnInfo.StripePlanId = pro.StripePlanId;
                            plnInfo.NoOfLikesDuration = pro.NoOfLikesDuration;
                            plnInfo.StatusId = pro.StatusId??0;
                            plnInfo.SortOrder = pro.SortOrder;
                            plnInfo.IsDefault = pro.IsDefault??false;
                            plnInfo.PlantypeName = pro.PlanType;
                            plnInfo.StatusName = pro.StatusName;
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
                using (var _db = new SocialGrowth2Entities())
                {
                    var pro = _db.SG2_usp_PlanInformation_GetById(PlanInformationId).FirstOrDefault();
                    if (pro != null)
                    {
                        PlanInformationDTO plnInfo = new PlanInformationDTO()
                        {
                            PlanId = pro.PlanId,
                            PlanDescription = pro.PlanDescription,
                            PlanName = pro.PlanName,
                            PlanType = Convert.ToString(pro.PlanType),
                            Likes = pro.Likes,
                            PlanPrice = pro.PlanPrice,
                            DisplayPrice=pro.DisplayPrice,
                            StripePlanId=pro.StripePlanId,
                            NoOfLikesDuration=pro.NoOfLikesDuration,
                            StatusId=pro.StatusId,
                            SortOrder=pro.SortOrder,
                            IsDefault=pro.IsDefault,
                            SocialPlanTypeId=pro.SocialPlanTypeId

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
                using (var _db = new SocialGrowth2Entities())
                {
                    var PLinfo = _db.SG2_usp_PlanInformation_Delete(PlanInformationId);
                    if (PLinfo == 1)
                        return true;
                    else
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
                using (var _db = new SocialGrowth2Entities())
                {

                    if (PlanId == 0) return !_db.SG2_SocialProfile_PaymentPlan.Any(u => u.PlanName.Equals(PlanName));
                    var user = _db.SG2_SocialProfile_PaymentPlan.Find(PlanId);
                    if (user.PlanName.Equals(PlanName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    return !_db.SG2_SocialProfile_PaymentPlan.Any(r => r.PlanName.Equals(PlanName) && r.PlanId != PlanId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

    }
}
