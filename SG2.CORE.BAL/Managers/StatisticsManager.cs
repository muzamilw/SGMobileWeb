using SG2.CORE.DAL.Repositories;
using SG2.CORE.MODAL.DTO.Statistics;
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

        public List<StatisticsDTO> GetFollersStatistics(int socialProfileId, DateTime fromDate, DateTime ToDate)
        {
            try
            {
                var model = _statistics.GetFollersStatistics(socialProfileId, fromDate, ToDate).ToList();
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StatisticsViewModel GetStatistics(int socialProfileId, DateTime fromDate, DateTime ToDate)
        {
            try
            {
                StatisticsViewModel followersStatisticsViewModel = new StatisticsViewModel();
                followersStatisticsViewModel.StatisticsListing = _statistics.GetFollersStatistics(socialProfileId, fromDate, ToDate).ToList();
                var model = _statistics.GetStatistics(socialProfileId);
                followersStatisticsViewModel.TotalComment = model.TotalComment;
                followersStatisticsViewModel.TotalEngagement = model.TotalEngagement;
                followersStatisticsViewModel.TotalLike = model.TotalLike;
                followersStatisticsViewModel.TotalLikeComment = model.TotalLikeComment;
                followersStatisticsViewModel.TotalFollowingsRatio = model.TotalFollowingsRatio;
                followersStatisticsViewModel.TotalFollowings = model.TotalFollowings;
                followersStatisticsViewModel.TotalFollowersGain = model.TotalFollowersGain;
                followersStatisticsViewModel.TotalFollowers = model.TotalFollowers;
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
