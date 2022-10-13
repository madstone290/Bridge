using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Shared
{
    /// <summary>
    /// API 경로.
    /// [FromRoute]을 이용한 모델 바인딩이 진행될 때 primitive타입은 case-insensitive하고 object타입의 속성은 case-sensitive하다.
    /// 따라서 Route 파라미터에는 CamelCase를 적용한다.
    /// </summary>
    public static class ApiRoutes
    {
        public static class Users
        {
            public const string Register = "/api/Users/Register";
            public const string SendVerificationEmail = "/api/Users/SendVerificationEmail";
            public const string VerifyEmail = "/api/Users/VerifyEmail";
            public const string Login = "/api/Users/Login";
            public const string Refresh = "/api/Users/Refresh";
        }

        public static class Places
        {
            public const string Get = "/api/Places/{id}";
            public const string GetList = "/api/Places";
            public const string Search = "/api/Places/Search";
            public const string Create = "/api/Places";
            public const string Update = "/api/Places/{id}";
            public const string AddOpeningTime = "/api/Places/{id}/OpeningTimes";
            public const string UpdateCategories = "/api/Places/{id}/Categories";
        }

        public static class Products
        {
            public const string Get = "/api/Products/{id}";
            public const string GetList = "/api/Products";

            public const string Create = "/api/Products";

            public const string Update = "/api/Products/{id}";
        }
    }
}
