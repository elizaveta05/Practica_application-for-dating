namespace makets.helper.EmailSender
{
    public record SMTPContent
    {

        public string Subject { get; }
        public string Content { get; }

        public SMTPContent(string subject, string content)
        {
            this.Subject = subject;
            this.Content = content;
        }
    }
}
