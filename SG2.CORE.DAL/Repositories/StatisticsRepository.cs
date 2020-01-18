using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.ViewModals.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using SG2.CORE.MODAL;
using SG2.CORE.MODAL.MobileViewModels;

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

        public bool SaveInitialStatistics(MobileIniitalStatsRequest model)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var profile = _db.SocialProfiles.Where(g => g.SocialProfileId == model.SocialProfileId).Single();

                    //only save initial stats if old one does not exists. skip if any older stats exists.
                    if ((profile.InitialStatsReceived.HasValue == false || profile.InitialStatsReceived.Value == false ) &&  _db.SocialProfile_Statistics.Where(g => g.SocialProfileId == model.SocialProfileId && g.Date <= DateTime.Today).Count() == 0)
                    {

                        var stats = new SocialProfile_Statistics();
                        stats.SocialProfileId = model.SocialProfileId;
                        stats.Posts = model.InitialPosts;
                        stats.PostsTotal = model.InitialPosts;

                        stats.Followers = model.InitialFollowers;
                        stats.FollowersTotal = model.InitialFollowers;

                        stats.Followings = model.InitialFollowings;
                        stats.FollowingsTotal = model.InitialFollowings;

                        stats.Date = DateTime.Today;
                        stats.CreatedDate = DateTime.Now;
                        stats.UpdateDate = DateTime.Now;

                        _db.SocialProfile_Statistics.Add(stats);
                        
                        profile.InitialStatsReceived = true;
                        profile.InitialStatsReceivedDateTime = DateTime.Now;

                        _db.SaveChanges();

                    }
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }


        public bool UpdateStatistics(SocialProfile_Statistics model)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var stats = _db.SocialProfile_Statistics.Where(g => g.SocialStatisticsId == model.SocialStatisticsId).FirstOrDefault();
                    //only save initial stats if old one does not exists. skip if any older stats exists.
                    if (stats != null)
                    {

                        stats.Followings = model.Followings;
                        stats.FollowingsTotal = model.FollowingsTotal;

                        stats.Like = model.Like;
                        stats.LikeTotal = model.LikeTotal;

                        stats.Comment = model.Comment;
                        stats.StoryViews = model.StoryViews;
                        stats.StoryViewsTotal = model.StoryViewsTotal;

                        stats.Unfollow = model.Unfollow;

                        stats.Followers = model.Followers;
                        stats.FollowersTotal = model.FollowersTotal;

                        stats.UpdateDate = DateTime.Now;

                        _db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }


        public bool InsertStatistics(SocialProfile_Statistics model)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var stats = new SocialProfile_Statistics();
                    //only save initial stats if old one does not exists. skip if any older stats exists.
                    if (stats != null)
                    {

                        stats.Followings = model.Followings;
                        stats.FollowingsTotal = model.FollowingsTotal;

                        stats.Like = model.Like;
                        stats.LikeTotal = model.LikeTotal;

                        stats.Comment = model.Comment;
                        stats.StoryViews = model.StoryViews;
                        stats.StoryViewsTotal = model.StoryViewsTotal;

                        stats.Followers = model.Followers;
                        stats.FollowersTotal = model.FollowersTotal;

                        stats.Unfollow = model.Unfollow;

                        stats.Date = model.Date;
                        stats.CreatedDate = DateTime.Now;
                        stats.UpdateDate = DateTime.Now;

                        stats.SocialProfileId = model.SocialProfileId;

                        _db.SocialProfile_Statistics.Add(stats);

                        _db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }

        public IList<SocialProfile_Statistics>  GetProfileTrends(int socialProfileId, DateTime fromDate, DateTime ToDate)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                   
                    var statsData = _db.SocialProfile_Statistics.Where (g=> g.SocialProfileId ==  socialProfileId && g.Date >= fromDate && g.Date <= ToDate).ToList();

                  
                    return statsData;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SocialProfile_Statistics GetLatestStatistics(int socialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {

                    var statsData = _db.SocialProfile_Statistics.Where(g => g.SocialProfileId == socialProfileId).OrderByDescending( g=> g.Date).FirstOrDefault();

                    return statsData;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SocialProfile_Statistics> GetStatisticsFirstAndRecent(int socialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    List<SocialProfile_Statistics> lstStatisticsDTO = new List<SocialProfile_Statistics>();
                    var first = _db.SocialProfile_Statistics.Where(g => g.SocialProfileId == socialProfileId).OrderBy(g => g.Date).FirstOrDefault();
                    if (first != null) ;
                        lstStatisticsDTO.Add(first); ///very first record

                    var recent = _db.SocialProfile_Statistics.Where(g => g.SocialProfileId == socialProfileId).OrderByDescending(g => g.Date).FirstOrDefault();
                    if ( recent!= null )
                        lstStatisticsDTO.Add(recent); ///recent record
                    return lstStatisticsDTO;
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
