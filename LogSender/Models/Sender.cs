namespace LogSender.Models
{
    public class Sender
    {
        public bool UseForAnySender { get; set; }

        public string From { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public string Smtp { get; set; }

        public bool Ssl { get; set; }

        public bool UseAttachment { get; set; }
    }
}
