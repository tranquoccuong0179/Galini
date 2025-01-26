namespace Galini.API.Constants
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;
        
        public static class User
        {
            public const string UserEndPoint = ApiEndpoint + "/user";
            public const string RegisterUser = UserEndPoint;
            public const string VerifyOtp = UserEndPoint + "/verify-otp";
        }

        public static class Notification
        {
            public const string NotificationEndPoint = ApiEndpoint + "/notification";
            public const string CreateNotification = NotificationEndPoint;
            public const string GetNotifications = NotificationEndPoint;
            public const string GetNotification = NotificationEndPoint + "/{id}";
            public const string RemoveNotification = NotificationEndPoint + "/{id}";
            public const string MarkNotificationAsRead = NotificationEndPoint + "/{id}/mark-as-read";
        }
    }
}
