namespace Bridge.IntegrationTests.Config
{
    public static class Seeds
    {
        static Seeds()
        {
        }

        public static TestUser[] TestUsers { get; } = new TestUser[]
        {
            TestUser.Admin, 
            TestUser.Consumer
        };
    }
}
