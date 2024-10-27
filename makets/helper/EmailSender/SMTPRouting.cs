namespace makets.helper.EmailSender
{
    public record SMTPRouting
    {

        public string fromEmail { get; }
        public string toEmail { get; }
        public SMTPRouting(string from, string to)
        {
            this.fromEmail = from;
            this.toEmail = to;
        }
    }
}
