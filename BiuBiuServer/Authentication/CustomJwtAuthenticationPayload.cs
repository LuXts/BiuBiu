namespace BiuBiuServer.Authentication
{
    public class CustomJwtAuthenticationPayload
    {
        public ulong UserId { get; set; }
        public string DisplayName { get; set; }
        public bool IsAdmin { get; set; }
    }
}