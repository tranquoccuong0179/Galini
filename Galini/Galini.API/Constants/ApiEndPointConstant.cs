﻿namespace Galini.API.Constants
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Authentication
        {
            public const string AuthenticationEndPoint = ApiEndpoint + "/auth";
            public const string Authenticate = AuthenticationEndPoint;
        }

        public static class GoogleAuthentication
        {
            public const string GoogleAuthenticationEndPoint = ApiEndpoint + "/google-auth";
            public const string GoogleLogin = GoogleAuthenticationEndPoint + "/login";
            public const string GoogleSignIn = GoogleAuthenticationEndPoint + "/signin-google";
        }
        
        public static class User
        {
            public const string UserEndPoint = ApiEndpoint + "/user";
            public const string RegisterUser = UserEndPoint;
            public const string VerifyOtp = UserEndPoint + "/verify-otp";
            public const string ResendOtp = UserEndPoint + "/resend-otp";
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

        public static class UserInfo
        {
            public const string UserInfoEndPoint = ApiEndpoint + "/userinfo";
            public const string CreateUserInfo = UserInfoEndPoint;
            public const string GetAllUserInfo = UserInfoEndPoint;
            public const string RemoveUserInfo = UserInfoEndPoint + "/{id}";
            public const string UpdateUserInfo = UserInfoEndPoint + "/{id}";
            public const string GetUserInfoByAccountId = UserInfoEndPoint + "/account/{id}";
            public const string GetUserInfoById = UserInfoEndPoint + "/{id}";
        }

        public static class Premium
        {
            public const string PremiumEndPoint = ApiEndpoint + "/premium";
            public const string CreatePremium = PremiumEndPoint;
            public const string GetAllPremium = PremiumEndPoint;
            public const string RemovePremium = PremiumEndPoint + "/{id}";
            public const string UpdatePremium = PremiumEndPoint + "/{id}";
            public const string GetPremiumById = PremiumEndPoint + "/{id}";
        }

        public static class UserCall
        {
            public const string UserCallEndPoint = ApiEndpoint + "/usercall";
            public const string CreateUserCall = UserCallEndPoint;
            public const string GetAllUserCall = UserCallEndPoint;
            public const string GetUserCallByAccountId = UserCallEndPoint + "/account/{id}";
            public const string GetUserCallById = UserCallEndPoint + "/{id}";
            public const string RemoveUserCall = UserCallEndPoint + "/{id}";
            public const string UpdateUserCall = UserCallEndPoint + "/{id}";
        }

        public static class CallHistory
        {
            public const string CallHistoryEndPoint = ApiEndpoint + "/callhistory";
            public const string CreateCallHistory = CallHistoryEndPoint;
            public const string GetAllCallHistory = CallHistoryEndPoint;
            public const string GetCallHistoryById = CallHistoryEndPoint + "/{id}";
            public const string RemoveCallHistory = CallHistoryEndPoint + "/{id}";
            public const string UpdateCallHistory = CallHistoryEndPoint + "/{id}";
        }

        public static class FriendShip
        {
            public const string FriendShipEndPoint = ApiEndpoint + "/friendship";
            public const string CreateFriendShip = FriendShipEndPoint;
            public const string GetAllFriendShip = FriendShipEndPoint;
            public const string GetFriendShipById = FriendShipEndPoint + "/{id}";
            public const string GetFriendShipByAccountIdAndStatus = FriendShipEndPoint + "/account/{id}/status";
            public const string GetFriendByAccountId = FriendShipEndPoint + "/account/{id}";
            public const string UpdateFriendShip = FriendShipEndPoint + "/{id}";
            public const string RemoveFriendShip = FriendShipEndPoint + "/{id}";
        }

        public static class ListenerInfo
        {
            public const string ListenerInfoEndPoint = ApiEndpoint + "/listener";
            public const string CreateListenerInfo = ListenerInfoEndPoint;
            public const string GetListListenerInfo = ListenerInfoEndPoint;
            public const string GetListenerInfo = ListenerInfoEndPoint + "/{id}";
            public const string GetListenerInfoByAccountId = ListenerInfoEndPoint + "/account/{id}";
            public const string UpdateListenerInfo = ListenerInfoEndPoint + "/{id}";
            public const string DeleteListenerInfo = ListenerInfoEndPoint + "/{id}";
        }
        
        public static class Topic
        {
            public const string TopicEndPoint = ApiEndpoint + "/topic";
            public const string CreateTopic = TopicEndPoint;
            public const string GetListTopic = TopicEndPoint;
            public const string GetTopic = TopicEndPoint + "/{id}";
            public const string UpdateTopic = TopicEndPoint + "/{id}";
            public const string DeleteTopic = TopicEndPoint + "/{id}";
        }

        public static class Wallet
        {
            public const string WalletEndPoint = ApiEndpoint + "/wallet";
            public const string CreateLink = WalletEndPoint;
        }

        public static class Message
        {
            public const string MessageEndPoint = ApiEndpoint + "/message";
            public const string CreateMessage = MessageEndPoint;
            public const string CreateMessageCall = MessageEndPoint + "/call";
            public const string GetAllMessage = MessageEndPoint;
            public const string GetMessageByDirectChatId = MessageEndPoint + "/directchat/{id}";
            public const string GetMessageById = MessageEndPoint + "/{id}";
            public const string RemoveMessage = MessageEndPoint + "/{id}";
            public const string SearchMessageByDirectChatId = MessageEndPoint + "search/{id}";
            public const string UpdateMessage = MessageEndPoint + "/{id}";
        }
    }
}
