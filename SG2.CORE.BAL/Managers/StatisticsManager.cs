using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.MobileViewModels;
using SG2.CORE.MODAL.ViewModals.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.BAL.Managers
{
    public class StatisticsManager
    {
        private readonly StatisticsRepository _statistics;
        private readonly SessionManager _sessionManager;

        public StatisticsManager()
        {
            _statistics = new StatisticsRepository();
            _sessionManager = new SessionManager();
        }

       
        public void SaveStatistics(string data)
        { 
            try
            {
                _statistics.SaveStatistics(data);

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
                return _statistics.SaveInitialStatistics(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

            public IList<SocialProfile_Statistics> GetProfileTrends(int socialProfileId, DateTime fromDate, DateTime ToDate)
        {
            try
            {
                return _statistics.GetProfileTrends(socialProfileId, fromDate, ToDate);

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool UpdateStatistics(int socialProfileId, int FollowingCount, int LikeCount, int CommentCount, int StoryCount, int FollowCount)
        {
            try
            {
                var prevStats =  _statistics.GetLatestStatistics(socialProfileId);
                if ( prevStats != null)
                {

                    prevStats.Followings = FollowingCount;
                    prevStats.FollowingsTotal = prevStats.FollowingsTotal + FollowingCount;

                    prevStats.Like = LikeCount;
                    prevStats.LikeTotal += LikeCount;

                    prevStats.Comment = CommentCount;


                    prevStats.StoryViews = StoryCount;
                    prevStats.StoryViewsTotal += StoryCount;

                    prevStats.Follow = Math.Abs(prevStats.Follow.Value - FollowCount);
                    prevStats.FollowTotal = Math.Abs(prevStats.FollowTotal.Value - FollowCount);

                    if ( prevStats.Date == DateTime.Today)
                    {
                        
                        _statistics.UpdateStatistics(prevStats);
                    }
                    else
                    {
                        _statistics.InsertStatistics(prevStats);
                    }


                }



                return true;
            }
            catch (Exception e)
            {

                throw e;
            }


        }



            public StatisticsViewModel GetStatistics(int socialProfileId)
        {
            try
            {
                StatisticsViewModel followersStatisticsViewModel = new StatisticsViewModel();
                //followersStatisticsViewModel.StatisticsListing = _statistics.GetStatistics(socialProfileId, fromDate, ToDate).ToList();
                var model = _statistics.GetStatisticsFirstAndRecent(socialProfileId);
                if (model != null)
                {
                    followersStatisticsViewModel.FollowersInitial = model[0].Followers;
                    followersStatisticsViewModel.FollowersTotal = model[1].FollowersTotal;

                    followersStatisticsViewModel.FollowingsInitial = model[0].Followings;
                    followersStatisticsViewModel.FollowingsTotal = model[1].FollowingsTotal;

                    followersStatisticsViewModel.PostsInitial = model[0].Posts;
                    followersStatisticsViewModel.PostsTotal = model[1].PostsTotal;

                    followersStatisticsViewModel.FollowsRecent = model[1].Follow;
                    followersStatisticsViewModel.FollowsTotal = model[1].FollowTotal;

                    followersStatisticsViewModel.LikesRecent = model[1].Like;
                    followersStatisticsViewModel.LikesTotal = model[1].LikeTotal;

                    followersStatisticsViewModel.UnFollowRecent = model[1].Unfollow;
                    followersStatisticsViewModel.UnFollowTotal = model[1].UnfollowTotal;

                    followersStatisticsViewModel.StoryViewsRecent = model[1].StoryViews;
                    followersStatisticsViewModel.StoryViewsTotal = model[1].StoryViewsTotal;

                }
                return followersStatisticsViewModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public AdminReportViewModel GetAdminReports()
        //{
        //    try
        //    {

        //        var model = _statistics.GetAdminReports();
        //        return model;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public AdminReportViewModel GetJVBoxandProxyIPsData(DateTime fromDate, DateTime toDate)
        //{
        //    try
        //    {

        //        var model = _statistics.GetJVBoxandProxyIPsData(fromDate, toDate);
        //        return model;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<PlanListReportDTO> GetMostUsedProductData(DateTime fromDate, DateTime toDate)
        {
            try
            {

                var model = _statistics.GetMostUsedProductData(fromDate, toDate);
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
