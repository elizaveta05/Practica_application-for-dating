namespace makets.helper.EmailSender
{
    public record SmtpCredentials
    {

        public string Username { get; }
        public string Password { get; }

        public SmtpCredentials(string login, string password)
        {
            
        }
    }
}
