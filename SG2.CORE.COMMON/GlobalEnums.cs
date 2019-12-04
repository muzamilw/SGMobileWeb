namespace SG2.CORE.COMMON
{
    public static class GlobalEnums
    {

        public static string[] NotificationMessages = new string[] {
            "Customer signup.",
            "Customer email verification required.",
            "Customer verified email successfully.",
            "Customer successfull subscribe for plan {0}",
            "Customer update target preferences.",
            "Customer update social profile.",
            "Customer enter email verification code {0}",
            "Customer enter 2FA verification code {0}",
            "Customer unsubscribe social profile plan.",
            "Customer MP account setup IP failed : {0} Port : {1} and no of attempts are {2}",
            "Customer MP Box is assigned -  {0}",
            "Customer assigned IP : {0} Port : {1}",
            "Customer profile unarchived",
        };

        public enum NotificationMessagesIndexes
        {
            NewSignup = 0,
            CustomerVerificationRequired=1,
            CustomerEmailVerified = 2,
            PlanSubscribe = 3,
            UpdateTargetPreferences=4,
            UpdateSocailProfile=5,
            ProfileVerificationCode=6,
            Profile2FACode=7,
            Unsubscribe=8,            
            InternetProxyIssue = 9,
            MPBoxAssign = 10,
            IPAssinged = 11,
            UnarchiveFromTargetPreferences = 12,
        }

        public enum StatusCodes
        {
            Available = 1,
            Published = 2,
            Visible = 3,
            Deleted = 4,
            Expired = 5,
            NotPublished = 6,
            NotAvailable = 7,
            Active = 8,
            InActive = 9
        }

        public enum SecurityActions
        {
            Create = 1,
            Update = 2,
            Delete = 3,
            View = 4
        }

        public class SessionConstants
        {
            public static string Customer = "Customer";
            public static string SellerUser = "SellerUser";
            public static string SystemUser = "SystemUser";
            public static string JVRPCSESSIONID = "JVRPCSESSIONID";
			public static string SocialProfile = "SocialProfile";
		}

        public enum CustomersStatus
        {
            Live=1,
            NewUnRegistered = 2,
            Cancelled=3,
            Deleted=4,
            Active=5,
            InActive=6,
            EmailverificationRequired=7

        }

        public enum UserStatus
        {
            Active=8,
            InActive=9,
            Deleted=10
        }

       

        public enum GeneralStatus
        {
            Deleted=18,
            Active=19,
            InActive=20,
            Unread=51,
            Read=52,
            Close=53
        }

       
        public enum SystemUserStatus
        {
            Invited=21,
            Active=22,
            Suspend=23,
            Delete=24

        }

        public enum PlanSubscription
        {
            Active=25,
            canceled=26,
            Unsubscribe=27,
            ActivePlan = 24
        }

      
        public enum SocialMedia
        {
            Instagram=30,
            LinkedIn = 56,
            FaceBook = 57,
            TikTok = 58


        }

      
        public enum DeviceStatus
        {
            Connected = 54,
            NotConnected = 55
        }


        public enum BlockStatus
        {
            UnBlocked = 0,
            Block1 = 66,
            Block2 = 67,
            Block3 = 68,
            Block4 = 69
        }

    }
}



