using SG2.CORE.MODAL.DTO.Common;
using SG2.CORE.MODAL.DTO.PlanInformation;
using SG2.CORE.MODAL.ViewModals.Backend;
using SG2.CORE.MODAL.ViewModals.Customers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SG2.CORE.MODAL.ViewModals.TargetPreferences
{
    public class TargetPreferencesViewModel
    {

       // [Required(ErrorMessage = "Profile name required")]
       // [Display(Name = "Profile Name: ")]
        public string ProfileName { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Hashtags you want to engage with:  ")]
        public string Preference1 { get; set; }

        [Display(Name = "Hashtags you DO NOT want to engage with (optional):")]
        public string Preference2 { get; set; }

        [Display(Name = "Choose up to 8 locations to focus on. Cities work best (optional):")]
        public string Preference3  { get; set; }

        [Display(Name = "Choose up to 8 locations to skip (optional):")]
        public string Preference4 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "A very effective growth strategy is to incorporate an amount of Follow & Unfollow activities on target accounts. Do you permit this activity?")]
        public int? Preference5 { get; set; }

        //  [Required(ErrorMessage = "Required")]
        [Display(Name = "Do you want to engage with Males, Females or Both")]
        public int? Preference6 { get; set; }

        //   [Required(ErrorMessage = "Required")]
        [Display(Name = "Do you want to include Business Accounts")]
        public int? Preference7 { get; set; }

         [Display(Name = "Targeted User Demographics(optional):")]
        public string Preference8 { get; set; }

        //  [Required(ErrorMessage = "Required")]
        [Display(Name = "Track and mirror competitors. Provide at least  10 accounts with a following of at least 2000 followers:")]
        public string Preference9 { get; set; }

        //  [Required(ErrorMessage = "Required")]
      //  [Display(Name = "Turn on AI targeting. We will seek out our competitors and match their hashtags")]
        public int? Preference10 { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Your Instagram Username:")]
        public string InstaUser { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Instagram Password:")]
        [DataType(DataType.Password)]
        public string InstaPassword { get; set; }

        [Display(Name = "What country do you mostly log in from?")]
        public int? Country { get; set; }

        [Display(Name = "Select your nearest city.")]
        public int? City { get; set; }

        public IList<CountryDTO> Countries { get; set; }

        public IList<CityDTO> Cities { get; set; }

        public int TargetInformationId { get; set; }

        public int? SocialProfileId { get; set; }

        public List<PlanInformationDTO> Plans { get; set; }

        public int? ActivePlanId { get; set; }
        public double? ActivePlanPrice { get; set; }
        public int? ActivePlanLikes { get; set; }

        public int? SPStatusId { get; set; }

        public IList<CustomerOrderHistoryViewModel> OrderHistoryViewModels { get; set; }
        public List<CustomerPaymentCardsViewModel> PaymentCards { get; set; }
        public PlanInformationDTO DefaultPaymentPlan { get; set; }
        public string JVStatus { get; set; }
        public int? JVStatusId { get; set; }
        public string ActivePlanName { get; set; }
        public string StripeApiKey { get; set; }
        public string StripePublishKey { get; set; }

        [Display(Name = "I want to learn more about the upcomming influencer market place")]
        public bool IsOptedMarketingEmail { get; set; }
        [Display(Name = "I want to grow my instagram account as")]
        public int? SocialAccAS { get; set; }
    }


    public class ExecutionInterval
    {
        public string FollAccSearchTags { get; set; }
        public string UnFoll16DaysEngage { get; set; }
        public string LikeFollowingPosts { get; set; }
        public string VwStoriesFollowing { get; set; }
        public string CommFollowingPosts { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
    }

}
