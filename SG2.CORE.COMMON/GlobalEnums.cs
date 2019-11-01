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

        public enum JVStatus
        {
            TwoFactor = 32,
            EmailVerificationRequired=36,
            InvalidCredentials=17,
            InvalidCredentialReSend=31,
            APIBlock=33,
            ValidAndNotSetup=39,
            ValidAndSetUp = 40,
            ProfileAdding=11,
            ProfileAdded=41,
            TargetToBeUpdated = 14,
            Deleted = 35,
            CancelButNotDeletedOnMP = 37,
            ProfileNotSetup = 42,
            ProxyAndInternetIssue = 50,
            ProfileRequiresCancelling = 15
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
            Unsubscribe=27
        }

        public enum JVServerType
        {
            Likey=28,
            GrowthEngine=29
        }

        public enum SocialMedia
        {
            Instagram=30

        }

        public enum QueueStatus
        {
            Pending=43,
            Inprogress=44,
            Completed=45,
            Error = 48,

        }

        public enum QueueType
        {
            RPC=47,
            Regular=46,
        }

        public enum ProxyIP
        {
            BadIP= 49
        }


        public enum DeviceStatus
        {
            Connected = 54,
            NotConnected = 55
        }


    }
}



