namespace AutoEmail.WebAPI.Options
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = default!;
        public int SmtpPort { get; set; } = default!;
        public string SmtpUsername { get; set; } = default!;
        public string SmtpPassword { get; set; } = default!;
    }
}