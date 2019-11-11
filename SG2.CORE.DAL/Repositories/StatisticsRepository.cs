using SG2.CORE.DAL.DB;
using SG2.CORE.MODAL.DTO.Statistics;
using SG2.CORE.MODAL.ViewModals.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using SG2.CORE.MODAL;

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

        public IList<SocialProfile_Statistics>  GetStatistics(int socialProfileId, DateTime fromDate, DateTime ToDate)
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

        public List<SocialProfile_Statistics> GetStatisticsFirstAndRecent(int socialProfileId)
        {
            try
            {
                using (var _db = new SocialGrowth2Connection())
                {
                    List<SocialProfile_Statistics> lstStatisticsDTO = new List<SocialProfile_Statistics>();
                    lstStatisticsDTO.Add(_db.SocialProfile_Statistics.Where(g => g.SocialProfileId == socialProfileId).OrderBy(g => g.Date).FirstOrDefault()); ///very first record
                    lstStatisticsDTO.Add(_db.SocialProfile_Statistics.Where(g => g.SocialProfileId == socialProfileId).OrderByDescending(g => g.Date).FirstOrDefault()); ///recent record
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
