using SG2.CORE.DAL.DB;
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
        private readonly CustomerRepository _cm;

        public StatisticsManager()
        {
            _statistics = new StatisticsRepository();
            _sessionManager = new SessionManager();
            _cm = new CustomerRepository();
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

        public bool UpdateStatistics(int socialProfileId, int FollowingCount, int LikeCount, int CommentCount, int StoryCount, int FollowCount, DateTime UpdateDateTime, int UnFollowCount, int PostCount)
        {
            try
            {
                var prevStats = _statistics.GetLatestStatistics(socialProfileId);
                if (prevStats != null)
                {




                    if (prevStats.Date.Date.CompareTo(UpdateDateTime.Date) == 0)
                    {
                        prevStats.Followings = (prevStats.Followings ?? 0) + FollowingCount;
                        prevStats.FollowingsTotal = (prevStats.FollowingsTotal ?? 0) + FollowingCount;

                        prevStats.Like = (prevStats.Like ?? 0) + LikeCount;
                        prevStats.LikeTotal = (prevStats.LikeTotal ?? 0) + LikeCount;

                        prevStats.Comment = (prevStats.Comment ?? 0) + CommentCount;


                        prevStats.StoryViews = (prevStats.StoryViews ?? 0) + StoryCount;
                        prevStats.StoryViewsTotal = (prevStats.StoryViewsTotal ?? 0) + StoryCount;

                        if (FollowCount > 0)
                        {
                            prevStats.Followers = FollowCount;
                            prevStats.FollowersTotal = FollowCount;
                        }

                        prevStats.Unfollow = (prevStats.Unfollow ?? 0) + UnFollowCount;
                        prevStats.UnfollowTotal = (prevStats.UnfollowTotal ?? 0) + UnFollowCount;

                        prevStats.Posts = PostCount;


                        _statistics.UpdateStatistics(prevStats);
                    }
                    else
                    {
                        prevStats.Followings = FollowingCount;
                        prevStats.FollowingsTotal = (prevStats.FollowingsTotal ?? 0) + FollowingCount;

                        prevStats.Like = LikeCount;
                        prevStats.LikeTotal = (prevStats.LikeTotal ?? 0) + LikeCount;

                        prevStats.Comment = CommentCount;

                        prevStats.StoryViews = StoryCount;
                        prevStats.StoryViewsTotal = (prevStats.StoryViewsTotal ?? 0) + StoryCount;

                        
                        prevStats.Followers = FollowCount;
                        prevStats.FollowersTotal = FollowCount;

                        prevStats.Unfollow = UnFollowCount;
                        prevStats.UnfollowTotal = UnFollowCount;

                        prevStats.Posts = PostCount;

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
                    prevStats.FollowingsTotal = (prevStats.FollowingsTotal ?? 0) + FollowingCount;

                    prevStats.Like = LikeCount;
                    prevStats.LikeTotal = (prevStats.LikeTotal ?? 0) + LikeCount;

                    prevStats.Comment = CommentCount;


                    prevStats.StoryViews = StoryCount;
                    prevStats.StoryViewsTotal = (prevStats.StoryViewsTotal ?? 0) + StoryCount;

                    prevStats.Followers = FollowCount;
                    prevStats.FollowersTotal = FollowCount;

                    prevStats.Unfollow = UnFollowCount;

                    prevStats.Posts = PostCount;

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


                //double appoffset = 0;
                //var serveroffset = DateTimeOffset.Now.Offset.Hours;//TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);

                var profile = _cm.GetSocialProfilesById(socialProfileId);
                //appoffset = Convert.ToDouble(string.IsNullOrEmpty(profile.SocialProfile.AppTimeZoneOffSet) ? "0" : profile.SocialProfile.AppTimeZoneOffSet);

                // +3 +5 
                //double offset = 0;
                //if (serveroffset > 0)
                 //   offset = appoffset - serveroffset;
                //else
                //    offset = appoffset + serveroffset;



                StatisticsViewModel followersStatisticsViewModel = new StatisticsViewModel();
                //followersStatisticsViewModel.StatisticsListing = _statistics.GetStatistics(socialProfileId, fromDate, ToDate).ToList();
                var model = _statistics.GetStatisticsFirstAndRecent(socialProfileId);
                if (model != null && model.Count == 2)
                {

                    followersStatisticsViewModel.FollowersInitial = model[0].Followers.HasValue ? model[0].Followers.Value : 0;
                    followersStatisticsViewModel.FollowersTotal = model[1].FollowersTotal.HasValue ? model[1].FollowersTotal.Value : 0;
                    followersStatisticsViewModel.FollowersChange = (followersStatisticsViewModel.FollowersTotal - (model[0].FollowersTotal.HasValue ? model[0].FollowersTotal.Value : 0));

                    followersStatisticsViewModel.FollowingsInitial = model[0].Followings;
                    followersStatisticsViewModel.FollowingsTotal = model[1].FollowingsTotal.HasValue ? model[1].FollowingsTotal.Value : 0;
                    followersStatisticsViewModel.FollowingsChange = (followersStatisticsViewModel.FollowingsTotal - (model[0].FollowingsTotal.HasValue ? model[0].FollowingsTotal.Value : 0));


                    followersStatisticsViewModel.PostsInitial = model[0].Posts.HasValue ? model[0].Posts.Value : 0;
                    followersStatisticsViewModel.PostsTotal = model[1].PostsTotal.HasValue ? model[1].PostsTotal.Value : 0;

                    followersStatisticsViewModel.FollowsRecent = model[1].Follow.HasValue ? model[1].Follow.Value : 0;
                    followersStatisticsViewModel.FollowsTotal = model[1].FollowTotal.HasValue ? model[1].FollowTotal.Value : 0;

                    followersStatisticsViewModel.LikesInitial = model[0].Like.HasValue ? model[0].Like.Value : 0;
                    followersStatisticsViewModel.LikesTotal = model[1].LikeTotal.HasValue ? model[1].LikeTotal.Value : 0;

                    followersStatisticsViewModel.UnFollowInitial = model[0].Unfollow.HasValue ? model[0].Unfollow.Value : 0;
                    followersStatisticsViewModel.UnFollowTotal = model[1].UnfollowTotal.HasValue ? model[1].UnfollowTotal.Value : 0;

                    followersStatisticsViewModel.StoryViewsInitial = model[0].StoryViews.HasValue ? model[0].StoryViews.Value : 0;
                    followersStatisticsViewModel.StoryViewsTotal = model[1].StoryViewsTotal.HasValue ? model[1].StoryViewsTotal.Value : 0;
                    followersStatisticsViewModel.FollowersGrowthRate = (model[0].Followers > 0 && model[1].Followers > 0) ? (model[0].Followers / model[1].Followers) * 100 : 0;
                    followersStatisticsViewModel.LikesGrowthRate = (model[0].Like > 0 && model[1].Like > 0) ? (model[0].Like / model[1].Like) * 100 : 0;

                    followersStatisticsViewModel.LikesChange = model[0].Like - model[1].Like;
                    followersStatisticsViewModel.FollowersAverageChange = (model[0].Followers + model[1].Followers) / 2;
                    followersStatisticsViewModel.LikesAverageChange = (model[0].Like + model[1].Like) / 2;
                    var date = model[1].Date;


                    var intervals = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Intervals>>(profile.SocialProfile_Instagram_TargetingInformation.ExecutionIntervals).First();
                    double TimezoneOffset = 0;
                    var recs = _cm.ReturnLastActions(socialProfileId, 10000, out TimezoneOffset).Where(g => g.ActionDateTime.Value.Year == date.Year && g.ActionDateTime.Value.Month == date.Month && g.ActionDateTime.Value.Day == date.Day).OrderBy(g => g.ActionDateTime);
                    if (recs != null && recs.Count() >= 2)
                    {
                        var tDiff = recs.Last().ActionDateTime - recs.First().ActionDateTime;
                        followersStatisticsViewModel.LastSessionIndex = (Double)recs.Count() / Convert.ToDouble(tDiff.Value.TotalMinutes == 0 ? 1.0 : tDiff.Value.TotalMinutes);

                        followersStatisticsViewModel.FollowingToday = recs.Where(g => g.ActionID == 60).Count();
                        followersStatisticsViewModel.FollowingTodayLimit = Convert.ToInt32(intervals.FollAccSearchTags);
                        followersStatisticsViewModel.LikeToday = recs.Where(g => g.ActionID == 62).Count();
                        followersStatisticsViewModel.LikeLimit = Convert.ToInt32(intervals.LikeFollowingPosts);
                        followersStatisticsViewModel.UnFollowToday = recs.Where(g => g.ActionID == 61).Count();
                        followersStatisticsViewModel.UnFollowLimit = Convert.ToInt32(intervals.UnFoll16DaysEngage);
                        followersStatisticsViewModel.StoryViewToday = recs.Where(g => g.ActionID == 64).Count();
                        followersStatisticsViewModel.StoryViewLimit = Convert.ToInt32(intervals.VwStoriesFollowing);

                    }
                    else
                    {
                        followersStatisticsViewModel.LastSessionIndex = 0;

                        followersStatisticsViewModel.FollowingToday = recs.Where(g => g.ActionID == 60).Count();
                        followersStatisticsViewModel.FollowingTodayLimit = Convert.ToInt32(intervals.FollAccSearchTags);
                        followersStatisticsViewModel.LikeToday = recs.Where(g => g.ActionID == 62).Count();
                        followersStatisticsViewModel.LikeLimit = Convert.ToInt32(intervals.LikeFollowingPosts);
                        followersStatisticsViewModel.UnFollowToday = recs.Where(g => g.ActionID == 61).Count();
                        followersStatisticsViewModel.UnFollowLimit = Convert.ToInt32(intervals.UnFoll16DaysEngage);
                        followersStatisticsViewModel.StoryViewToday = recs.Where(g => g.ActionID == 64).Count();
                        followersStatisticsViewModel.StoryViewLimit = Convert.ToInt32(intervals.VwStoriesFollowing);
                    }



                }
                else
                {
                    followersStatisticsViewModel.FollowersInitial = 0;
                    followersStatisticsViewModel.FollowersTotal = 0;

                    followersStatisticsViewModel.FollowingsInitial = 0;
                    followersStatisticsViewModel.FollowingsTotal = 0;

                    followersStatisticsViewModel.PostsInitial = 0;
                    followersStatisticsViewModel.PostsTotal = 0;

                    followersStatisticsViewModel.FollowsRecent = 0;
                    followersStatisticsViewModel.FollowsTotal = 0;

                    followersStatisticsViewModel.LikesInitial = 0;
                    followersStatisticsViewModel.LikesTotal = 0;

                    followersStatisticsViewModel.UnFollowInitial = 0;
                    followersStatisticsViewModel.UnFollowTotal = 0;

                    followersStatisticsViewModel.StoryViewsInitial = 0;
                    followersStatisticsViewModel.StoryViewsTotal = 0;
                    followersStatisticsViewModel.FollowersGrowthRate = 0;
                    followersStatisticsViewModel.LikesGrowthRate = 0;
                    followersStatisticsViewModel.FollowersChange = 0;
                    followersStatisticsViewModel.LikesChange = 0;
                    followersStatisticsViewModel.FollowersAverageChange = 0;
                    followersStatisticsViewModel.LikesAverageChange = 0;
                }
                return followersStatisticsViewModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool ClearStatsActions(int SocialProfileId)
        {
            try
            {
                return _statistics.ClearStatsActions(SocialProfileId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



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

        public List<SG2_usp_Report_GetProfileEvents_Result> GetProfileEvents(DateTime fromDate, DateTime toDate)
        {
            try
            {

                var model = _statistics.GetProfileEvents(fromDate, toDate);
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SG2_usp_Report_GetActions_Result> GetActionReport(DateTime fromDate, DateTime toDate)
        {
            try
            {

                var model = _statistics.GetActionsReport(fromDate, toDate);
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public S2_usp_Stats_GrowthSummary_Result GetStatsGrowthSummary(int socialProfileId)
        {
            try
            {

                return _statistics.GetStatsGrowthSummary(socialProfileId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StatsDailyActivityDTO> GetStatsDailyActivity(int socialProfileId, int days)
        {
            try
            {
                List<StatsDailyActivityDTO> statsDailyActivityDTOs = new List<StatsDailyActivityDTO>();
                List<SocialProfile_Statistics> profile_Statistics = _statistics.GetStatsByProfileIdAndDays(socialProfileId, days);
                SocialProfile_Statistics previousRec = null;
                int count = 1;
                foreach (SocialProfile_Statistics stats in profile_Statistics)
                {
                    stats.FollowersTotal = stats.FollowersTotal.HasValue ? stats.FollowersTotal.Value : 0;
                    stats.FollowingsTotal = stats.FollowingsTotal.HasValue ? stats.FollowingsTotal.Value : 0;
                    stats.PostsTotal = stats.PostsTotal.HasValue ? stats.PostsTotal.Value : 0;
                    if (count == 1)
                    {
                        StatsDailyActivityDTO statsDailyActivityDTO = new StatsDailyActivityDTO
                        {

                            Date = stats.Date,
                            Followers = stats.FollowersTotal,
                            Followings = stats.FollowingsTotal,
                            Posts = stats.PostsTotal,
                            FollowersDiff = stats.Followers ?? 0,
                            FollowingsDiff = stats.Followings ?? 0,
                            PostDiff = stats.Posts ?? 0,

                        };
                        statsDailyActivityDTO.EngagmentRate = statsDailyActivityDTO.Followers + statsDailyActivityDTO.Followings + statsDailyActivityDTO.Posts;
                        statsDailyActivityDTO.EngagmentRatePercentage = statsDailyActivityDTO.FollowersDiff + statsDailyActivityDTO.FollowingsDiff + statsDailyActivityDTO.PostDiff;
                        statsDailyActivityDTOs.Add(statsDailyActivityDTO);
                    }
                    else
                    {
                        StatsDailyActivityDTO statsDailyActivityDTO = new StatsDailyActivityDTO
                        {

                            Date = stats.Date,
                            Followers = stats.FollowersTotal,
                            Followings = stats.FollowingsTotal,
                            Posts = stats.PostsTotal,
                            FollowersDiff = stats.FollowersTotal  - previousRec.FollowersTotal,
                            FollowingsDiff = stats.FollowingsTotal  - previousRec.FollowingsTotal,
                            PostDiff = stats.PostsTotal  - previousRec.PostsTotal,

                        };
                        statsDailyActivityDTO.EngagmentRate = statsDailyActivityDTO.Followers + statsDailyActivityDTO.Followings + statsDailyActivityDTO.Posts;
                        statsDailyActivityDTO.EngagmentRatePercentage = statsDailyActivityDTO.FollowersDiff + statsDailyActivityDTO.FollowingsDiff + statsDailyActivityDTO.PostDiff;
                        statsDailyActivityDTOs.Add(statsDailyActivityDTO);
                    }
                   
                    count++;
                    previousRec = stats;
                }
                return statsDailyActivityDTOs.OrderByDescending(x => x.Date).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
