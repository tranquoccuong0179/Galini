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
            public const string UserEndPoint = ApiEndpoint + "/userinfo";
            public const string CreateUserInfo = UserEndPoint;
            public const string GetAllUserInfo = UserEndPoint;
            public const string RemoveUserInfo = UserEndPoint + "/{id}";
            public const string UpdateUserInfo = UserEndPoint + "/{id}";
            public const string GetUserInfoByAccountId = UserEndPoint + "/account/{id}";
            public const string GetUserInfoById = UserEndPoint + "/{id}";
        }
    }
}
