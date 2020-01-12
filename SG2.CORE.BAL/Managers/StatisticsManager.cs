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

        public bool UpdateStatistics(int socialProfileId, int FollowingCount, int LikeCount, int CommentCount, int StoryCount, int FollowCount, DateTime UpdateDateTime, int UnFollowCount)
        {
            try
            {
                var prevStats =  _statistics.GetLatestStatistics(socialProfileId);
                if (prevStats != null)
                {

                
                    

                    if (prevStats.Date.Date.CompareTo(UpdateDateTime.Date)  == 0)
                    {
                        prevStats.Followings = (prevStats.Followings ?? 0) + FollowingCount;
                        prevStats.FollowingsTotal = (prevStats.FollowingsTotal ?? 0 ) + FollowingCount;

                        prevStats.Like = (prevStats.Like ?? 0) + LikeCount;
                        prevStats.LikeTotal = (prevStats.LikeTotal ?? 0) + LikeCount;

                        prevStats.Comment = (prevStats.Comment ?? 0 )+ CommentCount;


                        prevStats.StoryViews = (prevStats.StoryViews ?? 0) + StoryCount;
                        prevStats.StoryViewsTotal = (prevStats.StoryViewsTotal ?? 0) + StoryCount;

                        prevStats.Followers = FollowCount;
                        prevStats.FollowersTotal = FollowCount;

                        prevStats.Unfollow = (prevStats.Unfollow ??0 ) + UnFollowCount;


                        _statistics.UpdateStatistics(prevStats);
                    }
                    else
                    {
                        prevStats.Followings =  FollowingCount;
                        prevStats.FollowingsTotal = (prevStats.FollowingsTotal ?? 0) + FollowingCount;

                        prevStats.Like =  LikeCount;
                        prevStats.LikeTotal = (prevStats.LikeTotal ?? 0) + LikeCount;

                        prevStats.Comment = CommentCount;

                        prevStats.StoryViews = StoryCount;
                        prevStats.StoryViewsTotal = (prevStats.StoryViewsTotal ?? 0) + StoryCount;

                        prevStats.Followers = FollowCount;
                        prevStats.FollowersTotal = FollowCount;

                        prevStats.Unfollow = UnFollowCount;

                        prevStats.Date = UpdateDateTime;
                        _statistics.InsertStatistics(prevStats);
                    }


                }
                else
                {
                    

                    prevStats = new SocialProfile_Statistics();
                    prevStats.Date = UpdateDateTime;
                    prevStats.SocialProfileId = socialProfileId;

                    prevStats.Followings = FollowingCount;
                    prevStats.FollowingsTotal = (prevStats.FollowingsTotal ?? 0 )+ FollowingCount;

                    prevStats.Like = LikeCount;
                    prevStats.LikeTotal = (prevStats.LikeTotal ?? 0) + LikeCount;

                    prevStats.Comment = CommentCount;


                    prevStats.StoryViews = StoryCount;
                    prevStats.StoryViewsTotal = (prevStats.StoryViewsTotal ?? 0) + StoryCount;

                    prevStats.Followers = FollowCount;
                    prevStats.FollowersTotal = FollowCount;

                    prevStats.Unfollow = UnFollowCount;

                    _statistics.InsertStatistics(prevStats);
                    
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
                if (model != null && model.Count == 2)
                {
                    followersStatisticsViewModel.FollowersInitial = model[0].Followers.HasValue ? model[0].Followers.Value : 0;
                    followersStatisticsViewModel.FollowersTotal = model[1].FollowersTotal.HasValue ? model[1].FollowersTotal.Value :0;

                    followersStatisticsViewModel.FollowingsInitial = model[0].Followings;
                    followersStatisticsViewModel.FollowingsTotal = model[1].FollowingsTotal.HasValue ? model[1].FollowingsTotal.Value : 0;

                    followersStatisticsViewModel.PostsInitial = model[0].Posts.HasValue ? model[0].Posts.Value:0;
                    followersStatisticsViewModel.PostsTotal = model[1].PostsTotal.HasValue ? model[1].PostsTotal.Value : 0;

                    followersStatisticsViewModel.FollowsRecent = model[1].Follow.HasValue ? model[1].Follow.Value : 0;
                    followersStatisticsViewModel.FollowsTotal = model[1].FollowTotal.HasValue ? model[1].FollowTotal.Value : 0;

                    followersStatisticsViewModel.LikesRecent = model[1].Like.HasValue ? model[1].Like.Value : 0;
                    followersStatisticsViewModel.LikesTotal = model[1].LikeTotal.HasValue ? model[1].LikeTotal.Value : 0;

                    followersStatisticsViewModel.UnFollowRecent = model[1].Unfollow.HasValue ? model[1].Unfollow.Value : 0;
                    followersStatisticsViewModel.UnFollowTotal = model[1].UnfollowTotal.HasValue ? model[1].UnfollowTotal.Value : 0;

                    followersStatisticsViewModel.StoryViewsRecent = model[1].StoryViews.HasValue ? model[1].StoryViews.Value : 0;
                    followersStatisticsViewModel.StoryViewsTotal = model[1].StoryViewsTotal.HasValue ? model[1].StoryViewsTotal.Value :0;

                }
                else
                {
                    followersStatisticsViewModel.FollowersInitial = 0;
                    followersStatisticsViewModel.FollowersTotal = 0;

                    followersStatisticsViewModel.FollowingsInitial =0;
                    followersStatisticsViewModel.FollowingsTotal = 0;

                    followersStatisticsViewModel.PostsInitial =0;
                    followersStatisticsViewModel.PostsTotal = 0;

                    followersStatisticsViewModel.FollowsRecent = 0;
                    followersStatisticsViewModel.FollowsTotal = 0;

                    followersStatisticsViewModel.LikesRecent = 0;
                    followersStatisticsViewModel.LikesTotal = 0;

                    followersStatisticsViewModel.UnFollowRecent = 0;
                    followersStatisticsViewModel.UnFollowTotal = 0;

                    followersStatisticsViewModel.StoryViewsRecent = 0;
                    followersStatisticsViewModel.StoryViewsTotal = 0;
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
