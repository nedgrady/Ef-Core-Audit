namespace EFCoreAudit
{
    // Extra interface wrapper around IHttpContextAccessor to decouple the db + aspnet core concerns
    public interface IUsernameProvider
    {
        public string Username { get; }
    }

    class UsernameProvider : IUsernameProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly static string[] Usernames = new[]
        {
            "Ned",
            "Sally",
            "Timothy",
            "Soldier"
        };

        public UsernameProvider(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        // TODO - check http user's claims to return REAL username..
        public string Username => Usernames[new Random().Next(0, Usernames.Length)];

    }
}
