using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.ViewModals.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SG2.CORE.DAL.Repositories
{
    public class StatisticsRepository
    {
        public void SaveStatistics(string data)
        {
            try
            {

                using (var _db = new SocialGrowth2Connection())
                {
                    _db.SG2_usp_SocialProfile_SaveStatistics(data);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<StatisticsDTO>  GetFollersStatistics(int socialProfileId, DateTime fromDate, DateTime ToDate)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    List<StatisticsDTO> lstStatisticsDTO = new List<StatisticsDTO>();
                    var statsData = _db.SG2_usp_SocialProfile_Statistics_GetFollowers(socialProfileId, fromDate, ToDate).ToList();
                    if (statsData != null)
                    {

                        foreach (var item in statsData)
                        {
                            StatisticsDTO statisticsDTO = new StatisticsDTO();
                            statisticsDTO.SocialProfileId = item.SocialProfileId;
                            statisticsDTO.Username = item.Username;
                            statisticsDTO.Followers = item.followers;
                            statisticsDTO.FollowersGain = item.FollowersGain;
                            statisticsDTO.Followings = item.Followings;
                            statisticsDTO.FollowingsRatio = item.FollowingsRatio;
                            statisticsDTO.AVGFollowers = item.AVGFollowers;
                            statisticsDTO.Date = item.Date;
                            statisticsDTO.Like = item.Like;
                            statisticsDTO.Comment = item.Comment;
                            statisticsDTO.Engagement = item.Engagement;
                            statisticsDTO.LikeComments = item.LikeComments;

                            lstStatisticsDTO.Add(statisticsDTO);
                        }
                        return lstStatisticsDTO;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StatisticsViewModel GetStatistics(int socialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    List<StatisticsDTO> lstStatisticsDTO = new List<StatisticsDTO>();
                    var statsData = _db.SG2_usp_SocialProfile_Statistics_GetStatistics(socialProfileId).FirstOrDefault();
                    if (statsData != null)
                    {
                             StatisticsViewModel model = new StatisticsViewModel();
                            model.TotalFollowers = statsData.TotalFollowers;
                            model.TotalFollowersGain = statsData.TotalFollowersGain;
                            model.TotalFollowings = statsData.TotalFollowings;
                            model.TotalFollowingsRatio = statsData.TotalFollowingsRatio;
                            model.TotalLike = statsData.TotalLike;
                            model.TotalLikeComment = statsData.TotalLikeComment;
                            model.TotalLikeComment = statsData.TotalLikeComment;
                            model.TotalEngagement = statsData.TotalEngagement;

                        return model;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdminReportViewModel GetAdminReports()
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                  
                    var reportData = _db.SG2_usp_Report_GetReportData().FirstOrDefault();
                    if (reportData != null)
                    {
                        AdminReportViewModel model = new AdminReportViewModel();
                        model.AllSlotsOnJVBox = reportData.TotalJVServer;
                        model.UsedSlotsOnJVBox = reportData.TotalJVServersUsage;
                        model.FreeSlotsOnJVServer = reportData.FreeSlotsPerServer;
                        model.AllAvailableIPs = reportData.AllAvailableIPs;
                        model.TotalUsedIPs = reportData.TotalUsedIPs;
                       
                        return model;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AdminReportViewModel GetJVBoxandProxyIPsData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var reportData = _db.SG2_usp_Report_GetJVBoxandProxyIPsData(fromDate, toDate).FirstOrDefault();
                    if (reportData != null)
                    {
                        AdminReportViewModel model = new AdminReportViewModel();
                        model.AllSlotsOnJVBox = reportData.AllSlotsOnJVBox;
                        model.UsedSlotsOnJVBox = reportData.UsedSlotsOnJVBox;
                        model.FreeSlotsOnJVServer = reportData.FreeSlotsOnJVServer;
                        model.AllAvailableIPs = reportData.AllProxyIPs;
                        model.TotalUsedIPs = reportData.UsedProxyIPs;
                        model.RemainingProxyIPs = reportData.RemainingProxyIPs;
                        return model;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PlanListReportDTO> GetMostUsedProductData(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<PlanListReportDTO> lstPlanReportDTO = new List<PlanListReportDTO>();
                using (var _db = new SocialGrowth2Connection())
                {

                    var reportData = _db.SG2_usp_Report_GetMostUsedProductData(fromDate, toDate);
                    if (reportData != null)
                    {

                        foreach (var item in reportData)
                        {
                            PlanListReportDTO dto = new PlanListReportDTO();
                            dto.TotalPlanSold = item.TotalPlanSold;
                            dto.name = item.PlanName;
                            dto.value = item.PlanSold.ToString();
                            lstPlanReportDTO.Add(dto);
                        }
                        return lstPlanReportDTO;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
