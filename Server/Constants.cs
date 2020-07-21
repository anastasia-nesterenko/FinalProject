namespace Server
{
    public class Constants
    {
        public const string Issuer = Audience;
        public const string Audience = "https://localhost:44368/";
        public const string Secret = "not_too_short_secret_otherwise_it_might_cause_error";
    }
}
