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
    }
}
