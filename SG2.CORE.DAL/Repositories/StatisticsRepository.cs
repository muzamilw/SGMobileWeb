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
                    //_db.SG2_usp_SocialProfile_SaveStatistics(data);

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
                    var statsData = _db.SocialProfile_Statistics.Where (g=> g.SocialProfileId ==  socialProfileId && g.Date >= fromDate && g.Date <= ToDate).ToList();
                    if (statsData != null)
                    {

                        foreach (var item in statsData)
                        {
                            StatisticsDTO statisticsDTO = new StatisticsDTO();
                            statisticsDTO.SocialProfileId = item.SocialProfileId;
                            statisticsDTO.Username = item.Username;
                            statisticsDTO.Followers = item.Followers;
                            statisticsDTO.FollowersGain = item.FollowersGain;
                            statisticsDTO.Followings = item.Followings;
                            statisticsDTO.FollowingsRatio = item.FollowingsRatio;
                            statisticsDTO.AVGFollowers = item.Followers/24;
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
                    var statsData = _db.SocialProfile_Statistics.Where ( g=> g.SocialProfileId == socialProfileId).FirstOrDefault();
                    if (statsData != null)
                    {
                             StatisticsViewModel model = new StatisticsViewModel();
                            model.TotalFollowers = statsData.Followers;
                            model.TotalFollowersGain = statsData.FollowersGain;
                            model.TotalFollowings = statsData.Followings;
                            model.TotalFollowingsRatio = Convert.ToInt32( statsData.FollowingsRatio);
                            model.TotalLike = statsData.Like;
                            model.TotalLikeComment = statsData.LikeComments;
                            
                            model.TotalEngagement = statsData.Engagement;

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
