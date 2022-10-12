namespace Bridge.WebApp.Pages
{
    public static class PageRoutes
    {
        public const string Home = "/";

        public static class Identity
        {
            public const string Register = "/Identity/Register";
            public const string SendVerificationEmail = "/Identity/SendVerificationEmail";
            public const string Login = "/Identity/Login";
            public const string Logout = "/Identity/Logout";
        }

        public static class Admin
        {
            public const string Home = "/Admin";
            public const string PlaceList = "/Admin/Places";
            public const string PlaceCreate = "/Admin/Place";
            public const string PlaceProducts = "/Admin/Places/{placeId}/Products";
        }
    }
}

