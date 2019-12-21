using SG2.CORE.MODAL.DTO.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SG2.CORE.MODAL.ViewModals.Backend.PlanInformation
{
    public class PlanInformationSaveViewModel
    {
        public int PlanId { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required]
        public Nullable<int> NoOfFollow { get; set; }
        [Required]
        public Nullable<int> NoOfStoryView { get; set; }
        [Required]
        public Nullable<int> NoOfComments { get; set; }

        [Required]
        public double PlanPrice { get; set; }

        [Required]
        public double DisplayPrice { get; set; }

        [Required]
        [Remote("IsPlanNameExists", "PlanInformation", ErrorMessage = "Plan already exist.", AdditionalFields = "PlanId")]
        public string PlanName { get; set; }

        [Required]
        public string PlanDescription { get; set; }

       

        [Required]
        public int StatusId { get; set; }

        public string StripePlanId { get; set; }

        public short? SortOrder { get; set; }

        [Range(1, 8, ErrorMessage = "No of session per day must be between 1 to 8")]
        public int NoOfLikesDuration { get; set; }
        
        public bool IsDefault { get; set; }

        // public IList<StatusDTO> SocialPlanTypes { get; set; }
        [Required]
        public int? SocialPlanTypeId { get; set; }
        public IList<StatusDTO> PlanTypes { get; set; }

    }
}
