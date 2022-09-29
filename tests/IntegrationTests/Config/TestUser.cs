namespace Bridge.IntegrationTests.Config
{
    public class TestUser
    {
        public static TestUser Admin { get; } = new TestUser()
        {
            Email = "admin@test.com",
            Password = "adminAdmin3#"
        };

        public static TestUser Consumer { get; } = new TestUser()
        {
            Email = "consumer@test.com",
            Password = "consumerConsumer3#"
        };

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
